using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base91Test
{
   #region Tests

   [Test]
   public void Base91_Test()
   {
      const string plain = "BogaNet rülez!";
      string output;
      string plain2;

      //BogaNet.Util.StopWatch watch = new();
      //watch.Start();
      for (int ii = 0; ii < 10000; ii++)
      {
         //Byte-array
         output = Base91.ToBase91String(plain.BNToByteArray());
         plain2 = Base91.FromBase91String(output).BNToString();
         Assert.That(plain2, Is.EqualTo(plain));
      }

      //watch.Stop();
      //Console.WriteLine(watch.ElapsedTime);

      //String
      output = Base91.ToBase91String(plain);
      byte[] bytes = Base91.FromBase91String(output);
      plain2 = bytes.BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "_q2fL3&Y$F=Ec18jOEB";
      plain2 = Base91.FromBase91String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}