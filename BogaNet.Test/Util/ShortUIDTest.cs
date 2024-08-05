using System;
using BogaNet.Util;

namespace BogaNet.Test.Util;

public class ShortUIDTest
{
   #region Tests

   [Test]
   public void ShortUID_Test()
   {
      Guid refGuid;
      ShortUID suid1;

      for (int ii = 0; ii < 10000; ii++)
      {
         refGuid = Guid.NewGuid();
         suid1 = refGuid.BNToShortUID();

         Assert.That(suid1.Code, Has.Length.EqualTo(22));
         //Assert.That(suid1.Code, Has.Length.EqualTo(20)); //Base85 and 91

         Guid resGuid = suid1.ToGuid();
         Assert.That(resGuid, Is.EqualTo(refGuid));
      }

      suid1 = new ShortUID("4gZaAOk7jkexO8Zjz8anjQ");
      //suid1 = new ShortUID("3ID%E_o,JEOMekS0!O7O"); //Base85
      //suid1 = new ShortUID("PlPqwT/,5M]qBgoq8icI"); //Base91
      refGuid = suid1.ToGuid();
      ShortUID suid2 = refGuid.BNToShortUID();

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