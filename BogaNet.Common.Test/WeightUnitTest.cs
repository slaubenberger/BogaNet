using BogaNet.Unit;

namespace BogaNet.Test;

public class WeightUnitTest
{
   #region Tests

   [Test]
   public void WeightUnit_Convert_Test()
   {
      WeightUnitExtension.IgnoreSameUnit = false;

      const double valIn = 1234.5678901234;
      decimal val = valIn.BNToDecimal();
      decimal conv;

      //Console.WriteLine(UnitWeight.TON.Convert(UnitWeight.KILOGRAM, val));
      conv = WeightUnit.MILLIGRAM.Convert(WeightUnit.POUND, valIn);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.MILLIGRAM, conv)));

      conv = WeightUnit.GRAM.Convert(WeightUnit.POUND, valIn);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.GRAM, conv)));

      conv = WeightUnit.KILOGRAM.Convert(WeightUnit.POUND, valIn);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.KILOGRAM, conv)));

      conv = WeightUnit.OUNCE.Convert(WeightUnit.POUND, valIn);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.OUNCE, conv)));

      conv = WeightUnit.POUND.Convert(WeightUnit.POUND, valIn);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.POUND, conv)));

      conv = WeightUnit.TON.Convert(WeightUnit.POUND, valIn);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.TON, conv)));
   }

   #endregion
}