using BogaNet.CRC;

namespace BogaNet.Test.CRC;

public class CRC64Test
{
   #region Tests

   [Test]
   public void CRC64_Test()
   {
      const string plain = "BogaNet rulez!";
      const ulong refValue = 18056882464249972536;

      ulong crc = CRC64.CalcCRC(plain);

      Assert.That(crc, Is.EqualTo(refValue));
   }

   #endregion
}