using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using BogaNet.Helper;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BogaNet.BWF;
using BogaNet.Util;

namespace BogaNet.BWF.Filter
{
   /// <summary>Filter for domains. The class can also replace all domains inside a string.</summary>
   public class DomainFilter : Singleton<DomainFilter>, IDomainFilter
   {
      #region Variables

      private static readonly ILogger<DomainFilter> _logger = GlobalLogging.CreateLogger<DomainFilter>();

      private const string DOMAIN_REGEX_START = @"\b{0,1}((ht|f)tp(s?)\:\/\/)?[\w\-\.\@]*[\.]";

      //private const string domainRegexEnd = @"(:\d{1,5})?(\/|\b)([\a-zA-Z0-9\-\.\?\!\,\=\'\/\&\%#_]*)?\b";
      private const string DOMAIN_REGEX_END = @"(:\d{1,5})?(\/|\b)";

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

      private readonly Dictionary<string, Regex> _domainsRegex = new Dictionary<string, Regex>();
      private readonly Dictionary<string, List<Regex>> _debugDomainsRegex = new Dictionary<string, List<Regex>>();

      #endregion


      #region Properties

      public int Count { get; }
      public List<string> SourceNames { get; }
      public bool IsLoaded { get; }

      #endregion


      #region Constructor

      private DomainFilter()
      {
      }

      #endregion


      #region Implemented methods

      public event ISourceFilter.FilesLoaded? OnFilesLoaded;

      public bool Remove(string srcName)
      {
         throw new NotImplementedException();
      }

      public bool ContainsSource(string srcName)
      {
         throw new NotImplementedException();
      }

      public void Clear()
      {
         throw new NotImplementedException();
      }

      public void Load(Dictionary<string, List<string>> dataDict)
      {
         throw new NotImplementedException();
      }

      public bool LoadFiles(params Tuple<string, string>[] files)
      {
         throw new NotImplementedException();
      }

      public Task<bool> LoadFilesAsync(params Tuple<string, string>[] files)
      {
         throw new NotImplementedException();
      }

      public bool LoadFilesFromUrl(params Tuple<string, string>[] urls)
      {
         throw new NotImplementedException();
      }

      public Task<bool> LoadFilesFromUrlAsync(params Tuple<string, string>[] urls)
      {
         throw new NotImplementedException();
      }

      public char[] ReplaceCharacters { get; set; }

      public void Add(string srcName, List<string> regexes)
      {
         throw new NotImplementedException();
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
                           logResourceNotFound(domainResource);
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
                           logResourceNotFound(domainResource);
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
                           logResourceNotFound(domainResource);
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
                           logResourceNotFound(domainResource);
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

      public string ReplaceAll(string text, string prefix = "", string postfix = "", params string[] sourceNames)
      {
         string result = text;

         if (IsLoaded)
         {
            if (string.IsNullOrEmpty(text))
            {
               logReplaceAll();

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
                           logResourceNotFound(domainResource);
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
                           logResourceNotFound(domainResource);
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

      private void process(Dictionary<string, List<string>> dataDict)
      {
         if (Config.DEBUG_DOMAINS)
            _logger.LogDebug("++ DomainFilter started in debug-mode ++");

         foreach (var kvp in dataDict)
         {
            string source = kvp.Key;
            List<string> domains = kvp.Value;

            if (Config.DEBUG_DOMAINS)
            {
               try
               {
                  List<Regex> domainRegexes = new List<Regex>(domains.Count);
                  domainRegexes.AddRange(domains.Select(line => new Regex(DOMAIN_REGEX_START + line + DOMAIN_REGEX_END, RegexOption1 | RegexOption2 | RegexOption3 | RegexOption4 | RegexOption5)));

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
                     _domainsRegex.Add(source, new Regex($"{DOMAIN_REGEX_START}({string.Join("|", domains.ToArray())}){DOMAIN_REGEX_END}", RegexOption1 | RegexOption2 | RegexOption3 | RegexOption4 | RegexOption5));
               }
               catch (System.Exception ex)
               {
                  _logger.LogError(ex, $"Could not generate exact regex for source '{source}'");
               }
            }

            if (Config.DEBUG_DOMAINS)
               _logger.LogDebug($"Domain resource '{source}' loaded and {domains.Count} entries found.");
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