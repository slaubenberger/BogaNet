using System.Reflection;
using Microsoft.Extensions.Logging;

namespace BogaNet.Avalonia.Helper;

/// <summary>
/// Helper for resources in Avalonia.
/// </summary>
public abstract class ResourceHelper
{
   //private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ResourceHelper));

   /// <summary>
   /// Validates a given resource path.
   /// </summary>
   /// <param name="resourcePath">Resource path to validate</param>
   /// <returns>Validated resource path</returns>
   public static string ValidateResource(string resourcePath)
   {
      if (!resourcePath.BNStartsWith("avares://"))
      {
         string? assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
         return $"avares://{assemblyName}/{resourcePath.TrimStart('/')}";
      }

      return resourcePath;
   }
}