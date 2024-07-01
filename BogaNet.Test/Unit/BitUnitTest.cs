using BogaNet.Unit;

namespace BogaNet.Test.Unit;

public class BitUnitTest
{
   #region Tests

   [OneTimeSetUp]
   public static void Init()
   {
      BitUnitExtension.IgnoreSameUnit = false;
   }

   [Test]
   public void BitUnit_Convert_Test()
   {
      const long valIn = 1027;
      decimal refValue = valIn.BNToDecimal();

      decimal conv = BitUnit.BIT.Convert(BitUnit.kbit, valIn);
      decimal tRef = 1.027m;
      Assert.That(conv, Is.EqualTo(tRef));
      decimal res = BitUnit.kbit.Convert(BitUnit.BIT, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Kibit.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(BitUnit.Kibit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Kibit.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(BitUnit.Kibit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Mibit.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(BitUnit.Mibit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Gibit.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(BitUnit.Gibit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Tibit.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(BitUnit.Tibit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Pibit.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(BitUnit.Pibit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Eibit.Convert(BitUnit.kbit, valIn);
      res = BitUnit.kbit.Convert(BitUnit.Eibit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.kbit.Convert(BitUnit.Kibit, valIn);
      res = BitUnit.Kibit.Convert(BitUnit.kbit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Mbit.Convert(BitUnit.Kibit, valIn);
      res = BitUnit.Kibit.Convert(BitUnit.Mbit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Gbit.Convert(BitUnit.Kibit, valIn);
      res = BitUnit.Kibit.Convert(BitUnit.Gbit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Tbit.Convert(BitUnit.Kibit, valIn);
      res = BitUnit.Kibit.Convert(BitUnit.Tbit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Pbit.Convert(BitUnit.Kibit, valIn);
      res = BitUnit.Kibit.Convert(BitUnit.Pbit, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = BitUnit.Ebit.Convert(BitUnit.Kibit, valIn);
      res = BitUnit.Kibit.Convert(BitUnit.Ebit, conv);
      Assert.That(res, Is.EqualTo(refValue));
   }

   #endregion
}