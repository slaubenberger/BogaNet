﻿using BogaNet.Util;

namespace BogaNet.Test;

public class ShortUIDTest
{
   #region Tests

   [Test]
   public void ShortUID_Test()
   {
      Guid guid1 = Guid.NewGuid();
      ShortUID suid1 = guid1.BNToShortUID();
      Guid? guid2 = suid1.BNToGuid();

      Assert.That(guid1, Is.EqualTo(guid2));

      suid1 = new ShortUID("4gZaAOk7jkexO8Zjz8anjQ");
      guid1 = suid1.BNToGuid();
      ShortUID suid2 = guid1.BNToShortUID();

      Assert.That(suid1, Is.EqualTo(suid2));

      /*
       //should fail
      suid1 = new ShortUID("sdasfysayfsadfyxa");
      guid1 = suid1.BNToGuid();
      suid2 = guid1?.BNToShortUID();

      Assert.That(suid1, Is.EqualTo(suid2));
      */
   }

   #endregion
}