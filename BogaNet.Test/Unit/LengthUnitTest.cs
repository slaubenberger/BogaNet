using BogaNet.Unit;

namespace BogaNet.Test.Unit;

public class LengthUnitTest
{
   #region Tests

   [Test]
   public void LengthUnit_Convert_Test()
   {
      LengthUnitExtension.IgnoreSameUnit = false;

      const double valIn = 1234.5678901234;
      decimal val = valIn.BNToDecimal();

      decimal conv = LengthUnit.M.Convert(LengthUnit.YARD, valIn);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.M, conv)));

      conv = LengthUnit.MM.Convert(LengthUnit.YARD, valIn);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.MM, conv)));

      conv = LengthUnit.CM.Convert(LengthUnit.YARD, valIn);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.CM, conv)));

      conv = LengthUnit.KM.Convert(LengthUnit.YARD, valIn);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.KM, conv)));

      conv = LengthUnit.INCH.Convert(LengthUnit.YARD, valIn);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.INCH, conv)));

      conv = LengthUnit.FOOT.Convert(LengthUnit.YARD, valIn);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.FOOT, conv)));

      conv = LengthUnit.YARD.Convert(LengthUnit.YARD, valIn);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.YARD, conv)));

      conv = LengthUnit.MILE.Convert(LengthUnit.YARD, valIn);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.MILE, conv)));

      conv = LengthUnit.NAUTICAL_MILE.Convert(LengthUnit.YARD, valIn);
      Assert.That(val, Is.EqualTo(LengthUnit.YARD.Convert(LengthUnit.NAUTICAL_MILE, conv)));
   }

   #endregion
}