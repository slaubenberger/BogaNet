using BogaNet.TrueRandom;

namespace BogaNet.Test.TrueRandom;

public class IntegerTRNGTest
{
   #region Tests

   [Test]
   public void Generate_Test()
   {
      int quotaStart = CheckQuota.GetQuota();
      Assert.That(quotaStart, Is.GreaterThan(0));

      const int min = -10;
      //min = -1000000000;
      const int max = 10;
      //max = 1000000000;
      const int number = 2;

      var result = IntegerTRNG.Generate(min, max, number);

      Assert.That(result, Has.Count.EqualTo(number));

      foreach (var res in result)
      {
         Assert.That(res, Is.InRange(min, max));
      }
/*
      int calcBits = TRNGInteger.CalcBits(min, max, number);
      int quotaEnd = CheckQuota.GetQuota();
      int quotaDiff = quotaStart - quotaEnd;

      Assert.That(quotaDiff, Is.InRange(calcBits * 0.9f, calcBits * 1.1f));
*/
   }

   [Test]
   public void GeneratePRNG_Test()
   {
      const int min = -1500000000;
      const int max = 1500000000;
      const int number = 10;
      var result = IntegerTRNG.GeneratePRNG(min, max, number);

      Assert.That(result, Has.Count.EqualTo(number));

      foreach (var res in result)
      {
         Assert.That(res, Is.InRange(min, max));
      }
   }

   #endregion
}