using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base91Test
{
   #region Tests

   [Test]
   public void Base91_Test()
   {
      const string plain = Constants.SIGNS_EXT;
      string output;
      string plain2;

      //BogaNet.Util.StopWatch watch = new();
      //watch.Start();
      // for (int ii = 0; ii < 10000; ii++)
      // {
      //Byte-array
      output = Base91.ToBase91String(plain.BNToByteArray());
      plain2 = Base91.FromBase91String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
      // }

      //watch.Stop();
      //Console.WriteLine(watch.ElapsedTime);

      //String
      output = Base91.ToBase91String(plain);
      byte[] bytes = Base91.FromBase91String(output);
      plain2 = bytes.BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "fG^F%w_o%5qOdwQbFrzd[5eYAP;gMP+fNCS!rv.T$)^K2I*){J%?YIG^j?AF/!3zr.$_xvGJc!Qz},gkqa?<W<P[1Tx*,m{oQ#tKn`zTZ/[w31.)YKY]8c`2kiOFcpp9fsSRCxLU9=F_31MRQztEml0o[2c";
      plain2 = Base91.FromBase91String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   [Test]
   public void Base91_NonLatinTest()
   {
      const string plain = TestConstants.NonLatinText;

      string output = Base91.ToBase91String(plain);
      string plain2 = Base91.FromBase91String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "`KM@&C#URXYyz_F@T@w|2e@VR*RtCKA149h5DC<fQ:N(yT{>}g)T`b{xWfA@wGB%5B*mB)d8rnf,?J~6..sLAo6Dv=LvA";
      plain2 = Base91.FromBase91String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}