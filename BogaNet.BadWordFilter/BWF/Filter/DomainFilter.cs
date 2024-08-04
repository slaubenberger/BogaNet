using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using BogaNet.Helper;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BogaNet.Extension;
using BogaNet.Util;

namespace BogaNet.BWF.Filter
{
   /// <summary>Filter for domains. The class can also replace all domains inside a string.</summary>
   public class DomainFilter : Singleton<DomainFilter>, IDomainFilter
   {
      #region Variables

      private static readonly ILogger<DomainFilter> _logger = GlobalLogging.CreateLogger<DomainFilter>();

      protected const string DOMAIN_REGEX_START = @"\b{0,1}((ht|f)tp(s?)\:\/\/)?[\w\-\.\@]*[\.]";
      protected const string DOMAIN_REGEX_END = @"(:\d{1,5})?(\/|\b)";

      protected const RegexOptions REGEX_IC = RegexOptions.IgnoreCase; //DEFAULT
      protected const RegexOptions REGEX_CI = RegexOptions.CultureInvariant; //DEFAULT
      protected const RegexOptions REGEX_COMPILED = RegexOptions.Compiled;

      private readonly Dictionary<string, Regex> _domainsRegex = new();
      private readonly Dictionary<string, List<Regex>> _debugDomainsRegex = new();

      #endregion

      #region Properties

      public virtual int Count => Config.DEBUG_DOMAINS ? _debugDomainsRegex.Count : _domainsRegex.Count;
      public virtual List<string> SourceNames => Config.DEBUG_DOMAINS ? _debugDomainsRegex.BNKeys() : _domainsRegex.BNKeys();
      public virtual bool IsLoaded { get; private set; }

      public virtual char[] ReplaceCharacters { get; set; }

      #endregion

      #region Events

      public virtual event ISourceFilter.FilesLoaded? OnFilesLoaded;

      #endregion

      #region Constructor

      private DomainFilter()
      {
      }

      #endregion

      #region Public methods

      public virtual bool Remove(string srcName)
      {
         if (ContainsSource(srcName))
            return Config.DEBUG_DOMAINS ? _debugDomainsRegex.Remove(srcName) : _domainsRegex.Remove(srcName);

         return false;
      }

      public virtual bool ContainsSource(string srcName)
      {
         ArgumentNullException.ThrowIfNullOrEmpty(srcName);

         return Config.DEBUG_DOMAINS ? _debugDomainsRegex.ContainsKey(srcName) : _domainsRegex.ContainsKey(srcName);
      }

      public virtual void Clear()
      {
         _domainsRegex.Clear();
         _debugDomainsRegex.Clear();
      }

      public virtual void Load(Dictionary<string, string[]> dataDict)
      {
         process(dataDict);
      }

      public virtual bool LoadFiles(params Tuple<string, string>[] files)
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

      public virtual async Task<bool> LoadFilesAsync(params Tuple<string, string>[] files)
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

      public virtual bool LoadFilesFromUrl(params Tuple<string, string>[] urls)
      {
         return Task.Run(() => LoadFilesFromUrlAsync(urls)).GetAwaiter().GetResult();
      }

      public virtual async Task<bool> LoadFilesFromUrlAsync(params Tuple<string, string>[] urls)
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


      public virtual void Add(string srcName, params string[] domains)
      {
         process(new() { { srcName, domains } });
      }

      public virtual bool Contains(string text, params string[] sourceNames)
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
               #region DEBUG

               if (Config.DEBUG_DOMAINS)
               {
                  if (sourceNames == null || sourceNames.Length == 0)
                  {
                     foreach (List<Regex> domainRegexes in _debugDomainsRegex.Values)
                     {
                        foreach (Regex domainRegex in domainRegexes)
                        {
                           Match match = domainRegex.Match(text);
                           if (match.Success)
                           {
                              _logger.LogDebug($"Test string contains a domain: '{match.Value}' detected by regex '{domainRegex}'");
                              result = true;
                              break;
                           }
                        }
                     }
                  }
                  else
                  {
                     foreach (string domainResource in sourceNames)
                     {
                        if (_debugDomainsRegex.TryGetValue(domainResource, out List<Regex> domainRegexes))
                        {
                           foreach (Regex domainRegex in domainRegexes)
                           {
                              Match match = domainRegex.Match(text);
                              if (match.Success)
                              {
                                 _logger.LogDebug($"Test string contains a domain: '{match.Value}' detected by regex '{domainRegex}' from source '{domainResource}'");
                                 result = true;
                                 break;
                              }
                           }
                        }
                        else
                        {
                           logSourceNotFound(domainResource);
                        }
                     }
                  }
               }

