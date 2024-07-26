using BogaNet.Helper;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;

namespace BogaNet.Prefs;

/// <summary>
/// Container for Avalonia preferences in the browser.
/// </summary>
public partial class WebPreferencesContainer : PreferencesContainer
{
   #region Variables

   private const string _containerKey = "BogaNetPrefs";

   private static WebPreferencesContainer? _lazyInstance;

   #endregion

   #region Events

   public override event IFilePreferences.FileLoaded? OnFileLoaded;

   public override event IFilePreferences.FileSaved? OnFileSaved;

   #endregion

   #region Constructors

   public WebPreferencesContainer()
   {
      Console.WriteLine("WebPrefs START");

      Task.Run(initAsync);

      Console.WriteLine("WebPrefs instantiated"); //TODO remove!

      _lazyInstance = this;
      _file = _containerKey;
   }

   #endregion

   #region Overridden methods

   public override bool Load(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      Console.WriteLine("Load...");
      onLoaded(GetPreference(_file).ToString());
      Console.WriteLine("Load completed!");

      return true;
   }

   public override async Task<bool> LoadAsync(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      Console.WriteLine("LoadAsync...");
      onLoaded((await GetPreference(_file)));
      Console.WriteLine("LoadAsync completed!");

      return true;
   }

   public override bool Save(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      Console.WriteLine("Save");
      SetPreference(_file, JsonHelper.SerializeToString(_preferences));

      OnFileSaved?.Invoke(_file);
      IsSaved = true;

      return true;
   }

   public override async Task<bool> SaveAsync(string filepath = "")
   {
      return Save(filepath);
   }

   public override bool Delete(string filepath = "")
   {
      //TODO implement
      return base.Delete(filepath);
   }

   #endregion

   #region Private methods

   private async Task initAsync()
   {
      Task.Delay(100);

      await JSHost.ImportAsync("bogabridge", "../boganet_bridge.js");

      Load();
   }

   private void onLoaded(string value)
   {
      _preferences = JsonHelper.DeserializeFromString<Dictionary<string, object?>>(value);

      IsLoaded = true;

      OnFileLoaded?.Invoke(_file);
   }

   [JSExport]
   internal static void Preference(string key, string? value)
   {
      Console.WriteLine($"Preference received: {key} - {value}"); //TODO remove or replace

      if (value != null)
         _lazyInstance?.onLoaded(value);
   }

   [JSImport("setPreference", "bogabridge")]
   internal static partial Task SetPreference(string key, string? value);

   [JSImport("getPreference", "bogabridge")]
   [return: JSMarshalAs<JSType.Promise<JSType.String>>()]
   internal static partial Task<string> GetPreference(string key);

   #endregion
}