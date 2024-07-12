using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace BogaNet.i18n;

/// <summary>
/// Interface for localizers of the application.
/// </summary>
public interface ILocalizer
{
   /// <summary>
   /// Current culture.
   /// </summary>
   CultureInfo Culture { get; set; }

   /// <summary>
   /// Supported cultures.
   /// </summary>
   List<CultureInfo> SupportedCultures { get; }

   /// <summary>
   /// List of missing translations.
   /// </summary>
   List<string> MissingTranslations { get; }

   /// <summary>
   /// List of missing country codes.
   /// </summary>
   List<string> MissingCountries { get; }

   /// <summary>
   /// List of removed translations.
   /// </summary>
   List<string> RemovedTranslations { get; }

   /// <summary>
   /// List of added translations.
   /// </summary>
   List<string> AddedTranslations { get; }

   /// <summary>
   /// Delegate for culture changes.
   /// </summary>
   delegate void CultureChange(CultureInfo lang);

   /// <summary>
   /// Event triggered whenever the culture changes.
   /// </summary>
   event CultureChange OnCultureChange;

   /// <summary>
   /// Gets the text for a key and the current culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <returns>Text for the key</returns>
   string? GetText(string key, TextType textType = TextType.LABEL);

   /// <summary>
   /// Gets the text for a key and a given culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="culture">Culture for the text</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <returns>Text for the key</returns>
   string? GetText(string key, CultureInfo culture, TextType textType = TextType.LABEL);

   /// <summary>
   /// Gets the text for a key with replacements (for placeholders like '{0}') and the current culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <param name="replacements">Replacements for the text</param>
   /// <returns>Text with replacements for the key</returns>
   string? GetTextWithReplacements(string key, TextType textType = TextType.LABEL, params string[] replacements);

   /// <summary>
   /// Gets the text for a key with replacements (for placeholders like '{0}') and a given culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="culture">Culture for the text</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <param name="replacements">Replacements for the text</param>
   /// <returns>Text with replacements for the key</returns>
   string? GetTextWithReplacements(string key, CultureInfo culture, TextType textType = TextType.LABEL, params string[] replacements);

   //void Load(Dictionary<string, string[]> dataDict);

   /// <summary>
   /// Checks if a given key exists in the localizer.
   /// </summary>
   /// <param name="key">Key to check</param>
   /// <returns>True if the key exists in the localizer</returns>
   public bool ContainsKey(string key);

   /// <summary>
   /// Adds a translated text.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="culture">Culture for the text</param>
   /// <param name="value">Value of the text</param>
   void Add(string key, CultureInfo culture, string value);

   /// <summary>
   /// Removes a key and all assigned translated texts.
   /// </summary>
   /// <param name="key">Key to remove</param>
   void Remove(string key);

   /// <summary>
   /// Clears all translations.
   /// </summary>
   void Clear();

   /// <summary>
   /// Load translation files (CSV) from a given path.
   /// </summary>
   /// <param name="files">Files to load</param>
   /// <exception cref="Exception"></exception>
   void LoadFiles(params string[] files);

   /// <summary>
   /// Load translation files (CSV) from a given path asynchronously.
   /// </summary>
   /// <param name="files">Files to load</param>
   /// <exception cref="Exception"></exception>
   Task LoadFilesAsync(params string[] files);

   /// <summary>
   /// Load translation files (CSV) from given URLs.
   /// </summary>
   /// <param name="urls">URLs of files to load</param>
   /// <exception cref="Exception"></exception>
   void LoadFilesFromUrl(params string[] urls);

   /// <summary>
   /// Load translation files (CSV) from given URLs asynchronously.
   /// </summary>
   /// <param name="urls">URLs of files to load</param>
   /// <exception cref="Exception"></exception>
   Task LoadFilesFromUrlAsync(params string[] urls);
   
   /// <summary>
   /// Saves all translations to a given file (CSV).
   /// </summary>
   /// <param name="filename">File for the translations</param>
   /// <exception cref="Exception"></exception>
   void SaveFile(string filename);

   /// <summary>
   /// Saves all translations to a given file (CSV) asynchronously.
   /// </summary>
   /// <param name="filename">File for the translations</param>
   /// <exception cref="Exception"></exception>
   Task SaveFileAsync(string filename);
}