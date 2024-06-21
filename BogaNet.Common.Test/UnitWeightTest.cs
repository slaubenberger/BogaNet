using BogaNet.Unit;

namespace BogaNet.Test;

public class UnitWeightTest
{
   #region Tests

   [Test]
   public void UnitWeight_Convert_Test()
   {
      ExtensionUnitArea.IgnoreSameUnit = false;

      const decimal val = 1m;
      decimal conv;

      Console.WriteLine(UnitWeight.TON.BNConvert(UnitWeight.KILOGRAM, val));
      conv = UnitWeight.MILLIGRAM.BNConvert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.BNConvert(UnitWeight.MILLIGRAM, conv)));

      conv = UnitWeight.GRAM.BNConvert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.BNConvert(UnitWeight.GRAM, conv)));

      conv = UnitWeight.KILOGRAM.BNConvert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.BNConvert(UnitWeight.KILOGRAM, conv)));

      conv = UnitWeight.OUNCE.BNConvert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.BNConvert(UnitWeight.OUNCE, conv)));

      conv = UnitWeight.POUND.BNConvert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.BNConvert(UnitWeight.POUND, conv)));

      conv = UnitWeight.TON.BNConvert(UnitWeight.POUND, val);
      Assert.That(val, Is.EqualTo(UnitWeight.POUND.BNConvert(UnitWeight.TON, conv)));
   }

   #endregion
}