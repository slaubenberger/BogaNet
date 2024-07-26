using BogaNet.CRC;

namespace BogaNet.Test.CRC;

public class CRC64Test
{
   #region Tests

   [Test]
   public void CRC64_Test()
   {
      const string plain = TestConstants.LatinText;
      const ulong refValue = 7127998075804121923;

      ulong crc = CRC64.CalcCRC(plain);

      Assert.That(crc, Is.EqualTo(refValue));
      
      ulong crc2 = CRC64.CalcCRC(TestConstants.LatinTextCorrupted);

      Assert.That(crc2, !Is.EqualTo(crc));
   }

   #endregion
}