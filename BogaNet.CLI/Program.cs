using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

      //fileRenamer();
      fileMerger();

      //await testBWF2();
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

   private static void fileRenamer()
   {
      var files = FileHelper.GetFiles("/Volumes/NAS_Public/video/series/Arrested Development", true);

      foreach (var file in files)
      {
         string newFile = file.Replace("[", "- ").Replace("]", " -");
         _logger.LogWarning(file + " -> " + newFile);
         FileHelper.RenameFile(file, newFile);
      }
   }

   private static void fileMerger()
   {
      _logger.LogInformation("File merge started");

      string outputDir = "/Volumes/NAS_Public/video/movies hd";
      var dirs = FileHelper.GetDirectories("/Volumes/NAS_Public/video/_replace/");

      foreach (var dir in dirs)
      {
         string dirName = FileHelper.GetDirectoryName(dir);
         _logger.LogInformation(dir);
         mergeMtsFiles(dir, $"{outputDir}/{dirName}.m2ts");

         //break;
      }

      _logger.LogInformation("File merge finished!");
   }

   private static void mergeMtsFiles(string sourceDirectory, string outputFile)
   {
      const int bufferSize = 128 * 1024 * 1024;

      try
      {
         // Get all M2TS files in the directory
         var mtsFiles = FileHelper.GetFiles(sourceDirectory, true, "m2ts")
            .OrderBy(f => f)
            .ToList();

         if (mtsFiles.Count == 0)
         {
            _logger.LogWarning($"No MTS files found in the specified directory: {sourceDirectory}");
            return;
         }

         //return;

         // Create a FileStream for the output file
         using (var outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize))
         {
            byte[] buffer = new byte[bufferSize];
            long totalBytes = 0;
            long totalSize = mtsFiles.Sum(f => new FileInfo(f).Length);

            foreach (var file in mtsFiles)
            {
               using (var inputStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize))
               {
                  int bytesRead;
                  while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                  {
                     outputStream.Write(buffer, 0, bytesRead);
                     totalBytes += bytesRead;
                     reportProgress(totalBytes, totalSize);
                  }
               }

               Console.WriteLine($"\nMerged: {Path.GetFileName(file)}");
            }
         }

         _logger.LogInformation($"All files merged successfully: {outputFile}");
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "An error occurred!");
      }
   }

   private static void reportProgress(long current, long total)
   {
      int percentage = (int)((current * 100) / total);
      Console.Write($"\rProgress: {percentage}% [{current}/{total} bytes]");
   }

/*
   private static async Task testBWF2()
   {
      //await BadWordFilter.Instance.LoadFilesAsync(true, new Tuple<string, string>("en", "./Resources/Filters/ltr/en.txt"), new Tuple<string, string>("de", "./Resources/Filters/ltr/de.txt"));
      await BadWordFilter.Instance.LoadFilesAsync(true, BWFConstants.BWF_LTR);
      await BadWordFilter.Instance.LoadFilesAsync(false, BWFConstants.BWF_RTL);

      do
      {
         await Task.Delay(100);
      } while (!BadWordFilter.Instance.IsLoaded);

      await DomainFilter.Instance.LoadFilesAsync(BWFConstants.DOMAINS);

      do
      {
         await Task.Delay(100);
      } while (!DomainFilter.Instance.IsLoaded);

      const string foulText = "MARTIANS are assholes/arschlöcher/culo!!!!!!!!!!  => WATCH: https//mytruthpage.com/weirdowatch/martians123.divx or WRITE an EMAIL: weirdo@gmail.com";

      bool contains = Pacifier.Instance.Contains(foulText);
      _logger.LogInformation("Contains: " + contains);

      //var allBaddies = Pacifier.Instance.GetAll(foulText);
      //_logger.LogInformation(allBaddies.BNDump());

      string removedProfanity = Pacifier.Instance.ReplaceAll(foulText);
      _logger.LogInformation(removedProfanity);
   }

   private static async Task testBWF()
   {
      const string foulText = "MARTIANS are assholes.... => watch mypage.com => badguy@evilmail.com";

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
*/
/*
   private static void testPrefs()
   {
      Prefs.Preferences.Instance.Load();

      if (Prefs.Preferences.Instance.IsLoaded)
      {
         _logger.LogInformation("ready!");

         _logger.LogInformation(Prefs.Preferences.Instance.GetString("user"));
      }

      Prefs.Preferences.Instance.Set("user", "ueli " + DateTime.Now);
   }
*/
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

/*
   private static void testTTS()
   {
      //await FileHelper.WriteAllTextAsync("WindowsWrapper.txt", await Base64.Base64FromFileAsync("./contentFiles/BogaNetTTSWrapper.exe"));
      //_logger.LogDebug(Base64.ToBase64String(HashHelper.HashSHA256File("./contentFiles/BogaNetTTSWrapper.exe")));

      //var tts = BogaNet.TTS.Provider.OSXVoiceProvider.Instance;
      var tts = TTS.Speaker.Instance;
      tts.UseESpeak = false;

      if (Constants.IsOSX)
      {
         TTS.Speaker.ESpeakApplication = "/opt/local/bin/espeak-ng";
      }
      else if (Constants.IsWindows)
      {
         TTS.Speaker.ESpeakApplication = @"C:\Program Files\eSpeak NG\espeak-ng.exe";
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
*/
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
      _logger.LogInformation("CPD: " + NetworkHelper.CheckInternetAvailability());

      _logger.LogInformation("Ping: " + NetworkHelper.Ping("crosstales.com"));

      _logger.LogInformation("Public IP: " + NetworkHelper.GetPublicIP());

      _logger.LogInformation("Network adapters: " + NetworkHelper.GetNetworkAdapters().BNDump(false));
   }

   private static void testBitrateHRF()
   {
      bool useSI = true;

      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF(1000, useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1000, 2), useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1000, 3), useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1000, 4), useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1000, 5), useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1000, 6), useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF(1024, useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1024, 2), useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1024, 3), useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1024, 4), useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1024, 5), useSI));
      _logger.LogInformation(GeneralHelper.FormatBitrateToHRF((long)Math.Pow(1024, 6), useSI));
   }

   private static void testBytesHRF()
   {
      bool useSI = true;

      _logger.LogInformation(GeneralHelper.FormatBytesToHRF(1000, useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF((long)Math.Pow(1000, 2), useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF((long)Math.Pow(1000, 3), useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF((long)Math.Pow(1000, 4), useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF((long)Math.Pow(1000, 5), useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF((long)Math.Pow(1000, 6), useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF(1024, useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF((long)Math.Pow(1024, 2), useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF((long)Math.Pow(1024, 3), useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF((long)Math.Pow(1024, 4), useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF((long)Math.Pow(1024, 5), useSI));
      _logger.LogInformation(GeneralHelper.FormatBytesToHRF((long)Math.Pow(1024, 6), useSI));
   }

   #endregion
}