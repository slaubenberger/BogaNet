using BogaNet.CRC;

namespace BogaNet.Test.CRC;

public class CRC8Test
{
   #region Tests

   [Test]
   public void CRC8_Test()
   {
      string plain = "BogaNet rulez!";
      const byte refValue = 109;

      byte crc = CRC8.CalcCRC8(plain);

      Assert.That(crc, Is.EqualTo(refValue));
   }

   #endregion
}