using BogaNet.CRC;

namespace BogaNet.Test.CRC;

public class CRC32Test
{
   #region Tests

   [Test]
   public void CRC32_Test()
   {
      const string plain = "BogaNet rulez!";
      const uint refValue = 903012262;

      uint crc = CRC32.CalcCRC(plain);

      Assert.That(crc, Is.EqualTo(refValue));
   }

   #endregion
}