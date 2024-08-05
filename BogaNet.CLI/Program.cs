using BogaNet.BWF;
using BogaNet.BWF.Filter;
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

      await testBWF2();
      //await testBWF();
      //testTTS();
      //testPrefs();

      /*
      Task task1 = Task1();
      Task task2 = Task2();
      Task task3 = testTTS();

      await Task.WhenAll(task1, task2, task3);
      */

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

   private static async Task testBWF2()
   {
      await BadWordFilter.Instance.LoadFilesAsync(true, new Tuple<string, string>("en", "./Resources/Filters/ltr/en.txt"), new Tuple<string, string>("de", "./Resources/Filters/ltr/de.txt"));

      do
      {
         await Task.Delay(100);
      } while (!BadWordFilter.Instance.IsLoaded);

      await DomainFilter.Instance.LoadFilesAsync(new Tuple<string, string>("domains", "./Resources/Filters/domains.txt"));

      do
      {
         await Task.Delay(100);
      } while (!DomainFilter.Instance.IsLoaded);

      string foulText = "MARTIANS are assholes.... arsch => watch mypage.com => badguy@evilmail.com";

      bool contains = Pacifier.Instance.Contains(foulText);
      _logger.LogInformation("Contains: " + contains);

      //var allBaddies = Pacifier.Instance.GetAll(foulText);
      //_logger.LogInformation(allBaddies.BNDump());

      string removedProfanity = Pacifier.Instance.ReplaceAll(foulText);
      _logger.LogInformation(removedProfanity);
   }

   private static async Task testBWF()
   {
      string foulText = "MARTIANS are assholes.... => watch mypage.com => badguy@evilmail.com";

      string removedCapitalization = CapitalizationFilter.Instance.ReplaceAll(foulText);

      _logger.LogInformation(removedCapitalization);

      string removedPunctuation = PunctuationFilter.Instance.ReplaceAll(removedCapitalization);

      _logger.LogInformation(removedPunctuation);

      await BadWordFilter.Instance.LoadFilesAsync(true, new Tuple<string, string>("en", "./Resources/Filters/ltr/en.txt"));

      do
      {
         await Task.Delay(100);
      } while (!BadWordFilter.Instance.IsLoaded);

      string removedProfanity = BadWordFilter.Instance.ReplaceAll(removedPunctuation);

      //BadWordFilter.Instance.SimpleCheck = true;
      //_logger.LogInformation("CONTAINS: " + BadWordFilter.Instance.Contains(foulText));

      _logger.LogInformation(removedProfanity);

      await DomainFilter.Instance.LoadFilesAsync(new Tuple<string, string>("domains", "./Resources/Filters/domains.txt"));

      do
      {
         await Task.Delay(100);
      } while (!DomainFilter.Instance.IsLoaded);

      string removedDomain = DomainFilter.Instance.ReplaceAll(removedProfanity);

      _logger.LogInformation(removedDomain);
   }

   private static void testPrefs()
   {
      BogaNet.Prefs.Preferences.Instance.Load();

      if (BogaNet.Prefs.Preferences.Instance.IsLoaded)
      {
         _logger.LogInformation("ready!");

         _logger.LogInformation(BogaNet.Prefs.Preferences.Instance.GetString("user"));
      }

      BogaNet.Prefs.Preferences.Instance.Set("user", "ueli " + DateTime.Now);
   }

   private static async Task Task1()
   {
      await Task.Delay(1000);
      _logger.LogInformation("Finished Task1");
   }

   private static async Task Task2()
   {
      await Task.Delay(2000);
      _logger.LogInformation("Finished Task2");
   }

   private static void testTTS()
   {
      //await FileHelper.WriteAllTextAsync("WindowsWrapper.txt", await Base64.Base64FromFileAsync("./contentFiles/BogaNetTTSWrapper.exe"));
      //_logger.LogDebug(Base64.ToBase64String(HashHelper.HashSHA256File("./contentFiles/BogaNetTTSWrapper.exe")));

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

      var voices = tts.GetVoices();

      //_logger.LogDebug(voices.BNDump());

      var voice = tts.VoiceForCulture("de");
      //var voice = tts.VoiceForCulture("hi");

      tts.Speak("Hallo Ramon, wie geht es dir? Was für ein wunderbarer Tag!", voice, 1, 1, 1, true, true);

      Thread.Sleep(1000);

      tts.Speak("Hallo Stefan, wie geht es dir? Was für ein wunderbarer Tag!", voice, 1, 1, 1, true, true);

      Thread.Sleep(2000);

      tts.Silence();
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

   private static void testNetwork()
   {
      _logger.LogInformation("CPD: " + BogaNet.Helper.NetworkHelper.CheckInternetAvailability());

      _logger.LogInformation("Ping: " + BogaNet.Helper.NetworkHelper.Ping("crosstales.com"));

      _logger.LogInformation("Public IP: " + BogaNet.Helper.NetworkHelper.GetPublicIP());

      _logger.LogInformation("Network adapters: " + BogaNet.Helper.NetworkHelper.GetNetworkAdapters().BNDump(false));
   }

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

   #endregion
}