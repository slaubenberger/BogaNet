using System.Linq;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using BogaNet.Helper;
using BogaNet.Util;

namespace BogaNet.BWF.Filter
{
   /// <summary>Filter for excessive capitalization. The class can also replace all capitalizations inside a string.</summary>
   public class CapitalizationFilter : Singleton<CapitalizationFilter>, ICapitalizationFilter
   {
      #region Variables

      private static readonly ILogger<CapitalizationFilter> _logger = GlobalLogging.CreateLogger<CapitalizationFilter>();

      private int _characterNumber;

      #endregion


      #region Properties

      public virtual Regex RegularExpression { get; set; }

      public virtual int CharacterNumber
      {
         get => _characterNumber;
         set
         {
            _characterNumber = value < 1 ? 1 : value;

            RegularExpression = new Regex($@"\b\w*[A-ZÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝ]{{{_characterNumber + 1},}}\w*\b", RegexOptions.CultureInvariant);
         }
      }

      #endregion


      #region Constructor

      private CapitalizationFilter()
      {
         CharacterNumber = 3;
      }

      #endregion


      #region Implemented methods

      public virtual bool Contains(string text, params string[] sourceNames) //sources are ignored
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

      public virtual List<string> GetAll(string text, params string[] sourceNames) //sources are ignored
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
               _logger.LogDebug($"Test string contains an excessive capital word: '{capture.Value}'");

               if (!result.Contains(capture.Value))
                  result.Add(capture.Value);
            }
         }

         return result.Distinct().OrderBy(x => x).ToList();
      }

      public virtual string ReplaceAll(string text, string prefix = "", string postfix = "", params string[] sourceNames) //sources are ignored
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
               _logger.LogDebug($"Test string contains an excessive capital word: '{capture.Value}'");

               result = result.Replace(capture.Value, $"{prefix}{StringHelper.ToTitleCase(capture.Value)}{postfix}");
            }
         }

         return result;
      }

      #endregion
   }
}