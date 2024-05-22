namespace BogaNet.Test;

public class UnitLengthTest
{
   #region Tests

   [Test]
   public void UnitLength_Convert_Test()
   {
      ExtensionUnitLength.IgnoreSameUnit = false;

      const decimal val = 1m;
      decimal conv;

      conv = UnitLength.M.BNConvert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.BNConvert(UnitLength.M, conv)));

      conv = UnitLength.MM.BNConvert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.BNConvert(UnitLength.MM, conv)));

      conv = UnitLength.CM.BNConvert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.BNConvert(UnitLength.CM, conv)));

      conv = UnitLength.KM.BNConvert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.BNConvert(UnitLength.KM, conv)));

      conv = UnitLength.INCH.BNConvert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.BNConvert(UnitLength.INCH, conv)));

      conv = UnitLength.FOOT.BNConvert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.BNConvert(UnitLength.FOOT, conv)));

      conv = UnitLength.YARD.BNConvert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.BNConvert(UnitLength.YARD, conv)));

      conv = UnitLength.MILE.BNConvert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.BNConvert(UnitLength.MILE, conv)));

      conv = UnitLength.NAUTICAL_MILE.BNConvert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.BNConvert(UnitLength.NAUTICAL_MILE, conv)));
   }

   #endregion
}