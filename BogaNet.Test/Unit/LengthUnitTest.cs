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
      decimal tRef = 1350.1398623396762904636920385m;
      Assert.That(conv, Is.EqualTo(tRef));
      decimal res = LengthUnit.YARD.Convert(LengthUnit.M, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.MM.Convert(LengthUnit.YARD, valIn);
      tRef = 1.3501398623396762904636920385m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.MM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.CM.Convert(LengthUnit.YARD, valIn);
      tRef = 13.501398623396762904636920385m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.CM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.DECIMETER.Convert(LengthUnit.YARD, valIn);
      tRef = 135.01398623396762904636920385m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.DECIMETER, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.DECAMETER.Convert(LengthUnit.YARD, valIn);
      tRef = 13501.398623396762904636920385m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.DECAMETER, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.HECTOMETER.Convert(LengthUnit.YARD, valIn);
      tRef = 135013.98623396762904636920385m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.HECTOMETER, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.KM.Convert(LengthUnit.YARD, valIn);
      tRef = 1350139.8623396762904636920385m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.KM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.INCH.Convert(LengthUnit.YARD, valIn);
      tRef = 34.293552503427777777777777778m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.INCH, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.FOOT.Convert(LengthUnit.YARD, valIn);
      tRef = 411.52263004113333333333333333m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.FOOT, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.YARD.Convert(LengthUnit.YARD, valIn);
      tRef = 1234.5678901234m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.YARD, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.MILE.Convert(LengthUnit.YARD, valIn);
      tRef = 2172839.486617184m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.MILE, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.NAUTICAL_MILE.Convert(LengthUnit.YARD, valIn);
      tRef = 2500459.0250530804899387576553m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.NAUTICAL_MILE, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = LengthUnit.POINT.Convert(LengthUnit.YARD, valIn);
      tRef = 0.4762993703284938544619422572m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = LengthUnit.YARD.Convert(LengthUnit.POINT, conv);
      Assert.That(res, Is.EqualTo(refValue));
   }

   #endregion
}