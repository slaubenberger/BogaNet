using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using BogaNet.ObfuscatedType;

namespace BogaNet.Prefs;

/// <summary>
/// Interface for containers of the application preferences.
/// </summary>
public interface IPreferencesContainer
{
   #region Properties

   /// <summary>
   /// IV for the obfuscated data.
   /// </summary>
   ByteObf IV { set; }

   /// <summary>
   /// Is the preferences-container loaded?
   /// </summary>
   bool IsLoaded { get; }

   /// <summary>
   /// Is the current preferences-container saved?
   /// </summary>
   bool IsSaved { get; }

   /// <summary>
   /// Current keys of the preferences.
   /// </summary>
   List<string> Keys { get; }

   /// <summary>
   /// Current count of keys from the preferences.
   /// </summary>
   int Count { get; }

   #endregion

   #region Methods

   /// <summary>
   /// Load the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to load</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   bool Load(string filepath = "");

   /// <summary>
   /// Load the preference file asynchronously.
   /// </summary>
   /// <param name="filepath">Preference file to load</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   Task<bool> LoadAsync(string filepath = "");

   /// <summary>
   /// Save the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to save</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   bool Save(string filepath = "");

   /// <summary>
   /// Save the preference file asynchronously.
   /// </summary>
   /// <param name="filepath">Preference file to save</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   Task<bool> SaveAsync(string filepath = "");

   /// <summary>
   /// Delete all preferences, including the file.
   /// </summary>
   /// <param name="filepath">Preference file to delete</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   bool Delete(string filepath = "");

   /// <summary>
   /// Removes a key/value from the preferences.
   /// </summary>
   /// <param name="key">Key (and value) to delete</param>
   ///<returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool Remove(string key);

   /// <summary>
   /// Checks if a given key exists in the preferences.
   /// </summary>
   /// <param name="key">Key to check</param>
   /// <returns>True if the key exists in the preferences</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool ContainsKey(string key);

   /// <summary>Get an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Object for the key</returns>
   /// <exception cref="ArgumentNullException"></exception>
   object Get(string key, bool obfuscated = false);

   /// <summary>Tries to get an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   bool TryGet(string key, out object result, bool obfuscated = false);

   /// <summary>Set an object for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="value">Object for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <exception cref="ArgumentNullException"></exception>
   void Set(string key, object value, bool obfuscated = false);

   #endregion
}