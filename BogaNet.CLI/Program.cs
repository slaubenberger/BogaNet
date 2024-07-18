using BogaNet.Extension;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using BogaNet.Helper;

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
   //public static void Main(string[] args)
   {
      GlobalLogging.LoggerFactory = new NLogLoggerFactory();

      _logger.LogDebug("Hi there, this is a test app!");

      Task task1 = Task1();
      Task task2 = Task2();
      Task task3 = testTTS();

      await Task.WhenAll(task1, task2, task3);


      //await testTTSAsync();
      //buildCode();
      //await testTrueRandom();
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
         _logger.LogWarning($"Application exited with an error code {code}");
      }

      NLog.LogManager.Shutdown();
      Environment.Exit(code);
   }

   #endregion

   #region Private methods

   public static async Task Task1()
   {
      await Task.Delay(1000);
      _logger.LogInformation("Finished Task1");
   }

   public static async Task Task2()
   {
      await Task.Delay(2000);
      _logger.LogInformation("Finished Task2");
   }

   private static async Task testTTS()
   {
      //var tts = BogaNet.TTS.Provider.OSXVoiceProvider.Instance;
      var tts = BogaNet.TTS.Speaker.Instance;
      tts.UseESpeak = false;

      if (Constants.IsOSX)
      {
         tts.ESpeakApplication = "/opt/local/bin/espeak-ng";
      }
      else if (Constants.IsWindows)
      {
         tts.ESpeakApplication = @"C:\Program Files\eSpeak NG\espeak-ng.exe";
      }

      var voices = await tts.GetVoicesAsync();

      //_logger.LogDebug(voices.BNDump());

      var voice = tts.VoiceForCulture("de");
      //var voice = tts.VoiceForCulture("hi");

      await tts.SpeakAsync("Hallo Ramon, wie geht es dir?", voice);
   }

   private static async Task testTTSAsync()
   {
      var tts = BogaNet.TTS.Provider.OSXVoiceProvider.Instance;

      //tts.Speak("Hello there!", new BogaNet.TTS.Model.Voice("Daniel", null, BogaNet.TTS.Model.Enum.Gender.MALE, "unknown", "en"));
      await tts.SpeakAsync("Hello there, I'm TTS for c-sharp");
   }

   private static void buildCode()
   {
      var lines = FileHelper.ReadAllLines("~/Desktop/Locales.csv");

      System.Text.StringBuilder sb = new();

      List<string> keys = [];

      foreach (var line in lines)
      {
         var split = line.Split(',');

         sb.Append("{ ");
         sb.Append(split[0]);
         sb.Append(", ");
         sb.Append(StringHelper.AddQuotation(split[1]));
         sb.AppendLine(" },");

         if (keys.Contains(split[0]))
         {
            Console.Error.WriteLine("Key already used: " + split[0]);
         }
         else
         {
            keys.Add(split[0]);
         }
      }

      FileHelper.WriteAllText("~/Desktop/Locales.code", sb.ToString());
   }

/*
   private static async Task testTrueRandom()
   {
      int quotaStart = await BogaNet.TrueRandom.CheckQuota.GetQuotaAsync();
      _logger.LogInformation($"Quota start: {quotaStart}");

      //var res = await BogaNet.TrueRandom.BytesTRNG.GenerateAsync(5);
      //var res = await BogaNet.TrueRandom.StringTRNG.GenerateAsync(5, 3);
      //var res = await BogaNet.TrueRandom.IntegerTRNG.GenerateAsync(-10, 10, 5);
      //var res = await BogaNet.TrueRandom.FloatTRNG.GenerateAsync(-10, 10, 5);
      var res = await BogaNet.TrueRandom.SequenceTRNG.GenerateAsync(-5, 5);

      //_logger.LogInformation(BogaNet.TrueRandom.BytesTRNG.CalcBits(5).ToString());
      //_logger.LogInformation(BogaNet.TrueRandom.StringTRNG.CalcBits(5, 3).ToString());
      //_logger.LogInformation("Calc: " + BogaNet.TrueRandom.IntegerTRNG.CalcBits(-10, 10, 5).ToString());
      //_logger.LogInformation("Calc: " + BogaNet.TrueRandom.FloatTRNG.CalcBits(5).ToString());
      _logger.LogInformation("Calc: " + BogaNet.TrueRandom.SequenceTRNG.CalcBits(-5, 5).ToString());

      foreach (var number in res)
      {
         _logger.LogInformation(number.ToString());
         //_logger.LogInformation(number.BNToString());
      }

      int quota = await BogaNet.TrueRandom.CheckQuota.GetQuotaAsync();
      _logger.LogInformation($"Quota end: {quota} - {quotaStart - quota}");
   }
*/
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