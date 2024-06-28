using BogaNet.Helper;
using BogaNet.Util;
using System.Text;
using BogaNet.Encoder;
using System.Collections.Generic;
using BogaNet.ObfuscatedType;

namespace BogaNet.Prefs;

/// <summary>
/// Container for the application preferences.
/// </summary>
public class PreferencesContainer
{
   #region Variables

   private string _file = "BNPrefs.json";
   private Dictionary<string, object?>? _preferences = [];

   #endregion

   #region Properties

   /// <summary>
   /// IV for the obfuscated data.
   /// </summary>
   public ByteObf IV { get; set; } = 181;

   #endregion

   #region Public methods

   /// <summary>
   /// Load the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to load</param>
   public void Load(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      Dictionary<string, object?>? prefs = null;

      if (FileHelper.Exists(_file))
         prefs = JsonHelper.DeserializeFromFile<Dictionary<string, object?>>(_file);

      if (prefs != null)
         _preferences = prefs;
   }

   /// <summary>
   /// Save the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to save</param>
   public void Save(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      JsonHelper.SerializeToFile(_preferences, _file);
   }

   /// <summary>
   /// Delete all preferences, including the file.
   /// </summary>
   /// <param name="filepath">Preference file to delete</param>
   public void Delete(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      _preferences?.Clear();

      if (FileHelper.Exists(_file))
         FileHelper.Delete(_file);
   }

   /// <summary>
   /// Removes a key/value from the preferences.
   /// </summary>
   /// <param name="key">Key (and value) to delete</param>
   public void Remove(string key)
   {
      if (ContainsKey(key))
         _preferences?.Remove(key);
   }

   /// <summary>
   /// Checks if a given key exists in the preferences.
   /// </summary>
   /// <param name="key">Key to check</param>
   /// <returns>True if the key exists in the preferences</returns>
   public bool ContainsKey(string key)
   {
      return _preferences != null && _preferences.ContainsKey(key);
   }

   /// <summary>Get an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Object for the key</returns>
   public object? Get(string key, bool obfuscated)
   {
      if (ContainsKey(key))
         return obfuscated ? Obfuscator.DeobfuscateToString(Base64.FromBase64String(_preferences![key]?.ToString(), true), IV) : _preferences![key];

      return null;
   }

   /// <summary>Set an object for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="value">Object for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   public void Set(string key, object? value, bool obfuscated)
   {
      if (ContainsKey(key))
      {
         if (obfuscated)
         {
            _preferences![key] = Base64.ToBase64String(Obfuscator.Obfuscate(value?.ToString(), IV), true);
         }
         else
         {
            _preferences![key] = value;
         }
      }
      else
      {
         if (_preferences != null)
            _preferences.Add(key, obfuscated ? Base64.ToBase64String(Obfuscator.Obfuscate(value?.ToString(), IV), true) : value);
      }
   }

   public override string ToString()
   {
      var sb = new StringBuilder();

      sb.Append(GetType().Name);
      sb.Append("[");

      if (_preferences != null)
      {
         foreach (KeyValuePair<string, object?> kvp in _preferences)
         {
            sb.Append($"{kvp.Key}='{kvp.Value}', ");
         }
      }

      sb.Remove(sb.Length, 2); //remove last delimiter
      sb.Append("]");

      return sb.ToString();
   }

   #endregion
}