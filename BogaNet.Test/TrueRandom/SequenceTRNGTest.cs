using BogaNet.TrueRandom;

namespace BogaNet.Test.TrueRandom;

public class SequenceTRNGTest
{
   #region Tests

   [Test]
   public void Generate_Test()
   {
      int quotaStart = CheckQuota.GetQuota();
      Assert.That(quotaStart, Is.GreaterThan(0));

      const int min = -5;
      const int max = 5;

      var result = SequenceTRNG.Generate(min, max);

      Assert.That(result, Has.Count.EqualTo(max - min + 1));

      foreach (var res in result)
      {
         Assert.That(res, Is.InRange(min, max));
      }
/*
      int calcBits = TRNGSequence.CalcBits(min, max);
      int quotaEnd = CheckQuota.GetQuota();
      int quotaDiff = quotaStart - quotaEnd;

      Assert.That(quotaDiff, Is.InRange(calcBits * 0.9f, calcBits * 1.1f));
*/
   }

   [Test]
   public void GeneratePRNG_Test()
   {
      const int min = -6000;
      const int max = 6000;

      var result = SequenceTRNG.GeneratePRNG(min, max);

      Assert.That(result, Has.Count.EqualTo(max - min + 1));

      foreach (var res in result)
      {
         Assert.That(res, Is.InRange(min, max));
      }
   }

   #endregion
}