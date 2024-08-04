using System.Linq;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using BogaNet.Util;

namespace BogaNet.BWF.Filter
{
   /// <summary>Filter for excessive punctuation. The class can also replace all punctuations inside a string.</summary>
   public class PunctuationFilter : Singleton<PunctuationFilter>, IPunctuationFilter
   {
      #region Variables

      private static readonly ILogger<PunctuationFilter> _logger = GlobalLogging.CreateLogger<PunctuationFilter>();

      private int _characterNumber;
      //protected readonly List<string> _getAllResult = new List<string>();

      #endregion


      #region Properties

      public Regex RegularExpression { get; set; }

      /// <summary>Defines the number of allowed punctuations in a row.</summary>
      public int CharacterNumber
      {
         get => _characterNumber;
         set
         {
            _characterNumber = value < 1 ? 1 : value;

            RegularExpression = new Regex($@"[?!,.;:-]{{{_characterNumber + 1},}}", RegexOptions.CultureInvariant);
         }
      }

      #endregion


      #region Constructor

      private PunctuationFilter()
      {
         CharacterNumber = 3;
      }

      #endregion


      #region Implemented methods

      public bool Contains(string text, params string[] sourceNames) //sources are ignored
      {
         bool result = false;

         if (string.IsNullOrEmpty(text))
         {
            _logger.LogWarning("Parameter 'text' is null or empty! 'Contains()' will return 'false'.");
         }
         else
         {
            result = RegularExpression.Match(text).Success;
         }

         return result;
      }

      public List<string> GetAll(string text, params string[] sourceNames) //sources are ignored
      {
         List<string> result = new();

         if (string.IsNullOrEmpty(text))
         {
            _logger.LogWarning("Parameter 'text' is null or empty! 'GetAll()' will return an empty list.");
         }
         else
         {
            MatchCollection matches = RegularExpression.Matches(text);

            foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
            {
               _logger.LogDebug($"Test string contains an excessive punctuation: '{capture.Value}'");

               if (!result.Contains(capture.Value))
               {
                  result.Add(capture.Value);
               }
            }
         }

         return result.Distinct().OrderBy(x => x).ToList();
      }

      public string ReplaceAll(string text, string prefix = "", string postfix = "", params string[] sourceNames) //sources are ignored
      {
         string result = text;

         if (string.IsNullOrEmpty(text))
         {
            _logger.LogWarning("Parameter 'text' is null or empty! 'ReplaceAll()' will return an empty string.");

            result = string.Empty;
         }
         else
         {
            MatchCollection matches = RegularExpression.Matches(text);

            foreach (Capture capture in from Match match in matches from Capture capture in match.Captures select capture)
            {
               _logger.LogDebug($"Test string contains an excessive punctuation: '{capture.Value}'");

               result = result.Replace(capture.Value, prefix + capture.Value.Substring(0, _characterNumber) + postfix);
            }
         }

         return result;
      }

      #endregion
   }
}