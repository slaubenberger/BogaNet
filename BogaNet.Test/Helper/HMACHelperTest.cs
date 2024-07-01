using BogaNet.Helper;
using BogaNet.Extension;

namespace BogaNet.Test.Helper;

public class HMACHelperTest
{
   //TODO improve tests

   #region Tests

   [Test]
   public void HMACHelper_Test()
   {
      string plain = "BogaNet rulez!";
      byte[] secret = HMACHelper.GenerateSecret();

      byte[] h1 = HMACHelper.HMAC256(plain, secret);
      byte[] h2 = HMACHelper.HMAC256(plain, secret);

      Assert.That(h1, Is.EqualTo(h2));

      plain = "BogaNet ruleZ!";
      h2 = HMACHelper.HMAC256(plain, secret);

      Assert.That(h1 == h2, Is.False);
   }

   #endregion
}