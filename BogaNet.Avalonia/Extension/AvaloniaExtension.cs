using System.Collections.Generic;
using BogaNet.Helper;
using BogaNet.i18n;
using System;
using BogaNet.BWF.Filter;

namespace BogaNet.Extension;

/// <summary>
/// Extension methods for Avalonia.
/// </summary>
public static class AvaloniaExtension
{
   #region Public methods

   /// <summary>
   /// Load translation files (CSV) as resources for Localizer.
   /// </summary>
   /// <param name="localizer">Localizer-instance</param>
   /// <param name="translationFiles">Files to load</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void LoadResources(this ILocalizer localizer, params string[] translationFiles)
   {
      ArgumentNullException.ThrowIfNull(localizer);
      ArgumentNullException.ThrowIfNull(translationFiles);

      Dictionary<string, string[]> allLines = new();

      foreach (var translation in translationFiles)
      {
         var contents = StringHelper.SplitToLines(ResourceHelper.LoadText(translation)).ToArray();

         allLines.Add(translation, contents);
      }

      localizer.Load(allLines);
   }

   /// <summary>
   /// Load source files as resources for BadWordFilter.
   /// </summary>
   /// <param name="filter">BadWordFilter-instance</param>
   /// <param name="isLTR">Is source written left-to-right?</param>
   /// <param name="files">Files to load (Item1 = source name, Item2 = file)</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void LoadResources(this IBadWordFilter filter, bool isLTR, params Tuple<string, string>[] files)
   {
      ArgumentNullException.ThrowIfNull(filter);
      ArgumentNullException.ThrowIfNull(files);

      Dictionary<string, string[]> allLines = new();

      foreach (var file in files)
      {
         var contents = StringHelper.SplitToLines(ResourceHelper.LoadText(file.Item2)).ToArray();

         allLines.Add(file.Item1, contents);
      }

      filter.Load(isLTR, allLines);
   }

   /// <summary>
   /// Load source files as resources for DomainFilter.
   /// </summary>
   /// <param name="filter">DomainFilter-instance</param>
   /// <param name="files">Files to load (Item1 = source name, Item2 = file)</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void LoadResources(this IDomainFilter filter, params Tuple<string, string>[] files)
   {
      ArgumentNullException.ThrowIfNull(filter);
      ArgumentNullException.ThrowIfNull(files);

      Dictionary<string, string[]> allLines = new();

      foreach (var file in files)
      {
         var contents = StringHelper.SplitToLines(ResourceHelper.LoadText(file.Item2)).ToArray();

         allLines.Add(file.Item1, contents);
      }

      filter.Load(allLines);
   }

   #endregion
}