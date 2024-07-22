using BogaNet.Helper;
using BogaNet.Encoder;

namespace BogaNet.Test.Helper;

public class HashHelperTest
{
   #region Tests

   [Test]
   public void HashHelper_Test()
   {
      string plain = "BogaNet rulez!";

      byte[] h1 = HashHelper.HashSHA256(plain);
      byte[] h2 = HashHelper.HashSHA256(plain);

      Assert.That(h1, Is.EqualTo(h2));

      plain = "BogaNet ruleZ!";
      h2 = HashHelper.HashSHA256(plain);

      Assert.That(h1, Is.Not.EqualTo(h2));

      Base64.UseSaveFormat = false;
      string base64 = Base64.ToBase64String(h1);
      const string refValue = "8kNcJBdM1xzVVuYg9mdDkaItGCWPf+6v+DBwS0ppm6o=";

      Assert.That(base64, Is.EqualTo(refValue));
   }

   #endregion
}