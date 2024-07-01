using BogaNet.Unit;
using BogaNet.Extension;

namespace BogaNet.Test.Unit;

public class LengthUnitTest
{
   [OneTimeSetUp]
   public static void Init()
   {
      LengthUnitExtension.IgnoreSameUnit = false;
   }

   #region Tests

   [Test]
   public void LengthUnit_Convert_Test()
   {
      const double valIn = 1234.5678901234;
      decimal refValue = valIn.BNToDecimal();

      decimal conv = LengthUnit.M.Convert(LengthUnit.YARD, valIn);
      decimal res = LengthUnit.YARD.Convert(LengthUnit.M, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.MM.Convert(LengthUnit.YARD, valIn);
      res = LengthUnit.YARD.Convert(LengthUnit.MM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.CM.Convert(LengthUnit.YARD, valIn);
      res = LengthUnit.YARD.Convert(LengthUnit.CM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.KM.Convert(LengthUnit.YARD, valIn);
      res = LengthUnit.YARD.Convert(LengthUnit.KM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.INCH.Convert(LengthUnit.YARD, valIn);
      res = LengthUnit.YARD.Convert(LengthUnit.INCH, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.FOOT.Convert(LengthUnit.YARD, valIn);
      res = LengthUnit.YARD.Convert(LengthUnit.FOOT, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.YARD.Convert(LengthUnit.YARD, valIn);
      res = LengthUnit.YARD.Convert(LengthUnit.YARD, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.MILE.Convert(LengthUnit.YARD, valIn);
      res = LengthUnit.YARD.Convert(LengthUnit.MILE, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.NAUTICAL_MILE.Convert(LengthUnit.YARD, valIn);
      res = LengthUnit.YARD.Convert(LengthUnit.NAUTICAL_MILE, conv);
      Assert.That(res, Is.EqualTo(refValue));
   }

   #endregion
}