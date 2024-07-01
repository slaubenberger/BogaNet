using System.Numerics;
using System;

namespace BogaNet.Prefs;

/// <summary>
/// Interface for preferences of the application.
/// </summary>
public interface IPreferences
{
   /// <summary>
   /// Store the data automatically at application exit.
   /// </summary>
   bool AutoSaveOnExit { get; set; }

   /// <summary>
   /// Load the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to load</param>
   void Load(string filepath = "");

   /// <summary>
   /// Save the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to save</param>
   void Save(string filepath = "");

   /// <summary>
   /// Delete all preferences, including the file.
   /// </summary>
   /// <param name="filepath">Preference file to delete</param>
   void Delete(string filepath = "");

   /// <summary>
   /// Removes a key/value from the preferences.
   /// </summary>
   /// <param name="key">Key (and value) to delete</param>
   void Remove(string key);

   /// <summary>
   /// Checks if a given key exists in the preferences.
   /// </summary>
   /// <param name="key">Key to check</param>
   /// <returns>True if the key exists in the preferences</returns>
   bool ContainsKey(string key);

   #region Getter

   /// <summary>Get a string for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>String for the key</returns>
   string? GetString(string key, bool obfuscated = false);

   /// <summary>Get an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Object for the key</returns>
   T GetObject<T>(string key, bool obfuscated = false);

   /// <summary>Get a number for a key.</summary>
   /// <param name="key">Key for the number</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Number for the key</returns>
   T GetNumber<T>(string key, bool obfuscated = false) where T : INumber<T>;

   /// <summary>Get a bool for a key.</summary>
   /// <param name="key">Key for the bol.</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Bool for the key</returns>
   bool GetBool(string key, bool obfuscated = false);

   /// <summary>Get a DateTime for a key.</summary>
   /// <param name="key">Key for the DateTime.</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <param name="usedTZ">Time zone of the date (optional, default: local)</param>
   /// <returns>DateTime for the key</returns>
   DateTime GetDate(string key, bool obfuscated = false, TimeZoneInfo? usedTZ = null);

   #endregion


   #region Setter

   /// <summary>Set a string for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="value">String for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   void Set(string key, string? value, bool obfuscated = false);

   /// <summary>Set an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="value">Object for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   void Set(string key, object value, bool obfuscated = false);

   /// <summary>Set a number for a key.</summary>
   /// <param name="key">Key for the number</param>
   /// <param name="value">Number for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   void Set<T>(string key, T value, bool obfuscated = false) where T : INumber<T>;

   /// <summary>Set a bool for a key.</summary>
   /// <param name="key">Key for the bool</param>
   /// <param name="value">Bool for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   void Set(string key, bool value, bool obfuscated = false);

   /// <summary>Set a DateTime for a key.</summary>
   /// <param name="key">Key for the DateTime</param>
   /// <param name="value">DateTime for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   void Set(string key, DateTime value, bool obfuscated = false);

   #endregion
}