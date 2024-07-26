using BogaNet.CRC;

namespace BogaNet.Test.CRC;

public class CRC16Test
{
   #region Tests

   [Test]
   public void CRC16_Test()
   {
      const string plain = TestConstants.LatinText;
      const ushort refValue = 37545;

      ushort crc = CRC16.CalcCRC(plain);

      Assert.That(crc, Is.EqualTo(refValue));

      ushort crc2 = CRC16.CalcCRC(TestConstants.LatinTextCorrupted);

      Assert.That(crc2, !Is.EqualTo(crc));
   }

   #endregion
}