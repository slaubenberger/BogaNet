﻿using System.Globalization;
using Microsoft.Extensions.Logging;
using BogaNet.Helper;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using BogaNet.Util;
using System.Text.RegularExpressions;
using BogaNet.Extension;

namespace BogaNet.i18n;

/// <summary>
/// i18n localizer for the application.
/// </summary>
public partial class Localizer : Singleton<Localizer>, ILocalizer //NUnit
{
   #region Variables

   private static readonly ILogger<Localizer> _logger = GlobalLogging.CreateLogger<Localizer>();

   protected CultureInfo _culture = Constants.CurrentCulture;
   protected readonly List<CultureInfo> _cultures = [];
   protected readonly Dictionary<string, Dictionary<string, string>> _messages = new();
   private static readonly Regex _splitRegex = csvRegex();

   #endregion

   #region Properties

   public virtual CultureInfo Culture
   {
      get => _culture;

      set
      {
         if (!Equals(value, _culture))
         {
            if (!SupportedCultures.Contains(value))
               _logger.LogWarning($"No supported culture for {Culture} found!");

            _culture = value;
            OnCultureChange?.Invoke(_culture);
         }
      }
   }

   public virtual List<CultureInfo> SupportedCultures
   {
      get
      {
         if (_cultures.Count == 0)
         {
            List<string> culturesString = [];

            foreach (KeyValuePair<string, string> cultureKvp in _messages.SelectMany(translationKvp => translationKvp.Value.Where(cultureKvp => !culturesString.Contains(cultureKvp.Key))))
            {
               culturesString.Add(cultureKvp.Key);
            }

            foreach (var culture in culturesString)
            {
               _cultures.Add(new CultureInfo(culture));
            }
         }

         return _cultures;
      }
   }

   public virtual List<string> MissingTranslations { get; } = [];
   public virtual List<string> MissingCountries { get; } = [];
   public virtual List<string> RemovedTranslations { get; } = [];
   public virtual List<string> AddedTranslations { get; } = [];

   #endregion

   #region Events

   public event ILocalizer.CultureChange? OnCultureChange;

   #endregion

   #region Constructor

   private Localizer()
   {
   }

   #endregion

   #region Public methods

   public virtual string? GetText(string? key, TextType textType = TextType.LABEL) //NUnit
   {
      return GetText(key, Culture, textType);
   }

   public virtual string? GetText(string? key, CultureInfo culture, TextType textType = TextType.LABEL) //NUnit
   {
      return getText(key, culture.ToString(), textType);
   }

   public virtual string? GetTextWithReplacements(string? key, TextType textType = TextType.LABEL, params string[] replacements) //NUnit
   {
      return GetTextWithReplacements(key, Culture, textType, replacements);
   }

   public virtual string? GetTextWithReplacements(string? key, CultureInfo culture, TextType textType = TextType.LABEL, params string[] replacements) //NUnit
   {
      ArgumentNullException.ThrowIfNull(key);
      ArgumentNullException.ThrowIfNull(replacements);

      string? text = GetText(key, culture, textType);

      for (int ii = 0; ii < replacements.Length; ii++)
      {
         string replacement = "{" + ii + "}";
         text = text?.BNReplace(replacement, replacements[ii]);
      }

      return text;
   }

   public bool ContainsKey(string key)
   {
      return _messages.ContainsKey(key);
   }

   public virtual void Add(string key, CultureInfo culture, string value)
   {
      string lang = culture.ToString();

      if (_messages.TryGetValue(key, out Dictionary<string, string>? message))
      {
         message[lang] = value;
      }
      else
      {
         Dictionary<string, string> dict = new() { { lang, value } };
         _messages.Add(key, dict);
      }

      string id = $"{key},{culture},\"{value}\"";

      if (!AddedTranslations.Contains(id))
         AddedTranslations.Add(id);

      hasChanged();
   }

   public virtual void Remove(string key)
   {
      _messages.Remove(key);

      if (!RemovedTranslations.Contains(key))
         RemovedTranslations.Add(key);

      hasChanged();
   }

   public virtual void Clear()
   {
      _messages.Clear();
      MissingTranslations.Clear();
      MissingCountries.Clear();
      RemovedTranslations.Clear();
      AddedTranslations.Clear();
      hasChanged();
   }

