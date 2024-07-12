using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base2Test
{
   #region Tests

   [Test]
   public void Base2_Byte_Test()
   {
      string code = "00000001";
      byte bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      byte bRes = Base2.FromBase2String(code).BNToNumber<byte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "00000000";
      bVal = byte.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<byte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "11111111";
      bVal = byte.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<byte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "01111011";
      bVal = 123;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<byte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "1";
      bVal = 1;
      bRes = Base2.FromBase2String(code).BNToNumber<byte>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "001111011";
      bVal = 123;
      bRes = Base2.FromBase2String(code).BNToNumber<byte>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_SByte_Test()
   {
      string code = "00000001";
      sbyte bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      sbyte bRes = Base2.FromBase2String(code).BNToNumber<sbyte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "10000000";
      bVal = sbyte.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<sbyte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "01111111";
      bVal = sbyte.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<sbyte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "01111011";
      bVal = 123;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<sbyte>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "1";
      bVal = 1;
      bRes = Base2.FromBase2String(code).BNToNumber<sbyte>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "001111011";
      bVal = 123;
      bRes = Base2.FromBase2String(code).BNToNumber<sbyte>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_Short_Test()
   {
      string code = "0000000000000001";
      short bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      short bRes = Base2.FromBase2String(code).BNToNumber<short>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "1000000000000000";
      bVal = short.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<short>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0111111111111111";
      bVal = short.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<short>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0011000000111001";
      bVal = 12345;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<short>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "1100111111000111";
      bVal = -12345;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<short>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "1";
      bVal = 1;
      bRes = Base2.FromBase2String(code).BNToNumber<short>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "00011000000111001";
      bVal = 12345;
      bRes = Base2.FromBase2String(code).BNToNumber<short>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_UShort_Test()
   {
      string code = "0000000000000001";
      ushort bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      ushort bRes = Base2.FromBase2String(code).BNToNumber<ushort>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0000000000000000";
      bVal = ushort.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<ushort>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "1111111111111111";
      bVal = ushort.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<ushort>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0011000000111001";
      bVal = 12345;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<ushort>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "1";
      bVal = 1;
      bRes = Base2.FromBase2String(code).BNToNumber<ushort>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "00011000000111001";
      bVal = 12345;
      bRes = Base2.FromBase2String(code).BNToNumber<ushort>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_Char_Test()
   {
      string code = "0000000001000001";
      char bVal = 'A';
      string bCode = Base2.ToBase2String(bVal);
      ushort bRes = Base2.FromBase2String(code).BNToNumber<char>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0000000000000000";
      bVal = char.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<char>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "1111111111111111";
      bVal = char.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<char>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0011000001010011";
      bVal = 'こ';
      bCode = Base2.ToBase2String(bVal);
      var bTemp = Base2.FromBase2String(code);
      bRes = bTemp.BNToNumber<char>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "1000001";
      bVal = 'A';
      bRes = Base2.FromBase2String(code).BNToNumber<char>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "00000000001000001";
      bVal = 'A';
      bTemp = Base2.FromBase2String(code);
      bRes = bTemp.BNToNumber<char>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_Float_Test()
   {
      string code = "00111111100000000000000000000000";
      float bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      float bRes = Base2.FromBase2String(code).BNToNumber<float>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "11111111011111111111111111111111";
      bVal = float.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<float>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "01111111011111111111111111111111";
      bVal = float.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<float>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "01000000010010010000111111011011";
      bVal = Math.PI.BNToNumber<float>();
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<float>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "0";
      bVal = 0;
      bRes = Base2.FromBase2String(code).BNToNumber<float>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "000000000";
      bVal = 0;
      bRes = Base2.FromBase2String(code).BNToNumber<float>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_Int_Test()
   {
      string code = "00000000000000000000000000000001";
      int bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      int bRes = Base2.FromBase2String(code).BNToNumber<int>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "10000000000000000000000000000000";
      bVal = int.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<int>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "01111111111111111111111111111111";
      bVal = int.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<int>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "01001001100101100000001011010010";
      bVal = 1234567890;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<int>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "10110110011010011111110100101110";
      bVal = -1234567890;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<int>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "1";
      bVal = 1;
      bRes = Base2.FromBase2String(code).BNToNumber<int>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "01001001100101100000001011010010";
      bVal = 1234567890;
      bRes = Base2.FromBase2String(code).BNToNumber<int>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_UInt_Test()
   {
      string code = "00000000000000000000000000000001";
      uint bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      uint bRes = Base2.FromBase2String(code).BNToNumber<uint>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "00000000000000000000000000000000";
      bVal = uint.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<uint>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "11111111111111111111111111111111";
      bVal = uint.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<uint>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "01001001100101100000001011010010";
      bVal = 1234567890;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<uint>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "1";
      bVal = 1;
      bRes = Base2.FromBase2String(code).BNToNumber<uint>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "001001001100101100000001011010010";
      bVal = 1234567890;
      bRes = Base2.FromBase2String(code).BNToNumber<uint>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }


   [Test]
   public void Base2_Double_Test()
   {
      string code = "0011111111110000000000000000000000000000000000000000000000000000";
      double bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      double bRes = Base2.FromBase2String(code).BNToNumber<double>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "1111111111101111111111111111111111111111111111111111111111111111";
      bVal = double.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<double>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0111111111101111111111111111111111111111111111111111111111111111";
      bVal = double.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<double>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0100000000001001001000011111101101010100010001000010110100011000";
      bVal = Math.PI;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<double>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "0";
      bVal = 0;
      bRes = Base2.FromBase2String(code).BNToNumber<double>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "000000000";
      bVal = 0;
      bRes = Base2.FromBase2String(code).BNToNumber<double>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_Long_Test()
   {
      string code = "0000000000000000000000000000000000000000000000000000000000000001";
      long bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      long bRes = Base2.FromBase2String(code).BNToNumber<long>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "1000000000000000000000000000000000000000000000000000000000000000";
      bVal = long.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<long>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0111111111111111111111111111111111111111111111111111111111111111";
      bVal = long.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<long>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0001000100100010000100001111010001111101111010011000000100010101";
      bVal = 1234567890123456789;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<long>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "1110111011011101111011110000101110000010000101100111111011101011";
      bVal = -1234567890123456789;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<long>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "0";
      bVal = 0;
      bRes = Base2.FromBase2String(code).BNToNumber<long>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "00001000100100010000100001111010001111101111010011000000100010101";
      bVal = 1234567890123456789;
      bRes = Base2.FromBase2String(code).BNToNumber<long>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_ULong_Test()
   {
      string code = "0000000000000000000000000000000000000000000000000000000000000001";
      ulong bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      ulong bRes = Base2.FromBase2String(code).BNToNumber<ulong>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "0000000000000000000000000000000000000000000000000000000000000000";
      bVal = ulong.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<ulong>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "1111111111111111111111111111111111111111111111111111111111111111";
      bVal = ulong.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<ulong>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "1010101101010100101010011000110011101011000111110000101011010010";
      bVal = 12345678901234567890;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<ulong>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "1";
      bVal = 1;
      bRes = Base2.FromBase2String(code).BNToNumber<ulong>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "01010101101010100101010011000110011101011000111110000101011010010";
      bVal = 12345678901234567890;
      bRes = Base2.FromBase2String(code).BNToNumber<ulong>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_Decimal_Test()
   {
      string code = "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001";
      decimal bVal = 1;
      string bCode = Base2.ToBase2String(bVal);
      decimal bRes = Base2.FromBase2String(code).BNToNumber<decimal>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "10000000000000000000000000000000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111";
      bVal = decimal.MinValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<decimal>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "00000000000000000000000000000000111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111";
      bVal = decimal.MaxValue;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<decimal>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "00000000000011000000000000000000000000000000000000000000000000000000000000000000000000010111100010111010010101110101010010001101";
      bVal = Constants.FACTOR_GOLDEN_RATIO_A_TO_B;
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToNumber<decimal>();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //short test
      code = "0";
      bVal = 0;
      bRes = Base2.FromBase2String(code).BNToNumber<decimal>();
      Assert.That(bRes, Is.EqualTo(bVal));

      //overflow test
      code = "000000000000011000000000000000000000000000000000000000000000000000000000000000000000000010111100010111010010101110101010010001101";
      bVal = Constants.FACTOR_GOLDEN_RATIO_A_TO_B;
      bRes = Base2.FromBase2String(code).BNToNumber<decimal>();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   [Test]
   public void Base2_String_Test()
   {
      string code = "010000010100001001000011";
      string bVal = "ABC";
      string bCode = Base2.ToBase2String(bVal);
      string bRes = Base2.FromBase2String(code).BNToString();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      code = "010000100110111101100111011000010100111001100101011101000010000001110010110000111011110001101100011001010111101000100001";
      bVal = "BogaNet rülez!";
      bCode = Base2.ToBase2String(bVal);
      bRes = Base2.FromBase2String(code).BNToString();
      Assert.Multiple(() =>
      {
         Assert.That(bCode, Is.EqualTo(code));
         Assert.That(bRes, Is.EqualTo(bVal));
      });

      //overflow test
      code = "0010000010100001001000011";
      bVal = "ABC";
      bRes = Base2.FromBase2String(code).BNToString();
      Assert.That(bRes, Is.EqualTo(bVal));
   }

   #endregion
}