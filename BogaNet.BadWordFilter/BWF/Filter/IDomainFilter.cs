using System.Collections.Generic;

namespace BogaNet.BWF.Filter;

public interface IDomainFilter : ISourceFilter
{
   #region Properties

   /// <summary>Replace characters for domains.</summary>
   char[] ReplaceCharacters { get; set; }

   #endregion

   #region Methods

   void Add(string srcName, List<string> regexes);

   #endregion
}