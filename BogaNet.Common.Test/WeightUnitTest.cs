using BogaNet.Unit;

namespace BogaNet.Test;

public class WeightUnitTest
{
   #region Tests

   [Test]
   public void UnitWeight_Convert_Test()
   {
      WeightUnitExtension.IgnoreSameUnit = false;

      const decimal val = 1m;
      decimal conv;

      //Console.WriteLine(UnitWeight.TON.Convert(UnitWeight.KILOGRAM, val));
      conv = WeightUnit.MILLIGRAM.Convert(WeightUnit.POUND, val);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.MILLIGRAM, conv)));

      conv = WeightUnit.GRAM.Convert(WeightUnit.POUND, val);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.GRAM, conv)));

      conv = WeightUnit.KILOGRAM.Convert(WeightUnit.POUND, val);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.KILOGRAM, conv)));

      conv = WeightUnit.OUNCE.Convert(WeightUnit.POUND, val);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.OUNCE, conv)));

      conv = WeightUnit.POUND.Convert(WeightUnit.POUND, val);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.POUND, conv)));

      conv = WeightUnit.TON.Convert(WeightUnit.POUND, val);
      Assert.That(val, Is.EqualTo(WeightUnit.POUND.Convert(WeightUnit.TON, conv)));
   }

   #endregion
}