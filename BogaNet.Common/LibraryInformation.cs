using System.Reflection;
using System.Diagnostics;

namespace BogaNet;

/// <summary>
/// Information about the BogaNet library.
/// </summary>
public abstract class LibraryInformation
{
   #region Variables

   private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
   private static readonly FileVersionInfo _fvi = FileVersionInfo.GetVersionInfo(_assembly.Location);

   #endregion

   #region Properties

   /// <summary>
   /// Version of the library.
   /// </summary>
   public static string? Version
   {
      get
      {
         string? version = _fvi.ProductVersion;

         return version != null && version.Contains('+') ? version.Substring(0, version.IndexOf('+')) : version;
      }
   }

   /// <summary>
   /// Name of the library.
   /// </summary>
   public static string? Name => _fvi.ProductName;

   /// <summary>
   /// Company of the library.
   /// </summary>
   public static string? Company => _fvi.CompanyName;

   /// <summary>
   /// Copyright of the library.
   /// </summary>
   public static string? Copyright => _fvi.LegalCopyright;

   #endregion
}