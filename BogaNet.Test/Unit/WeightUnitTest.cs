using BogaNet.Unit;
using BogaNet.Extension;

namespace BogaNet.Test.Unit;

public class WeightUnitTest
{
   [OneTimeSetUp]
   public static void Init()
   {
      WeightUnitExtension.IgnoreSameUnit = false;
   }

   #region Tests

   [Test]
   public void WeightUnit_Convert_Test()
   {
      const double valIn = 1234.5678901234;
      decimal refValue = valIn.BNToDecimal();

      decimal conv = WeightUnit.MILLIGRAM.Convert(WeightUnit.POUND, valIn);
      decimal tRef = 0.0027217585189408102435669059m;
      Assert.That(conv, Is.EqualTo(tRef));
      decimal res = WeightUnit.POUND.Convert(WeightUnit.MILLIGRAM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.CENTIGRAM.Convert(WeightUnit.POUND, valIn);
      tRef = 0.0272175851894081024356690594m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = WeightUnit.POUND.Convert(WeightUnit.CENTIGRAM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.DECIGRAM.Convert(WeightUnit.POUND, valIn);
      tRef = 0.2721758518940810243566905942m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = WeightUnit.POUND.Convert(WeightUnit.DECIGRAM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.GRAM.Convert(WeightUnit.POUND, valIn);
      tRef = 2.7217585189408102435669059419m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = WeightUnit.POUND.Convert(WeightUnit.GRAM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.METRIC_TON.Convert(WeightUnit.POUND, valIn);
      tRef = 2721758.5189408102435669059419m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = WeightUnit.POUND.Convert(WeightUnit.METRIC_TON, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.KILOGRAM.Convert(WeightUnit.POUND, valIn);
      tRef = 2721.7585189408102435669059419m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = WeightUnit.POUND.Convert(WeightUnit.KILOGRAM, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.OUNCE.Convert(WeightUnit.POUND, valIn);
      tRef = 77.160547567882878816204871338m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = WeightUnit.POUND.Convert(WeightUnit.OUNCE, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.POUND.Convert(WeightUnit.POUND, valIn);
      tRef = 1234.5678901234m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = WeightUnit.POUND.Convert(WeightUnit.POUND, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.TON.Convert(WeightUnit.POUND, valIn);
      tRef = 2469137.6854777632585671704968m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = WeightUnit.POUND.Convert(WeightUnit.TON, conv);
      Assert.That(res, Is.EqualTo(refValue));

      conv = WeightUnit.STONE.Convert(WeightUnit.POUND, valIn);
      tRef = 17283.964070520194704051217835m;
      Assert.That(conv, Is.EqualTo(tRef));
      res = WeightUnit.POUND.Convert(WeightUnit.STONE, conv);
      Assert.That(res, Is.EqualTo(refValue));
   }

   #endregion
}