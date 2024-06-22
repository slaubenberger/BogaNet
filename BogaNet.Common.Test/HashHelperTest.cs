using BogaNet.Helper;

namespace BogaNet.Test;

public class HashHelperTest
{
   #region Tests

   [Test]
   public void HashHelper_Test()
   {
      string plain = "BogaNet rulez!";

      var h1 = HashHelper.SHA256(plain.BNToByteArray());
      var h2 = HashHelper.SHA256(plain.BNToByteArray());

      Assert.That(h1, Is.EqualTo(h2));

      plain = "BogaNet ruleZ!";
      h2 = HashHelper.SHA256(plain.BNToByteArray());

      Assert.False(h1 == h2);
   }

   #endregion
}