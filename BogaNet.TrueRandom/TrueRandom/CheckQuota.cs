using System;
using Microsoft.Extensions.Logging;

namespace BogaNet.TrueRandom;

/// <summary>
/// This module gets the remaining quota on www.random.org.
/// </summary>
public abstract class CheckQuota
{
   #region Variables

   private static readonly ILogger<CheckQuota> _logger = GlobalLogging.CreateLogger<CheckQuota>();

   private static int quota = 1000000;

   #endregion


   #region Static properties

   /// <summary>Returns the remaining quota in bits from the last check.</summary>
   /// <returns>Remaining quota in bits from the last check.</returns>
   public static int Quota => quota;

   #endregion


   #region Public methods

   /// <summary>Gets the remaining quota in bits from the server.</summary>
   public static async System.Threading.Tasks.Task<int> GetQuota()
   {
      string url = $"{TrueRandomNumberGenerator.GENERATOR_URL}quota/?format=plain";

      _logger.LogDebug("URL: " + url);


      using System.Net.Http.HttpClient client = new();
      using System.Net.Http.HttpResponseMessage response = client.GetAsync(url).Result;
      response.EnsureSuccessStatusCode();

      if (response.IsSuccessStatusCode)
      {
         string data = await response.Content.ReadAsStringAsync();

         if (int.TryParse(data, out quota))
            return quota;

         _logger.LogError("Could not parse value to integer: " + data);
      }
      else
      {
         _logger.LogError($"Could not download data: {response.StatusCode} - {response.ReasonPhrase}");
      }

      return 0;
   }

   #endregion
}