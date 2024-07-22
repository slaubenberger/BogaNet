using BogaNet.Helper;
using BogaNet.Util;
using System.Text;
using BogaNet.Encoder;
using System.Collections.Generic;
using BogaNet.Extension;
using BogaNet.ObfuscatedType;
using System;

namespace BogaNet.Prefs;

/// <summary>
/// Container for the application preferences.
/// </summary>
public class PreferencesContainer : IPreferencesContainer //NUnit
{
   #region Variables

   protected string _file = "BNPrefs.json";
   protected Dictionary<string, object> _preferences = [];

   #endregion

   #region Properties

   /// <summary>
   /// IV for the obfuscated data.
   /// </summary>
   public virtual ByteObf IV { get; set; } = 139;

   #endregion

   #region Public methods

   public virtual bool Load(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      if (FileHelper.Exists(_file))
      {
         Dictionary<string, object> prefs = JsonHelper.DeserializeFromFile<Dictionary<string, object>>(_file);

         _preferences = prefs;

         return true;
      }

      return false;
   }

   public virtual bool Save(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      return JsonHelper.SerializeToFile(_preferences, _file);
   }

   public virtual bool Delete(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      _preferences.Clear();

      if (FileHelper.Exists(_file))
         return FileHelper.Delete(_file);

      return true;
   }

   public virtual bool Remove(string key)
   {
      return ContainsKey(key) && _preferences.Remove(key);
   }

   public virtual bool ContainsKey(string key)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);

      return _preferences.ContainsKey(key);
   }

   public virtual object? Get(string key, bool obfuscated)
   {
      if (ContainsKey(key))
         return obfuscated ? Obfuscator.Deobfuscate(Base64.FromBase64String(_preferences[key].ToString()!), IV).BNToString() : _preferences[key];

      return null;
   }

   public virtual void Set(string key, object value, bool obfuscated)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);
      ArgumentNullException.ThrowIfNull(value);

      if (ContainsKey(key))
      {
         if (obfuscated)
         {
            _preferences[key] = Base64.ToBase64String(Obfuscator.Obfuscate(value.ToString()!, IV));
         }
         else
         {
            _preferences[key] = value;
         }
      }
      else
      {
         _preferences.Add(key, obfuscated ? Base64.ToBase64String(Obfuscator.Obfuscate(value.ToString()!, IV)) : value);
      }
   }

   public override string ToString()
   {
      var sb = new StringBuilder();

      sb.Append(GetType().Name);
      sb.Append('[');

      foreach (KeyValuePair<string, object> kvp in _preferences)
      {
         sb.Append($"{kvp.Key}='{kvp.Value}', ");
      }

      sb.Remove(sb.Length, 2); //remove last delimiter
      sb.Append(']');

      return sb.ToString();
   }

   #endregion
}