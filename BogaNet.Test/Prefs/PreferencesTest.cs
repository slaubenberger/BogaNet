using BogaNet.Prefs;
using BogaNet.Test.Testfiles;

namespace BogaNet.Test.Prefs;

public class PreferencesTest
{
   [OneTimeSetUp]
   public static void Init()
   {
      Preferences.Instance.AutoSaveOnExit = false;
   }

   #region Tests

   [Test]
   public void Preferences_String_Test()
   {
      const string refText = "Hello wörld!";
      const string key = "text";
      Preferences.Instance.Set(key, refText);
      string text = Preferences.Instance.GetString(key);
      Assert.That(text, Is.EqualTo(refText));

      const string refTextObf = "Hello obfuscated wörld!";
      const string keyObf = "textObf";
      Preferences.Instance.Set(keyObf, refTextObf, true);
      text = Preferences.Instance.GetString(keyObf, true);
      Assert.That(text, Is.EqualTo(refTextObf));
   }

   [Test]
   public void Preferences_Object_Test()
   {
      TestClass refValue = new()
      {
         PublicString = "Hello",
         PublicProp = "Wörld"
      };
      const string key = "object";
      Preferences.Instance.Set(key, refValue);
      TestClass testClass = Preferences.Instance.GetObject<TestClass>(key);
      Assert.That(testClass, Is.EqualTo(refValue));

      const string keyObf = "objectObf";
      Preferences.Instance.Set(keyObf, refValue, true);
      testClass = Preferences.Instance.GetObject<TestClass>(keyObf, true);
      Assert.That(testClass, Is.EqualTo(refValue));
   }

   [Test]
   public void Preferences_Number_Test()
   {
      const double refVal = 12.345;
      const string key = "number";
      Preferences.Instance.Set(key, refVal);
      double number = Preferences.Instance.GetNumber<double>(key);
      Assert.That(number, Is.EqualTo(refVal));

      const double refObf = 54.321;
      const string keyObf = "numberObf";
      Preferences.Instance.Set(keyObf, refObf, true);
      number = Preferences.Instance.GetNumber<double>(keyObf, true);
      Assert.That(number, Is.EqualTo(refObf));
   }

   [Test]
   public void Preferences_Bool_Test()
   {
      const bool refVal = true;
      const string key = "bool";
      Preferences.Instance.Set(key, refVal);
      bool boolean = Preferences.Instance.GetBool(key);
      Assert.That(boolean, Is.EqualTo(refVal));

      const string keyObf = "boolObf";
      Preferences.Instance.Set(keyObf, refVal, true);
      boolean = Preferences.Instance.GetBool(keyObf, true);
      Assert.That(boolean, Is.EqualTo(refVal));
   }

   [Test]
   public void Preferences_DateTimeUtc_Test()
   {
      DateTime refVal = DateTime.UtcNow;
      const string key = "date";
      Preferences.Instance.Set(key, refVal);
      DateTime dt = Preferences.Instance.GetDate(key, false, TimeZoneInfo.Utc);
      Assert.That(dt, Is.EqualTo(refVal));

      const string keyObf = "dateObf";
      Preferences.Instance.Set(keyObf, refVal, true);
      dt = Preferences.Instance.GetDate(keyObf, true, TimeZoneInfo.Utc);
      Assert.That(dt, Is.EqualTo(refVal));
   }

   [Test]
   public void Preferences_DateTime_Test()
   {
      DateTime refVal = DateTime.Now;
      const string key = "date";
      Preferences.Instance.Set(key, refVal);
      DateTime dt = Preferences.Instance.GetDate(key);
      Assert.That(dt, Is.EqualTo(refVal));

      const string keyObf = "dateObf";
      Preferences.Instance.Set(keyObf, refVal, true);
      dt = Preferences.Instance.GetDate(keyObf, true);
      Assert.That(dt, Is.EqualTo(refVal));
   }

   #endregion
}