using BogaNet.Helper;
using System.Numerics;
using Microsoft.Extensions.Logging;
using System;
using BogaNet.Extension;
using BogaNet.Util;

namespace BogaNet.Prefs;

/// <summary>
/// Preferences for the application.
/// </summary>
public class Preferences : Singleton<Preferences>, IPreferences //NUnit //TODO add support for web and mobile (Avalonia) ?
{
   #region Variables

   private static readonly ILogger<Preferences> _logger = GlobalLogging.CreateLogger<Preferences>();

   protected IPreferencesContainer _container = new PreferencesContainer();

   #endregion

   #region Properties

   public virtual bool AutoSaveOnExit { get; set; } = true;

   #endregion

   #region Constructor

   private Preferences()
   {
      Load();

      AppDomain.CurrentDomain.ProcessExit += cb_exit;
   }

   #endregion

   #region Public methods

   public virtual bool Load(string filepath = "")
   {
      return _container.Load(filepath);
   }

   public virtual bool Save(string filepath = "")
   {
      return _container.Save(filepath);
   }

   public virtual bool Delete(string filepath = "")
   {
      return _container.Delete(filepath);
   }

   public virtual bool Remove(string key)
   {
      return _container.Remove(key);
   }

   public virtual bool ContainsKey(string key)
   {
      return _container.ContainsKey(key);
   }

   #region Getter

   public virtual string? GetString(string key, bool obfuscated = false)
   {
      return _container.Get(key, obfuscated)?.ToString();
   }

   public virtual T? GetObject<T>(string key, bool obfuscated = false)
   {
      string? str = GetString(key, obfuscated);
      return str == null ? default : JsonHelper.DeserializeFromString<T>(str);
   }

   public virtual T? GetNumber<T>(string key, bool obfuscated = false) where T : INumber<T>
   {
      string? plainValue = GetString(key, obfuscated);

      if (plainValue == null)
         return default;

      Type type = typeof(T);

      switch (type)
      {
         case not null when type == typeof(double):
            double doubleVal = double.Parse(plainValue);
            return T.CreateTruncating(doubleVal);
         case not null when type == typeof(float):
            float floatVal = float.Parse(plainValue);
            return T.CreateTruncating(floatVal);
         case not null when type == typeof(long):
            long longVal = long.Parse(plainValue);
            return T.CreateTruncating(longVal);
         case not null when type == typeof(ulong):
            ulong ulongVal = ulong.Parse(plainValue);
            return T.CreateTruncating(ulongVal);
         case not null when type == typeof(int):
            int intVal = int.Parse(plainValue);
            return T.CreateTruncating(intVal);
         case not null when type == typeof(uint):
            uint uintVal = uint.Parse(plainValue);
            return T.CreateTruncating(uintVal);
         case not null when type == typeof(short):
            short shortVal = short.Parse(plainValue);
            return T.CreateTruncating(shortVal);
         case not null when type == typeof(ushort):
            ushort ushortVal = ushort.Parse(plainValue);
            return T.CreateTruncating(ushortVal);
         case not null when type == typeof(nint):
            nint nintVal = nint.Parse(plainValue);
            return T.CreateTruncating(nintVal);
         case not null when type == typeof(nuint):
            nint nuintVal = nint.Parse(plainValue);
            return T.CreateTruncating(nuintVal);
         case not null when type == typeof(byte):
            byte byteVal = byte.Parse(plainValue);
            return T.CreateTruncating(byteVal);
         case not null when type == typeof(sbyte):
            sbyte sbyteVal = sbyte.Parse(plainValue);
            return T.CreateTruncating(sbyteVal);
         case not null when type == typeof(char):
            char charVal = char.Parse(plainValue);
            return T.CreateTruncating(charVal);
         default:
            _logger.LogWarning("Number type is not supported!");
            break;
      }

      return T.CreateTruncating(0);
   }

   public virtual bool GetBool(string key, bool obfuscated = false)
   {
      string? result = GetString(key, obfuscated);
      return result != null && "true".Equals(result.ToLower());
   }

   public virtual DateTime? GetDate(string key, bool obfuscated = false, TimeZoneInfo? usedTZ = null)
   {
      string? date = GetString(key, obfuscated);

      return DateTime.TryParse(date, out DateTime dt) ? dt.BNConvertToTimeZone(usedTZ) : default(DateTime?);
   }

   #endregion


   #region Setter

   public virtual void Set(string key, string value, bool obfuscated = false)
   {
      _container.Set(key, value, obfuscated);
   }

   public virtual void Set(string key, object value, bool obfuscated = false)
   {
      ArgumentNullException.ThrowIfNull(value);

      Set(key, JsonHelper.SerializeToString(value), obfuscated);
   }

   public virtual void Set<T>(string key, T value, bool obfuscated = false) where T : INumber<T>
   {
      _container.Set(key, value, obfuscated);
   }

   public virtual void Set(string key, bool value, bool obfuscated = false)
   {
      _container.Set(key, value, obfuscated);
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