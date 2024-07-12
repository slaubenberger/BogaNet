using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Net.Http;
using BogaNet.Helper;

namespace BogaNet.Util;

/// <summary>
/// HttpClient for file downloads with progress-callback.
/// </summary>
public class HttpClientFileDownloader
{
   #region Variables

   private static readonly ILogger<HttpClientFileDownloader> _logger = GlobalLogging.CreateLogger<HttpClientFileDownloader>();

   private string? _downloadUrl;
   private string _destinationPath = string.Empty;

   #endregion

   #region Events

   /// <summary>
   /// Delegate for the progress changes.
   /// </summary>
   public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);

   /// <summary>
   /// Event triggered whenever the progress changes.
   /// </summary>
   public event ProgressChangedHandler? OnProgressChanged;

   #endregion

   #region Public methods

   /// <summary>
   /// Downloads a file.
   /// </summary>
   /// <param name="downloadUrl">URL of the file</param>
   /// <param name="destinationPath">Destination for the file</param>
   /// <param name="timeout">Timeout in seconds (optional, default: 3600)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public async Task<bool> Download(string downloadUrl, string destinationPath, int timeout = 3600)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(downloadUrl);
      ArgumentNullException.ThrowIfNullOrEmpty(destinationPath);

      try
      {
         _downloadUrl = NetworkHelper.ValidateURL(downloadUrl);
         _destinationPath = FileHelper.ValidateFile(destinationPath);

         using HttpClient _httpClient = new();
         _httpClient.Timeout = TimeSpan.FromSeconds(timeout);
         using HttpResponseMessage response = await _httpClient.GetAsync(_downloadUrl, HttpCompletionOption.ResponseHeadersRead);
         await downloadFileFromHttpResponseMessage(response);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not download file: {downloadUrl}");
         throw;
      }

      return true;
   }

   #endregion

   #region Private methods

   private async Task downloadFileFromHttpResponseMessage(HttpResponseMessage response)
   {
      response.EnsureSuccessStatusCode();

      long? totalBytes = response.Content.Headers.ContentLength;

      await using Stream contentStream = await response.Content.ReadAsStreamAsync();
      await processContentStream(totalBytes, contentStream);
   }

   private async Task processContentStream(long? totalDownloadSize, Stream contentStream)
   {
      long totalBytesRead = 0L;
      long readCount = 0L;
      byte[] buffer = new byte[8192];
      bool isMoreToRead = true;

      await using FileStream fileStream = new(_destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

      do
      {
         int bytesRead = await contentStream.ReadAsync(buffer);

         if (bytesRead == 0)
         {
            isMoreToRead = false;
            triggerProgressChanged(totalDownloadSize, totalBytesRead);
            continue;
         }

         await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));

         totalBytesRead += bytesRead;
         readCount += 1;

         if (readCount % 100 == 0)
            triggerProgressChanged(totalDownloadSize, totalBytesRead);
      } while (isMoreToRead);
   }

   private void triggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
   {
      double? progressPercentage = null;

      if (totalDownloadSize.HasValue)
         progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

      OnProgressChanged?.Invoke(totalDownloadSize, totalBytesRead, progressPercentage);
   }

   #endregion
}