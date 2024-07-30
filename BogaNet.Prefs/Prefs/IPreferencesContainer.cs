using System.Threading.Tasks;
using System.Collections.Generic;

namespace BogaNet.Prefs;

/// <summary>
/// Interface for containers of the application preferences.
/// </summary>
public interface IPreferencesContainer
{
   #region Properties

   /// <summary>
   /// Is the preferences-container loaded?
   /// </summary>
   bool IsLoaded { get; }

   /// <summary>
   /// Is the current preferences-container saved?
   /// </summary>
   bool IsSaved { get; }

   #endregion

   #region Methods

   /// <summary>
   /// Load the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to load</param>
   ///<returns>True if the operation was successful</returns>
   bool Load(string filepath = "");

   /// <summary>
   /// Load the preference file asynchronously.
   /// </summary>
   /// <param name="filepath">Preference file to load</param>
   ///<returns>True if the operation was successful</returns>
   Task<bool> LoadAsync(string filepath = "");

   /// <summary>
   /// Save the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to save</param>
   ///<returns>True if the operation was successful</returns>
   bool Save(string filepath = "");

   /// <summary>
   /// Save the preference file asynchronously.
   /// </summary>
   /// <param name="filepath">Preference file to save</param>
   ///<returns>True if the operation was successful</returns>
   Task<bool> SaveAsync(string filepath = "");

   /// <summary>
   /// Delete all preferences, including the file.
   /// </summary>
   /// <param name="filepath">Preference file to delete</param>
   ///<returns>True if the operation was successful</returns>
   bool Delete(string filepath = "");

   /// <summary>
   /// Removes a key/value from the preferences.
   /// </summary>
   /// <param name="key">Key (and value) to delete</param>
   ///<returns>True if the operation was successful</returns>
   bool Remove(string key);

   /// <summary>
   /// Checks if a given key exists in the preferences.
   /// </summary>
   /// <param name="key">Key to check</param>
   /// <returns>True if the key exists in the preferences</returns>
   bool ContainsKey(string key);

   /// <summary>Get an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>Object for the key</returns>
   /// <exception cref="KeyNotFoundException"></exception>
   object Get(string key, bool obfuscated);

   /// <summary>Tries to get an object for a key.</summary>
   /// <param name="key">Key for the object</param>
   /// <param name="result">out parameter for the result</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   /// <returns>True if the operation was successful</returns>
   bool TryGet(string key, out object result, bool obfuscated);

   /// <summary>Set an object for a key.</summary>
   /// <param name="key">Key for the string</param>
   /// <param name="value">Object for the preferences</param>
   /// <param name="obfuscated">Obfuscate value in the preferences (optional, default: false)</param>
   void Set(string key, object value, bool obfuscated);

   #endregion
}