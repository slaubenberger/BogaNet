using System.Collections.Generic;
using BogaNet.BWF.Enum;

namespace BogaNet.BWF.Filter
{
   /// <summary>Interface for bad word filters.</summary>
   public interface IBadWordFilter : ISourceFilter
   {
      #region Properties

      /// <summary>Replace characters for bad words.</summary>
      char[] ReplaceCharacters { get; set; }

      /// <summary>Replace mode operations on the input string.</summary>
      ReplaceMode Mode { get; set; }

      /// <summary>Remove unnecessary spaces between letters in the input string.</summary>
      bool RemoveSpaces { get; set; }

      /// <summary>Maximal text length for the space detection.</summary>
      int MaxTextLength { get; set; }

      /// <summary>Remove unnecessary characters from the input string.</summary>
      string RemoveCharacters { get; set; }

      /// <summary>Use simple detection algorithm (e.g. for Chinese).</summary>
      bool SimpleCheck { get; set; }

      #endregion

      #region Methods

      void Add(string srcName, List<string> regexes, bool isLTR = true);

      #endregion
   }
}