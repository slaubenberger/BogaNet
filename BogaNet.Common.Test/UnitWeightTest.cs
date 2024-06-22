using BogaNet.Unit;

namespace BogaNet.Test;

public class UnitWeightTest
{
   #region Tests

   [Test]
   public void UnitWeight_Convert_Test()
   {
      ExtensionUnitWeight.IgnoreSameUnit = false;

      const decimal val = 1m;
      decimal conv;

      //Console.WriteLine(UnitWeight.TON.Convert(UnitWeight.KILOGRAM, val));
      conv = UnitWeight.MILLIGRAM.Convert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.Convert(UnitWeight.MILLIGRAM, conv)));

      conv = UnitWeight.GRAM.Convert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.Convert(UnitWeight.GRAM, conv)));

      conv = UnitWeight.KILOGRAM.Convert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.Convert(UnitWeight.KILOGRAM, conv)));

      conv = UnitWeight.OUNCE.Convert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.Convert(UnitWeight.OUNCE, conv)));

      conv = UnitWeight.POUND.Convert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.Convert(UnitWeight.POUND, conv)));

      conv = UnitWeight.TON.Convert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.Convert(UnitWeight.TON, conv)));
   }

   #endregion
}