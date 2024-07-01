using BogaNet.Unit;
using BogaNet.Extension;

namespace BogaNet.Test.Unit;

public class VolumeUnitTest
{
   [OneTimeSetUp]
   public static void Init()
   {
      VolumeUnitExtension.IgnoreSameUnit = false;
   }

   #region Tests

   [Test]
   public void VolumeUnit_Convert_Test()
   {
      const double valIn = 1234.5678901234;
      decimal refValue = valIn.BNToDecimal();

      decimal conv = VolumeUnit.LITER.Convert(VolumeUnit.PINT_US, valIn);
      decimal tRef = 2609.1066664749411579472168727m;
      Assert.That(conv, Is.EqualTo(tRef));
      decimal res = VolumeUnit.PINT_US.Convert(VolumeUnit.LITER, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.MM3.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 0.0026091066664749411579472169m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.MM3, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.CM3.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 2.6091066664749411579472168727m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.CM3, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.CENTILITER.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 26.091066664749411579472168727m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.CENTILITER, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.DECILITER.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 260.91066664749411579472168727m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.DECILITER, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.DECALITER.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 26091.066664749411579472168727m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.DECALITER, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.HECTOLITER.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 260910.66664749411579472168727m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.HECTOLITER, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.M3.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 2609106.6664749411579472168727m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.M3, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.INCH3.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 42.755597926351515151515151515m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.INCH3, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.FOOT3.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 73881.720180655414730759024868m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.FOOT3, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.YARD3.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 1994805.2864760819895202186014m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.YARD3, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.PINT_US.Convert(VolumeUnit.PINT_US, valIn);
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.PINT_US, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.GALLON_US.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 9876.5431209872m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.GALLON_US, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.BARREL.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 414814.8110814624m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.BARREL, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.CUP_US.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 626.18559995398587790733204945m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.CUP_US, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.TABLESPOON_US.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 38.580234091565000916687588565m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.TABLESPOON_US, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.TEASPOON_US.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 12.860083248734999922111512083m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.TEASPOON_US, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = VolumeUnit.QUART_US.Convert(VolumeUnit.PINT_US, valIn);
      tRef = 2469.1359211385599896468225291m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = VolumeUnit.PINT_US.Convert(VolumeUnit.QUART_US, conv);
      Assert.That(res, Is.EqualTo(refValue));
   }

   #endregion
}