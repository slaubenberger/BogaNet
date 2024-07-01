using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base32Test
{
   #region Tests

   [Test]
   public void Base32_Test()
   {
      const string plain = "BogaNet rülez!";

      //Byte-array
      string? output = Base32.ToBase32String(plain.BNToByteArray());
      string? plain2 = Base32.FromBase32String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      //String
      output = Base32.ToBase32String(plain);
      plain2 = Base32.FromBase32String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "IJXWOYKOMV2CA4WDXRWGK6RB";
      plain2 = Base32.FromBase32String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}