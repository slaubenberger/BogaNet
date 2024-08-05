using System;
using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base16Test
{
   #region Tests

   [Test]
   public void Base16_Byte_Test()
   {
      string code = "01";
      byte bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      byte bRes = Base16.FromBase16String(code).BNToNumber<byte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "00";
      bVal = byte.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<byte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "FF";
      bVal = byte.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<byte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "7B";
      bVal = 123;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<byte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //prefix test
      code = "0x7B";
      bVal = 123;
      bRes = Base16.FromBase16String(code).BNToNumber<byte>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "0007B";
      bVal = 123;
      bRes = Base16.FromBase16String(code).BNToNumber<byte>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_SByte_Test()
   {
      string code = "01";
      sbyte bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      sbyte bRes = Base16.FromBase16String(code).BNToNumber<sbyte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "80";
      bVal = sbyte.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<sbyte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "7F";
      bVal = sbyte.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<sbyte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "7B";
      bVal = 123;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<sbyte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //overflow test
      code = "0007B";
      bVal = 123;
      bRes = Base16.FromBase16String(code).BNToNumber<sbyte>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_Short_Test()
   {
      string code = "0001";
      short bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      short bRes = Base16.FromBase16String(code).BNToNumber<short>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "8000";
      bVal = short.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<short>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "7FFF";
      bVal = short.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<short>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "3039";
      bVal = 12345;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<short>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "CFC7";
      bVal = -12345;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<short>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "7B";
      bVal = 123;
      bRes = Base16.FromBase16String(code).BNToNumber<short>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "03039";
      bVal = 12345;
      bRes = Base16.FromBase16String(code).BNToNumber<short>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_UShort_Test()
   {
      string code = "0001";
      ushort bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      ushort bRes = Base16.FromBase16String(code).BNToNumber<ushort>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0000";
      bVal = ushort.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<ushort>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "FFFF";
      bVal = ushort.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<ushort>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "3039";
      bVal = 12345;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<ushort>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "7B";
      bVal = 123;
      bRes = Base16.FromBase16String(code).BNToNumber<ushort>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "03039";
      bVal = 12345;
      bRes = Base16.FromBase16String(code).BNToNumber<ushort>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_Char_Test()
   {
      string code = "0041";
      char bVal = 'A';
      string bCode = Base16.ToBase16String(bVal);
      ushort bRes = Base16.FromBase16String(code).BNToNumber<char>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0000";
      bVal = char.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<char>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "FFFF";
      bVal = char.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<char>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "3053";
      bVal = 'こ';
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<char>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "41";
      bVal = 'A';
      bRes = Base16.FromBase16String(code).BNToNumber<char>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "00041";
      bVal = 'A';
      bRes = Base16.FromBase16String(code).BNToNumber<char>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_Float_Test()
   {
      string code = "3F800000";
      float bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      float bRes = Base16.FromBase16String(code).BNToNumber<float>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "FF7FFFFF";
      bVal = float.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<float>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "7F7FFFFF";
      bVal = float.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<float>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "40490FDB";
      bVal = Math.PI.BNToNumber<float>();
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<float>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "0";
      bVal = 0;
      bRes = Base16.FromBase16String(code).BNToNumber<float>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "000000000";
      bVal = 0;
      bRes = Base16.FromBase16String(code).BNToNumber<float>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_Int_Test()
   {
      string code = "00000001";
      int bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      int bRes = Base16.FromBase16String(code).BNToNumber<int>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "80000000";
      bVal = int.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<int>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "7FFFFFFF";
      bVal = int.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<int>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "499602D2";
      bVal = 1234567890;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<int>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "B669FD2E";
      bVal = -1234567890;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<int>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "7B";
      bVal = 123;
      bRes = Base16.FromBase16String(code).BNToNumber<int>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "0499602D2";
      bVal = 1234567890;
      bRes = Base16.FromBase16String(code).BNToNumber<int>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_UInt_Test()
   {
      string code = "00000001";
      uint bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      uint bRes = Base16.FromBase16String(code).BNToNumber<uint>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "00000000";
      bVal = uint.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<uint>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "FFFFFFFF";
      bVal = uint.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<uint>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "499602D2";
      bVal = 1234567890;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<uint>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "7B";
      bVal = 123;
      bRes = Base16.FromBase16String(code).BNToNumber<uint>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "0499602D2";
      bVal = 1234567890;
      bRes = Base16.FromBase16String(code).BNToNumber<uint>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }


   [Test]
   public void Base16_Double_Test()
   {
      string code = "3FF0000000000000";
      double bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      double bRes = Base16.FromBase16String(code).BNToNumber<double>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "FFEFFFFFFFFFFFFF";
      bVal = double.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<double>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "7FEFFFFFFFFFFFFF";
      bVal = double.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<double>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "400921FB54442D18";
      bVal = Math.PI;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<double>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "0";
      bVal = 0;
      bRes = Base16.FromBase16String(code).BNToNumber<double>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "000000000";
      bVal = 0;
      bRes = Base16.FromBase16String(code).BNToNumber<double>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_Long_Test()
   {
      string code = "0000000000000001";
      long bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      long bRes = Base16.FromBase16String(code).BNToNumber<long>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "8000000000000000";
      bVal = long.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<long>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "7FFFFFFFFFFFFFFF";
      bVal = long.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<long>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "112210F47DE98115";
      bVal = 1234567890123456789;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<long>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "EEDDEF0B82167EEB";
      bVal = -1234567890123456789;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<long>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "7B";
      bVal = 123;
      bRes = Base16.FromBase16String(code).BNToNumber<long>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "0112210F47DE98115";
      bVal = 1234567890123456789;
      bRes = Base16.FromBase16String(code).BNToNumber<long>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_ULong_Test()
   {
      string code = "0000000000000001";
      ulong bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      ulong bRes = Base16.FromBase16String(code).BNToNumber<ulong>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0000000000000000";
      bVal = ulong.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<ulong>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "FFFFFFFFFFFFFFFF";
      bVal = ulong.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<ulong>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "AB54A98CEB1F0AD2";
      bVal = 12345678901234567890;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<ulong>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "7B";
      bVal = 123;
      bRes = Base16.FromBase16String(code).BNToNumber<ulong>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "0AB54A98CEB1F0AD2";
      bVal = 12345678901234567890;
      bRes = Base16.FromBase16String(code).BNToNumber<ulong>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_Decimal_Test()
   {
      string code = "00000000000000000000000000000001";
      decimal bVal = 1;
      string bCode = Base16.ToBase16String(bVal);
      decimal bRes = Base16.FromBase16String(code).BNToNumber<decimal>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "80000000FFFFFFFFFFFFFFFFFFFFFFFF";
      bVal = decimal.MinValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<decimal>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "00000000FFFFFFFFFFFFFFFFFFFFFFFF";
      bVal = decimal.MaxValue;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<decimal>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "000C00000000000000000178BA57548D";
      bVal = Constants.FACTOR_GOLDEN_RATIO_A_TO_B;
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToNumber<decimal>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "0";
      bVal = 0;
      bRes = Base16.FromBase16String(code).BNToNumber<decimal>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "000000000";
      bVal = 0;
      bRes = Base16.FromBase16String(code).BNToNumber<decimal>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base16_String_Test()
   {
      string code = "414243";
      string bVal = "ABC";
      string bCode = Base16.ToBase16String(bVal);
      string bRes = Base16.FromBase16String(code).BNToString();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "426F67614E65742072C3BC6C657A21";
      bVal = "BogaNet rülez!";
      bCode = Base16.ToBase16String(bVal);
      bRes = Base16.FromBase16String(code).BNToString();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //overflow test
      code = "0414243";
      bVal = "ABC";
      bRes = Base16.FromBase16String(code).BNToString();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   #endregion
}