   public virtual void Load(Dictionary<string, string[]> dataDict)
   {
      foreach (var kvp in dataDict)
      {
         var lines = kvp.Value;

         if (lines.Length > 1)
         {
            string[] columns = _splitRegex.Split(lines[0]);
            CultureInfo? language;
            Dictionary<int, CultureInfo> supportedCultures = new();

            for (int ii = 1; ii < columns.Length; ii++)
            {
               language = new CultureInfo(columns[ii]);

               if (!supportedCultures.ContainsValue(language))
                  supportedCultures.Add(ii, language);
            }

            // process all messages and add them to the corresponding languages
            for (int ii = 1; ii < lines.Length; ii++)
            {
               string[] cols = _splitRegex.Split(lines[ii]);

               string? key = null;
               Dictionary<string, string>? translation = null;

               for (int yy = 0; yy < cols.Length; yy++)
               {
                  if (0 == yy)
                  {
                     key = cols[yy];
                     translation = new Dictionary<string, string>();
                  }
                  else
                  {
                     if (supportedCultures.TryGetValue(yy, out language))
                     {
                        translation?.Add(language.ToString(), cols[yy]?.Trim('"') ?? string.Empty);
                     }
                     else
                     {
                        _logger.LogWarning($"Language index not supported for key: {key} (line: {ii + 1}) - {yy} - {kvp}");
                     }
                  }
               }

               if (translation != null && key != null)
               {
                  if (_messages.TryGetValue(key, out Dictionary<string, string>? value))
                  {
                     foreach (var translationKvp in translation)
                     {
                        if (value.ContainsKey(translationKvp.Key))
                        {
                           _logger.LogInformation($"Duplicate key '{key}' for language '{translationKvp.Key}' found: {value[translationKvp.Key]} => {translationKvp.Value}");
                           value[translationKvp.Key] = translationKvp.Value;
                        }
                        else
                        {
                           value.Add(translationKvp.Key, translationKvp.Value);
                        }
                     }
                  }
                  else
                  {
                     _messages.Add(key, translation);
                  }
               }
            }
         }
         else
         {
            _logger.LogWarning($"No messages found: {kvp}");
         }
      }

      hasChanged();
   }

   public virtual void LoadFiles(params string[] files)
   {
      Task.Run(() => LoadFilesAsync(files)).GetAwaiter().GetResult();
   }

   public virtual async Task LoadFilesAsync(params string[] files)
   {
      Dictionary<string, string[]> allLines = new();

      foreach (string currentTranslation in files)
      {
         string[]? lines = await FileHelper.ReadAllLinesAsync(currentTranslation);

         if (lines != null && lines.Length > 1)
            allLines.Add(currentTranslation, lines);
      }

      Load(allLines);
   }

   public virtual void SaveFile(string filename)
   {
      Task.Run(() => SaveFileAsync(filename)).GetAwaiter().GetResult();
   }

   public virtual async Task SaveFileAsync(string filename)
   {
      string content = getEntries();
      await FileHelper.WriteAllTextAsync(filename, content);
   }

   #endregion

   #region Private methods

   protected string? getText(string? key, string culture, TextType textType, bool returnDefault = true)
   {
      ArgumentNullException.ThrowIfNull(key);

      string usedKey = key;

      switch (textType)
      {
         case TextType.TOOLTIP:
            usedKey += "_Tooltip";
            break;
         case TextType.PLACEHOLDER:
            usedKey += "_Placeholder";
            break;
         case TextType.URL:
            usedKey += "_Url";
            break;
      }

      if (_messages.TryGetValue(usedKey, out Dictionary<string, string>? translation))
      {
         //Dictionary<string, string> dict = languages[language];

         if (translation.TryGetValue(culture, out string? value))
            return value.BNReplace("\\n", System.Environment.NewLine);

         if (culture.Length > 2)
         {
            string newLang = culture[..2];
            _logger.LogDebug($"No translation found for key '{usedKey}' in '{culture}' - try to use '{newLang}'.");

            string id = $"{usedKey},{culture}";

            if (!MissingCountries.Contains(id))
               MissingCountries.Add(id);

            return getText(key, newLang, textType);
         }
         else
         {
            string id = $"{usedKey},{culture}";

            if (!MissingTranslations.Contains(id))
               MissingTranslations.Add(id);

            _logger.LogInformation($"No translation found for key '{usedKey}' in '{culture}' - using the default value.");

            if (returnDefault)
            {
               var defaultValue = _messages[usedKey].Values.ElementAt(0);
               return defaultValue.BNReplace("\\n", System.Environment.NewLine);
            }
         }
      }
      else
      {
         string id = $"{usedKey},{culture}";

         if (!MissingTranslations.Contains(id))
            MissingTranslations.Add(id);

         _logger.LogWarning($"No translation found for key: {usedKey}");
      }

      return "???" + usedKey + "???";
   }

   protected string getEntries()
   {
      StringBuilder sb = new();

      //Header
      sb.Append("key");
      foreach (var language in SupportedCultures)
      {
         sb.Append(',');
         sb.Append(language);
      }

      //Content
      foreach (var translation in _messages)
      {
         sb.Append(Environment.NewLine);
         sb.Append(translation.Key);

         foreach (var language in SupportedCultures)
         {
            //preserve the same order as in the header
            sb.Append(',');
            sb.Append('"');
            sb.Append(getText(translation.Key, language.ToString(), TextType.LABEL, false));
            sb.Append('"');
         }
      }

      return sb.ToString();
   }

   protected void hasChanged()
   {
      _cultures.Clear();
   }

   #endregion

   #region Overridden methods

   public override string ToString()
   {
      return this.BNToString();
   }

    [GeneratedRegex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)")]
    private static partial Regex csvRegex();

    #endregion
}