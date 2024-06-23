using System.Collections.Generic;
using BogaNet.Avalonia.Helper;
using BogaNet.i18n;
using System;
using BogaNet.Helper;

namespace BogaNet.Avalonia;

/// <summary>
/// Extension methods for Avalonia.
/// </summary>
public static class AvaloniaExtension
{
   /// <summary>
   /// Load translation files (CSV) as resources for Localizer.
   /// </summary>
   /// <param name="localizer">Localizer-instance</param>
   /// <param name="translationFiles">Files to load</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void LoadResources(this Localizer? localizer, params string[] translationFiles)
   {
      if (localizer == null)
         throw new ArgumentNullException(nameof(localizer));

      Dictionary<string, string[]> allLines = new();

      foreach (var translation in translationFiles)
      {
         var contents = StringHelper.SplitToLines(ResourceHelper.LoadText(translation)).ToArray();

         allLines.Add(translation, contents);
      }

      localizer.Load(allLines);
   }
}