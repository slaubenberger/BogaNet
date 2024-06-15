using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using BogaNet.Crypto;
using BogaNet.Unit;
using System.Globalization;
using BogaNet.i18n;
using BogaNet.IO;
using System.Security.Cryptography.X509Certificates;
using BogaNet.Util;

namespace BogaNet.CLI;

/// <summary>
/// Main class for the CLI application
/// This is mainly used for some quick and dirty experiments
/// </summary>
public static class Program
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(Program));

   public static void Main(string[] args)
   {
      GlobalLogging.LoggerFactory = new NLogLoggerFactory();

      _logger.LogDebug("Hi there, this is a test app!");

      //testBitrateHRF();
      //testBytesHRF();
      //testDrive();
      testNetwork();
      //testRSA();
      //testLocalizer();
      //testConvert();
      //testObf();
      //testToString();

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

   private static void testBitrateHRF()
   {
      bool useSI = true;

      _logger.LogInformation(Helper.FormatBitrateToHRF(1000, useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF((long)Math.Pow(1000, 2), useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF((long)Math.Pow(1000, 3), useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF((long)Math.Pow(1000, 4), useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF((long)Math.Pow(1000, 5), useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF((long)Math.Pow(1000, 6), useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF(1024, useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF((long)Math.Pow(1024, 2), useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF((long)Math.Pow(1024, 3), useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF((long)Math.Pow(1024, 4), useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF((long)Math.Pow(1024, 5), useSI));
      _logger.LogInformation(Helper.FormatBitrateToHRF((long)Math.Pow(1024, 6), useSI));
   }
   
   private static void testBytesHRF()
   {
      bool useSI = true;

      _logger.LogInformation(Helper.FormatBytesToHRF(1000, useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF((long)Math.Pow(1000, 2), useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF((long)Math.Pow(1000, 3), useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF((long)Math.Pow(1000, 4), useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF((long)Math.Pow(1000, 5), useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF((long)Math.Pow(1000, 6), useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF(1024, useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF((long)Math.Pow(1024, 2), useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF((long)Math.Pow(1024, 3), useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF((long)Math.Pow(1024, 4), useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF((long)Math.Pow(1024, 5), useSI));
      _logger.LogInformation(Helper.FormatBytesToHRF((long)Math.Pow(1024, 6), useSI));
   }

   private static void testDrive() //yay, what a game :-)
   {
      //_logger.LogInformation("Drive details: " + FileHelper.GetDriveInfo().BNDump(false));
   }

   private static void testNetwork()
   {
      _logger.LogInformation("CPD: " + NetworkHelper.CheckInternetAvailability());
      
      _logger.LogInformation("Ping: " + NetworkHelper.Ping("crosstales.com"));
/*
      _logger.LogInformation("Public IP: " + NetworkHelper.GetPublicIP());

      _logger.LogInformation("Network adapters: " + NetworkHelper.GetNetworkAdapters().BNDump(false));
*/
   }

   private static void testRSA()
   {
      string text = "Hello there!";

      var cert = RSAHelper.GenerateSelfSignedCertificate("BogaTest");

      var certPrivateFile = FileHelper.TempFile;
      RSAHelper.WritePrivateCertificateToFile(certPrivateFile, cert, "W3NeedASaf3rPassw0rd");
      var certPrivate = RSAHelper.ReadCertificateFromFile(certPrivateFile, "W3NeedASaf3rPassw0rd");

      var certPublicFile = FileHelper.TempFile;
      RSAHelper.WritePublicCertificateToFile(certPublicFile, cert);
      var certPublic = RSAHelper.ReadCertificateFromFile(certPublicFile);

      var enc = RSAHelper.Encrypt(text.BNToByteArray(), certPublic);
      var dec = RSAHelper.Decrypt(enc, certPrivate);

      string result = dec.BNToString();

      _logger.LogInformation($"{text} - {result}");
   }


   private static void testLocalizer()
   {
      var sw = new BogaNet.Util.StopWatch();

      sw.Start();
      var loc = Localizer.Instance;
      loc.LoadFiles("./Resources/Translation.csv", "./Resources/Translation_de.csv");
      sw.AddPoint("Load");

      loc.Add("GreetingText2", new CultureInfo("en"), "Hello world");
      loc.Add("GreetingText", new CultureInfo("it"), "Ciao!");
      sw.AddPoint("Add");

      //loc.Remove("GreetingText");

      loc.Culture = new CultureInfo("en");
      _logger.LogInformation(loc.GetText("GreetingText"));
      _logger.LogInformation(loc.GetText("GreetingText2"));
      _logger.LogInformation(loc.GetText("GreetingText2", TextType.TOOLTIP));
      sw.AddPoint("en");

      loc.Culture = new CultureInfo("de");
      _logger.LogInformation(loc.GetText("GreetingText"));
      _logger.LogInformation(loc.GetTextWithReplacements("GreetingText2", TextType.LABEL, "1000"));
      _logger.LogInformation(loc.GetText("GreetingText2", TextType.TOOLTIP));
      sw.AddPoint("de");

      loc.Culture = new CultureInfo("it");
      _logger.LogInformation(loc.GetText("GreetingText"));
      _logger.LogInformation(loc.GetText("GreetingText2"));
      _logger.LogInformation(loc.GetText("GreetingText2", TextType.TOOLTIP));
      sw.AddPoint("it");

      sw.Stop();
      _logger.LogInformation($"{sw.ElapsedTime} - {sw.PointsAndTime.BNDump(false)}");
      //loc.SaveFile("./MyTranslations.csv");
   }

   private static void testConvert()
   {
      const float inVal = 1;

      var outVal = UnitArea.M2.BNConvert(UnitArea.FOOT2, inVal);

      _logger.LogInformation($"{inVal} => {outVal}");
   }


   private static void testObf()
   {
      //string plain = Helper.CreateString("ハローワールド", 50);
      string plain = Constants.SIGNS_EXT.BNCreateString(80);
      //string plain = Helper.CreateString(Constants.SIGNS, 80);

      string? obf = Obfuscator.Obfuscate(plain, 23);
      string? plainAgain = Obfuscator.Deobfuscate(obf, 23);

      _logger.LogInformation($"'{plain}'");
      _logger.LogInformation($"'{obf}'");
      _logger.LogInformation($"'{plainAgain}' - Equals: {plain.Equals(plainAgain)}");
   }

   private static void testToString()
   {
      //
      //List<TestModel> tm =
      TestModel[] tm =
      [
         new TestModel(),
         new TestModel(),
         new TestModel()
         //TestModel tm = new TestModel();
      ];

      _logger.LogInformation(tm.BNDump());
      //TestModel tm = new TestModel();

      _logger.LogInformation(tm.BNToString());
   }
}

public class TestModel
{
   public string PublicString = "PublicString";
   public static string PublicStaticString = "PublicStaticString";
   private string privateString = "privateString";
   private static string privateStaticString = "privateStaticString";

   public string PublicProp => "PublicProp";
   public static string PublicStaticProp => "PublicStaticProp";
   private string privateProp => "privateProp";
   private static string privateStaticProp => "privateStaticProp";
}