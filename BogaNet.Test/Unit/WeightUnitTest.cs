using BogaNet.Unit;

namespace BogaNet.Test.Unit;

public class WeightUnitTest
{
   #region Tests

   [OneTimeSetUp]
   public static void Init()
   {
      WeightUnitExtension.IgnoreSameUnit = false;
   }

   [Test]
   public void WeightUnit_Convert_Test()
   {
      const double valIn = 1234.5678901234;
      decimal refValue = valIn.BNToDecimal();

      decimal conv = WeightUnit.MILLIGRAM.Convert(WeightUnit.POUND, valIn);
      decimal res = WeightUnit.POUND.Convert(WeightUnit.MILLIGRAM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.GRAM.Convert(WeightUnit.POUND, valIn);
      res = WeightUnit.POUND.Convert(WeightUnit.GRAM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.KILOGRAM.Convert(WeightUnit.POUND, valIn);
      res = WeightUnit.POUND.Convert(WeightUnit.KILOGRAM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.OUNCE.Convert(WeightUnit.POUND, valIn);
      res = WeightUnit.POUND.Convert(WeightUnit.OUNCE, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.POUND.Convert(WeightUnit.POUND, valIn);
      res = WeightUnit.POUND.Convert(WeightUnit.POUND, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.TON.Convert(WeightUnit.POUND, valIn);
      res = WeightUnit.POUND.Convert(WeightUnit.TON, conv);
      Assert.That(res, Is.EqualTo(refValue));
   }

   #endregion
}