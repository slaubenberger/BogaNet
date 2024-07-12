using Microsoft.Extensions.Logging;
using BogaNet.Helper;
using System.Threading.Tasks;

namespace BogaNet.TrueRandom;

/// <summary>
/// Gets the remaining quota from www.random.org.
/// </summary>
public abstract class CheckQuota //NUnit
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

   /// <summary>
   /// Gets the remaining quota in bits from the server.
   /// </summary>
   public static int GetQuota()
   {
      return Task.Run(GetQuotaAsync).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Gets the remaining quota in bits from the server asynchronously.
   /// </summary>
   public static async Task<int> GetQuotaAsync()
   {
      bool hasInternet = await NetworkHelper.CheckInternetAvailabilityAsync();

      if (!hasInternet)
      {
         _logger.LogWarning("No Internet access available - can not get quota!");
      }
      else
      {
         const string url = $"{BaseTRNG.GENERATOR_URL}quota/?format=plain";

         _logger.LogDebug("URL: " + url);

         string data = await NetworkHelper.ReadAllTextAsync(url);

         if (int.TryParse(data, out quota))
            return quota;

         _logger.LogWarning("Could not parse value to integer: " + data);
      }

      return 0;
   }

   #endregion
}