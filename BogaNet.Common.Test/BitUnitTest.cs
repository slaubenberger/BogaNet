using BogaNet.Unit;

namespace BogaNet.Test;

public class BitUnitTest
{
   #region Tests

   [Test]
   public void BitUnit_Convert_Test()
   {
      ByteUnitExtension.IgnoreSameUnit = false;

      const long valIn = 1027;
      decimal val = valIn.BNToDecimal();
      decimal conv;

      conv = BitUnit.BIT.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(BitUnit.BIT, conv)));

      conv = BitUnit.Kibit.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(BitUnit.Kibit, conv)));

      conv = BitUnit.Kibit.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(BitUnit.Kibit, conv)));

      conv = BitUnit.Mibit.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(BitUnit.Mibit, conv)));

      conv = BitUnit.Gibit.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(BitUnit.Gibit, conv)));

      conv = BitUnit.Tibit.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(BitUnit.Tibit, conv)));

      conv = BitUnit.Pibit.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(BitUnit.Pibit, conv)));

      conv = BitUnit.Eibit.Convert(BitUnit.kbit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.kbit.Convert(BitUnit.Eibit, conv)));

      conv = BitUnit.kbit.Convert(BitUnit.Kibit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.Kibit.Convert(BitUnit.kbit, conv)));

      conv = BitUnit.Mbit.Convert(BitUnit.Kibit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.Kibit.Convert(BitUnit.Mbit, conv)));

      conv = BitUnit.Gbit.Convert(BitUnit.Kibit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.Kibit.Convert(BitUnit.Gbit, conv)));

      conv = BitUnit.Tbit.Convert(BitUnit.Kibit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.Kibit.Convert(BitUnit.Tbit, conv)));

      conv = BitUnit.Pbit.Convert(BitUnit.Kibit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.Kibit.Convert(BitUnit.Pbit, conv)));

      conv = BitUnit.Ebit.Convert(BitUnit.Kibit, valIn);
      Assert.That(val, Is.EqualTo(BitUnit.Kibit.Convert(BitUnit.Ebit, conv)));
   }

   #endregion
}