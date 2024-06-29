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
      string? plain2 = Base16.FromBase16String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      //TODO re-enable!
      //String
      output = Base16.ToBase16String(plain);
      plain2 = Base16.FromBase16String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      int iValue = 123;
      output = Base16.ToBase16String(iValue, true, true);
      var iRes = Base16.FromBase16String(output).BNToNumber<int>();
      Assert.That(iRes, Is.EqualTo(iValue));

      long lValue = 123;
      output = Base16.ToBase16String(lValue, true, true);
      var lRes = Base16.FromBase16String(output).BNToNumber<long>();
      Assert.That(lRes, Is.EqualTo(lValue));

      //FP Number
      decimal decValue = 123.456m;
      output = Base16.ToBase16String(decValue, true, true);
      var decRes = Base16.FromBase16String(output).BNToNumber<decimal>();
      Assert.That(decRes, Is.EqualTo(decValue));

      string hexi = Base16.ToBase16String((short)1);
      string str = "7B8"; //1976 without leading 0
       //str = hexi;
      var sRes = Base16.FromBase16String(str).BNToNumber<short>();
      Assert.That(sRes, Is.EqualTo(1976));
   }

   #endregion
}