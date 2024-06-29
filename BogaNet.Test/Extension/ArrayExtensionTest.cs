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

   [Test]
   public void BNToStringLength_Test()
   {
      string input = "crosstales LLC";
      byte[]? bytes = input.BNToByteArray();
      string? result = bytes.BNToString(0, 4);

      Assert.That(result, Is.EqualTo(input.Substring(0, 4)));

      input = "こんにちは";
      bytes = input.BNToByteArray();
      result = bytes.BNToString(0, 3); //3 bytes are one char

      Assert.That(result, Is.EqualTo(input.Substring(0, 1)));
   }

   [Test]
   public void BNToNumber_Test()
   {
      //byte
      byte bNumber = byte.MaxValue;
      byte[]? bytes = bNumber.BNToByteArray();
      byte bRes = bytes.BNToNumber<byte>();
      Assert.That(bRes, Is.EqualTo(bNumber));

      //sbyte
      sbyte sbNumber = sbyte.MinValue;
      bytes = sbNumber.BNToByteArray();
      sbyte sbRes = bytes.BNToNumber<sbyte>();
      Assert.That(sbRes, Is.EqualTo(sbNumber));

      //short
      short shortNumber = short.MinValue;
      bytes = shortNumber.BNToByteArray();
      short shortRes = bytes.BNToNumber<short>();
      Assert.That(shortRes, Is.EqualTo(shortNumber));

      //ushort
      ushort ushortNumber = ushort.MaxValue;
      bytes = ushortNumber.BNToByteArray();
      ushort ushortRes = bytes.BNToNumber<ushort>();
      Assert.That(ushortRes, Is.EqualTo(ushortNumber));

      //char
      char aChar = 'C';
      bytes = aChar.BNToByteArray();
      char aCharRes = bytes.BNToNumber<char>();
      Assert.That(aCharRes, Is.EqualTo(aChar));

      //float
      float fpNumber = Math.PI.BNToNumber<float>();
      bytes = fpNumber.BNToByteArray();
      float fpRes = bytes.BNToNumber<float>();
      Assert.That(fpRes, Is.EqualTo(fpNumber));

      //int
      int intNumber = int.MinValue;
      bytes = intNumber.BNToByteArray();
      int intRes = bytes.BNToNumber<int>();
      Assert.That(intRes, Is.EqualTo(intNumber));

      //uint
      uint uintNumber = uint.MaxValue;
      bytes = uintNumber.BNToByteArray();
      uint uintRes = bytes.BNToNumber<uint>();
      Assert.That(uintRes, Is.EqualTo(uintNumber));

      //double
      double dpNumber = Math.PI;
      bytes = dpNumber.BNToByteArray();
      double dpRes = bytes.BNToNumber<double>();
      Assert.That(dpRes, Is.EqualTo(dpNumber));

      //long
      long lNumber = long.MinValue;
      bytes = lNumber.BNToByteArray();
      long longRes = bytes.BNToNumber<long>();
      Assert.That(longRes, Is.EqualTo(lNumber));

      //ulong
      ulong ulNumber = ulong.MaxValue;
      bytes = ulNumber.BNToByteArray();
      ulong ulRes = bytes.BNToNumber<ulong>();
      Assert.That(ulRes, Is.EqualTo(ulNumber));
   }
}