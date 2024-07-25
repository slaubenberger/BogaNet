using BogaNet.Helper;
using System.Collections.Generic;
using System;
using System.IO.IsolatedStorage;
using System.IO;

namespace BogaNet.Prefs;

/// <summary>
/// Container for Avalonia preferences.
/// </summary>
public class AvaloniaPreferencesContainer : PreferencesContainer //NUnit
{
   #region Variables

   private static IsolatedStorageFile Store => IsolatedStorageFile.GetUserStoreForDomain();

   #endregion

   #region Public methods

   public override bool Load(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      if (Store.FileExists(_file))
      {
         using var stream = Store.OpenFile(_file, FileMode.Open);
         using var sw = new StreamReader(stream);
         sw.ReadToEnd();

         Dictionary<string, object> prefs = JsonHelper.DeserializeFromString<Dictionary<string, object>>(sw.ReadToEnd());

         _preferences = prefs;

         return true;
      }

      return false;
   }

   public override bool Save(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      try
      {
         using var stream = Store.OpenFile(_file, FileMode.Create, FileAccess.Write);
         using var sw = new StreamWriter(stream);
         sw.Write(JsonHelper.SerializeToString(_preferences));

         return true;
      }
      catch (Exception)
      {
         return false;
      }
   }

   public override bool Delete(string filepath = "")
   {
      if (!string.IsNullOrEmpty(filepath))
         _file = filepath;

      _preferences.Clear();

      if (Store.FileExists(_file))
         Store.DeleteFile(_file);

      return true;
   }

   #endregion
}