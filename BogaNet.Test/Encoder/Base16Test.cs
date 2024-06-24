using BogaNet.Encoder;

namespace BogaNet.Test.Encoder;

public class Base16Test
{
   #region Tests

   [Test]
   public void Base16_Test()
   {
      string plain = "BogaNet rülez!";

      //Byte-array
      string? output = Base16.ToBase16String(plain.BNToByteArray(), true);
      string? plain2 = Base16.StringFromBase16String(output);
      Assert.That(plain, Is.EqualTo(plain2));

      //String
      output = Base16.ToBase16String(plain);
      plain2 = Base16.StringFromBase16String(output);
      Assert.That(plain, Is.EqualTo(plain2));

      //Integer Number
      long value = 123;
      output = Base16.ToBase16String(value, true, true);
      var value2 = Base16.NumberFromBase16String<long>(output);
      Assert.That(value, Is.EqualTo(value2));

      //FP Number
      decimal valueFP = 123.456m;
      output = Base16.ToBase16String(valueFP, true, true);
      var value2FP = Base16.NumberFromBase16String<decimal>(output);

      Assert.That(valueFP, Is.EqualTo(value2FP));
   }

   #endregion
}