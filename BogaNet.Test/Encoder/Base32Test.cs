using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base32Test
{
   #region Tests

   [Test]
   public void Base32_Test()
   {
      const string plain = TestConstants.LatinText;

      string output;
      string plain2;

      //BogaNet.Util.StopWatch watch = new();
      //watch.Start();
      for (int ii = 0; ii < 10000; ii++)
      {
         //Byte-array
         output = Base32.ToBase32String(plain.BNToByteArray());
         plain2 = Base32.FromBase32String(output).BNToString();
         Assert.That(plain2, Is.EqualTo(plain));
      }

      //watch.Stop();
      //Console.WriteLine(watch.ElapsedTime);

      //String
      output = Base32.ToBase32String(plain);
      plain2 = Base32.FromBase32String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "IFBEGRCFIZDUQSKKJNGE2TSPKBIVEU2UKVLFOWCZLLBYBQ4CYOCMHBWDQ7BYRQ4JYOFMHC6DR3BY7Q4UYWJMHGODTPBZYYLCMNSGKZTHNBUWU23MNVXG64DROJZXI5LWO54HS6WDUDB2FQ5EYOTMHJ6DVDB2TQ5KYOV4HLWDV7B3JRMTYO44HO6DXQYDCMRTGQ2TMNZYHE======";
      plain2 = Base32.FromBase32String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   [Test]
   public void Base32_NonLatinTest()
   {
      const string plain = TestConstants.NonLatinText;

      string output = Base32.ToBase32String(plain);
      string plain2 = Base32.FromBase32String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "4OBY7Y4DVXRYHPHDQOX6HA544OB2XY4DREQSBZFYS3TZLDHGQKUOLJN5566IDYFYVPQLRJ7AXCY6BOEU4C4JJYFYWXQLRCXAXCZOBOFH4C4YFYFYUXQLRAJB";
      plain2 = Base32.FromBase32String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}