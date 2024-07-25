namespace BogaNet.Prefs;

/// <summary>
/// Preferences for Avalonia.
/// </summary>
public class AvaloniaPreferences : Preferences
{
   #region Constructor

   protected AvaloniaPreferences()
   {
      _container = new AvaloniaPreferencesContainer();
   }

   #endregion
}