using BogaNet.Unit;

namespace BogaNet.Test;

public class UnitAreaTest
{
   #region Tests

   [Test]
   public void UnitArea_Convert_Test()
   {
      ExtensionUnitArea.IgnoreSameUnit = false;

      const decimal val = 1m;
      decimal conv;

      conv = UnitArea.M2.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.M2, conv)));

      conv = UnitArea.MM2.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.MM2, conv)));

      conv = UnitArea.CM2.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.CM2, conv)));

      conv = UnitArea.AREA.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.AREA, conv)));

      conv = UnitArea.HECTARE.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.HECTARE, conv)));

      conv = UnitArea.KM2.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.KM2, conv)));

      conv = UnitArea.INCH2.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(Math.Round(UnitArea.YARD2.Convert(UnitArea.INCH2, conv))));

      conv = UnitArea.FOOT2.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.FOOT2, conv)));

      conv = UnitArea.YARD2.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.YARD2, conv)));

      conv = UnitArea.PERCH.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.PERCH, conv)));

      conv = UnitArea.ACRE.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.ACRE, conv)));

      conv = UnitArea.MILE2.Convert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.Convert(UnitArea.MILE2, conv)));
   }

   #endregion
}