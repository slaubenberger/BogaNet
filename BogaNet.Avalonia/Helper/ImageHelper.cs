using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Microsoft.Extensions.Logging;

namespace BogaNet.Helper;

/// <summary>
/// Helper for images in Avalonia.
/// </summary>
public class ImageHelper //TODO make partial?
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ImageHelper));

   #region Public methods

   /// <summary>
   /// Loads an image from a given resource path.
   /// </summary>
   /// <param name="imageResource">Resource path of the image</param>
   /// <param name="resourceAssembly">Assembly with the resource (optional, default: ResourceAssembly)</param>
   /// <returns>Loaded image as Bitmap</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static Bitmap LoadFromResource(string imageResource, string? resourceAssembly = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(imageResource);

      return new Bitmap(AssetLoader.Open(new Uri(ResourceHelper.ValidateResource(imageResource, resourceAssembly))));
   }

   /// <summary>
   /// Loads an image from a given url.
   /// </summary>
   /// <param name="imageUrl">URL of the image</param>
   /// <returns>Loaded image as Bitmap</returns>
   /// <exception cref="Exception"></exception>
   public static Bitmap LoadFromUrl(string imageUrl)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(imageUrl);

      try
      {
         byte[] data = NetworkHelper.ReadAllBytes(imageUrl);

         return new Bitmap(new MemoryStream(data));
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"An error occurred while downloading the image '{imageUrl}'");
         throw;
      }
   }

   /// <summary>
   /// Loads an image from a given url asynchronously.
   /// </summary>
   /// <param name="imageUrl">URL of the image</param>
   /// <returns>Loaded image as Bitmap</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<Bitmap> LoadFromUrlAsync(string imageUrl)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(imageUrl);

      try
      {
         byte[] data = await NetworkHelper.ReadAllBytesAsync(imageUrl);

         return new Bitmap(new MemoryStream(data));
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"An error occurred while downloading the image '{imageUrl}'");
         throw;
      }
   }

   #endregion
}