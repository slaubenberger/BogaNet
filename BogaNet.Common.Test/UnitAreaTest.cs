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

      conv = UnitArea.M2.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.M2, conv)));

      conv = UnitArea.MM2.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.MM2, conv)));

      conv = UnitArea.CM2.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.CM2, conv)));

      conv = UnitArea.AREA.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.AREA, conv)));

      conv = UnitArea.HECTARE.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.HECTARE, conv)));

      conv = UnitArea.KM2.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.KM2, conv)));

      conv = UnitArea.INCH2.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(Math.Round(UnitArea.YARD2.BNConvert(UnitArea.INCH2, conv))));

      conv = UnitArea.FOOT2.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.FOOT2, conv)));

      conv = UnitArea.YARD2.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.YARD2, conv)));

      conv = UnitArea.PERCH.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.PERCH, conv)));

      conv = UnitArea.ACRE.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.ACRE, conv)));

      conv = UnitArea.MILE2.BNConvert(UnitArea.YARD2, val);
      Assert.That(val, Is.EqualTo(UnitArea.YARD2.BNConvert(UnitArea.MILE2, conv)));
   }

   #endregion
}