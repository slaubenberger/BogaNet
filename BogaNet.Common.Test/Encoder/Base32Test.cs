using BogaNet.Encoder;

namespace BogaNet.Test.Encoder;

public class Base32Test
{
   #region Tests

   [Test]
   public void Base32_Test()
   {
      string plain = "BogaNet rülez!";

      //Byte-array
      string? output = Base32.ToBase32String(plain.BNToByteArray());
      string? plain2 = Base32.StringFromBase32String(output);
      Assert.That(plain, Is.EqualTo(plain2));

      //String
      output = Base32.ToBase32String(plain);
      plain2 = Base32.StringFromBase32String(output);
      Assert.That(plain, Is.EqualTo(plain2));
   }

   #endregion
}