namespace BogaNet;

/// <summary>
/// Information about the BogaNet library.
/// </summary>
public abstract class LibraryInformation
{
    private static readonly System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
    private static readonly System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);

    /// <summary>
    /// Version of the library.
    /// </summary>
    public static string? Version
    {
        get
        {
            string? version = fvi.ProductVersion;

            return version != null && version.Contains('+') ? version.Substring(0, version.IndexOf('+')) : version;
        }
    }

    /// <summary>
    /// Name of the library.
    /// </summary>
    public static string? Name => fvi.ProductName;

    /// <summary>
    /// Company of the library.
    /// </summary>
    public static string? Company => fvi.CompanyName;

    /// <summary>
    /// Copyright of the library..
    /// </summary>
    public static string? Copyright => fvi.LegalCopyright;
}