using BogaNet.Unit;

namespace BogaNet.Test.Unit;

public class ByteUnitTest
{
   #region Tests

   [Test]
   public void ByteUnit_Convert_Test()
   {
      ByteUnitExtension.IgnoreSameUnit = false;

      const long valIn = 1027;
      decimal val = valIn.BNToDecimal();
      decimal conv;

      conv = ByteUnit.BYTE.Convert(ByteUnit.BYTE, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.BYTE.Convert(ByteUnit.BYTE, conv)));

      conv = ByteUnit.BYTE.Convert(ByteUnit.kB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.kB.Convert(ByteUnit.BYTE, conv)));

      conv = ByteUnit.KiB.Convert(ByteUnit.kB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.kB.Convert(ByteUnit.KiB, conv)));

      conv = ByteUnit.MiB.Convert(ByteUnit.kB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.kB.Convert(ByteUnit.MiB, conv)));

      conv = ByteUnit.GiB.Convert(ByteUnit.kB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.kB.Convert(ByteUnit.GiB, conv)));

      conv = ByteUnit.TiB.Convert(ByteUnit.kB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.kB.Convert(ByteUnit.TiB, conv)));

      conv = ByteUnit.PiB.Convert(ByteUnit.kB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.kB.Convert(ByteUnit.PiB, conv)));

      conv = ByteUnit.EiB.Convert(ByteUnit.kB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.kB.Convert(ByteUnit.EiB, conv)));

      conv = ByteUnit.kB.Convert(ByteUnit.KiB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.KiB.Convert(ByteUnit.kB, conv)));

      conv = ByteUnit.kB.Convert(ByteUnit.KiB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.KiB.Convert(ByteUnit.kB, conv)));

      conv = ByteUnit.MB.Convert(ByteUnit.KiB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.KiB.Convert(ByteUnit.MB, conv)));

      conv = ByteUnit.GB.Convert(ByteUnit.KiB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.KiB.Convert(ByteUnit.GB, conv)));

      conv = ByteUnit.TB.Convert(ByteUnit.KiB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.KiB.Convert(ByteUnit.TB, conv)));

      conv = ByteUnit.PB.Convert(ByteUnit.KiB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.KiB.Convert(ByteUnit.PB, conv)));

      conv = ByteUnit.EB.Convert(ByteUnit.KiB, valIn);
      Assert.That(val, Is.EqualTo(ByteUnit.KiB.Convert(ByteUnit.EB, conv)));
   }

   [Test]
   public void ByteUnit_Convert_BitUnit_Test()
   {
      ByteUnitExtension.IgnoreSameUnit = false;

      const double valIn = 1027;
      decimal val = valIn.BNToDecimal();
      decimal conv;

      //Console.WriteLine(ByteUnit.BYTE.Convert(BitUnit.kbit, val));

      conv = ByteUnit.BYTE.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.BYTE, conv)));

      conv = ByteUnit.KiB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.KiB, conv)));

      conv = ByteUnit.MiB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.MiB, conv)));

      conv = ByteUnit.GiB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.GiB, conv)));

      conv = ByteUnit.TiB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.TiB, conv)));

      conv = ByteUnit.PiB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.PiB, conv)));

      conv = ByteUnit.EiB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.EiB, conv)));

      conv = ByteUnit.kB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.kB, conv)));

      conv = ByteUnit.MB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.MB, conv)));

      conv = ByteUnit.GB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.GB, conv)));

      conv = ByteUnit.TB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.TB, conv)));

      conv = ByteUnit.PB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.PB, conv)));

      conv = ByteUnit.EB.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(ByteUnit.EB, conv)));
   }

   #endregion
}