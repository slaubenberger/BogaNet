using BogaNet.Helper;
using BogaNet.Extension;
using BogaNet.Encoder;

namespace BogaNet.Test.Helper;

public class HMACHelperTest
{
   #region Tests

   [Test]
   public void HMACHelper_Test()
   {
      string plain = "BogaNet rulez!";
      byte[] secret = HMACHelper.GenerateSecret();

      byte[] h1 = HMACHelper.HashHMACSHA256(plain, secret);
      byte[] h2 = HMACHelper.HashHMACSHA256(plain, secret);

      Assert.That(h1, Is.EqualTo(h2));

      plain = "BogaNet ruleZ!";
      h2 = HMACHelper.HashHMACSHA256(plain, secret);

      Assert.That(h1, Is.Not.EqualTo(h2));

      plain = "BogaNet rulez!";
      secret = "abc123".BNToByteArray();
      h1 = HMACHelper.HashHMACSHA256(plain, secret);

      Base64.UseSaveFormat = false;
      string base64 = Base64.ToBase64String(h1);
      const string refValue = "ffJ5XqLTaKW6+Jis/M8ZoWL39mEjuzCR+jzcSunYs6o=";

      Assert.That(base64, Is.EqualTo(refValue));
   }

   #endregion
}