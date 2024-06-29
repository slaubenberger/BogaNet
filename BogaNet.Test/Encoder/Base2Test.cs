using BogaNet.Encoder;

namespace BogaNet.Test.Encoder;

public class Base2Test
{
   #region Tests

   [Test]
   public void Base2_Test()
   {
      string plain = "BogaNet rülez!";

      //Byte-array
      string? output = Base2.ToBase2String(plain.BNToByteArray());
      string? plain2 = Base2.StringFromBase2String(output);
      Assert.That(plain, Is.EqualTo(plain2));

      //String
      output = Base2.ToBase2String(plain);
      plain2 = Base2.StringFromBase2String(output);
      Assert.That(plain, Is.EqualTo(plain2));

      //Integer Number
      long value = 123;
      output = Base2.ToBase2String(value);
      var value2 = Base2.NumberFromBase2String<long>(output);
      Assert.That(value, Is.EqualTo(value2));

      //FP Number
      decimal valueFP = 123.456m;
      output = Base2.ToBase2String(valueFP);
      var value2FP = Base2.NumberFromBase2String<decimal>(output);

      Assert.That(valueFP, Is.EqualTo(value2FP));
   }
   
   [Test]
   public void Base2_Ext_Test()
   {
      byte input = 0;
      string expected = "00000000";
      string? result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));

      input = 1;
      expected = "00000001";
      result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));

      input = 2;
      expected = "00000010";
      result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));

      input = 4;
      expected = "00000100";
      result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));

      input = 8;
      expected = "00001000";
      result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));

      input = 16;
      expected = "00010000";
      result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));

      input = 32;
      expected = "00100000";
      result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));

      input = 64;
      expected = "01000000";
      result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));

      input = 128;
      expected = "10000000";
      result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));

      input = 255;
      expected = "11111111";
      result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));

      input = 113;
      expected = "01110001";
      result = Base2.ToBase2String(input);

      Assert.That(result, Is.EqualTo(expected));
   }

   #endregion
}