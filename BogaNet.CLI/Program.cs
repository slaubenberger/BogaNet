using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace BogaNet.CLI;

/// <summary>
/// Main class for the CLI application
/// </summary>
public static class Program
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger("Program");

   public static void Main(string[] args)
   {
      GlobalLogging.LoggerFactory = new NLogLoggerFactory();

      _logger.LogTrace("Hi there, this is a test app!");

      //testObf();
      testToString();

      Exit(0);
   }

   public static void Exit(int code)
   {
      NLog.LogManager.Shutdown();
      Environment.Exit(code);
   }

   private static void testObf()
   {
      //string plain = Helper.CreateString("ハローワールド", 50);
      string plain = Helper.CreateString(Constants.SIGNS_EXT, 80);
      //string plain = Helper.CreateString(Constants.SIGNS, 80);

      string? obf = Obfuscator.Obfuscate(plain, 23);
      string? plainAgain = Obfuscator.Deobfuscate(obf, 23);

      Console.WriteLine($"'{plain}'");
      Console.WriteLine($"'{obf}'");
      Console.WriteLine($"'{plainAgain}' - Equals: {plain.Equals(plainAgain)}");

      Console.WriteLine();
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

      Console.WriteLine(tm.CTDump());
      //TestModel tm = new TestModel();

      Console.WriteLine(tm.CTToString());

      Console.WriteLine();
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