using BogaNet.Helper;


namespace BogaNet.Test.Helper;

public class AESHelperTest
{
   #region Tests

   [Test]
   public void AESHelper_Test()
   {
      string plain = "BogaNet rulez!";

      string initVector = "SL$2OIjLS$2aIj76";
      byte[] IV = System.Text.Encoding.ASCII.GetBytes(initVector);

      string keyPlain = "abce1235";
      byte[] key = HashHelper.SHA256(keyPlain.BNToByteArray());

      var output = AESHelper.Encrypt(plain.BNToByteArray(), key, IV);
      string plain2 = AESHelper.Decrypt(output, key, IV).BNToString();

      Assert.That(plain, Is.EqualTo(plain2));
   }

   #endregion
}