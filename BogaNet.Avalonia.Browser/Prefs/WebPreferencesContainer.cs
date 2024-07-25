using System;
using System.Runtime.InteropServices.JavaScript;
//using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.Helper;

namespace BogaNet.Prefs;

/// <summary>
/// Container for Avalonia preferences in the browser.
/// </summary>
public partial class WebPreferencesContainer : PreferencesContainer
{
   #region Variables

   private static Dictionary<string, object?> _staticPreferences = [];

   private const string _containerKey = "BogaNetPreferences";

   #endregion

   #region Constructors

   public WebPreferencesContainer()
   {
      Console.WriteLine("WebPreferencesContainer START");

      //Task.Run(init).GetAwaiter();
      init();

      Console.WriteLine("WebPreferencesContainer instantiated"); //TODO remove!
   }

   public WebPreferencesContainer(params string[] keys) : this()
   {
      foreach (var key in keys)
      {
         GetPreference(key);
      }
   }

   #endregion

   #region Overridden methods

   public override bool Load(string filepath = "")
   {
      GetPreference(_containerKey);

      return true;
   }

   public override bool Save(string filepath = "")
   {
      SetPreference(_containerKey, JsonHelper.SerializeToString(_staticPreferences));

      return true;
   }

   public override bool Delete(string filepath = "")
   {
      //TODO implement
      return base.Delete(filepath);
   }

   public override bool ContainsKey(string key)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);

      return _staticPreferences.ContainsKey(key);
   }

   public override object? Get(string key, bool obfuscated)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);

      if (_staticPreferences.ContainsKey(key))
         return _staticPreferences[key];

      //GetPreference(key);

      return null;
   }

   public override void Set(string key, object value, bool obfuscated)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);
      ArgumentNullException.ThrowIfNull(value);

      if (_staticPreferences.ContainsKey(key))
      {
         _staticPreferences[key] = value;
      }
      else
      {
         _staticPreferences.Add(key, value);
      }

      //if (_staticPreferences.TryGetValue(key, out object? v) && v!.Equals(value))
      //   return;

      //SetPreference(key, obfuscated ? Base91.ToBase91String(Obfuscator.Obfuscate(value.ToString()!, IV)) : value.ToString());
   }

   #endregion

   #region Private methods

   // private async Task init()
   // {
   //    await JSHost.ImportAsync("bogabridge", "../boganet_bridge.js")
   //
   //    Load();
   // }

   private void init()
   {
      JSHost.ImportAsync("bogabridge", "../boganet_bridge.js");

      Load();
   }

   [JSExport]
   public static void Preference(string key, string? value)
   {
      Console.WriteLine($"Preference received: {key} - {value}"); //TODO remove or replace

      if (value != null)
         _staticPreferences = JsonHelper.DeserializeFromString<Dictionary<string, object?>>(value);
   }

   [JSImport("setPreference", "bogabridge")]
   public static partial void SetPreference(string key, string? value);

   [JSImport("getPreference", "bogabridge")]
   public static partial void GetPreference(string key);

   #endregion
}