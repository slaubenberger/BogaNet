using BogaNet.CRC;

namespace BogaNet.Test.CRC;

public class CRC16Test
{
   #region Tests

   [Test]
   public void CRC16_Test()
   {
      string plain = "BogaNet rulez!";
      const ushort refValue = 5090;

      ushort crc = CRC16.CalcCRC16(plain);

      Assert.That(crc, Is.EqualTo(refValue));
   }

   #endregion
}