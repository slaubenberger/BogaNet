using System.Runtime.InteropServices.JavaScript;

namespace BogaNet.Helper;

/// <summary>
/// Helper for URL-operations in Avalonia (browser).
/// </summary>
public partial class UrlHelper
{
   /// <summary>
   /// Set/get the URL of the application.
   /// </summary>
   public static string URL
   {
      get => GetUrl();
      set => SetUrl(value);
   }

   [JSImport("setUrl", "boganet_url")]
   internal static partial void SetUrl(string url);

   [JSImport("getUrl", "boganet_url")]
   internal static partial string GetUrl();
}