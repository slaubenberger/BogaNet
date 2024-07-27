using BogaNet.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;

namespace BogaNet.Prefs;

/// <summary>
/// Container for preferences in Avalonia (browser).
/// </summary>
public partial class BrowserPreferencesContainer : PreferencesContainer
{
   #region Variables

   private const string _containerKey = "BogaNetPrefs";

   #endregion

   #region Events

   public override event IFilePreferences.FileLoaded? OnFileLoaded;

   public override event IFilePreferences.FileSaved? OnFileSaved;

   #endregion

   #region Constructors

   public BrowserPreferencesContainer()
   {
      _file = _containerKey;
   }

   #endregion

   #region Overridden methods

   public override bool Load(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      string res = JSGetPreference(_file);

      if (string.IsNullOrEmpty(res))
         return false;

      _preferences = JsonHelper.DeserializeFromString<Dictionary<string, object?>>(res)!;

      IsLoaded = true;

      OnFileLoaded?.Invoke(_file);

      return true;
   }

   public override Task<bool> LoadAsync(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      return Task.FromResult(Load());
   }

   public override bool Save(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      JSSetPreference(_file, JsonHelper.SerializeToString(_preferences));

      OnFileSaved?.Invoke(_file);
      IsSaved = true;

      return true;
   }

   public override Task<bool> SaveAsync(string filepath = "")
   {
      return Task.FromResult(Save(filepath));
   }

   public override bool Delete(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      JSDeletePreference(_file);

      return true;
   }

   #endregion

   #region JavaScript methods

   [JSImport("setPreference", "boganet_prefs")]
   internal static partial void JSSetPreference(string key, string? value);

   [JSImport("getPreference", "boganet_prefs")]
   internal static partial string JSGetPreference(string key);

   [JSImport("deletePreference", "boganet_prefs")]
   internal static partial string JSDeletePreference(string key);

   #endregion
}