using BogaNet.Unit;

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

      conv = UnitLength.M.Convert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.Convert(UnitLength.M, conv)));

      conv = UnitLength.MM.Convert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.Convert(UnitLength.MM, conv)));

      conv = UnitLength.CM.Convert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.Convert(UnitLength.CM, conv)));

      conv = UnitLength.KM.Convert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.Convert(UnitLength.KM, conv)));

      conv = UnitLength.INCH.Convert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.Convert(UnitLength.INCH, conv)));

      conv = UnitLength.FOOT.Convert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.Convert(UnitLength.FOOT, conv)));

      conv = UnitLength.YARD.Convert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.Convert(UnitLength.YARD, conv)));

      conv = UnitLength.MILE.Convert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.Convert(UnitLength.MILE, conv)));

      conv = UnitLength.NAUTICAL_MILE.Convert(UnitLength.YARD, val);
      Assert.That(val, Is.EqualTo(UnitLength.YARD.Convert(UnitLength.NAUTICAL_MILE, conv)));
   }

   #endregion
}