using BogaNet.Util;

namespace BogaNet.Test.Util;

public class WindowsLocaleMapTest
{
   [Test]
   public void GetLocale_Test()
   {
      int locale = WindowsLocaleMap.GetLocale("de-ch");
      Assert.That(locale, Is.EqualTo(2055));

      locale = WindowsLocaleMap.GetLocale("dE");
      Assert.That(locale, Is.EqualTo(1031));

      locale = WindowsLocaleMap.GetLocale("fr");
      Assert.That(locale, Is.EqualTo(1036));

      locale = WindowsLocaleMap.GetLocale("It");
      Assert.That(locale, Is.EqualTo(1040));

      locale = WindowsLocaleMap.GetLocale("en");
      Assert.That(locale, Is.EqualTo(1033));

      locale = WindowsLocaleMap.GetLocale("hsdgbhagdsahjsg");
      Assert.That(locale, Is.EqualTo(1033));
   }

   [Test]
   public void GetISO_Test()
   {
      string ext = WindowsLocaleMap.GetISO(2055);
      Assert.That(ext, Is.EqualTo("de-CH"));

      ext = WindowsLocaleMap.GetISO(4108);
      Assert.That(ext, Is.EqualTo("fr-CH"));

      ext = WindowsLocaleMap.GetISO(999999);
      Assert.That(ext, Is.EqualTo("en-us"));
   }
}