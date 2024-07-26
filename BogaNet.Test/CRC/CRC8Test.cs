using BogaNet.CRC;

namespace BogaNet.Test.CRC;

public class CRC8Test
{
   #region Tests

   [Test]
   public void CRC8_Test()
   {
      const string plain = TestConstants.LatinText;
      const byte refValue = 4;

      byte crc = CRC8.CalcCRC(plain);

      Assert.That(crc, Is.EqualTo(refValue));

      byte crc2 = CRC8.CalcCRC(TestConstants.LatinTextCorrupted);

      Assert.That(crc2, !Is.EqualTo(crc));
   }

   #endregion
}