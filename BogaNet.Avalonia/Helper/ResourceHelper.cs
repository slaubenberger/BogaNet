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
   /// Main assembly containing the resources
   /// </summary>
   public static string? ResourceAssembly { get; set; }

   /// <summary>
   /// Validates a given resource path.
   /// </summary>
   /// <param name="resourcePath">Resource path to validate</param>
   /// <param name="resourceAssembly">Assembly with the resource (optional, default: ResourceAssembly)</param>
   /// <returns>Validated resource path</returns>
   public static string ValidateResource(string resourcePath, string? resourceAssembly = null)
   {
      if (!resourcePath.BNStartsWith("avares://"))
      {
         if (ResourceAssembly == null)
            ResourceAssembly = Assembly.GetEntryAssembly()?.GetName().Name;

         return $"avares://{resourceAssembly ?? ResourceAssembly}/{resourcePath.TrimStart('/')}";
      }

      return resourcePath;
   }

   /// <summary>
   /// Reads a resource as text.
   /// </summary>
   /// <param name="resourcePath"></param>
   /// <param name="resourceAssembly">Assembly with the resource (optional, default: ResourceAssembly)</param>
   /// <returns></returns>
   public static string LoadText(string resourcePath, string? resourceAssembly = null)
   {
      Uri fileUri = new(ValidateResource(resourcePath, resourceAssembly));
      using StreamReader streamReader = new(AssetLoader.Open(fileUri));
      return streamReader.ReadToEnd();
   }

   /// <summary>
   /// Reads a resource as binary.
   /// </summary>
   /// <param name="resourcePath"></param>
   /// <param name="resourceAssembly">Assembly with the resource (optional, default: ResourceAssembly)</param>
   /// <returns></returns>
   public static byte[] LoadBinary(string resourcePath, string? resourceAssembly = null)
   {
      Uri fileUri = new(ValidateResource(resourcePath, resourceAssembly));
      using BufferedStream streamReader = new(AssetLoader.Open(fileUri));
      return streamReader.BNReadFully();
   }
}