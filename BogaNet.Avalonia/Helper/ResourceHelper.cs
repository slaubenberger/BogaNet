using System.Reflection;
using System;
using System.IO;
using Avalonia.Platform;
using BogaNet.Extension;
using System.Threading.Tasks;

namespace BogaNet.Helper;

/// <summary>
/// Helper for resources in Avalonia.
/// </summary>
public abstract class ResourceHelper //TODO make partial?
{
   //private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ResourceHelper));

   #region Properties

   /// <summary>
   /// Main assembly containing the resources
   /// </summary>
   public static string? ResourceAssembly { get; set; }

   #endregion

   #region Public methods

   /// <summary>
   /// Validates a given resource path.
   /// </summary>
   /// <param name="resourcePath">Resource path to validate</param>
   /// <param name="resourceAssembly">Assembly with the resource (optional, default: ResourceAssembly)</param>
   /// <returns>Validated resource path</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ValidateResource(string resourcePath, string? resourceAssembly = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(resourcePath);

      if (!resourcePath.BNStartsWith("avares://"))
      {
         ResourceAssembly ??= Assembly.GetEntryAssembly()?.GetName().Name;

         return $"avares://{resourceAssembly ?? ResourceAssembly}/{resourcePath.TrimStart('/')}";
      }

      return resourcePath;
   }

   /// <summary>
   /// Reads a resource as text.
   /// </summary>
   /// <param name="resourcePath"></param>
   /// <param name="resourceAssembly">Assembly with the resource (optional, default: ResourceAssembly)</param>
   /// <returns>Text from the given resource</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string LoadText(string resourcePath, string? resourceAssembly = null)
   {
      return Task.Run(() => LoadTextAsync(resourcePath, resourceAssembly)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Reads a resource as text asynchronously.
   /// </summary>
   /// <param name="resourcePath"></param>
   /// <param name="resourceAssembly">Assembly with the resource (optional, default: ResourceAssembly)</param>
   /// <returns>Text from the given resource</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static async Task<string> LoadTextAsync(string resourcePath, string? resourceAssembly = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(resourcePath);

      Uri fileUri = new(ValidateResource(resourcePath, resourceAssembly));
      using StreamReader streamReader = new(AssetLoader.Open(fileUri));
      return await streamReader.ReadToEndAsync();
   }

   /// <summary>
   /// Reads a resource as binary.
   /// </summary>
   /// <param name="resourcePath"></param>
   /// <param name="resourceAssembly">Assembly with the resource (optional, default: ResourceAssembly)</param>
   /// <returns>Binary data from the given resource</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] LoadBinary(string resourcePath, string? resourceAssembly = null)
   {
      return Task.Run(() => LoadBinaryAsync(resourcePath, resourceAssembly)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Reads a resource as binary asynchronously.
   /// </summary>
   /// <param name="resourcePath"></param>
   /// <param name="resourceAssembly">Assembly with the resource (optional, default: ResourceAssembly)</param>
   /// <returns>Binary data from the given resource</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static async Task<byte[]> LoadBinaryAsync(string resourcePath, string? resourceAssembly = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(resourcePath);

      Uri fileUri = new(ValidateResource(resourcePath, resourceAssembly));
      await using BufferedStream streamReader = new(AssetLoader.Open(fileUri));
      return await streamReader.BNReadFullyAsync();
   }

   #endregion
}