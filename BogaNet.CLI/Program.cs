using BogaNet.Extension;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace BogaNet.CLI;

/// <summary>
/// Main class for the CLI application.
/// This is mainly used for some quick and dirty experiments :-)
/// </summary>
public static class Program
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(Program));

   #region Public methods

   public static async Task Main(string[] args)
   {
      GlobalLogging.LoggerFactory = new NLogLoggerFactory();

      _logger.LogDebug("Hi there, this is a test app!");

      await testTrueRandom();
      //testNetwork();
      //testBitrateHRF();
      //testBytesHRF();

      Exit(0);
   }

   public static void Exit(int code)
   {
      if (code == 0)
      {
         _logger.LogDebug($"Application exited with code {code}");
      }
      else
      {
         _logger.LogError($"Application exited with an error code {code}");
      }

      NLog.LogManager.Shutdown();
      Environment.Exit(code);
   }

   #endregion

   #region Private methods

   private static async Task testTrueRandom()
   {
      int quotaStart = await BogaNet.TrueRandom.CheckQuota.GetQuotaAsync();
      _logger.LogInformation($"Quota start: {quotaStart}");

      //var res = await BogaNet.TrueRandom.TRNGBytes.GenerateAsync(5);
      //var res = await BogaNet.TrueRandom.TRNGString.GenerateAsync(5);
      //var res = await BogaNet.TrueRandom.TRNGInteger.GenerateAsync(1, 10, 3);
      //var res = await BogaNet.TrueRandom.TRNGFloat.GenerateAsync(1, 10, 5);
      var res = await BogaNet.TrueRandom.TRNGSequence.GenerateAsync(1, 10);

      //_logger.LogInformation(BogaNet.TrueRandom.TRNGBytes.CalcBits(5).ToString());
      //_logger.LogInformation(BogaNet.TrueRandom.TRNGString.CalcBits(5).ToString());
      //_logger.LogInformation("Calc: " + BogaNet.TrueRandom.TRNGInteger.CalcBits(10, 3).ToString());
      //_logger.LogInformation("Calc: " + BogaNet.TrueRandom.TRNGFloat.CalcBits(5).ToString());
      _logger.LogInformation("Calc: " + BogaNet.TrueRandom.TRNGSequence.CalcBits(1, 10).ToString());

      foreach (var number in res)
      {
         _logger.LogInformation(number.ToString());
         //_logger.LogInformation(number.BNToString());
      }

      int quota = await BogaNet.TrueRandom.CheckQuota.GetQuotaAsync();
      _logger.LogInformation($"Quota end: {quota} - {quotaStart - quota}");
   }

   private static void testNetwork()
   {
      _logger.LogInformation("CPD: " + BogaNet.Helper.NetworkHelper.CheckInternetAvailability());

      _logger.LogInformation("Ping: " + BogaNet.Helper.NetworkHelper.Ping("crosstales.com"));

      _logger.LogInformation("Public IP: " + BogaNet.Helper.NetworkHelper.GetPublicIP());

      _logger.LogInformation("Network adapters: " + BogaNet.Helper.NetworkHelper.GetNetworkAdapters().BNDump(false));
   }

/*
   private static void testBitrateHRF()
   {
      bool useSI = true;

      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF(1000, useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1000, 2), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1000, 3), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1000, 4), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1000, 5), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1000, 6), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF(1024, useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1024, 2), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1024, 3), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1024, 4), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1024, 5), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1024, 6), useSI));
   }
*/
/*
   private static void testBytesHRF()
   {
      bool useSI = true;

      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF(1000, useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF((long)Math.Pow(1000, 2), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF((long)Math.Pow(1000, 3), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF((long)Math.Pow(1000, 4), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF((long)Math.Pow(1000, 5), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF((long)Math.Pow(1000, 6), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF(1024, useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF((long)Math.Pow(1024, 2), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF((long)Math.Pow(1024, 3), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF((long)Math.Pow(1024, 4), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF((long)Math.Pow(1024, 5), useSI));
      _logger.LogInformation(BogaNet.Helper.GeneralHelper.FormatBytesToHRF((long)Math.Pow(1024, 6), useSI));
   }
*/

   #endregion
}