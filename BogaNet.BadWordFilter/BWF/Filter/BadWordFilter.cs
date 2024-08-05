using System;
using System.Linq;
using BogaNet.Extension;
using BogaNet.Helper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BogaNet.BWF.Enum;
using BogaNet.Util;

namespace BogaNet.BWF.Filter;

/// <summary>
/// Filter to remove bad words aka profanity.
/// </summary>
public class BadWordFilter : Singleton<BadWordFilter>, IBadWordFilter
{
   #region Variables

   private static readonly ILogger<BadWordFilter> _logger = GlobalLogging.CreateLogger<BadWordFilter>();

   protected const string EXACT_REGEX_START = @"(?<![\w\d])";
   protected const string EXACT_REGEX_END = @"s?(?![\w\d])";

   protected const RegexOptions REGEX_IC = RegexOptions.IgnoreCase;
   protected const RegexOptions REGEX_CI = RegexOptions.CultureInvariant;
   protected const RegexOptions REGEX_COMPILED = RegexOptions.Compiled;
   protected const RegexOptions REGEX_RTL = RegexOptions.RightToLeft;
   protected const RegexOptions REGEX_NONE = RegexOptions.None;

   private readonly Dictionary<string, Regex?> _exactBadwordsRegex = new(30);
   private readonly Dictionary<string, List<Regex>?> _debugExactBadwordsRegex = new(30);
   private readonly Dictionary<string, List<string>?> _simpleBadwords = new(30);

   #endregion

   #region Properties

   public virtual int Count => BWFConstants.DEBUG_BADWORDS ? _debugExactBadwordsRegex.Count : _exactBadwordsRegex.Count;
   public virtual List<string> SourceNames => BWFConstants.DEBUG_BADWORDS ? _debugExactBadwordsRegex.BNKeys() : _exactBadwordsRegex.BNKeys();
   public virtual bool IsLoaded { get; private set; }
   public virtual char[] ReplaceCharacters { get; set; } = ['*'];
   public virtual ReplaceMode Mode { get; set; } = ReplaceMode.Default;
   public virtual bool RemoveSpaces { get; set; }
   public virtual int MaxTextLength { get; set; } = 3;
   public virtual string RemoveCharacters { get; set; } = string.Empty;
   public virtual bool SimpleCheck { get; set; }

   #endregion

   #region Events

   public event ISourceFilter.FilesLoaded? OnFilesLoaded;

   #endregion

   #region Constructor

   private BadWordFilter()
   {
   }

   #endregion

   #region Public methods

   public virtual bool Remove(string srcName)
   {
      if (ContainsSource(srcName))
         return (BWFConstants.DEBUG_BADWORDS ? _debugExactBadwordsRegex.Remove(srcName) : _exactBadwordsRegex.Remove(srcName)) & _simpleBadwords.Remove(srcName);

      return false;
   }

   public virtual bool ContainsSource(string srcName)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(srcName);

