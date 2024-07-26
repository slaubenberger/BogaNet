using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base64Test
{
   #region Tests

   [Test]
   public void Base64_Test()
   {
      const string plain = Constants.SIGNS_EXT;
      string output;
      string plain2;

      //BogaNet.Util.StopWatch watch = new();
      //watch.Start();
      // for (int ii = 0; ii < 10000; ii++)
      // {
      //Byte-array
      output = Base64.ToBase64String(plain.BNToByteArray());
      plain2 = Base64.FromBase64String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
      // }

      //watch.Stop();
      //Console.WriteLine(watch.ElapsedTime);

      //String
      output = Base64.ToBase64String(plain);
      byte[] bytes = Base64.FromBase64String(output);
      plain2 = bytes.BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "QUJDREVGR0hJSktMTU5PUFFSU1RVVldYWVrDgMOCw4TDhsOHw4jDicOKw4vDjsOPw5TFksOZw5vDnGFiY2RlZmdoaWprbG1ub3BxcnN0dXZ3eHl6w6DDosOkw6bDp8Oow6nDqsOrw67Dr8O0xZPDucO7w7wwMTIzNDU2Nzg5";
      plain2 = Base64.FromBase64String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   [Test]
   public void Base64_NonLatinTest()
   {
      const string plain = TestConstants.NonLatinText;

      string output = Base64.ToBase64String(plain);
      string plain2 = Base64.FromBase64String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "44OP44Ot44O844Ov44O844Or44OJISDkuJbnlYzmgqjlpb3vvIHguKvguKfguLHguJTguJTguLXguIrguLLguKfguYLguKXguIEh";
      plain2 = Base64.FromBase64String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}