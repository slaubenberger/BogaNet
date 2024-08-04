using System.Text.RegularExpressions;

namespace BogaNet.BWF.Filter;

public interface ICapitalizationFilter : IFilter
{
   #region Properties

   /// <summary>RegEx to find excessive capitalization.</summary>
   public Regex RegularExpression { get; set; }

   /// <summary>Defines the number of allowed capital letters in a row.</summary>
   public int CharacterNumber { get; set; }

   #endregion
}