using BogaNet.Helper;
using BogaNet.Util;
using System.Text;
using BogaNet.Encoder;
using System.Collections.Generic;

namespace BogaNet.Prefs;

/// <summary>
/// Container for the application preferences.
/// </summary>
public class PreferencesContainer
{
   #region Variables

   private string _file = "BNConfig.json";
   private Dictionary<string, string?>? _preferences = [];

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

      Dictionary<string, string?>? prefs = null;

      if (FileHelper.Exists(_file))
         prefs = JsonHelper.DeserializeFromFile<Dictionary<string, string?>>(_file);

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
   public void DeleteAll(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      _preferences?.Clear();

      if (FileHelper.Exists(_file))
         FileHelper.Delete(_file);
   }

   /// <summary>
   /// Delete a key/value from the preferences.
   /// </summary>
   /// <param name="key">Key (and value) to delete</param>
   public void Delete(string key)
   {
      if (HasKey(key))
         _preferences?.Remove(key);
   }

   /// <summary>
   /// Checks if a given key exists in the preferences.
   /// </summary>
   /// <param name="key">Key to check</param>
   /// <returns>True if the key exists in the preferences</returns>
   public bool HasKey(string key)
   {
      return _preferences != null && _preferences.ContainsKey(key);
   }

   /// <summary>Get a string for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>String for the key</returns>
   public string? Get(string key, bool obfuscated)
   {
      if (HasKey(key))
      {
         if (obfuscated)
            return Obfuscator.DeobfuscateToString(Base64.FromBase64String(_preferences![key], true));

         return _preferences![key];
      }

      return null;
   }

   /// <summary>Set a string for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="value">String for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   public void Set(string key, string? value, bool obfuscated)
   {
      if (HasKey(key))
      {
         if (obfuscated)
         {
            _preferences![key] = Base64.ToBase64String(Obfuscator.Obfuscate(value), true);
         }
         else
         {
            _preferences![key] = value;
         }
      }
      else
      {
         if (_preferences != null)
         {
            if (obfuscated)
            {
               _preferences.Add(key, Base64.ToBase64String(Obfuscator.Obfuscate(value), true));
            }
            else
            {
               _preferences.Add(key, value);
            }
         }
      }
   }

   public override string ToString()
   {
      var sb = new StringBuilder();

      sb.Append(GetType().Name);
      sb.Append("[");

      if (_preferences != null)
      {
         foreach (KeyValuePair<string, string?> kvp in _preferences)
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