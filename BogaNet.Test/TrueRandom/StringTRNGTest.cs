using BogaNet.TrueRandom;

namespace BogaNet.Test.TrueRandom;

public class StringTRNGTest
{
   #region Tests

   [Test]
   public void Generate_Test()
   {
      int quotaStart = CheckQuota.GetQuota();
      Assert.That(quotaStart, Is.GreaterThan(0));

      int length = 8;
      int number = 2;

      var result = StringTRNG.Generate(length, number);

      Assert.That(result.Count, Is.EqualTo(number));

      foreach (var res in result)
      {
         Assert.That(res.Length, Is.EqualTo(length));
      }
/*
      int calcBits = TRNGString.CalcBits(length, number);
      int quotaEnd = CheckQuota.GetQuota();
      int quotaDiff = quotaStart - quotaEnd;

      Assert.That(quotaDiff, Is.InRange(calcBits * 0.9f, calcBits * 1.1f));
*/
   }

   [Test]
   public void GeneratePRNG_Test()
   {
      int length = 24;
      int number = 10;
      var result = StringTRNG.GeneratePRNG(length, number);

      Assert.That(result.Count, Is.EqualTo(number));

      foreach (var res in result)
      {
         Assert.That(res.Length, Is.EqualTo(length));
      }
   }

   #endregion
}