using BogaNet.Helper;
using BogaNet.Extension;

namespace BogaNet.Test.Helper;

public class AESHelperTest
{
   //TODO improve tests
   #region Tests

   [Test]
   public void AESHelper_Test()
   {
      const string plain = "BogaNet rulez!";

      const string initVector = "SL$2OIjLS$2aIj76";
      byte[]? IV = initVector.BNToByteArray(System.Text.Encoding.ASCII);

      const string keyPlain = "abc123";
      byte[] key = HashHelper.SHA256(keyPlain.BNToByteArray());

      var output = AESHelper.Encrypt(plain.BNToByteArray(), key, IV);
      string? res = AESHelper.Decrypt(output, key, IV).BNToString();

      Assert.That(res, Is.EqualTo(plain));
   }

   #endregion
}