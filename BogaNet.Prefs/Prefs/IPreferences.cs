using System.Numerics;
using System;

namespace BogaNet.Prefs;

/// <summary>
/// Interface for preferences of the application.
/// </summary>
public interface IPreferences
{
   #region Properties

   /// <summary>
   /// Are the preferences loaded?
   /// </summary>
   bool IsLoaded { get; }

   /// <summary>
   /// Are the current preferences saved?
   /// </summary>
   bool IsSaved { get; }

   #endregion

   #region Methods

   /// <summary>
   /// Removes a key/value from the preferences.
   /// </summary>
   /// <param name="key">Key (and value) to delete</param>
   /// <exception cref="ArgumentNullException"></exception>
   bool Remove(string key);

   /// <summary>
   /// Checks if a given key exists in the preferences.
   /// </summary>
   /// <param name="key">Key to check</param>
   /// <returns>True if the key exists in the preferences</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool ContainsKey(string key);

   #region Getter

   /// <summary>Get a string for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>String for the key</returns>
   /// <exception cref="ArgumentNullException"></exception>
   string GetString(string key, bool obfuscated = false);

   /// <summary>Tries to get a string for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool TryGetString(string key, out string result, bool obfuscated = false);

   /// <summary>Get an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Object for the key</returns>
   /// <exception cref="ArgumentNullException"></exception>
   T GetObject<T>(string key, bool obfuscated = false);

   /// <summary>Tries to get an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool TryGetObject<T>(string key, out T result, bool obfuscated = false);

   /// <summary>Get a number for a key.</summary>
   /// <param name="key">Key for the number</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Number for the key</returns>
   /// <exception cref="ArgumentNullException"></exception>
   T GetNumber<T>(string key, bool obfuscated = false) where T : INumber<T>;

   /// <summary>Tries to get a number for a key.</summary>
   /// <param name="key">Key for the number</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool TryGetNumber<T>(string key, out T result, bool obfuscated = false) where T : INumber<T>;

   /// <summary>Get a bool for a key.</summary>
   /// <param name="key">Key for the bol.</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Bool for the key</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool GetBool(string key, bool obfuscated = false);

   /// <summary>Tries to get a bool for a key.</summary>
   /// <param name="key">Key for the bol.</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool TryGetBool(string key, out bool result, bool obfuscated = false);

   /// <summary>Get a DateTime for a key.</summary>
   /// <param name="key">Key for the DateTime.</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <param name="usedTZ">Time zone of the date (optional, default: local)</param>
   /// <returns>DateTime for the key</returns>
   /// <exception cref="ArgumentNullException"></exception>
   DateTime GetDate(string key, bool obfuscated = false, TimeZoneInfo? usedTZ = null);

   /// <summary>Tries to get a DateTime for a key.</summary>
   /// <param name="key">Key for the DateTime.</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <param name="usedTZ">Time zone of the date (optional, default: local)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool TryGetDate(string key, out DateTime result, bool obfuscated = false, TimeZoneInfo? usedTZ = null);

   #endregion

   #region Setter

   /// <summary>Set a string for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="value">String for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <exception cref="ArgumentNullException"></exception>
   void Set(string key, string value, bool obfuscated = false);

   /// <summary>Set an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="value">Object for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <exception cref="ArgumentNullException"></exception>
   void Set(string key, object value, bool obfuscated = false);

   /// <summary>Set a number for a key.</summary>
   /// <param name="key">Key for the number</param>
   /// <param name="value">Number for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <exception cref="ArgumentNullException"></exception>
   void Set<T>(string key, T value, bool obfuscated = false) where T : INumber<T>;

   /// <summary>Set a bool for a key.</summary>
   /// <param name="key">Key for the bool</param>
   /// <param name="value">Bool for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <exception cref="ArgumentNullException"></exception>
   void Set(string key, bool value, bool obfuscated = false);

   /// <summary>Set a DateTime for a key.</summary>
   /// <param name="key">Key for the DateTime</param>
   /// <param name="value">DateTime for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <exception cref="ArgumentNullException"></exception>
   void Set(string key, DateTime value, bool obfuscated = false);

   #endregion

   #endregion
}