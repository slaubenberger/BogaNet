using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BogaNet.BWF.Enum;

namespace BogaNet.BWF.Filter;

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

   /// <summary>Use simple detection algorithm (e.g. for Chinese, Japanese, Korean, Thai etc.).</summary>
   bool SimpleCheck { get; set; }

   #endregion

   #region Methods

   /// <summary>
   /// Load sources from a given Dictionary.
   /// </summary>
   /// <param name="isLTR">Is source written left-to-right?</param>
   /// <param name="dataDict">Dictionary to load</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   void Load(bool isLTR, Dictionary<string, string[]> dataDict);

   /// <summary>
   /// Load source files (CSV) from a given path.
   /// </summary>
   /// <param name="isLTR">Is source written left-to-right?</param>
   /// <param name="files">Files to load (Item1 = source name, Item2 = file)</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   bool LoadFiles(bool isLTR, params Tuple<string, string>[] files);

   /// <summary>
   /// Load source files (CSV) from a given path asynchronously.
   /// </summary>
   /// <param name="isLTR">Is source written left-to-right?</param>
   /// <param name="files">Files to load (Item1 = source name, Item2 = file)</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   Task<bool> LoadFilesAsync(bool isLTR, params Tuple<string, string>[] files);

   /// <summary>
   /// Load source files (CSV) from given URLs.
   /// </summary>
   /// <param name="isLTR">Is source written left-to-right?</param>
   /// <param name="urls">URLs of files to load (Item1 = source name, Item2 = file)</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   bool LoadFilesFromUrl(bool isLTR, params Tuple<string, string>[] urls);

   /// <summary>
   /// Load source files (CSV) from given URLs asynchronously.
   /// </summary>
   /// <param name="isLTR">Is source written left-to-right?</param>
   /// <param name="urls">URLs of files to load (Item1 = source name, Item2 = file)</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   Task<bool> LoadFilesFromUrlAsync(bool isLTR, params Tuple<string, string>[] urls);

   /// <summary>
   /// Adds a source with words.
   /// </summary>
   /// <param name="isLTR">Is source written left-to-right?</param>
   /// <param name="srcName">Source name</param>
   /// <param name="words">Words for the source</param>
   /// <exception cref="ArgumentNullException"></exception>
   void Add(bool isLTR, string srcName, params string[] words);

   #endregion
}