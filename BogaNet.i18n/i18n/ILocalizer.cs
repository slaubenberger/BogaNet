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
   #region Properties

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
   /// Is the localizer loaded?
   /// </summary>
   bool IsLoaded { get; }

   /// <summary>
   /// Current keys of the localizer.
   /// </summary>
   List<string> Keys { get; }

   /// <summary>
   /// Current count of keys from the localizer.
   /// </summary>
   int Count { get; }

   #endregion

   #region Events

   /// <summary>
   /// Delegate for culture changes.
   /// </summary>
   delegate void CultureChanged(CultureInfo lang);

   /// <summary>
   /// Event triggered whenever the culture changes.
   /// </summary>
   event CultureChanged OnCultureChanged;

   /// <summary>
   /// Delegate for the load status of the files.
   /// </summary>
   delegate void FilesLoaded(params string[] files);

   /// <summary>
   /// Event triggered whenever the files are loaded.
   /// </summary>
   event FilesLoaded OnFilesLoaded;

   /// <summary>
   /// Delegate for the save status of the file.
   /// </summary>
   delegate void FileSaved(string file);

   /// <summary>
   /// Event triggered whenever the file is saved.
   /// </summary>
   event FileSaved OnFileSaved;

   #endregion

   #region Methods

   /// <summary>
   /// Gets the text for a key and the current culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <returns>Text for the key</returns>
   /// <exception cref="ArgumentNullException"></exception>
   string GetText(string key, TextType textType = TextType.LABEL);

   /// <summary>
   /// Tries to get the text for a key and the current culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool TryGetText(string key, out string result, TextType textType = TextType.LABEL);

   /// <summary>
   /// Gets the text for a key and a given culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="culture">Culture for the text</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <returns>Text for the key</returns>
   /// <exception cref="ArgumentNullException"></exception>
   string GetText(string key, CultureInfo culture, TextType textType = TextType.LABEL);

   /// <summary>
   /// Tries to get the text for a key and a given culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="culture">Culture for the text</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool TryGetText(string key, out string result, CultureInfo culture, TextType textType = TextType.LABEL);

   /// <summary>
   /// Gets the text for a key with replacements (for placeholders like '{0}') and the current culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <param name="replacements">Replacements for the text</param>
   /// <returns>Text with replacements for the key</returns>
   /// <exception cref="ArgumentNullException"></exception>
   string GetTextWithReplacements(string key, TextType textType = TextType.LABEL, params string[] replacements);

   /// <summary>
   /// Tries to get the text for a key with replacements (for placeholders like '{0}') and the current culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <param name="replacements">Replacements for the text</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool TryGetTextWithReplacements(string key, out string result, TextType textType = TextType.LABEL, params string[] replacements);

   /// <summary>
   /// Gets the text for a key with replacements (for placeholders like '{0}') and a given culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="culture">Culture for the text</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <param name="replacements">Replacements for the text</param>
   /// <returns>Text with replacements for the key</returns>
   /// <exception cref="ArgumentNullException"></exception>
   string GetTextWithReplacements(string key, CultureInfo culture, TextType textType = TextType.LABEL, params string[] replacements);

   /// <summary>
   /// Tries to get the text for a key with replacements (for placeholders like '{0}') and a given culture.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="culture">Culture for the text</param>
   /// <param name="textType">Type of the text (optional, default: LABEL)</param>
   /// <param name="replacements">Replacements for the text</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool TryGetTextWithReplacements(string key, out string result, CultureInfo culture, TextType textType = TextType.LABEL, params string[] replacements);

   /// <summary>
   /// Checks if a given key exists in the localizer.
   /// </summary>
   /// <param name="key">Key to check</param>
   /// <param name="culture">Culture of the key (optional, default: any)</param>
   /// <returns>True if the key exists in the localizer</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public bool ContainsKey(string key, CultureInfo? culture = null);

   /// <summary>
   /// Adds a translated text.
   /// </summary>
   /// <param name="key">Key for the text</param>
   /// <param name="culture">Culture for the text</param>
   /// <param name="value">Value of the text</param>
   /// <exception cref="ArgumentNullException"></exception>
   void Add(string key, CultureInfo culture, string value);

   /// <summary>
   /// Removes a key and assigned translated texts.
   /// </summary>
   /// <param name="key">Key to remove</param>
   /// <param name="culture">Culture of the key (optional, default: all)</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool Remove(string key, CultureInfo? culture = null);

   /// <summary>
   /// Clears all translations.
   /// </summary>
   void Clear();

   /// <summary>
   /// Loads translations from a given Dictionary.
   /// </summary>
   /// <param name="dataDict">Dictionary to load</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   void Load(Dictionary<string, string[]> dataDict);

   /// <summary>
   /// Load translation files (CSV) from a given path.
   /// </summary>
   /// <param name="files">Files to load</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   bool LoadFiles(params string[] files);

   /// <summary>
   /// Load translation files (CSV) from a given path asynchronously.
   /// </summary>
   /// <param name="files">Files to load</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   Task<bool> LoadFilesAsync(params string[] files);

   /// <summary>
   /// Load translation files (CSV) from given URLs.
   /// </summary>
   /// <param name="urls">URLs of files to load</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   bool LoadFilesFromUrl(params string[] urls);

   /// <summary>
   /// Load translation files (CSV) from given URLs asynchronously.
   /// </summary>
   /// <param name="urls">URLs of files to load</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   Task<bool> LoadFilesFromUrlAsync(params string[] urls);

   /// <summary>
   /// Saves all translations to a given file (CSV).
   /// </summary>
   /// <param name="filename">File for the translations</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   bool SaveFile(string filename);

   /// <summary>
   /// Saves all translations to a given file (CSV) asynchronously.
   /// </summary>
   /// <param name="filename">File for the translations</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   Task<bool> SaveFileAsync(string filename);

   #endregion
}