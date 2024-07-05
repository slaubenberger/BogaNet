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
      //var res = await BogaNet.TrueRandom.ModuleString.Generate(5);
      //var res = await BogaNet.TrueRandom.ModuleInteger.Generate(1, 10, 3);
      //var res = await BogaNet.TrueRandom.ModuleFloat.Generate(1, 10, 3);
      var res = await BogaNet.TrueRandom.TRNGSequence.Generate(1, 5);

      foreach (var number in res)
      {
         _logger.LogInformation(number.ToString());
      }

      int quota = await BogaNet.TrueRandom.CheckQuota.GetQuota();

      _logger.LogInformation(quota.ToString());
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