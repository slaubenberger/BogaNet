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

      conv = UnitVolume.LITER.Convert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.Convert(UnitVolume.LITER, conv)));

      conv = UnitVolume.MM3.Convert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.Convert(UnitVolume.MM3, conv)));

      conv = UnitVolume.CM3.Convert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.Convert(UnitVolume.CM3, conv)));

      conv = UnitVolume.M3.Convert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.Convert(UnitVolume.M3, conv)));

      conv = UnitVolume.INCH3.Convert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.Convert(UnitVolume.INCH3, conv)));

      conv = UnitVolume.FOOT3.Convert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.Convert(UnitVolume.FOOT3, conv)));

      conv = UnitVolume.PINT.Convert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.Convert(UnitVolume.PINT, conv)));

      conv = UnitVolume.GALLON.Convert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.Convert(UnitVolume.GALLON, conv)));

      conv = UnitVolume.BARREL.Convert(UnitVolume.PINT, val);
      Assert.That(val, Is.EqualTo(UnitVolume.PINT.Convert(UnitVolume.BARREL, conv)));
   }

   #endregion
}