      return BWFConstants.DEBUG_DOMAINS ? _debugExactBadwordsRegex.ContainsKey(srcName) : _exactBadwordsRegex.ContainsKey(srcName);
   }

   public virtual void Clear()
   {
      _exactBadwordsRegex.Clear();
      _debugExactBadwordsRegex.Clear();
      _simpleBadwords.Clear();
   }

   public virtual void Load(bool isLTR, Dictionary<string, string[]> dataDict)
   {
      process(dataDict, isLTR);
   }

   public virtual bool LoadFiles(bool isLTR, params Tuple<string, string>[] files)
   {
      ArgumentNullException.ThrowIfNull(files);

      Dictionary<string, string[]> allLines = new();

      foreach (var srcFile in files)
      {
         string[] lines = FileHelper.ReadAllLines(srcFile.Item2);

         if (lines.Length > 1)
            allLines.Add(srcFile.Item1, lines);
      }

      bool res = allLines.Count > 0;

      if (res)
         Load(isLTR, allLines);

      OnFilesLoaded?.Invoke(files);
      IsLoaded = res; //too simple?

      return res;
   }

   public virtual async Task<bool> LoadFilesAsync(bool isLTR, params Tuple<string, string>[] files)
   {
      ArgumentNullException.ThrowIfNull(files);

      Dictionary<string, string[]> allLines = new();

      foreach (var srcFile in files)
      {
         string[] lines = await FileHelper.ReadAllLinesAsync(srcFile.Item2);

         if (lines.Length > 1)
            allLines.Add(srcFile.Item1, lines);
      }

      bool res = allLines.Count > 0;

      if (res)
         Load(isLTR, allLines);

      OnFilesLoaded?.Invoke(files);
      IsLoaded = res; //too simple?

      return res;
   }

   public virtual bool LoadFilesFromUrl(bool isLTR, params Tuple<string, string>[] urls)
   {
      return Task.Run(() => LoadFilesFromUrlAsync(isLTR, urls)).GetAwaiter().GetResult();
   }

   public virtual async Task<bool> LoadFilesFromUrlAsync(bool isLTR, params Tuple<string, string>[] urls)
   {
      ArgumentNullException.ThrowIfNull(urls);

      Dictionary<string, string[]> allLines = new();

      foreach (var srcFile in urls)
      {
         string[] lines = await NetworkHelper.ReadAllLinesAsync(srcFile.Item2);

         if (lines.Length > 1)
            allLines.Add(srcFile.Item1, lines);
      }

      bool res = allLines.Count > 0;

      if (res)
         Load(isLTR, allLines);

      OnFilesLoaded?.Invoke(urls);
      IsLoaded = res; //too simple?

      return res;
   }

   public virtual void Add(bool isLTR, string srcName, string[] words)
   {
      process(new() { { srcName, words } }, isLTR);
   }

   public virtual bool Contains(string text, params string[]? sourceNames)
   {
      bool result = false;

      if (IsLoaded)
      {
         if (string.IsNullOrEmpty(text))
         {
            _logger.LogWarning("Parameter 'text' is null or empty! 'Contains()' will return 'false'.");
         }
         else
         {
            string _text = replaceText(text);
            Match? match;

            #region DEBUG

            if (BWFConstants.DEBUG_BADWORDS)
            {
               if (sourceNames == null || sourceNames.Length == 0)
               {
                  if (SimpleCheck)
                  {
                     foreach (List<string>? words in _simpleBadwords.Values)
                     {
                        if (words != null)
                           result = words.Any(simpleWord => _text.BNContains(simpleWord));

                        if (result)
                        {
                           _logger.LogDebug("Test string contains a bad word.");
                           break;
                        }
                     }
                  }
                  else
                  {
                     foreach (List<Regex>? badWordRegexes in _debugExactBadwordsRegex.Values)
                     {
                        if (badWordRegexes != null)
                        {
                           foreach (Regex badWordRegex in badWordRegexes)
                           {
                              match = badWordRegex.Match(_text);
                              if (match.Success)
                              {
                                 _logger.LogDebug($"Test string contains a bad word: '{match.Value}' detected by regex '{badWordRegex}'");
                                 result = true;
                                 break;
                              }
                           }
                        }
                     }
                  }
               }
               else
               {
                  for (int ii = 0; ii < sourceNames.Length && !result; ii++)
                  {
                     if (SimpleCheck)
                     {
                        if (_simpleBadwords.TryGetValue(sourceNames[ii], out List<string>? words))
                        {
                           if (words != null)
                              result = words.Any(simpleWord => _text.BNContains(simpleWord));

                           if (result)
                           {
                              _logger.LogDebug($"Test string contains a bad word from source '{sourceNames[ii]}'");
                              break;
                           }
                        }
                        else
                        {
                           logSourceNotFound(sourceNames[ii]);
                        }
                     }
                     else
                     {
                        if (_debugExactBadwordsRegex.TryGetValue(sourceNames[ii], out List<Regex>? badWordRegexes))
                        {
                           if (badWordRegexes != null)
                           {
                              foreach (Regex badWordRegex in badWordRegexes)
                              {
                                 match = badWordRegex.Match(_text);
                                 if (match.Success)
                                 {
                                    _logger.LogDebug($"Test string contains a bad word: '{match.Value}' detected by regex '{badWordRegex}' from source '{sourceNames[ii]}'");
                                    result = true;
                                    break;
                                 }
                              }
                           }
                        }
                        else
                        {
                           logSourceNotFound(sourceNames[ii]);
                        }
                     }
                  }
               }
            }

            #endregion

            else
            {
               if (sourceNames == null || sourceNames.Length == 0)
               {
                  if (SimpleCheck)
                  {
                     if (_simpleBadwords.Values.Any(words => words != null && words.Any(simpleWord => _text.BNContains(simpleWord))))
                        result = true;
                  }
                  else
                  {
                     if (_exactBadwordsRegex.Values.Any(badWordRegex => badWordRegex != null && badWordRegex.Match(_text).Success))
                        result = true;
                  }
               }
               else
               {
                  foreach (string badWordsResource in sourceNames)
                  {
                     if (SimpleCheck)
                     {
                        if (_simpleBadwords.TryGetValue(badWordsResource, out List<string>? words))
                        {
                           if (words != null && words.Any(simpleWord => _text.BNContains(simpleWord)))
                           {
                              result = true;
                              break;
                           }
                        }
                        else
                        {
                           logSourceNotFound(badWordsResource);
                        }
                     }
                     else
                     {
                        if (_exactBadwordsRegex.TryGetValue(badWordsResource, out Regex? badWordRegex))
                        {
                           match = badWordRegex?.Match(_text);
                           if (match != null && match.Success)
                           {
                              result = true;
                              break;
                           }
                        }
                        else
                        {
                           logSourceNotFound(badWordsResource);
                        }
                     }
                  }
               }
            }
         }
      }
      else
      {
         logFilterNotReady();
      }

      return result;
   }

   public virtual List<string> GetAll(string text, params string[]? sourceNames)
   {
      List<string> result = [];

      if (IsLoaded)
      {
         if (string.IsNullOrEmpty(text))
         {
            _logger.LogWarning("Parameter 'text' is null or empty! 'GetAll()' will return an empty list.");
         }
         else
         {
            string _text = replaceText(text);

            #region DEBUG

            if (BWFConstants.DEBUG_BADWORDS)
            {
               if (sourceNames == null || sourceNames.Length == 0)
               {
                  if (SimpleCheck)
                  {
                     foreach (string simpleWord in from words in _simpleBadwords.Values from simpleWord in words where _text.BNContains(simpleWord) select simpleWord)
                     {
                        _logger.LogDebug($"Test string contains a bad word detected by word '{simpleWord}'");

                        if (!result.Contains(simpleWord))
                           result.Add(simpleWord);
                     }
                  }
                  else
                  {
                     foreach (List<Regex>? badWordsResources in _debugExactBadwordsRegex.Values)
                     {
                        if (badWordsResources != null)
                        {
                           foreach (Regex badWordsResource in badWordsResources)
                           {
                              MatchCollection matches = badWordsResource.Matches(_text);

                              foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                              {
                                 _logger.LogDebug($"Test string contains a bad word: '{capture.Value}' detected by regex '{badWordsResource}'");

                                 if (!result.Contains(capture.Value))
                                    result.Add(capture.Value);
                              }
                           }
                        }
                     }
                  }
               }
               else
               {
                  foreach (string badWordsResource in sourceNames)
                  {
                     if (SimpleCheck)
                     {
                        if (_simpleBadwords.TryGetValue(badWordsResource, out List<string>? words))
                        {
                           if (words != null)
                           {
                              foreach (string simpleWord in words.Where(simpleWord => _text.BNContains(simpleWord)))
                              {
                                 _logger.LogDebug($"Test string contains a bad word detected by word '{simpleWord}' from source '{badWordsResource}'");

                                 if (!result.Contains(simpleWord))
                                    result.Add(simpleWord);
                              }
                           }
                        }
                        else
                        {
                           logSourceNotFound(badWordsResource);
                        }
                     }
                     else
                     {
                        if (_debugExactBadwordsRegex.TryGetValue(badWordsResource, out List<Regex>? badWordRegexes))
                        {
                           if (badWordRegexes != null)
                           {
                              foreach (Regex badWordRegex in badWordRegexes)
                              {
                                 MatchCollection matches = badWordRegex.Matches(_text);

                                 foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                                 {
                                    _logger.LogDebug($"Test string contains a bad word: '{capture.Value}' detected by regex '{badWordRegex}' from source '{badWordsResource}'");

                                    if (!result.Contains(capture.Value))
                                       result.Add(capture.Value);
                                 }
                              }
                           }
                        }
                        else
                        {
                           logSourceNotFound(badWordsResource);
                        }
                     }
                  }
               }
            }

            #endregion

            else
            {
               if (sourceNames == null || sourceNames.Length == 0)
               {
                  if (SimpleCheck)
                  {
                     foreach (string simpleWord in from words in _simpleBadwords.Values from simpleWord in words where _text.BNContains(simpleWord) where !result.Contains(simpleWord) select simpleWord)
                     {
                        result.Add(simpleWord);
                     }
                  }
                  else
                  {
                     foreach (Capture capture in from badWordsResource in _exactBadwordsRegex.Values select badWordsResource.Matches(_text) into matches from Match match in matches from Capture capture in match.Captures where !result.Contains(capture.Value) select capture)
                     {
                        result.Add(capture.Value);
                     }
                  }
               }
               else
               {
                  foreach (string badWordsResource in sourceNames)
                  {
                     if (SimpleCheck)
                     {
                        if (_simpleBadwords.TryGetValue(badWordsResource, out List<string>? words))
                        {
                           if (words != null)
                           {
                              foreach (string simpleWord in words.Where(simpleWord => _text.BNContains(simpleWord)).Where(simpleWord => !result.Contains(simpleWord)))
                              {
                                 result.Add(simpleWord);
                              }
                           }
                        }
                        else
                        {
                           logSourceNotFound(badWordsResource);
                        }
                     }
                     else
                     {
                        if (_exactBadwordsRegex.TryGetValue(badWordsResource, out Regex? badWordRegex))
                        {
                           MatchCollection? matches = badWordRegex?.Matches(_text);

                           if (matches != null)
                           {
                              foreach (Capture capture in from Match match in matches from Capture capture in match.Captures where !result.Contains(capture.Value) select capture)
                              {
                                 result.Add(capture.Value);
                              }
                           }
                        }
                        else
                        {
                           logSourceNotFound(badWordsResource);
                        }
                     }
                  }
               }
            }
         }
      }
      else
      {
         logFilterNotReady();
      }

      //Debug.Log("GETALL: " + DisableOrdering);

      return result.Distinct().OrderBy(x => x).ToList();
   }

   public virtual string ReplaceAll(string text, params string[]? sourceNames)
   {
      string result = string.Empty;
      bool hasBadWords = false;

      if (IsLoaded)
      {
         if (string.IsNullOrEmpty(text))
         {
            _logger.LogWarning("Parameter 'text' is null or empty! 'ReplaceAll()' will return an empty string.");
         }
         else
         {
            string _text = result = replaceText(text);

            if (SimpleCheck)
            {
               foreach (string badword in GetAll(_text, sourceNames))
               {
                  _text = Regex.Replace(_text, badword, StringHelper.CreateString(badword.Length, ReplaceCharacters), RegexOptions.IgnoreCase);
                  hasBadWords = true;
               }

               result = _text;
            }

            #region DEBUG

            else if (BWFConstants.DEBUG_BADWORDS)
            {
               if (sourceNames == null || sourceNames.Length == 0)
               {
                  foreach (List<Regex>? badWordsResources in _debugExactBadwordsRegex.Values)
                  {
                     if (badWordsResources != null)
                     {
                        foreach (Regex badWordsResource in badWordsResources)
                        {
                           MatchCollection matches = badWordsResource.Matches(_text);

                           foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                           {
                              _logger.LogDebug($"Test string contains a bad word: '{capture.Value}' detected by regex '{badWordsResource}'");

                              result = replaceCapture(result, capture, result.Length - _text.Length);

                              hasBadWords = true;
                           }
                        }
                     }
                  }
               }
               else
               {
                  foreach (string badWordsResource in sourceNames)
                  {
                     if (_debugExactBadwordsRegex.TryGetValue(badWordsResource, out List<Regex>? badWordRegexes))
                     {
                        if (badWordRegexes != null)
                        {
                           foreach (Regex badWordRegex in badWordRegexes)
                           {
                              MatchCollection matches = badWordRegex.Matches(_text);

                              foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                              {
                                 _logger.LogDebug($"Test string contains a bad word: '{capture.Value}' detected by regex '{badWordRegex}'' from source '{badWordsResource}'");

                                 result = replaceCapture(result, capture, result.Length - _text.Length);

                                 hasBadWords = true;
                              }
                           }
                        }
                     }
                     else
                     {
                        logSourceNotFound(badWordsResource);
                     }
                  }
               }
            }

            #endregion

            else
            {
               if (sourceNames == null || sourceNames.Length == 0)
               {
                  foreach (Capture capture in from badWordsResource in _exactBadwordsRegex.Values select badWordsResource.Matches(_text) into matches from Match match in matches from Capture capture in match.Captures select capture)
                  {
                     _logger.LogDebug($"Test string contains a bad word: '{capture.Value}'");

                     result = replaceCapture(result, capture, result.Length - _text.Length);

                     hasBadWords = true;
                  }
               }
               else
               {
                  foreach (string badWordsResource in sourceNames)
                  {
                     if (_exactBadwordsRegex.TryGetValue(badWordsResource, out Regex? badWordRegex))
                     {
                        MatchCollection? matches = badWordRegex?.Matches(_text);

                        if (matches != null)
                        {
                           foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                           {
                              _logger.LogDebug($"Test string contains a bad word: '{capture.Value}'");

                              result = replaceCapture(result, capture, result.Length - _text.Length);

                              hasBadWords = true;
                           }
                        }
                     }
                     else
                     {
                        logSourceNotFound(badWordsResource);
                     }
                  }
               }
            }
         }
      }
      else
      {
         logFilterNotReady();
      }

      return hasBadWords ? result : text;
   }

   #endregion

   #region Private methods

   private string replaceCapture(string text, Capture capture, int offset)
   {
      System.Text.StringBuilder sb = new(text);

      string replacement = StringHelper.CreateString(capture.Value.Length, ReplaceCharacters);

      sb.Remove(capture.Index + offset, capture.Value.Length);
      sb.Insert(capture.Index + offset, replacement);

      return sb.ToString();
   }

   protected string replaceText(string input)
   {
      string result = input;

      if (RemoveSpaces)
         result = replaceSpacesBetweenLetters(result, MaxTextLength);

      if (!string.IsNullOrEmpty(RemoveCharacters))
         result = removeChars(result, RemoveCharacters);

      switch (Mode)
      {
         case ReplaceMode.LeetSpeak:
            result = replaceLeetToText(result);
            break;
         case ReplaceMode.LeetSpeakAdvanced:
            result = replaceLeetAdvancedToText(result);
            result = replaceLeetToText(result);
            break;
         case ReplaceMode.NonLettersOrDigits:
            result = replaceNonLettersOrDigits(result);
            break;
      }

      return result;
   }

   private static string replaceNonLettersOrDigits(string input)
   {
      char[] arr = input.ToCharArray();

      arr = Array.FindAll(arr, c => char.IsLetterOrDigit(c)
                                    || char.IsWhiteSpace(c)
                                    || c == ','
                                    || c == '?'
                                    || c == '!'
                                    || c == '-'
                                    || c == ';'
                                    || c == ':'
                                    || c == '"'
                                    || c == '\''
                                    || c == '.');

      //Debug.Log(new string(arr));
      return new string(arr);
   }

   private static string replaceSpacesBetweenLetters(string text, int maxTextLength = 4)
   {
      if (string.IsNullOrEmpty(text))
         return text;

      string[] textArray = text.Split([" "], StringSplitOptions.RemoveEmptyEntries);

      System.Text.StringBuilder sb = new();

      for (int ii = 0; ii < textArray.Length; ii++)
      {
         string currentText = textArray[ii];

         if (currentText.Length <= maxTextLength)
         {
            sb.Append(currentText);

            for (int xx = ii + 1; xx < textArray.Length; xx++)
            {
               string nextText = textArray[xx];

               if (nextText.Length <= maxTextLength)
               {
                  ii = xx;
                  sb.Append(nextText);
               }
               else
               {
                  break;
               }
            }
         }
         else
         {
            sb.Append(currentText);
         }

         if (ii < textArray.Length - 1)
            sb.Append(' ');
      }

      //Debug.Log(sb.ToString());
      return sb.ToString();
   }

   private static string removeChars(string input, string removeChars)
   {
      return input.BNRemoveChars(removeChars.ToCharArray());
   }

   private static string replaceLeetToText(string input)
   {
      if (string.IsNullOrEmpty(input))
         return input;

      string result = input;

      // A
      result = result.Replace("@", "a");
      result = result.Replace("4", "a");
      result = result.Replace("^", "a");

      // B
      result = result.Replace("8", "b");

      // C
      result = result.Replace("©", "c");
      result = result.Replace('¢', 'c');

      // D

      // E
      result = result.Replace("€", "e");
      result = result.Replace("3", "e");
      result = result.Replace("£", "e");

      // F
      result = result.Replace("ƒ", "f");

      // G
      result = result.Replace("6", "g");
      result = result.Replace("9", "g");

      // H
      result = result.Replace("#", "h");

      // I
      result = result.Replace("1", "i");
      result = result.Replace("!", "i");
      result = result.Replace("|", "i");

      // J

      // K

      // L

      // M

      // N

      // O
      result = result.Replace("0", "o");

      // P

      // Q

      // R
      result = result.Replace("2", "r");
      result = result.Replace("®", "r");

      // S
      result = result.Replace("$", "s");
      result = result.Replace("5", "s");
      result = result.Replace("§", "s");

      // T
      result = result.Replace("7", "t");
      result = result.Replace("+", "t");
      result = result.Replace("†", "t");

      // U

      // V

      // W

      // X

      // Y
      result = result.Replace("¥", "y");

      // Z

      return result;
   }

   private static string replaceLeetAdvancedToText(string input)
   {
      if (string.IsNullOrEmpty(input))
         return input;

      string result = input;


      // B

      // C

      // D

      // E

      // F

      // G

      // H
      result = result.Replace("|-|", "h");
      result = result.Replace("}{", "h");
      result = result.Replace("]-[", "h");
      result = result.Replace("/-/", "h");
      result = result.Replace(")-(", "h");

      // I
      result = result.Replace("][", "i");

      // J

      // K
      result = result.Replace("|<", "k");
      result = result.Replace("|{", "k");
      result = result.Replace("|(", "k");

      // L
      result = result.Replace("|_", "l");
      result = result.Replace("][_", "l");

      // M
      result = result.Replace("/\\/\\", "m");
      result = result.Replace("/v\\", "m");
      result = result.Replace("|V|", "m");
      result = result.Replace("]V[", "m");
      result = result.Replace("|\\/|", "m");

      // N
      result = result.Replace("|\\|", "n");
      result = result.Replace("/\\/", "n");
      result = result.Replace("/V", "n");

      // O
      result = result.Replace("()", "o");

      // P
      result = result.Replace("|°", "p");
      result = result.Replace("|>", "p");

      // Q

      // R

      // S

      // T
      //result = result.Replace ("']['", "t");

      // U
      result = result.Replace("µ", "u");
      result = result.Replace("|_|", "u");

      // W
      result = result.Replace("\\/\\/", "w");

      // V
      result = result.Replace("\\/", "v");

      // X
      result = result.Replace("><", "x");
      result = result.Replace(")(", "x");

      // Y

      // Z

      // A
      result = result.Replace("/\\", "a");
      result = result.Replace("/-\\", "a");

      //Debug.Log("RESULT: " + result);
      return result;
   }
