﻿using BogaNet.Helper;
using BogaNet.Util;
using System.Text;
using BogaNet.Encoder;
using System.Collections.Generic;
using BogaNet.Extension;
using BogaNet.ObfuscatedType;
using System;
using System.Threading.Tasks;

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

   public virtual bool IsLoaded { get; protected set; }
   public bool IsSaved { get; protected set; }

   #endregion

   #region Events

   public virtual event IFilePreferences.FileLoaded? OnFileLoaded;

   public virtual event IFilePreferences.FileSaved? OnFileSaved;

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

         IsLoaded = _preferences.Count > 0;
         OnFileLoaded?.Invoke(filepath);

         return true;
      }

      OnFileLoaded?.Invoke(filepath);
      return false;
   }

   public virtual async Task<bool> LoadAsync(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      if (FileHelper.Exists(_file))
      {
         Dictionary<string, object> prefs = await JsonHelper.DeserializeFromFileAsync<Dictionary<string, object>>(_file);

         _preferences = prefs;

         IsLoaded = _preferences.Count > 0;
         OnFileLoaded?.Invoke(filepath);

         return true;
      }

      OnFileLoaded?.Invoke(filepath);
      return false;
   }

   public virtual bool Save(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      bool res = JsonHelper.SerializeToFile(_preferences, _file);
      IsSaved = res;

      OnFileSaved?.Invoke(filepath);

      return res;
   }

   public virtual async Task<bool> SaveAsync(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      bool res = await JsonHelper.SerializeToFileAsync(_preferences, _file);
      IsSaved = res;

      OnFileSaved?.Invoke(filepath);

      return res;
   }

   public virtual bool Delete(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      _preferences.Clear();

      if (FileHelper.Exists(_file))
         return FileHelper.Delete(_file);

      IsSaved = IsLoaded = false;
      return true;
   }

   public virtual bool Remove(string key)
   {
      bool res = false;

      if (ContainsKey(key))
         res = _preferences.Remove(key);

      IsSaved = !res;
      return res;
   }

   public virtual bool ContainsKey(string key)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);

      return _preferences.ContainsKey(key);
   }

   public virtual object Get(string key, bool obfuscated)
   {
      if (!ContainsKey(key))
         throw new KeyNotFoundException();

      return obfuscated ? Obfuscator.Deobfuscate(Base91.FromBase91String(_preferences[key].ToString()!), IV).BNToString() : _preferences[key];
   }

   public virtual bool TryGet(string key, out object result, bool obfuscated)
   {
      if (!ContainsKey(key))
      {
         result = null!;
         return false;
      }

      result = obfuscated ? Obfuscator.Deobfuscate(Base91.FromBase91String(_preferences[key].ToString()!), IV).BNToString() : _preferences[key];
      return true;
   }

   public virtual void Set(string key, object value, bool obfuscated)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);
      ArgumentNullException.ThrowIfNull(value);

      if (ContainsKey(key))
      {
         if (obfuscated)
         {
            //_preferences[key] = Base64.ToBase64String(Obfuscator.Obfuscate(value.ToString()!, IV));
            _preferences[key] = Base91.ToBase91String(Obfuscator.Obfuscate(value.ToString()!, IV));
         }
         else
         {
            _preferences[key] = value;
         }
      }
      else
      {
         //_preferences.Add(key, obfuscated ? Base64.ToBase64String(Obfuscator.Obfuscate(value.ToString()!, IV)) : value);
         _preferences.Add(key, obfuscated ? Base91.ToBase91String(Obfuscator.Obfuscate(value.ToString()!, IV)) : value);
      }

      IsSaved = false;
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