               #endregion

               else
               {
                  if (sourceNames == null || sourceNames.Length == 0)
                  {
                     if (_domainsRegex.Values.Any(domainRegex => domainRegex.Match(text).Success))
                     {
                        result = true;
                     }
                  }
                  else
                  {
                     foreach (string domainResource in sourceNames)
                     {
                        if (_domainsRegex.TryGetValue(domainResource, out Regex domainRegex))
                        {
                           Match match = domainRegex.Match(text);
                           if (match.Success)
                           {
                              result = true;
                              break;
                           }
                        }
                        else
                        {
                           logSourceNotFound(domainResource);
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

      public virtual List<string> GetAll(string text, params string[] sourceNames)
      {
         List<string> result = new List<string>();

         if (IsLoaded)
         {
            if (string.IsNullOrEmpty(text))
            {
               _logger.LogWarning("Parameter 'text' is null or empty! 'GetAll()' will return an empty list.");
            }
            else
            {
               #region DEBUG

               if (Config.DEBUG_DOMAINS)
               {
                  if (sourceNames == null || sourceNames.Length == 0)
                  {
                     foreach (List<Regex> domainRegexes in _debugDomainsRegex.Values)
                     {
                        foreach (Regex domainRegex in domainRegexes)
                        {
                           MatchCollection matches = domainRegex.Matches(text);

                           foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                           {
                              _logger.LogDebug($"Test string contains a domain: '{capture.Value}' detected by regex '{domainRegex}'");

                              if (!result.Contains(capture.Value))
                                 result.Add(capture.Value);
                           }
                        }
                     }
                  }
                  else
                  {
                     foreach (string domainResource in sourceNames)
                     {
                        if (_debugDomainsRegex.TryGetValue(domainResource, out List<Regex> domainRegexes))
                        {
                           foreach (Regex domainRegex in domainRegexes)
                           {
                              MatchCollection matches = domainRegex.Matches(text);

                              foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                              {
                                 _logger.LogDebug($"Test string contains a domain: '{capture.Value}' detected by regex '{domainRegex}'' from source '{domainResource}'");

                                 if (!result.Contains(capture.Value))
                                    result.Add(capture.Value);
                              }
                           }
                        }
                        else
                        {
                           logSourceNotFound(domainResource);
                        }
                     }
                  }
               }

               #endregion

               else
               {
                  if (sourceNames == null || sourceNames.Length == 0)
                  {
                     foreach (Capture capture in from domainRegex in _domainsRegex.Values select domainRegex.Matches(text) into matches from Match match in matches from Capture capture in match.Captures where !result.Contains(capture.Value) select capture)
                     {
                        result.Add(capture.Value);
                     }
                  }
                  else
                  {
                     foreach (string domainResource in sourceNames)
                     {
                        if (_domainsRegex.TryGetValue(domainResource, out Regex domainRegex))
                        {
                           MatchCollection matches = domainRegex.Matches(text);

                           foreach (Capture capture in from Match match in matches from Capture capture in match.Captures where !result.Contains(capture.Value) select capture)
                           {
                              result.Add(capture.Value);
                           }
                        }
                        else
                        {
                           logSourceNotFound(domainResource);
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

         return result.Distinct().OrderBy(x => x).ToList();
      }

      public virtual string ReplaceAll(string text, string prefix = "", string postfix = "", params string[] sourceNames)
      {
         string result = text;

         if (IsLoaded)
         {
            if (string.IsNullOrEmpty(text))
            {
               _logger.LogWarning("Parameter 'text' is null or empty! 'ReplaceAll()' will return an empty string.");

               result = string.Empty;
            }
            else
            {
               #region DEBUG

               if (Config.DEBUG_DOMAINS)
               {
                  if (sourceNames == null || sourceNames.Length == 0)
                  {
                     foreach (List<Regex> domainRegexes in _debugDomainsRegex.Values)
                     {
                        foreach (Regex domainRegex in domainRegexes)
                        {
                           MatchCollection matches = domainRegex.Matches(text);

                           foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                           {
                              _logger.LogDebug($"Test string contains a domain: '{capture.Value}' detected by regex '{domainRegex}'");

                              result = result.Replace(capture.Value, prefix + StringHelper.CreateString(capture.Value.Length, ReplaceCharacters) + postfix);
                           }
                        }
                     }
                  }
                  else
                  {
                     foreach (string domainResource in sourceNames)
                     {
                        if (_debugDomainsRegex.TryGetValue(domainResource, out List<Regex> domainRegexes))
                        {
                           foreach (Regex domainRegex in domainRegexes)
                           {
                              MatchCollection matches = domainRegex.Matches(text);

                              foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
                              {
                                 _logger.LogDebug($"Test string contains a domain: '{capture.Value}' detected by regex '{domainRegex}'' from source '{domainResource}'");

                                 result = result.Replace(capture.Value, prefix + StringHelper.CreateString(capture.Value.Length, ReplaceCharacters) + postfix);
                              }
                           }
                        }
                        else
                        {
                           logSourceNotFound(domainResource);
                        }
                     }
                  }
               }

               #endregion

               else
               {
                  if (sourceNames == null || sourceNames.Length == 0)
                  {
                     result = (from domainRegex in _domainsRegex.Values from Match match in domainRegex.Matches(text) from Capture capture in match.Captures select capture).Aggregate(result, (current, capture) => current.Replace(capture.Value, prefix + StringHelper.CreateString(capture.Value.Length, ReplaceCharacters) + postfix));
                  }
                  else
                  {
                     foreach (string domainResource in sourceNames)
                     {
                        if (_domainsRegex.TryGetValue(domainResource, out Regex domainRegex))
                        {
                           MatchCollection matches = domainRegex.Matches(text);

                           result = (from Match match in matches from Capture capture in match.Captures select capture).Aggregate(result, (current, capture) => current.Replace(capture.Value, prefix + StringHelper.CreateString(capture.Value.Length, ReplaceCharacters) + postfix));
                        }
                        else
                        {
                           logSourceNotFound(domainResource);
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

      #endregion

      #region Private methods

      private void process(Dictionary<string, string[]> dataDict)
      {
         if (Config.DEBUG_DOMAINS)
            _logger.LogDebug("++ DomainFilter started in debug-mode ++");

         foreach (var kvp in dataDict)
         {
            string source = kvp.Key;
            string[] domains = kvp.Value;

            if (Config.DEBUG_DOMAINS)
            {
               try
               {
                  List<Regex> domainRegexes = new(domains.Length);
                  domainRegexes.AddRange(domains.Select(line => new Regex(DOMAIN_REGEX_START + line + DOMAIN_REGEX_END, REGEX_IC | REGEX_CI | REGEX_COMPILED)));

                  if (!_debugDomainsRegex.ContainsKey(source))
                     _debugDomainsRegex.Add(source, domainRegexes);
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
                  if (!_domainsRegex.ContainsKey(source))
                     _domainsRegex.Add(source, new Regex($"{DOMAIN_REGEX_START}({string.Join("|", domains.ToArray())}){DOMAIN_REGEX_END}", REGEX_IC | REGEX_CI | REGEX_COMPILED));
               }
               catch (System.Exception ex)
               {
                  _logger.LogError(ex, $"Could not generate exact regex for source '{source}'");
               }
            }

            if (Config.DEBUG_DOMAINS)
               _logger.LogDebug($"Domain resource '{source}' loaded and {domains.Length} entries found.");
         }

         IsLoaded = true;
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
}