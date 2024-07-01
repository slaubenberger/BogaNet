﻿using BogaNet.Helper;
using BogaNet.Extension;

namespace BogaNet.Test.Helper;

public class HashHelperTest
{
   //TODO improve tests
   #region Tests

   [Test]
   public void HashHelper_Test()
   {
      string plain = "BogaNet rulez!";

      byte[] h1 = HashHelper.SHA256(plain);
      byte[] h2 = HashHelper.SHA256(plain);

      Assert.That(h1, Is.EqualTo(h2));

      plain = "BogaNet ruleZ!";
      h2 = HashHelper.SHA256(plain);

      Assert.That(h1 == h2, Is.False);
   }

   #endregion
}