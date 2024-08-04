using BogaNet.Helper;
using System.Numerics;
using Microsoft.Extensions.Logging;
using System;
using BogaNet.Extension;
using BogaNet.Util;
using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.ObfuscatedType;

namespace BogaNet.Prefs;

/// <summary>
/// Preferences for the application.
/// </summary>
public class Preferences : Singleton<Preferences>, IFilePreferences //NUnit
{
   #region Variables

   private static readonly ILogger<Preferences> _logger = GlobalLogging.CreateLogger<Preferences>();

   #endregion

   #region Properties

   public virtual ByteObf IV
   {
      set => Container.IV = value;
   }

   public virtual IPreferencesContainer Container { get; set; } = new PreferencesContainer();
   public virtual bool AutoSaveOnExit { get; set; } = true;
   public virtual bool IsLoaded => Container.IsLoaded;
   public bool IsSaved => Container.IsSaved;
   public virtual List<string> Keys => Container.Keys;
   public virtual int Count => Container.Count;

   #endregion

   #region Events

   public event IFilePreferences.FileLoaded? OnFileLoaded;

   public event IFilePreferences.FileSaved? OnFileSaved;

   #endregion

   #region Constructor

   protected Preferences()
   {
      //Load(); //TODO good idea?

      AppDomain.CurrentDomain.ProcessExit += cb_exit;
   }

   #endregion

   #region Public methods

   public virtual bool Load(string filepath = "")
   {
      bool res = Container.Load(filepath);
      OnFileLoaded?.Invoke(filepath);
      return res;
   }

   public virtual async Task<bool> LoadAsync(string filepath = "")
   {
      bool res = await Container.LoadAsync(filepath);
      OnFileLoaded?.Invoke(filepath);
      return res;
   }

   public virtual bool Save(string filepath = "")
   {
      bool res = Container.Save(filepath);
      OnFileSaved?.Invoke(filepath);
      return res;
   }

   public virtual async Task<bool> SaveAsync(string filepath = "")
   {
      bool res = await Container.SaveAsync(filepath);
      OnFileSaved?.Invoke(filepath);
      return res;
   }

   public virtual bool Delete(string filepath = "")
   {
      return Container.Delete(filepath);
   }

   public virtual bool Remove(string key)
   {
      return Container.Remove(key);
   }

   public virtual bool ContainsKey(string key)
   {
      return Container.ContainsKey(key);
   }

   #region Getter

   public virtual string GetString(string key, bool obfuscated = false)
   {
      return TryGetString(key, out string result, obfuscated) ? result : null!;
   }

   public virtual bool TryGetString(string key, out string result, bool obfuscated = false)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(key);

      bool res = Container.TryGet(key, out object obj, obfuscated);

