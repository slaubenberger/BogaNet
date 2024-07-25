namespace BogaNet.Prefs;

/// <summary>
/// Interface for file-based preferences of the application.
/// </summary>
public interface IFilePreferences : IPreferences
{
   /// <summary>
   /// Store the data automatically at application exit.
   /// </summary>
   bool AutoSaveOnExit { get; set; }

   /// <summary>
   /// Load the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to load</param>
   ///<returns>True if the operation was successful</returns>
   bool Load(string filepath = "");

   /// <summary>
   /// Save the preference file.
   /// </summary>
   /// <param name="filepath">Preference file to save</param>
   ///<returns>True if the operation was successful</returns>
   bool Save(string filepath = "");

   /// <summary>
   /// Delete all preferences, including the file.
   /// </summary>
   /// <param name="filepath">Preference file to delete</param>
   ///<returns>True if the operation was successful</returns>
   bool Delete(string filepath = "");
}