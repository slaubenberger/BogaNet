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

   public virtual void Load(string filepath = "")
   {
      _container.Load(filepath);
   }

   public virtual void Save(string filepath = "")
   {
      _container.Save(filepath);
   }

   public virtual void Delete(string filepath = "")
   {
      _container.Delete(filepath);
   }

   public virtual void Remove(string key)
   {
      _container.Remove(key);
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

   public virtual T GetObject<T>(string key, bool obfuscated = false)
   {
      return JsonHelper.DeserializeFromString<T>(GetString(key, obfuscated))!;
   }

   public virtual T GetNumber<T>(string key, bool obfuscated = false) where T : INumber<T>
   {
      if (string.IsNullOrEmpty(key))
         throw new ArgumentNullException(nameof(key));

      Type type = typeof(T);

      string? plainValue = GetString(key, obfuscated);

      if (plainValue == null)
         return T.CreateTruncating(0);

      switch (type)
      {
         case Type when type == typeof(double):
            double doubleVal = double.Parse(plainValue);
            return T.CreateTruncating(doubleVal);
         case Type when type == typeof(float):
            float floatVal = float.Parse(plainValue);
            return T.CreateTruncating(floatVal);
         case Type when type == typeof(long):
            long longVal = long.Parse(plainValue);
            return T.CreateTruncating(longVal);
         case Type when type == typeof(ulong):
            ulong ulongVal = ulong.Parse(plainValue);
            return T.CreateTruncating(ulongVal);
         case Type when type == typeof(int):
            int intVal = int.Parse(plainValue);
            return T.CreateTruncating(intVal);
         case Type when type == typeof(uint):
            uint uintVal = uint.Parse(plainValue);
            return T.CreateTruncating(uintVal);
         case Type when type == typeof(short):
            short shortVal = short.Parse(plainValue);
            return T.CreateTruncating(shortVal);
         case Type when type == typeof(ushort):
            ushort ushortVal = ushort.Parse(plainValue);
            return T.CreateTruncating(ushortVal);
         case Type when type == typeof(nint):
            nint nintVal = nint.Parse(plainValue);
            return T.CreateTruncating(nintVal);
         case Type when type == typeof(nuint):
            nint nuintVal = nint.Parse(plainValue);
            return T.CreateTruncating(nuintVal);
         case Type when type == typeof(byte):
            byte byteVal = byte.Parse(plainValue);
            return T.CreateTruncating(byteVal);
         case Type when type == typeof(sbyte):
            sbyte sbyteVal = sbyte.Parse(plainValue);
            return T.CreateTruncating(sbyteVal);
         case Type when type == typeof(char):
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

   public virtual DateTime GetDate(string key, bool obfuscated = false, TimeZoneInfo? usedTZ = null)
   {
      string? date = GetString(key, obfuscated);

      if (DateTime.TryParse(date, out DateTime dt))
         return dt.BNConvertToTimeZone(usedTZ);

      return default;
   }

   #endregion


   #region Setter

   public virtual void Set(string key, string? value, bool obfuscated = false)
   {
      _container.Set(key, value, obfuscated);
   }

   public virtual void Set(string key, object value, bool obfuscated = false)
   {
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