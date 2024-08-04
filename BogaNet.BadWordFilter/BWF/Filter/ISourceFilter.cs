using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BogaNet.BWF.Filter
{
   /// <summary>Interface for all source-based filters.</summary>
   public interface ISourceFilter : IFilter
   {
      #region Properties

      /// <summary>
      /// Current count of sources from the filter.
      /// </summary>
      int Count { get; }

      /// <summary>All source names of the current filter.</summary>
      /// <returns>List with all source names of the current filter</returns>
      List<string> SourceNames { get; }
/*
      /// <summary>Checks the readiness status of the current filter.</summary>
      /// <returns>True if the filter is ready.</returns>
      bool IsReady { get; }
*/

      /// <summary>
      /// Is the filter loaded?
      /// </summary>
      bool IsLoaded { get; }

      #endregion

      #region Events

      /// <summary>
      /// Delegate for the load status of the files.
      /// </summary>
      delegate void FilesLoaded(params Tuple<string, string>[] files);

      /// <summary>
      /// Event triggered whenever the files are loaded.
      /// </summary>
      event FilesLoaded OnFilesLoaded;

      #endregion

      #region Methods

      /// <summary>
      /// Removes a source and assigned data.
      /// </summary>
      /// <param name="srcName">Source name to remove</param>
      ///<returns>True if the operation was successful</returns>
      /// <exception cref="ArgumentNullException"></exception>
      bool Remove(string srcName);

      /// <summary>
      /// Checks if a given source name exists in the filter.
      /// </summary>
      /// <param name="srcName">Source name to check</param>
      /// <returns>True if the key exists in the filter</returns>
      /// <exception cref="ArgumentNullException"></exception>
      bool ContainsSource(string srcName);

      /// <summary>
      /// Clears all sources.
      /// </summary>
      void Clear();


      #endregion
   }
}