using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BogaNet.BWF.Filter;

public interface IDomainFilter : ISourceFilter
{
   #region Properties

   /// <summary>Replace characters for domains.</summary>
   char[] ReplaceCharacters { get; set; }

   #endregion

   #region Methods

   /// <summary>
   /// Load sources from a given Dictionary.
   /// </summary>
   /// <param name="dataDict">Dictionary to load</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   void Load(Dictionary<string, string[]> dataDict);

   /// <summary>
   /// Load source files (CSV) from a given path.
   /// </summary>
   /// <param name="files">Files to load (Item1 = source name, Item2 = file)</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   bool LoadFiles(params Tuple<string, string>[] files);

   /// <summary>
   /// Load source files (CSV) from a given path asynchronously.
   /// </summary>
   /// <param name="files">Files to load (Item1 = source name, Item2 = file)</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   Task<bool> LoadFilesAsync(params Tuple<string, string>[] files);

   /// <summary>
   /// Load source files (CSV) from given URLs.
   /// </summary>
   /// <param name="urls">URLs of files to load (Item1 = source name, Item2 = file)</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   bool LoadFilesFromUrl(params Tuple<string, string>[] urls);

   /// <summary>
   /// Load source files (CSV) from given URLs asynchronously.
   /// </summary>
   /// <param name="urls">URLs of files to load (Item1 = source name, Item2 = file)</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   Task<bool> LoadFilesFromUrlAsync(params Tuple<string, string>[] urls);

   /// <summary>
   /// Adds a source with domains.
   /// </summary>
   /// <param name="srcName">Source name</param>
   /// <param name="domains">Domains for the source</param>
   /// <exception cref="ArgumentNullException"></exception>
   void Add(string srcName, params string[] domains);

   #endregion
}