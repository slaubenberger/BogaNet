using BogaNet.Unit;

namespace BogaNet.Test;

public class UnitVolumeTest
{
   #region Tests

   [Test]
   public void UnitVolume_Convert_Test()
   {
      ExtensionUnitVolume.IgnoreSameUnit = false;

      const decimal val = 1m;
      decimal conv;

      conv = UnitVolume.LITER.BNConvert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.BNConvert(UnitVolume.LITER, conv)));

      conv = UnitVolume.MM3.BNConvert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.BNConvert(UnitVolume.MM3, conv)));

      conv = UnitVolume.CM3.BNConvert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.BNConvert(UnitVolume.CM3, conv)));

      conv = UnitVolume.M3.BNConvert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.BNConvert(UnitVolume.M3, conv)));

      conv = UnitVolume.INCH3.BNConvert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.BNConvert(UnitVolume.INCH3, conv)));

      conv = UnitVolume.FOOT3.BNConvert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.BNConvert(UnitVolume.FOOT3, conv)));

      conv = UnitVolume.PINT.BNConvert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.BNConvert(UnitVolume.PINT, conv)));

      conv = UnitVolume.GALLON.BNConvert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.BNConvert(UnitVolume.GALLON, conv)));

      conv = UnitVolume.BARREL.BNConvert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.BNConvert(UnitVolume.BARREL, conv)));
   }

   #endregion
}