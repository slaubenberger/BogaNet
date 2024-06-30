﻿using BogaNet.Unit;

namespace BogaNet.Test.Unit;

public class VolumeUnitTest
{
   #region Tests

   [Test]
   public void VolumeUnit_Convert_Test()
   {
      VolumeUnitExtension.IgnoreSameUnit = false;

      const double valIn = 1234.5678901234;
      decimal val = valIn.BNToDecimal();

      decimal conv = VolumeUnit.LITER.Convert(VolumeUnit.PINT, valIn);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.LITER, conv)));

      conv = VolumeUnit.MM3.Convert(VolumeUnit.PINT, valIn);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.MM3, conv)));

      conv = VolumeUnit.CM3.Convert(VolumeUnit.PINT, valIn);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.CM3, conv)));

      conv = VolumeUnit.M3.Convert(VolumeUnit.PINT, valIn);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.M3, conv)));

      conv = VolumeUnit.INCH3.Convert(VolumeUnit.PINT, valIn);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.INCH3, conv)));

      conv = VolumeUnit.FOOT3.Convert(VolumeUnit.PINT, valIn);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.FOOT3, conv)));

      conv = VolumeUnit.PINT.Convert(VolumeUnit.PINT, valIn);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.PINT, conv)));

      conv = VolumeUnit.GALLON.Convert(VolumeUnit.PINT, valIn);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.GALLON, conv)));

      conv = VolumeUnit.BARREL.Convert(VolumeUnit.PINT, valIn);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.BARREL, conv)));

      conv = VolumeUnit.CUP.Convert(VolumeUnit.PINT, valIn);
      Assert.That(val, Is.EqualTo(VolumeUnit.PINT.Convert(VolumeUnit.CUP, conv)));
   }

   #endregion
}