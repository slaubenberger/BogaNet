using BogaNet.Unit;

namespace BogaNet.Test;

public class VolumeUnitTest
{
   #region Tests

   [Test]
   public void UnitVolume_Convert_Test()
   {
      VolumeUnitExtension.IgnoreSameUnit = false;

      const decimal val = 1m;
      decimal conv;

      conv = VolumeUnit.LITER.Convert(VolumeUnit.PINT, val);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.LITER, conv)));

      conv = VolumeUnit.MM3.Convert(VolumeUnit.PINT, val);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.MM3, conv)));

      conv = VolumeUnit.CM3.Convert(VolumeUnit.PINT, val);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.CM3, conv)));

      conv = VolumeUnit.M3.Convert(VolumeUnit.PINT, val);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.M3, conv)));

      conv = VolumeUnit.INCH3.Convert(VolumeUnit.PINT, val);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.INCH3, conv)));

      conv = VolumeUnit.FOOT3.Convert(VolumeUnit.PINT, val);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.FOOT3, conv)));

      conv = VolumeUnit.PINT.Convert(VolumeUnit.PINT, val);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.PINT, conv)));

      conv = VolumeUnit.GALLON.Convert(VolumeUnit.PINT, val);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.GALLON, conv)));

      conv = VolumeUnit.BARREL.Convert(VolumeUnit.PINT, val);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.BARREL, conv)));
   }

   #endregion
}