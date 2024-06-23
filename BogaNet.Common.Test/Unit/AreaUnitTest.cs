using BogaNet.Unit;

namespace BogaNet.Test.Unit;

public class AreaUnitTest
{
   #region Tests

   [Test]
   public void AreaUnit_Convert_Test()
   {
      AreaUnitExtension.IgnoreSameUnit = false;

      const double valIn = 1234.5678901234;
      decimal val = valIn.BNToDecimal();
      decimal conv;

      conv = AreaUnit.M2.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.M2, conv)));

      conv = AreaUnit.MM2.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.MM2, conv)));

      conv = AreaUnit.CM2.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.CM2, conv)));

      conv = AreaUnit.AREA.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.AREA, conv)));

      conv = AreaUnit.HECTARE.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.HECTARE, conv)));

      conv = AreaUnit.KM2.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.KM2, conv)));

      conv = AreaUnit.INCH2.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(Decimal.Round(AreaUnit.YARD2.Convert(AreaUnit.INCH2, conv), 10)));

      conv = AreaUnit.FOOT2.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.FOOT2, conv)));

      conv = AreaUnit.YARD2.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.YARD2, conv)));

      conv = AreaUnit.PERCH.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.PERCH, conv)));

      conv = AreaUnit.ACRE.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.ACRE, conv)));

      conv = AreaUnit.MILE2.Convert(AreaUnit.YARD2, valIn);
      Assert.That(val, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.MILE2, conv)));
   }

   #endregion
}