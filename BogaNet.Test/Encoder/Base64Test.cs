using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base64Test
{
   #region Tests

   [Test]
   public void Base64_Test()
   {
      const string plain = "BogaNet rülez!";
      string output;
      string plain2;

      //BogaNet.Util.StopWatch watch = new();
      //watch.Start();
      for (int ii = 0; ii < 10000; ii++)
      {
         //Byte-array
         output = Base64.ToBase64String(plain.BNToByteArray());
         plain2 = Base64.FromBase64String(output).BNToString();
         Assert.That(plain2, Is.EqualTo(plain));
      }

      //watch.Stop();
      //Console.WriteLine(watch.ElapsedTime);

      //String
      output = Base64.ToBase64String(plain);
      byte[] bytes = Base64.FromBase64String(output);
      plain2 = bytes.BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "Qm9nYU5ldCByw7xsZXoh";
      plain2 = Base64.FromBase64String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}