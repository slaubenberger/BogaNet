using System.Text.RegularExpressions;

namespace BogaNet.BWF.Filter;

public interface IPunctuationFilter : IFilter
{
   #region Properties

   /// <summary>RegEx to find excessive punctuation.</summary>
   public Regex RegularExpression { get; set; }

   /// <summary>Defines the number of allowed punctuations in a row.</summary>
   public int CharacterNumber { get; set; }

   #endregion
}