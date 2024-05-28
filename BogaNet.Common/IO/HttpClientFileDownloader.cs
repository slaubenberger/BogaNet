using Microsoft.Extensions.Logging;

namespace BogaNet.IO;

/// <summary>
/// HttpClient for file downloads with progress-callback
/// </summary>
public class HttpClientFileDownloader
{
   #region Variables

   private static readonly ILogger<HttpClientFileDownloader> _logger = GlobalLogging.CreateLogger<HttpClientFileDownloader>();

   /// <summary>Delegate for the progress changes.</summary>
   public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);

   /// <summary>Event triggered whenever the progress changes.</summary>
   public event ProgressChangedHandler? ProgressChanged;

   private string? _downloadUrl;
   private string? _destinationPath;

   #endregion

   #region Public methods

   /// <summary>
   /// Downloads the file
   /// </summary>
   /// <param name="downloadUrl">URL of the file</param>
   /// <param name="destinationPath">Destination for the file</param>
   /// <param name="timeout">Timeout in seconds (optional, default: 3600)</param>
   /// <returns>True if the operation was successful</returns>
   public async Task<bool> Download(string downloadUrl, string destinationPath, int timeout = 3600)
   {
      if (string.IsNullOrEmpty(downloadUrl))
         return false;

      if (string.IsNullOrEmpty(destinationPath))
         return false;

      try
      {
         _downloadUrl = NetworkHelper.ValidateURL(downloadUrl);
         _destinationPath = FileHelper.ValidateFile(destinationPath);

         using HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(timeout) };
         using HttpResponseMessage response = await _httpClient.GetAsync(_downloadUrl, HttpCompletionOption.ResponseHeadersRead);
         await downloadFileFromHttpResponseMessage(response);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not download file '{downloadUrl}'");
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
      if (_destinationPath != null)
      {
         long totalBytesRead = 0L;
         long readCount = 0L;
         byte[] buffer = new byte[8192];
         bool isMoreToRead = true;

         await using FileStream fileStream = new(_destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

         do
         {
            int bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);

            if (bytesRead == 0)
            {
               isMoreToRead = false;
               triggerProgressChanged(totalDownloadSize, totalBytesRead);
               continue;
            }

            await fileStream.WriteAsync(buffer, 0, bytesRead);

            totalBytesRead += bytesRead;
            readCount += 1;

            if (readCount % 100 == 0)
               triggerProgressChanged(totalDownloadSize, totalBytesRead);
         } while (isMoreToRead);
      }
   }

   private void triggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
   {
      if (ProgressChanged == null)
         return;

      double? progressPercentage = null;

      if (totalDownloadSize.HasValue)
         progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

      ProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage);
   }

   #endregion
}