/*
        protected string replaceTextToLeet(string input, bool obvious = true)
        {
            string result = input;

            if (ReplaceLeetSpeak && !string.IsNullOrEmpty(input))
            {
                if (obvious)
                {
                    // I
                    //result = result.Replace("i", "!");

                    // S
                    result = result.Replace("s", "$");
                }
                else
                {
                    // A
                    result = result.Replace("a", "@");
                    //result = result.Replace("4", "a");
                    //result = result.Replace("^", "a");

                    // B
                    result = result.Replace("b", "8");

                    // C
                    //result = result.Replace("©", "c");
                    //result = result.Replace('¢', 'c');

                    // D

                    // E
                    //result = result.Replace("€", "e");
                    result = result.Replace("e", "3");
                    //result = result.Replace("£", "e");

                    // F
                    //result = result.Replace("ƒ", "f");

                    // G
                    //result = result.Replace("6", "g");
                    result = result.Replace("g", "9");

                    // H
                    //result = result.Replace("#", "h");
                    //result = result.Replace ("|-|", "h");
                    //result = result.Replace ("}{", "h");
                    //result = result.Replace ("]-[", "h");
                    //result = result.Replace ("/-/", "h");
                    //result = result.Replace (")-(", "h");

                    // I
                    result = result.Replace("i", "1");
                    //result = result.Replace("i", "!");
                    //result = result.Replace("|", "i");
                    //result = result.Replace ("][", "i");

                    // J

                    // K
                    //result = result.Replace ("|<", "k");
                    //result = result.Replace ("|{", "k");
                    //result = result.Replace ("|(", "k");

                    // L
                    //result = result.Replace ("|_", "l");
                    //result = result.Replace ("][_", "l");

                    // M
                    //result = result.Replace ("/\\/\\", "m");
                    //result = result.Replace ("/v\\", "m");
                    //result = result.Replace ("|V|", "m");
                    //result = result.Replace ("]V[", "m");
                    //result = result.Replace ("|\\/|", "m");

                    // N
                    //result = result.Replace ("|\\|", "n");
                    //result = result.Replace ("/\\/", "n");
                    //result = result.Replace ("/V", "n");

                    // O
                    result = result.Replace("o", "0");
                    //result = result.Replace ("()", "o");

                    // P
                    //result = result.Replace ("|°", "p");
                    //result = result.Replace ("|>", "p");

                    // Q

                    // R
                    result = result.Replace("r", "2");
                    //result = result.Replace("®", "r");

                    // S
                    result = result.Replace("s", "$");
                    //result = result.Replace("s", "5");
                    //result = result.Replace("§", "s");

                    // T
                    result = result.Replace("t", "7");
                    //result = result.Replace("+", "t");
                    //result = result.Replace("†", "t");
                    //result = result.Replace ("']['", "t");

                    // U
                    //result = result.Replace ("µ", "u");
                    //result = result.Replace ("|_|", "u");

                    // V
                    //result = result.Replace ("\\/", "v");

                    // W
                    //result = result.Replace ("\\/\\/", "w");

                    // X
                    //result = result.Replace ("><", "x");
                    //result = result.Replace (")(", "x");

                    // Y
                    //result = result.Replace("¥", "y");

                    // Z
                }
            }

            //Debug.LogWarning (result);

            return result;
        }
*/

   private void process(Dictionary<string, string[]> dataDict, bool isLTR)
   {
      if (BWFConstants.DEBUG_BADWORDS)
         _logger.LogDebug("++ BadWordFilter started in debug-mode ++");

      foreach (var kvp in dataDict)
      {
         string source = kvp.Key;
         List<string> list = [];

         list.AddRange(from str in kvp.Value where !str.BNStartsWith("#") select str.Split('#')[0]);
         string[] words = list.ToArray();

         if (BWFConstants.DEBUG_BADWORDS)
         {
            try
            {
               List<Regex> exactRegexes = new(words.Length);
               exactRegexes.AddRange(words.Select(line => new Regex(EXACT_REGEX_START + line + EXACT_REGEX_END, REGEX_IC | REGEX_CI | REGEX_COMPILED | (isLTR ? REGEX_NONE : REGEX_RTL))));

               _debugExactBadwordsRegex.TryAdd(source, exactRegexes);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, $"Could not generate debug regex for source '{source}'");
            }
         }
         else
         {
            try
            {
               _exactBadwordsRegex.TryAdd(source, new Regex($"{EXACT_REGEX_START}({string.Join("|", words.ToArray())}){EXACT_REGEX_END}", REGEX_IC | REGEX_CI | REGEX_COMPILED | (isLTR ? REGEX_NONE : REGEX_RTL)));
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, $"Could not generate exact regex for source '{source}'");
            }
         }

         List<string> simpleWords = new(words.Length);

         simpleWords.AddRange(words);

         _simpleBadwords.TryAdd(source, simpleWords);

         if (BWFConstants.DEBUG_BADWORDS)
            _logger.LogDebug($"Bad word resource '{source}' loaded and {words.Length} entries found.");

         IsLoaded = true;
      }
   }

   protected static void logFilterNotReady()
   {
      _logger.LogWarning("Filter is not ready - please wait until 'IsLoaded' returns true.");
   }

   protected static void logSourceNotFound(string res)
   {
      _logger.LogWarning($"Source not found: '{res}' - Did you call the method with the correct source name?");
   }

   #endregion
}