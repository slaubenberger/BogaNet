using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace BogaNet.Avalonia.Helper;

using Microsoft.Extensions.Logging;

/// <summary>
/// Helper for images in Avalonia.
/// </summary>
public abstract class ImageHelper
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ImageHelper));

   /// <summary>
   /// Loads an image from a given resource path.
   /// </summary>
   /// <param name="imageResource">Resource path of the image</param>
   /// <returns>Loaded image as Bitmap</returns>
   public static Bitmap LoadFromResource(string imageResource)
   {
      return new Bitmap(AssetLoader.Open(new Uri(ResourceHelper.ValidateResource(imageResource))));
   }

   /// <summary>
   /// Loads an image from a given url asynchronously.
   /// </summary>
   /// <param name="imageUrl">URL of the image</param>
   /// <returns>Loaded image as Bitmap</returns>
   public static async Task<Bitmap?> LoadFromUrl(string imageUrl)
   {
      using var client = new HttpClient();

      try
      {
         byte[] data = await client.GetByteArrayAsync(imageUrl);
         return new Bitmap(new MemoryStream(data));
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"An error occurred while downloading the image '{imageUrl}'");
         return null;
      }
   }
}