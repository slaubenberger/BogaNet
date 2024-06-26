﻿using BogaNet.Helper;
using BogaNet.Util;
using System.Text;
using BogaNet.Encoder;
using System.Collections.Generic;
using BogaNet.Extension;
using BogaNet.ObfuscatedType;

namespace BogaNet.Prefs;

/// <summary>
/// Container for the application preferences.
/// </summary>
public class PreferencesContainer : IPreferencesContainer //NUnit
{
   #region Variables

   protected string _file = "BNPrefs.json";
   protected Dictionary<string, object?>? _preferences = [];

   #endregion

   #region Properties

   /// <summary>
   /// IV for the obfuscated data.
   /// </summary>
   public virtual ByteObf IV { get; set; } = 139;

   #endregion

   #region Public methods

   public virtual void Load(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      Dictionary<string, object?>? prefs = null;

      if (FileHelper.Exists(_file))
         prefs = JsonHelper.DeserializeFromFile<Dictionary<string, object?>>(_file);

      if (prefs != null)
         _preferences = prefs;
   }

   public virtual void Save(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      JsonHelper.SerializeToFile(_preferences, _file);
   }

   public virtual void Delete(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      _preferences?.Clear();

      if (FileHelper.Exists(_file))
         FileHelper.Delete(_file);
   }

   public virtual void Remove(string key)
   {
      if (ContainsKey(key))
         _preferences?.Remove(key);
   }

   public virtual bool ContainsKey(string key)
   {
      return _preferences != null && _preferences.ContainsKey(key);
   }

   public virtual object? Get(string key, bool obfuscated)
   {
      if (ContainsKey(key))
         return obfuscated ? Obfuscator.Deobfuscate(Base64.FromBase64String(_preferences![key]?.ToString(), true), IV).BNToString() : _preferences![key];

      return null;
   }

   public virtual void Set(string key, object? value, bool obfuscated)
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
         _preferences?.Add(key, obfuscated ? Base64.ToBase64String(Obfuscator.Obfuscate(value?.ToString(), IV), true) : value);
      }
   }

   public override string ToString()
   {
      var sb = new StringBuilder();

      sb.Append(GetType().Name);
      sb.Append('[');

      if (_preferences != null)
      {
         foreach (KeyValuePair<string, object?> kvp in _preferences)
         {
            sb.Append($"{kvp.Key}='{kvp.Value}', ");
         }
      }

      sb.Remove(sb.Length, 2); //remove last delimiter
      sb.Append(']');

      return sb.ToString();
   }

   #endregion
}