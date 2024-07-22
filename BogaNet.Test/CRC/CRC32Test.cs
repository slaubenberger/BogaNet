using BogaNet.CRC;

namespace BogaNet.Test.CRC;

public class CRC32Test
{
   #region Tests

   [Test]
   public void CRC32_Test()
   {
      const string plain = TestConstants.LatinText;
      const uint refValue = 2461463659;

      uint crc = CRC32.CalcCRC(plain);

      Assert.That(crc, Is.EqualTo(refValue));
   }

   #endregion
}