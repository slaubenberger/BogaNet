using System;
using BogaNet.Extension;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using BogaNet.Util;
using BogaNet.Encoder;

namespace BogaNet.Prefs;

/// <summary>
/// Container for Avalonia preferences in the browser.
/// </summary>
public partial class WebPreferencesContainer : PreferencesContainer
{
   private static string? _value;
   private static bool _receivedPreference;

   public WebPreferencesContainer()
   {
      Console.WriteLine("Web-version of AvaloniaPreferencesContainer STARTED"); //TODO remove!

      Task.Run(init).GetAwaiter().GetResult();

      Console.WriteLine("Web-version of AvaloniaPreferencesContainer instantiated!"); //TODO remove!
   }

   private async Task init()
   {
      await JSHost.ImportAsync("bogabridge", "../boganet_bridge.js");
   }

   public override bool ContainsKey(string key)
   {
      return Task.Run(() => ContainsKeyAsync(key)).GetAwaiter().GetResult();
   }

   public virtual async Task<bool> ContainsKeyAsync(string key)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);

      GetPreference(key);

      do
      {
         await Task.Delay(100);
      } while (!_receivedPreference);

      _receivedPreference = false;

      return !string.IsNullOrEmpty(_value);
   }

   public override object? Get(string key, bool obfuscated)
   {
      return Task.Run(() => GetAsync(key, obfuscated)).GetAwaiter().GetResult();
   }

   public virtual async Task<object?> GetAsync(string key, bool obfuscated)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);

      GetPreference(key);

      do
      {
         await Task.Delay(100);
      } while (!_receivedPreference);

      _receivedPreference = false;

      if (!string.IsNullOrEmpty(_value))
         return obfuscated ? Obfuscator.Deobfuscate(Base91.FromBase91String(_value), IV).BNToString() : _value;

      return null;
   }

   public override void Set(string key, object value, bool obfuscated)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);
      ArgumentNullException.ThrowIfNull(value);

      SetPreference(key, obfuscated ? Base91.ToBase91String(Obfuscator.Obfuscate(value.ToString()!, IV)) : value.ToString());
   }

   [JSExport]
   internal static void Preference(string pref)
   {
      _receivedPreference = true;
      _value = pref;
      Console.WriteLine("Preference received: " + pref); //TODO remove or replace
   }

   [JSImport("setPreference", "bogabridge")]
   internal static partial void SetPreference(string key, string? value);

   [JSImport("getPreference", "bogabridge")]
   internal static partial void GetPreference(string key);
}