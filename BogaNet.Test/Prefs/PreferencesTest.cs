using BogaNet.Prefs;
using BogaNet.Test.Testfiles;

namespace BogaNet.Test.Prefs;

public class PreferencesTest
{
   [OneTimeSetUp]
   public static void Init()
   {
      Preferences.Instance.AutoSave = false;
   }

   #region Tests

   [Test]
   public void Preferences_String_Test()
   {
      string refText = "Hello wörld!";
      string key = "text";
      Preferences.Instance.Set(key, refText);
      string text = Preferences.Instance.GetString(key);
      Assert.That(text, Is.EqualTo(refText));

      string refTextObf = "Hello obfuscated wörld!";
      string keyObf = "textObf";
      Preferences.Instance.Set(keyObf, refTextObf, true);
      text = Preferences.Instance.GetString(keyObf, true);
      Assert.That(text, Is.EqualTo(refTextObf));
   }

   [Test]
   public void Preferences_Object_Test()
   {
      TestModel refValue = new();
      refValue.PublicString = "Hello";
      refValue.PublicProp = "Wörld";
      string key = "object";
      Preferences.Instance.Set(key, refValue);
      TestModel testModel = Preferences.Instance.GetObject<TestModel>(key);
      Assert.That(testModel, Is.EqualTo(refValue));

      string keyObf = "objectObf";
      Preferences.Instance.Set(keyObf, refValue, true);
      testModel = Preferences.Instance.GetObject<TestModel>(keyObf, true);
      Assert.That(testModel, Is.EqualTo(refValue));
   }

   [Test]
   public void Preferences_Number_Test()
   {
      double refVal = 12.345;
      string key = "number";
      Preferences.Instance.Set(key, refVal);
      double number = Preferences.Instance.GetNumber<double>(key);
      Assert.That(number, Is.EqualTo(refVal));

      double refObf = 54.321;
      string keyObf = "numberObf";
      Preferences.Instance.Set(keyObf, refObf, true);
      number = Preferences.Instance.GetNumber<double>(keyObf, true);
      Assert.That(number, Is.EqualTo(refObf));
   }

   [Test]
   public void Preferences_Bool_Test()
   {
      bool refVal = true;
      string key = "bool";
      Preferences.Instance.Set(key, refVal);
      bool boolean = Preferences.Instance.GetBool(key);
      Assert.That(boolean, Is.EqualTo(refVal));

      string keyObf = "boolObf";
      Preferences.Instance.Set(keyObf, refVal, true);
      boolean = Preferences.Instance.GetBool(keyObf, true);
      Assert.That(boolean, Is.EqualTo(refVal));
   }

   [Test]
   public void Preferences_DateTimeUtc_Test()
   {
      DateTime refVal = DateTime.UtcNow;
      string key = "date";
      Preferences.Instance.Set(key, refVal);
      DateTime dt = Preferences.Instance.GetDate(key, false, TimeZoneInfo.Utc);
      Assert.That(dt, Is.EqualTo(refVal));

      string keyObf = "dateObf";
      Preferences.Instance.Set(keyObf, refVal, true);
      dt = Preferences.Instance.GetDate(keyObf, true, TimeZoneInfo.Utc);
      Assert.That(dt, Is.EqualTo(refVal));
   }

   [Test]
   public void Preferences_DateTime_Test()
   {
      DateTime refVal = DateTime.Now;
      string key = "date";
      Preferences.Instance.Set(key, refVal);
      DateTime dt = Preferences.Instance.GetDate(key);
      Assert.That(dt, Is.EqualTo(refVal));

      string keyObf = "dateObf";
      Preferences.Instance.Set(keyObf, refVal, true);
      dt = Preferences.Instance.GetDate(keyObf, true);
      Assert.That(dt, Is.EqualTo(refVal));
   }

   #endregion
}