using BogaNet.Unit;

namespace BogaNet.Test.Unit;

public class ByteUnitTest
{
   #region Tests

   [OneTimeSetUp]
   public static void Init()
   {
      ByteUnitExtension.IgnoreSameUnit = false;
   }

   [Test]
   public void ByteUnit_Convert_Test()
   {
      const long valIn = 1027;
      decimal refValue = valIn.BNToDecimal();

      decimal conv = ByteUnit.BYTE.Convert(ByteUnit.BYTE, valIn);
      decimal tRef = 1027m;
      Assert.That(conv, Is.EqualTo(tRef));
      decimal res = ByteUnit.BYTE.Convert(ByteUnit.BYTE, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.BYTE.Convert(ByteUnit.kB, valIn);
      res = ByteUnit.kB.Convert(ByteUnit.BYTE, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.KiB.Convert(ByteUnit.kB, valIn);
      res = ByteUnit.kB.Convert(ByteUnit.KiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.MiB.Convert(ByteUnit.kB, valIn);
      res = ByteUnit.kB.Convert(ByteUnit.MiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.GiB.Convert(ByteUnit.kB, valIn);
      res = ByteUnit.kB.Convert(ByteUnit.GiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.TiB.Convert(ByteUnit.kB, valIn);
      res = ByteUnit.kB.Convert(ByteUnit.TiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.PiB.Convert(ByteUnit.kB, valIn);
      res = ByteUnit.kB.Convert(ByteUnit.PiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.EiB.Convert(ByteUnit.kB, valIn);
      res = ByteUnit.kB.Convert(ByteUnit.EiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.kB.Convert(ByteUnit.KiB, valIn);
      res = ByteUnit.KiB.Convert(ByteUnit.kB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.kB.Convert(ByteUnit.KiB, valIn);
      res = ByteUnit.KiB.Convert(ByteUnit.kB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.MB.Convert(ByteUnit.KiB, valIn);
      res = ByteUnit.KiB.Convert(ByteUnit.MB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.GB.Convert(ByteUnit.KiB, valIn);
      res = ByteUnit.KiB.Convert(ByteUnit.GB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.TB.Convert(ByteUnit.KiB, valIn);
      res = ByteUnit.KiB.Convert(ByteUnit.TB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.PB.Convert(ByteUnit.KiB, valIn);
      res = ByteUnit.KiB.Convert(ByteUnit.PB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.EB.Convert(ByteUnit.KiB, valIn);
      res = ByteUnit.KiB.Convert(ByteUnit.EB, conv);
      Assert.That(res, Is.EqualTo(refValue));
   }

   [Test]
   public void ByteUnit_Convert_BitUnit_Test()
   {
      const double valIn = 1027;
      decimal refValue = valIn.BNToDecimal();

      decimal conv = ByteUnit.BYTE.Convert(BitUnit.kbit, valIn);
      decimal tRef = 8.216m;
      Assert.That(conv, Is.EqualTo(tRef));
      decimal res = BitUnit.kbit.Convert(ByteUnit.BYTE, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.KiB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.KiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.MiB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.MiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.GiB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.GiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.TiB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.TiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.PiB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.PiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.EiB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.EiB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.kB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.kB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.MB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.MB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.GB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.GB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.TB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.TB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.PB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.PB, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = ByteUnit.EB.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(ByteUnit.EB, conv);
      Assert.That(res, Is.EqualTo(refValue));
   }

   #endregion
}