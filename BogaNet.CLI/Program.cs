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

   public static void Main(string[] args)
   {
      GlobalLogging.LoggerFactory = new NLogLoggerFactory();

      _logger.LogDebug("Hi there, this is a test app!");

      testNumber();
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

   private static void testNumber()
   {
      char c = 'Q';
      var cBytes = c.BNToByteArray();

      char cNew = cBytes.BNToNumber<char>();

      _logger.LogInformation($"{c} - {cNew}");

      sbyte s = -123;
      var sBytes = s.BNToByteArray();

      sbyte sNew = sBytes.BNToNumber<sbyte>();

      _logger.LogInformation($"{s} - {sNew}");

      decimal number = -1234.56789m;

      byte[]? bytes = number.BNToByteArray();

      _logger.LogInformation($"{bytes?.Length}");

      decimal numberCopy = bytes.BNToNumber<decimal>();

      _logger.LogInformation($"{number} - {numberCopy}");
   }
/*
   private static void testBase16()
   {
      string test = "Grüezi wohl!";

      string base16 = Base16.ToBase16String(test, true);

      _logger.LogInformation($"Base16: {base16} - {base16.Length}");

      //base16 = Base16.ToBase16String(255, true);
      //_logger.LogInformation($"Base16: {base16} - {base16.Length}");

      string fromBase16 = Base16.StringFromBase16String(base16);

      _logger.LogInformation($"FromBase16: {fromBase16} - {fromBase16.Equals(test)}");
   }

   private static void testBase32()
   {
      string test = "Grüezi wohl!";

      string base64 = Base64.ToBase64String(test);
      string base32 = Base32.ToBase32String(test);

      _logger.LogInformation($"Base64: {base64} - {base64.Length}");
      _logger.LogInformation($"Base32: {base32} - {base32.Length}");

      string fromBase64 = Base64.StringFromBase64String(base64);
      string fromBase32 = Base32.StringFromBase32String(base32);

      _logger.LogInformation($"FromBase64: {fromBase64} - {fromBase64.Equals(test)}");
      _logger.LogInformation($"FromBase32: {fromBase32} - {fromBase32.Equals(test)}");
   }

   private static void testDI()
   {
      DIContainer.Bind<ILocalizer, Localizer>(Localizer.Instance);
      DIContainer.Bind<ILocalizer, Localizer>(Localizer.Instance);

      _logger.LogInformation($"{DIContainer.Resolve<ILocalizer>()}");

      DIContainer.Unbind<ILocalizer>();

      _logger.LogInformation($"{DIContainer.Resolve<ILocalizer>()}");
   }

   private static void testCrc()
   {
      string input1 = "Hallo Welt";
      string input2 = "Hallp Welt";

      var crc1 = CRCHelper.CRC16(input1.BNToByteArray());
      var crc2 = CRCHelper.CRC16(input2.BNToByteArray());

      _logger.LogInformation($"{crc1} - {crc2}");
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

   private static void testDrive() //yay, what a game :-)
   {
      //_logger.LogInformation("Drive details: " + FileHelper.GetDriveInfo().BNDump(false));
   }

   private static void testNetwork()
   {
      _logger.LogInformation("CPD: " + NetworkHelper.CheckInternetAvailability());

      _logger.LogInformation("Ping: " + NetworkHelper.Ping("crosstales.com"));

      _logger.LogInformation("Public IP: " + NetworkHelper.GetPublicIP());

      _logger.LogInformation("Network adapters: " + NetworkHelper.GetNetworkAdapters().BNDump(false));
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

      var outVal = AreaUnit.M2.Convert(AreaUnit.FOOT2, inVal);

      _logger.LogInformation($"{inVal} => {outVal}");
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
   */
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