      result = (res ? obj.ToString() : null)!;
      return res;
   }

   public virtual T GetObject<T>(string key, bool obfuscated = false)
   {
      return TryGetObject(key, out T result, obfuscated) ? result : default!;
   }

   public virtual bool TryGetObject<T>(string key, out T result, bool obfuscated = false)
   {
      bool res = TryGetString(key, out string str, obfuscated);

      result = res ? JsonHelper.DeserializeFromString<T>(str) : default!;
      return res;
   }

   public virtual T GetNumber<T>(string key, bool obfuscated = false) where T : INumber<T>
   {
      return TryGetNumber(key, out T result, obfuscated) ? result : T.CreateTruncating(0);
   }

   public virtual bool TryGetNumber<T>(string key, out T result, bool obfuscated = false) where T : INumber<T>
   {
      bool res = TryGetString(key, out string str, obfuscated);

      if (!res)
      {
         result = T.CreateTruncating(0);
         return false;
      }

      Type type = typeof(T);

      switch (type)
      {
         case not null when type == typeof(double):
            double doubleVal = double.Parse(str);
            result = T.CreateTruncating(doubleVal);
            break;
         case not null when type == typeof(float):
            float floatVal = float.Parse(str);
            result = T.CreateTruncating(floatVal);
            break;
         case not null when type == typeof(long):
            long longVal = long.Parse(str);
            result = T.CreateTruncating(longVal);
            break;
         case not null when type == typeof(ulong):
            ulong ulongVal = ulong.Parse(str);
            result = T.CreateTruncating(ulongVal);
            break;
         case not null when type == typeof(int):
            int intVal = int.Parse(str);
            result = T.CreateTruncating(intVal);
            break;
         case not null when type == typeof(uint):
            uint uintVal = uint.Parse(str);
            result = T.CreateTruncating(uintVal);
            break;
         case not null when type == typeof(short):
            short shortVal = short.Parse(str);
            result = T.CreateTruncating(shortVal);
            break;
         case not null when type == typeof(ushort):
            ushort ushortVal = ushort.Parse(str);
            result = T.CreateTruncating(ushortVal);
            break;
         case not null when type == typeof(nint):
            nint nintVal = nint.Parse(str);
            result = T.CreateTruncating(nintVal);
            break;
         case not null when type == typeof(nuint):
            nint nuintVal = nint.Parse(str);
            result = T.CreateTruncating(nuintVal);
            break;
         case not null when type == typeof(byte):
            byte byteVal = byte.Parse(str);
            result = T.CreateTruncating(byteVal);
            break;
         case not null when type == typeof(sbyte):
            sbyte sbyteVal = sbyte.Parse(str);
            result = T.CreateTruncating(sbyteVal);
            break;
         case not null when type == typeof(char):
            char charVal = char.Parse(str);
            result = T.CreateTruncating(charVal);
            break;
         default:
            _logger.LogWarning("Number type is not supported!");
            result = T.CreateTruncating(0);
            return false;
      }

      return true;
   }

   public virtual bool GetBool(string key, bool obfuscated = false)
   {
      return TryGetBool(key, out bool result, obfuscated) && result;
   }

   public virtual bool TryGetBool(string key, out bool result, bool obfuscated = false)
   {
      bool res = TryGetString(key, out string str, obfuscated);

      result = res && "true".Equals(str.ToLower());
      return res;
   }

   public virtual DateTime GetDate(string key, bool obfuscated = false, TimeZoneInfo? usedTZ = null)
   {
      return TryGetDate(key, out DateTime result, obfuscated, usedTZ) ? result : default!;
   }

   public virtual bool TryGetDate(string key, out DateTime result, bool obfuscated = false, TimeZoneInfo? usedTZ = null)
   {
      bool res = TryGetString(key, out string str, obfuscated);

      bool parsed = DateTime.TryParse(str, out DateTime dt);

      result = res && parsed ? dt.BNConvertToTimeZone(usedTZ) : default!;

      return res && parsed;
   }

   #endregion

   #region Setter

   public virtual void Set(string key, string value, bool obfuscated = false)
   {
      Container.Set(key, value, obfuscated);
   }

   public virtual void Set(string key, object value, bool obfuscated = false)
   {
      ArgumentNullException.ThrowIfNull(value);

      Set(key, JsonHelper.SerializeToString(value), obfuscated);
   }

   public virtual void Set<T>(string key, T value, bool obfuscated = false) where T : INumber<T>
   {
      Container.Set(key, value, obfuscated);
   }

   public virtual void Set(string key, bool value, bool obfuscated = false)
   {
      Container.Set(key, value, obfuscated);
   }

   public virtual void Set(string key, DateTime value, bool obfuscated = false)
   {
      ArgumentNullException.ThrowIfNull(value);

      string dt = JsonHelper.SerializeToString(value).Replace("\"", "");
      Set(key, dt, obfuscated);
   }

   #endregion

   #endregion

   #region Private methods

   private void cb_exit(object? sender, EventArgs e)
   {
      if (AutoSaveOnExit)
         Save();
   }

   #endregion
}