using BogaNet.Extension;

namespace BogaNet.Test.Extension;

public class ArrayExtensionTest
{
   [Test]
   public void BNToString_Test()
   {
      string input = "crosstales LLC";
      byte[]? bytes = input.BNToByteArray();
      string? result = bytes.BNToString();

      Assert.That(result, Is.EqualTo(input));

      input = "こんにちは";
      bytes = input.BNToByteArray();
      result = bytes.BNToString();

      Assert.That(result, Is.EqualTo(input));
   }

/*
   [Test]
   public void BNToStringLength_Test()
   {
      string input = "crosstales LLC";
      byte[]? bytes = input.BNToByteArray();
      string? result = bytes.BNToString(null,0, 4);

      Assert.That(result, Is.EqualTo(input.Substring(0, 4)));

      input = "こんにちは";
      bytes = input.BNToByteArray();
      result = bytes.BNToString(null,0, 3); //3 bytes are one char

      Assert.That(result, Is.EqualTo(input.Substring(0, 1)));
   }
*/
   [Test]
   public void BNToNumber_Byte_Test()
   {
      byte bNumber = byte.MinValue;
      byte[]? bytes = bNumber.BNToByteArray();
      byte bRes = bytes.BNToNumber<byte>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = 113;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<byte>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = byte.MaxValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<byte>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      //overflow test
      bytes = ulong.MaxValue.BNToByteArray();
      bRes = bytes.BNToNumber<byte>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToNumber_SByte_Test()
   {
      sbyte bNumber = sbyte.MaxValue;
      byte[]? bytes = bNumber.BNToByteArray();
      sbyte bRes = bytes.BNToNumber<sbyte>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = -113;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<sbyte>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = sbyte.MinValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<sbyte>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      //overflow test
      bytes = long.MinValue.BNToByteArray();
      bRes = bytes.BNToNumber<sbyte>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToNumber_Short_Test()
   {
      short bNumber = short.MaxValue;
      byte[]? bytes = bNumber.BNToByteArray();
      short bRes = bytes.BNToNumber<short>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = -12345;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<short>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = short.MinValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<short>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      //overflow test
      bytes = long.MinValue.BNToByteArray();
      bRes = bytes.BNToNumber<short>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToNumber_UShort_Test()
   {
      ushort bNumber = ushort.MinValue;
      byte[]? bytes = bNumber.BNToByteArray();
      ushort bRes = bytes.BNToNumber<ushort>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = 12345;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<ushort>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = ushort.MaxValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<ushort>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      //overflow test
      bytes = ulong.MaxValue.BNToByteArray();
      bRes = bytes.BNToNumber<ushort>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToNumber_Char_Test()
   {
      char aChar = char.MinValue;
      byte[]? bytes = aChar.BNToByteArray();
      char bRes = bytes.BNToNumber<char>();
      Assert.That(bRes, Is.EqualTo(aChar));

      aChar = 'B';
      bytes = aChar.BNToByteArray();
      bRes = bytes.BNToNumber<char>();
      Assert.That(bRes, Is.EqualTo(aChar));

      aChar = char.MaxValue;
      bytes = aChar.BNToByteArray();
      bRes = bytes.BNToNumber<char>();
      Assert.That(bRes, Is.EqualTo(aChar));

      //overflow test
      bytes = ulong.MaxValue.BNToByteArray();
      bRes = bytes.BNToNumber<char>();
      Assert.That(bRes, Is.EqualTo(aChar));
   }

   [Test]
   public void BNToNumber_Float_Test()
   {
      float bNumber = float.MaxValue;
      byte[]? bytes = bNumber.BNToByteArray();
      float bRes = bytes.BNToNumber<float>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = Math.PI.BNToNumber<float>();
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<float>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = float.MinValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<float>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToNumber_Int_Test()
   {
      int bNumber = int.MaxValue;
      byte[]? bytes = bNumber.BNToByteArray();
      int bRes = bytes.BNToNumber<int>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = -1234567890;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<int>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = int.MinValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<int>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      //overflow test
      bytes = long.MinValue.BNToByteArray();
      bRes = bytes.BNToNumber<int>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToNumber_UInt_Test()
   {
      uint bNumber = uint.MinValue;
      byte[]? bytes = bNumber.BNToByteArray();
      uint bRes = bytes.BNToNumber<uint>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = 1234567890;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<uint>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = uint.MaxValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<uint>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      //overflow test
      bytes = ulong.MaxValue.BNToByteArray();
      bRes = bytes.BNToNumber<uint>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToNumber_Double_Test()
   {
      double bNumber = double.MaxValue;
      byte[]? bytes = bNumber.BNToByteArray();
      double bRes = bytes.BNToNumber<double>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = Math.PI;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<double>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = double.MinValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<double>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToNumber_Long_Test()
   {
      long bNumber = long.MaxValue;
      byte[]? bytes = bNumber.BNToByteArray();
      long bRes = bytes.BNToNumber<long>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = -1234567890123456789;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<long>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = long.MinValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<long>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToNumber_ULong_Test()
   {
      ulong bNumber = ulong.MinValue;
      byte[]? bytes = bNumber.BNToByteArray();
      ulong bRes = bytes.BNToNumber<ulong>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = 12345678901234567890;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<ulong>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = ulong.MaxValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<ulong>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToNumber_Decimal_Test()
   {
      decimal bNumber = decimal.MinValue;
      byte[]? bytes = bNumber.BNToByteArray();
      decimal bRes = bytes.BNToNumber<decimal>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = 12345678901234567890123456789m;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<decimal>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      bNumber = decimal.MaxValue;
      bytes = bNumber.BNToByteArray();
      bRes = bytes.BNToNumber<decimal>();
      Assert.That(bRes, Is.EqualTo(bNumber));
   }

   [Test]
   public void BNToObject_Test()
   {
      DateTime input = DateTime.Now;
      byte[]? bytes = input.BNToByteArray();
      DateTime? result = bytes.BNToObject<DateTime>().BNConvertToTimeZone(TimeZoneInfo.Local);

      Assert.That(result, Is.EqualTo(input));
   }
}