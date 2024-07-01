using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using BogaNet.Extension;

namespace BogaNet.CLI;

/// <summary>
/// Main class for the CLI application.
/// This is mainly used for some quick and dirty experiments :-)
/// </summary>
public static class Program
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(Program));

   #region Public methods

   public static void Main(string[] args)
   {
      GlobalLogging.LoggerFactory = new NLogLoggerFactory();

      _logger.LogDebug("Hi there, this is a test app!");
      
      //testPrefs();
      //testNumber();
      //testBase16();
      //testBase32();
      //testDI();
      //testCrc();
      //testBitrateHRF();
      //testBytesHRF();
      //testDrive();
      //testNetwork();
      //testRSA();
      //testLocalizer();
      //testConvert();
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

   #endregion

   #region Private methods

/*
   private static void testPrefs()
   {
      string text = "Hello wörld!";
      string key = "text";

      BogaNet.Prefs.Preferences.Set(key, text);
      Console.WriteLine(BogaNet.Prefs.Preferences.GetString(key));

      string textObf = "Hello obfuscated wörld!";
      string keyObf = "textObf";

      BogaNet.Prefs.Preferences.Set(keyObf, textObf, true);
      Console.WriteLine(BogaNet.Prefs.Preferences.GetString(keyObf, true));

      double number = 12.345;
      string keyNumber = "number";

      BogaNet.Prefs.Preferences.Set(keyNumber, number);
      Console.WriteLine(BogaNet.Prefs.Preferences.GetNumber<double>(keyNumber).ToString());

      bool boolean = true;
      string keyBool = "boolean";

      BogaNet.Prefs.Preferences.Set(keyBool, boolean);
      _logger.LogInformation(BogaNet.Prefs.Preferences.GetBool(keyBool).ToString());

      DateTime date = DateTime.UtcNow;
      string keyDate = "date";

      //BogaNet.Prefs.Preferences.Set(keyDate, date, false);
      _logger.LogInformation(date.BNConvertToTimeZone().ToString());
      _logger.LogInformation(BogaNet.Prefs.Preferences.GetDate(keyDate, false, null).ToString());
   }

   private static void testDI()
   {
      BogaNet.Util.DIContainer.Bind<ILocalizer, Localizer>(Localizer.Instance);
      BogaNet.Util.DIContainer.Bind<ILocalizer, Localizer>(Localizer.Instance);

      _logger.LogInformation($"{BogaNet.Util.DIContainer.Resolve<ILocalizer>()}");

      BogaNet.Util.DIContainer.Unbind<ILocalizer>();

      _logger.LogInformation($"{BogaNet.Util.DIContainer.Resolve<ILocalizer>()}");
   }

   private static void testCrc()
   {
      string input1 = "Hallo Welt";
      string input2 = "Hallp Welt";

      var crc1 = CRCHelper.CRC16(input1.BNToByteArray());
      var crc2 = CRCHelper.CRC16(input2.BNToByteArray());

      _logger.LogInformation($"{crc1} - {crc2}");
   }
   private static void testRSA()
   {
      string text = "Hello there!";

      var cert = RSAHelper.GenerateSelfSignedCertificate("BogaTest");

      var certPrivateFile = BogaNet.Helper.FileHelper.TempFile;
      RSAHelper.WritePrivateCertificateToFile(certPrivateFile, cert, "W3NeedASaf3rPassw0rd");
      var certPrivate = RSAHelper.ReadCertificateFromFile(certPrivateFile, "W3NeedASaf3rPassw0rd");

      var certPublicFile = BogaNet.Helper.FileHelper.TempFile;
      RSAHelper.WritePublicCertificateToFile(certPublicFile, cert);
      var certPublic = RSAHelper.ReadCertificateFromFile(certPublicFile);

      var enc = RSAHelper.Encrypt(text.BNToByteArray(), certPublic);
      var dec = RSAHelper.Decrypt(enc, certPrivate);

      string result = dec.BNToString();

      _logger.LogInformation($"{text} - {result}");
   }
*/




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
/*
   private static void testNetwork()
   {
      _logger.LogInformation("CPD: " + BogaNet.Helper.NetworkHelper.CheckInternetAvailability());

      _logger.LogInformation("Ping: " + BogaNet.Helper.NetworkHelper.Ping("crosstales.com"));

      _logger.LogInformation("Public IP: " + BogaNet.Helper.NetworkHelper.GetPublicIP());

      _logger.LogInformation("Network adapters: " + BogaNet.Helper.NetworkHelper.GetNetworkAdapters().BNDump(false));
   }
*/

   #endregion
}
/*
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
*/