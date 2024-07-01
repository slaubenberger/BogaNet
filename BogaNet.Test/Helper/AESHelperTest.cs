using BogaNet.Helper;
using BogaNet.Extension;

namespace BogaNet.Test.Helper;

public class AESHelperTest
{
   #region Tests

   [Test]
   public void AESHelper_Test()
   {
      const string plain = "BogaNet rulez!";

      string initVector = "SL$2OIjLS$2aIj76";
      byte[]? IV = initVector.BNToByteArray(System.Text.Encoding.ASCII);

      string keyPlain = "abce1235";
      byte[] key = HashHelper.SHA256(keyPlain.BNToByteArray());

      var output = AESHelper.Encrypt(plain.BNToByteArray(), key, IV);
      string? res = AESHelper.Decrypt(output, key, IV).BNToString();

      Assert.That(res, Is.EqualTo(plain));
   }

   #endregion
}