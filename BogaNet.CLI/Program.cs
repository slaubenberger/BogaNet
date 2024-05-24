using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using BogaNet.Util;
using BogaNet.Crypto;
using BogaNet.Unit;

namespace BogaNet.CLI;

/// <summary>
/// Main class for the CLI application
/// This is mainly used for some quick and dirty experiments
/// </summary>
public static class Program
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger("Program");

   public static void Main(string[] args)
   {
      GlobalLogging.LoggerFactory = new NLogLoggerFactory();

      _logger.LogDebug("Hi there, this is a test app!");
/*
      ushort v1 = 123;
      var bytes = v1.BNToByteArray();
      ushort v2 = bytes.BNToNumber<ushort>();
      
      _logger.LogDebug($"{v1} - {v2}");
*/
/*
      _logger.LogInformation(BogaNet.IO.FileHelper.TempDirectory);
      _logger.LogInformation(BogaNet.IO.FileHelper.TempFile);
      _logger.LogInformation(BogaNet.IO.FileHelper.CurrentDirectory);
      _logger.LogInformation(BogaNet.IO.FileHelper.TempPath);
*/
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

   private static void testConvert()
   {
      const float inVal = 1;

      var outVal = UnitArea.M2.BNConvert(UnitArea.FOOT2, inVal);

      _logger.LogInformation($"{inVal} => {outVal}");
   }


   private static void testObf()
   {
      //string plain = Helper.CreateString("ハローワールド", 50);
      string plain = Helper.CreateString(Constants.SIGNS_EXT, 80);
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