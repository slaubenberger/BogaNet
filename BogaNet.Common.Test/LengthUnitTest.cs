using BogaNet.Unit;

namespace BogaNet.Test;

public class LengthUnitTest
{
   #region Tests

   [Test]
   public void UnitLength_Convert_Test()
   {
      LengthUnitExtension.IgnoreSameUnit = false;

      const decimal val = 1m;
      decimal conv;

      conv = LengthUnit.M.Convert(LengthUnit.YARD, val);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.M, conv)));

      conv = LengthUnit.MM.Convert(LengthUnit.YARD, val);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.MM, conv)));

      conv = LengthUnit.CM.Convert(LengthUnit.YARD, val);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.CM, conv)));

      conv = LengthUnit.KM.Convert(LengthUnit.YARD, val);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.KM, conv)));

      conv = LengthUnit.INCH.Convert(LengthUnit.YARD, val);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.INCH, conv)));

      conv = LengthUnit.FOOT.Convert(LengthUnit.YARD, val);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.FOOT, conv)));

      conv = LengthUnit.YARD.Convert(LengthUnit.YARD, val);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.YARD, conv)));

      conv = LengthUnit.MILE.Convert(LengthUnit.YARD, val);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.MILE, conv)));

      conv = LengthUnit.NAUTICAL_MILE.Convert(LengthUnit.YARD, val);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.NAUTICAL_MILE, conv)));
   }

   #endregion
}