using BogaNet.Util;

namespace BogaNet.Test.Util;

public class ShortUIDTest
{
   #region Tests

   [Test]
   public void ShortUID_Test()
   {
      Guid refGuid = Guid.NewGuid();
      ShortUID suid1 = refGuid.BNToShortUID();
      
      Assert.That(suid1.Code.Length, Is.EqualTo(22));
      
      Guid? resGuid = suid1.ToGuid();
      Assert.That(resGuid, Is.EqualTo(refGuid));

      suid1 = new ShortUID("4gZaAOk7jkexO8Zjz8anjQ");
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