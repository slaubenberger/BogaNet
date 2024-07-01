using BogaNet.Unit;

namespace BogaNet.Test.Unit;

public class AreaUnitTest
{
   #region Tests

   [OneTimeSetUp]
   public static void Init()
   {
      AreaUnitExtension.IgnoreSameUnit = false;
   }

   [Test]
   public void AreaUnit_Convert_Test()
   {
      const double valIn = 1234.5678901234;
      decimal refValue = valIn.BNToDecimal();

      decimal conv = AreaUnit.M2.Convert(AreaUnit.YARD2, valIn);
      decimal tRef = 1476.5309080705121286785783448m;
      Assert.That(conv, Is.EqualTo(tRef));
      decimal res = AreaUnit.YARD2.Convert(AreaUnit.M2, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = AreaUnit.MM2.Convert(AreaUnit.YARD2, valIn);
      tRef = 0.0014765309080705121286785783m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = AreaUnit.YARD2.Convert(AreaUnit.MM2, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = AreaUnit.CM2.Convert(AreaUnit.YARD2, valIn);
      tRef = 0.1476530908070512128678578345m;
      Assert.That(conv, Is.EqualTo(tRef));
      Assert.That(refValue, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.CM2, conv)));

      conv = AreaUnit.AREA.Convert(AreaUnit.YARD2, valIn);
      tRef = 147653.09080705121286785783448m;
      Assert.That(conv, Is.EqualTo(tRef));
      Assert.That(refValue, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.AREA, conv)));

      conv = AreaUnit.HECTARE.Convert(AreaUnit.YARD2, valIn);
      tRef = 14765309.080705121286785783448m;
      Assert.That(conv, Is.EqualTo(tRef));
      Assert.That(refValue, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.HECTARE, conv)));

      conv = AreaUnit.KM2.Convert(AreaUnit.YARD2, valIn);
      tRef = 1476530908.0705121286785783448m;
      Assert.That(conv, Is.EqualTo(tRef));
      Assert.That(refValue, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.KM2, conv)));

      conv = AreaUnit.INCH2.Convert(AreaUnit.YARD2, valIn);
      tRef = 0.9525986806507716049382716049m;
      Assert.That(conv, Is.EqualTo(tRef));
      Assert.That(refValue, Is.EqualTo(Decimal.Round(AreaUnit.YARD2.Convert(AreaUnit.INCH2, conv), 10)));

      conv = AreaUnit.FOOT2.Convert(AreaUnit.YARD2, valIn);
      tRef = 137.17421001371111111111111111m;
      Assert.That(conv, Is.EqualTo(tRef));
      Assert.That(refValue, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.FOOT2, conv)));

      conv = AreaUnit.YARD2.Convert(AreaUnit.YARD2, valIn);
      tRef = 1234.5678901234m;
      Assert.That(conv, Is.EqualTo(tRef));
      Assert.That(refValue, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.YARD2, conv)));

      conv = AreaUnit.PERCH.Convert(AreaUnit.YARD2, valIn);
      tRef = 37345.678617171613677179514853m;
      Assert.That(conv, Is.EqualTo(tRef));
      Assert.That(refValue, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.PERCH, conv)));

      conv = AreaUnit.ACRE.Convert(AreaUnit.YARD2, valIn);
      tRef = 5975308.588197256m;
      Assert.That(conv, Is.EqualTo(tRef));
      Assert.That(refValue, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.ACRE, conv)));

      conv = AreaUnit.MILE2.Convert(AreaUnit.YARD2, valIn);
      tRef = 3824197496.3930887273094615634m;
      Assert.That(conv, Is.EqualTo(tRef));
      Assert.That(refValue, Is.EqualTo(AreaUnit.YARD2.Convert(AreaUnit.MILE2, conv)));
   }

   #endregion
}