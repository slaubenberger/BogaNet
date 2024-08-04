using System.Collections.Generic;

namespace BogaNet.BWF.Filter
{
   /// <summary>Interface for all filters.</summary>
   public interface IFilter
   {
      #region Methods

      /// <summary>Searches for bad words in a text.</summary>
      /// <param name="text">Text to check</param>
      /// <param name="sourceNames">Relevant sources (e.g. "english", optional)</param>
      /// <returns>True if a match was found</returns>
      bool Contains(string text, params string[] sourceNames);

      /// <summary>Searches for bad words in a text.</summary>
      /// <param name="text">Text to check</param>
      /// <param name="sourceNames">Relevant sources (e.g. "english", optional)</param>
      /// <returns>List with all the matches</returns>
      List<string> GetAll(string text, params string[] sourceNames);

      /// <summary>Searches and replaces all bad words in a text.</summary>
      /// <param name="text">Text to check</param>
      /// <param name="prefix">Prefix for every found bad word (optional)</param>
      /// <param name="postfix">Postfix for every found bad word (optional)</param>
      /// <param name="sourceNames">Relevant sources (e.g. "english", optional)</param>
      /// <returns>Clean text</returns>
      string ReplaceAll(string text, string prefix = "", string postfix = "", params string[] sourceNames);

      #endregion
   }
}