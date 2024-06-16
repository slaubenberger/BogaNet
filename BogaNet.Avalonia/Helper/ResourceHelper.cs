using System.Reflection;
using System;
using System.IO;
using Avalonia.Platform;

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

   /// <summary>
   /// Reads a resource as text.
   /// </summary>
   /// <param name="resourcePath"></param>
   /// <returns></returns>
   public static string LoadText(string resourcePath)
   {
      Uri fileUri = new(ValidateResource(resourcePath));
      using StreamReader streamReader = new(AssetLoader.Open(fileUri));
      return streamReader.ReadToEnd();
   }

   /// <summary>
   /// Reads a resource as binary.
   /// </summary>
   /// <param name="resourcePath"></param>
   /// <returns></returns>
   public static byte[] LoadBinary(string resourcePath)
   {
      Uri fileUri = new(ValidateResource(resourcePath));
      using BufferedStream streamReader = new(AssetLoader.Open(fileUri));
      return streamReader.BNReadFully();
   }
}