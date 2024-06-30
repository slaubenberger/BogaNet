namespace BogaNet.Test.Extension;

public class DateTimeExtensionTest
{
   [Test]
   public void BNConvert()
   {
      DateTime dtUtc = DateTime.UtcNow;

      DateTime dtLocal = dtUtc.BNConvertToTimeZone();

      if (TimeZoneInfo.Local != TimeZoneInfo.Utc)
         Assert.That(dtLocal, !Is.EqualTo(dtUtc));

      DateTime dtUtc2 = dtUtc.BNConvertTimeZoneToUtc();

      Assert.That(dtUtc2, Is.EqualTo(dtUtc));
   }
}