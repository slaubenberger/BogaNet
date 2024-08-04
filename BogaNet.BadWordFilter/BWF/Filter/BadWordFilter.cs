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

namespace BogaNet.BWF.Filter
{
   /// <summary>Filter for bad words. The class can also replace all bad words inside a string.</summary>
   public class BadWordFilter : Singleton<BadWordFilter>, IBadWordFilter
   {
      #region Variables

      private static readonly ILogger<BadWordFilter> _logger = GlobalLogging.CreateLogger<BadWordFilter>();

      private const string EXACT_REGEX_START = @"(?<![\w\d])";
      private const string EXACT_REGEX_END = @"s?(?![\w\d])";

      /// <summary>Option1 (default: RegexOptions.IgnoreCase).</summary>
      public RegexOptions RegexOption1 = RegexOptions.IgnoreCase; //DEFAULT

      /// <summary>Option2 (default: RegexOptions.CultureInvariant).</summary>
      public RegexOptions RegexOption2 = RegexOptions.CultureInvariant; //DEFAULT

      /// <summary>Option3 (default: RegexOptions.None).</summary>
      public RegexOptions RegexOption3 = RegexOptions.Compiled;

      /// <summary>Option4 (default: RegexOptions.None).</summary>
      public RegexOptions RegexOption4 = RegexOptions.None;

      /// <summary>Option5 (default: RegexOptions.None).</summary>
      public RegexOptions RegexOption5 = RegexOptions.None;

      private readonly Dictionary<string, Regex> _exactBadwordsRegex = new(30);
      private readonly Dictionary<string, List<Regex>> _debugExactBadwordsRegex = new(30);
      private readonly Dictionary<string, List<string>> _simpleBadwords = new(30);

      #endregion


      #region Properties

      public int Count { get; }
      public List<string> SourceNames { get; }
      public bool IsLoaded { get; private set; }
      public char[] ReplaceCharacters { get; set; }
      public ReplaceMode Mode { get; set; }
      public bool RemoveSpaces { get; set; }
      public int MaxTextLength { get; set; }
      public string RemoveCharacters { get; set; }
      public bool SimpleCheck { get; set; }

      #endregion


      #region Constructor

      private BadWordFilter()
      {
      }

      #endregion


      #region Implemented methods

      public event ISourceFilter.FilesLoaded? OnFilesLoaded;

      public bool Remove(string srcName)
      {
         if (ContainsSource(srcName))
            return _exactBadwordsRegex.Remove(srcName) & _debugExactBadwordsRegex.Remove(srcName) & _simpleBadwords.Remove(srcName);

         return false;
      }

      public bool ContainsSource(string srcName)
      {
         ArgumentNullException.ThrowIfNullOrEmpty(srcName);

        return _exactBadwordsRegex.ContainsKey(srcName) || _debugExactBadwordsRegex.ContainsKey(srcName);
      }

      public void Clear()
      {
         _exactBadwordsRegex.Clear();
         _debugExactBadwordsRegex.Clear();
         _simpleBadwords.Clear();
      }

      public void Load(Dictionary<string, string[]> dataDict) //TODO handle RTL?
      {
         process(dataDict);
      }

      public bool LoadFiles(params Tuple<string, string>[] files) //TODO handle RTL?
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
            Load(allLines);

         OnFilesLoaded?.Invoke(files);
         IsLoaded = res; //too simple?

         return res;
      }

      public async Task<bool> LoadFilesAsync(params Tuple<string, string>[] files) //TODO handle RTL?
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
            Load(allLines);

         OnFilesLoaded?.Invoke(files);
         IsLoaded = res; //too simple?

