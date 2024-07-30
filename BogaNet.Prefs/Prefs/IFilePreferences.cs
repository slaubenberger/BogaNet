using System.Threading.Tasks;
using System;

namespace BogaNet.Prefs;

/// <summary>
/// Interface for file-based preferences of the application.
/// </summary>
public interface IFilePreferences : IPreferences
{
   #region Properties

   /// <summary>
   /// Store the data automatically at application exit.
   /// </summary>
   bool AutoSaveOnExit { get; set; }

   #endregion

   #region Events

   /// <summary>
   /// Delegate for the load status of the file.
   /// </summary>
   delegate void FileLoaded(string file);

   /// <summary>
   /// Event triggered whenever the file is loaded.
   /// </summary>
   event FileLoaded OnFileLoaded;

   /// <summary>
   /// Delegate for the save status of the file.
   /// </summary>
   delegate void FileSaved(string file);

   /// <summary>
   /// Event triggered whenever the file is saved.
   /// </summary>
   event FileSaved OnFileSaved;

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

   #endregion
}