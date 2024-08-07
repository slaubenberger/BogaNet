using BogaNet.TrueRandom;

namespace BogaNet.Test.TrueRandom;

public class FloatTRNGTest
{
   #region Tests

   [Test]
   public void Generate_Test()
   {
      int quotaStart = CheckQuota.GetQuota();
      Assert.That(quotaStart, Is.GreaterThan(0));

      const float min = -10;
      //min = -1000000000f;
      const float max = 10;
      //max = 1000000000f;
      const int number = 2;

      var result = FloatTRNG.Generate(min, max, number);

      Assert.That(result, Has.Count.EqualTo(number));

      foreach (var res in result)
      {
         Assert.That(res, Is.InRange(min, max));
      }
/*
      int calcBits = TRNGFloat.CalcBits(number);
      int quotaEnd = CheckQuota.GetQuota();
      int quotaDiff = quotaStart - quotaEnd;

      Assert.That(quotaDiff, Is.InRange(calcBits * 0.9f, calcBits * 1.1f));
*/
   }

   [Test]
   public void GeneratePRNG_Test()
   {
      const float min = -2000000000f;
      const float max = 2000000000f;
      const int number = 10;
      var result = FloatTRNG.GeneratePRNG(min, max, number);

      Assert.That(result, Has.Count.EqualTo(number));

      foreach (var res in result)
      {
         Assert.That(res, Is.InRange(min, max));
      }
   }

   #endregion
}