         return res;
      }

      public bool LoadFilesFromUrl(params Tuple<string, string>[] urls) //TODO handle RTL?
      {
         return Task.Run(() => LoadFilesFromUrlAsync(urls)).GetAwaiter().GetResult();
      }

      public async Task<bool> LoadFilesFromUrlAsync(params Tuple<string, string>[] urls) //TODO handle RTL?
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
            Load(allLines);

         OnFilesLoaded?.Invoke(urls);
         IsLoaded = res; //too simple?

         return res;
      }

      public void Add(string srcName, string[] words, bool isLTR = true) //TODO handle RTL
      {
         process(new() { { srcName, words } });
      }

      public bool Contains(string text, params string[] sourceNames)
      {
         bool result = false;

         if (IsLoaded)
         {
            if (string.IsNullOrEmpty(text))
            {
               logContains();
            }
            else
            {
               string _text = replaceText(text);
               Match match;

               #region DEBUG

               if (Config.DEBUG_BADWORDS)
               {
                  if (sourceNames == null || sourceNames.Length == 0)
                  {
                     if (SimpleCheck)
                     {
                        foreach (List<string> words in _simpleBadwords.Values)
                        {
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
                        foreach (List<Regex> badWordRegexes in _debugExactBadwordsRegex.Values)
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
                  else
                  {
                     for (int ii = 0; ii < sourceNames.Length && !result; ii++)
                     {
                        if (SimpleCheck)
                        {
                           if (_simpleBadwords.TryGetValue(sourceNames[ii], out List<string> words))
                           {
                              result = words.Any(simpleWord => _text.BNContains(simpleWord));

                              if (result)
                              {
                                 _logger.LogDebug($"Test string contains a bad word from source '{sourceNames[ii]}'");
                                 break;
                              }
                           }
                           else
                           {
                              logResourceNotFound(sourceNames[ii]);
                           }
                        }
                        else
                        {
                           if (_debugExactBadwordsRegex.TryGetValue(sourceNames[ii], out List<Regex> badWordRegexes))
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
                           else
                           {
                              logResourceNotFound(sourceNames[ii]);
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
                        if (_simpleBadwords.Values.Any(words => words.Any(simpleWord => _text.BNContains(simpleWord))))
                           result = true;
                     }
                     else
                     {
                        if (_exactBadwordsRegex.Values.Any(badWordRegex => badWordRegex.Match(_text).Success))
                           result = true;
                     }
                  }
                  else
                  {
                     foreach (string badWordsResource in sourceNames)
                     {
                        if (SimpleCheck)
                        {
                           if (_simpleBadwords.TryGetValue(badWordsResource, out List<string> words))
                           {
                              if (words.Any(simpleWord => _text.BNContains(simpleWord)))
                              {
                                 result = true;
                                 break;
                              }
                           }
                           else
                           {
                              logResourceNotFound(badWordsResource);
                           }
                        }
                        else
                        {
                           if (_exactBadwordsRegex.TryGetValue(badWordsResource, out Regex badWordRegex))
                           {
                              match = badWordRegex.Match(_text);
                              if (match.Success)
                              {
                                 result = true;
                                 break;
                              }
                           }
                           else
                           {
                              logResourceNotFound(badWordsResource);
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

      public List<string> GetAll(string text, params string[] sourceNames)
      {
         List<string> result = new List<string>();

         if (IsLoaded)
         {
            if (string.IsNullOrEmpty(text))
            {
               logGetAll();
            }
            else
            {
               string _text = replaceText(text);

               #region DEBUG

               if (Config.DEBUG_BADWORDS)
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
                        foreach (List<Regex> badWordsResources in _debugExactBadwordsRegex.Values)
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
                  else
                  {
                     foreach (string badWordsResource in sourceNames)
                     {
                        if (SimpleCheck)
                        {
                           if (_simpleBadwords.TryGetValue(badWordsResource, out List<string> words))
                           {
                              foreach (string simpleWord in words.Where(simpleWord => _text.BNContains(simpleWord)))
                              {
                                 _logger.LogDebug($"Test string contains a bad word detected by word '{simpleWord}' from source '{badWordsResource}'");

                                 if (!result.Contains(simpleWord))
                                    result.Add(simpleWord);
                              }
                           }
                           else
                           {
                              logResourceNotFound(badWordsResource);
                           }
                        }
                        else
                        {
                           if (_debugExactBadwordsRegex.TryGetValue(badWordsResource, out List<Regex> badWordRegexes))
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
                           else
                           {
                              logResourceNotFound(badWordsResource);
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
                           if (_simpleBadwords.TryGetValue(badWordsResource, out List<string> words))
                           {
                              foreach (string simpleWord in words.Where(simpleWord => _text.BNContains(simpleWord)).Where(simpleWord => !result.Contains(simpleWord)))
                              {
                                 result.Add(simpleWord);
                              }
                           }
                           else
                           {
                              logResourceNotFound(badWordsResource);
                           }
                        }
                        else
                        {
                           if (_exactBadwordsRegex.TryGetValue(badWordsResource, out Regex badWordRegex))
                           {
                              MatchCollection matches = badWordRegex.Matches(_text);

                              foreach (Capture capture in from Match match in matches from Capture capture in match.Captures where !result.Contains(capture.Value) select capture)
                              {
                                 result.Add(capture.Value);
                              }
                           }
                           else
                           {
                              logResourceNotFound(badWordsResource);
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

      public string ReplaceAll(string text, string prefix = "", string postfix = "", params string[] sourceNames)
      {
         string result = string.Empty;
         bool hasBadWords = false;

         if (IsLoaded)
         {
            if (string.IsNullOrEmpty(text))
            {
               logReplaceAll();
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

               else if (Config.DEBUG_BADWORDS)
               {
                  if (sourceNames == null || sourceNames.Length == 0)
                  {
                     foreach (List<Regex> badWordsResources in _debugExactBadwordsRegex.Values)
                     {
                        foreach (Regex badWordsResource in badWordsResources)
                        {
                           MatchCollection matches = badWordsResource.Matches(_text);

                           foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                           {
                              _logger.LogDebug($"Test string contains a bad word: '{capture.Value}' detected by regex '{badWordsResource}'");

                              result = replaceCapture(result, capture, prefix, postfix, result.Length - _text.Length);

                              hasBadWords = true;
                           }
                        }
                     }
                  }
                  else
                  {
                     foreach (string badWordsResource in sourceNames)
                     {
                        if (_debugExactBadwordsRegex.TryGetValue(badWordsResource, out List<Regex> badWordRegexes))
                        {
                           foreach (Regex badWordRegex in badWordRegexes)
                           {
                              MatchCollection matches = badWordRegex.Matches(_text);

                              foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                              {
                                 _logger.LogDebug($"Test string contains a bad word: '{capture.Value}' detected by regex '{badWordRegex}'' from source '{badWordsResource}'");

                                 result = replaceCapture(result, capture, prefix, postfix, result.Length - _text.Length);

                                 hasBadWords = true;
                              }
                           }
                        }
                        else
                        {
                           logResourceNotFound(badWordsResource);
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
                        result = replaceCapture(result, capture, prefix, postfix, result.Length - _text.Length);

                        hasBadWords = true;
                     }
                  }
                  else
                  {
                     foreach (string badWordsResource in sourceNames)
                     {
                        if (_exactBadwordsRegex.TryGetValue(badWordsResource, out Regex badWordRegex))
                        {
                           MatchCollection matches = badWordRegex.Matches(_text);

                           foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                           {
                              result = replaceCapture(result, capture, prefix, postfix, result.Length - _text.Length);

                              hasBadWords = true;
                           }
                        }
                        else
                        {
                           logResourceNotFound(badWordsResource);
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

      private string replaceCapture(string text, Capture capture, string prefix, string postfix, int offset)
      {
         System.Text.StringBuilder sb = new System.Text.StringBuilder(text);

         string replacement = prefix + StringHelper.CreateString(capture.Value.Length, ReplaceCharacters) + postfix;

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

         string[] textArray = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

         System.Text.StringBuilder sb = new System.Text.StringBuilder();

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
               sb.Append(" ");
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

      private void process(Dictionary<string, string[]> dataDict) //TODO handle RTL?
      {
         if (Config.DEBUG_BADWORDS)
            _logger.LogDebug("++ BadWordFilter started in debug-mode ++");

         foreach (var kvp in dataDict)
         {
            string source = kvp.Key;
            string[] words = kvp.Value;

            if (Config.DEBUG_BADWORDS)
            {
               try
               {
                  List<Regex> exactRegexes = new(words.Length);
                  exactRegexes.AddRange(words.Select(line => new Regex(EXACT_REGEX_START + line + EXACT_REGEX_END, RegexOption1 | RegexOption2 | RegexOption3 | RegexOption4 | RegexOption5)));

                  if (!_debugExactBadwordsRegex.ContainsKey(source))
                     _debugExactBadwordsRegex.Add(source, exactRegexes);
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
                  if (!_exactBadwordsRegex.ContainsKey(source))
                  {
                     _exactBadwordsRegex.Add(source, new Regex($"{EXACT_REGEX_START}({string.Join("|", words.ToArray())}){EXACT_REGEX_END}", RegexOption1 | RegexOption2 | RegexOption3 | RegexOption4 | RegexOption5));
                  }
               }
               catch (System.Exception ex)
               {
                  _logger.LogError(ex, $"Could not generate exact regex for source '{source}'");
               }
            }

            List<string> simpleWords = new List<string>(words.Length);

            simpleWords.AddRange(words);

            if (!_simpleBadwords.ContainsKey(source))
               _simpleBadwords.Add(source, simpleWords);

            if (Config.DEBUG_BADWORDS)
               _logger.LogDebug($"Bad word resource '{source}' loaded and {words.Length} entries found.");
         }

         //isReady = true;
         //raiseOnProviderReady();
      }

      #endregion


      #region Protected methods

      protected static void logFilterNotReady()
      {
         _logger.LogWarning("Filter is not ready - please wait until 'isReady' returns true.");
      }

      protected static void logResourceNotFound(string res)
      {
         _logger.LogWarning($"Resource not found: '{res}'!Did you call the method with the correct resource name?");
      }

      protected static void logContains()
      {
         _logger.LogWarning("Parameter 'text' is null or empty! 'Contains()' will return 'false'.");
      }

      protected static void logGetAll()
      {
         _logger.LogWarning("Parameter 'text' is null or empty! 'GetAll()' will return an empty list.");
      }

      protected static void logReplaceAll()
      {
         _logger.LogWarning("Parameter 'text' is null or empty! 'ReplaceAll()' will return an empty string.");
      }

      #endregion
   }
}