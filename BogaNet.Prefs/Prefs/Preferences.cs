using BogaNet.Helper;
using System.Numerics;
using Microsoft.Extensions.Logging;
using System;

namespace BogaNet.Prefs;

/// <summary>
/// Preferences for the application.
/// </summary>
public static class Preferences //TODO add support for web and mobile (Avalonia) ?
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(Preferences));

   private static readonly PreferencesContainer _container = new();

   #endregion

   #region Static block

   static Preferences()
   {
      Load();

      AppDomain.CurrentDomain.ProcessExit += AppDomain_ProcessExit;
   }

   #endregion

   #region Public methods

   /// <summary>
   /// Load the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to load</param>
   public static void Load(string filepath = "")
   {
      _container.Load(filepath);
   }

   /// <summary>
   /// Save the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to save</param>
   public static void Save(string filepath = "")
   {
      _container.Save(filepath);
   }

   /// <summary>
   /// Delete all preferences, including the file.
   /// </summary>
   /// <param name="filepath">Preference file to delete</param>
   public static void Delete(string filepath = "")
   {
      _container.Delete(filepath);
   }

   /// <summary>
   /// Removes a key/value from the preferences.
   /// </summary>
   /// <param name="key">Key (and value) to delete</param>
   public static void Remove(string key)
   {
      _container.Remove(key);
   }

   /// <summary>
   /// Checks if a given key exists in the preferences.
   /// </summary>
   /// <param name="key">Key to check</param>
   /// <returns>True if the key exists in the preferences</returns>
   public static bool ContainsKey(string key)
   {
      return _container.ContainsKey(key);
   }

   #region Getter

   /// <summary>Get a string for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>String for the key</returns>
   public static string? GetString(string key, bool obfuscated = false)
   {
      return _container.Get(key, obfuscated)?.ToString();
   }

   /// <summary>Get an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Object for the key</returns>
   public static T GetObject<T>(string key, bool obfuscated = false)
   {
      return JsonHelper.DeserializeFromString<T>(GetString(key, obfuscated))!;
   }

   /// <summary>Get a number for a key.</summary>
   /// <param name="key">Key for the number</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Number for the key</returns>
   public static T GetNumber<T>(string key, bool obfuscated = false) where T : INumber<T>
   {
      if (string.IsNullOrEmpty(key))
         throw new ArgumentNullException(nameof(key));

      Type type = typeof(T);

      string? plainValue = GetString(key, obfuscated);

      if (plainValue == null)
         return T.CreateTruncating(0);

      switch (type)
      {
         case Type t when t == typeof(double):
            double doubleVal = double.Parse(plainValue);
            return T.CreateTruncating(doubleVal);
         case Type t when t == typeof(float):
            float floatVal = float.Parse(plainValue);
            return T.CreateTruncating(floatVal);
         case Type t when t == typeof(long):
            long longVal = long.Parse(plainValue);
            return T.CreateTruncating(longVal);
         case Type t when t == typeof(ulong):
            ulong ulongVal = ulong.Parse(plainValue);
            return T.CreateTruncating(ulongVal);
         case Type t when t == typeof(int):
            int intVal = int.Parse(plainValue);
            return T.CreateTruncating(intVal);
         case Type t when t == typeof(uint):
            uint uintVal = uint.Parse(plainValue);
            return T.CreateTruncating(uintVal);
         case Type t when t == typeof(short):
            short shortVal = short.Parse(plainValue);
            return T.CreateTruncating(shortVal);
         case Type t when t == typeof(ushort):
            ushort ushortVal = ushort.Parse(plainValue);
            return T.CreateTruncating(ushortVal);
         case Type t when t == typeof(nint):
            nint nintVal = nint.Parse(plainValue);
            return T.CreateTruncating(nintVal);
         case Type t when t == typeof(nuint):
            nint nuintVal = nint.Parse(plainValue);
            return T.CreateTruncating(nuintVal);
         case Type t when t == typeof(byte):
            byte byteVal = byte.Parse(plainValue);
            return T.CreateTruncating(byteVal);
         case Type t when t == typeof(sbyte):
            sbyte sbyteVal = sbyte.Parse(plainValue);
            return T.CreateTruncating(sbyteVal);
         case Type t when t == typeof(char):
            char charVal = char.Parse(plainValue);
            return T.CreateTruncating(charVal);
         default:
            _logger.LogWarning("Number type is not supported!");
            break;
      }

      return T.CreateTruncating(0);
   }

   /// <summary>Get a bool for a key.</summary>
   /// <param name="key">Key for the bol.</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Bool for the key</returns>
   public static bool GetBool(string key, bool obfuscated = false)
   {
      string? result = GetString(key, obfuscated);
      return result != null && "true".Equals(result.ToLower());
   }

   /// <summary>Get a DateTime for a key.</summary>
   /// <param name="key">Key for the DateTime.</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <param name="usedTZ">Time zone of the date (optional, default: local)</param>
   /// <returns>DateTime for the key</returns>
   public static DateTime GetDate(string key, bool obfuscated = false, TimeZoneInfo? usedTZ = null)
   {
      string? date = GetString(key, obfuscated);
      DateTime.TryParse(date, out DateTime dt);

      return dt; //.BNConvertUtcToTimeZone(usedTZ);
   }

   #endregion


   #region Setter

   /// <summary>Set a string for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="value">String for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   public static void Set(string key, string? value, bool obfuscated = false)
   {
      _container.Set(key, value, obfuscated);
   }

   /// <summary>Set an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="value">Object for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   public static void Set(string key, object value, bool obfuscated = false)
   {
      Set(key, JsonHelper.SerializeToString(value), obfuscated);
   }

   /// <summary>Set a number for a key.</summary>
   /// <param name="key">Key for the number</param>
   /// <param name="value">Number for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   public static void Set<T>(string key, T value, bool obfuscated = false) where T : INumber<T>
   {
      _container.Set(key, value, obfuscated);
   }

   /// <summary>Set a bool for a key.</summary>
   /// <param name="key">Key for the bool</param>
   /// <param name="value">Bool for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   public static void Set(string key, bool value, bool obfuscated = false)
   {
      _container.Set(key, value, obfuscated);
   }

   /// <summary>Set a DateTime for a key.</summary>
   /// <param name="key">Key for the DateTime</param>
   /// <param name="value">DateTime for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   public static void Set(string key, DateTime value, bool obfuscated = false)
   {
      string dt = JsonHelper.SerializeToString(value).Replace("\"", "");
      Set(key, dt, obfuscated);
   }

   #endregion

   #endregion

   #region Private methods

   private static void AppDomain_ProcessExit(object? sender, EventArgs e)
   {
      Save();
   }

   #endregion
}