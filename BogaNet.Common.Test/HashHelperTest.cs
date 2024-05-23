using BogaNet.Crypto;

namespace BogaNet.Test;

public class HashHelperTest
{
   #region Tests

   [Test]
   public void HashHelper_Test()
   {
      string plain = "BogaNet rulez!";

      string h1 = HashHelper.SHA256AsString(plain);
      string h2 = HashHelper.SHA256AsString(plain);

      Assert.That(h1, Is.EqualTo(h2));

      plain = "BogaNet ruleZ!";
      h2 = HashHelper.SHA256AsString(plain);

      Assert.False(h1 == h2);
   }

   #endregion
}