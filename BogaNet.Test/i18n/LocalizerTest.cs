using BogaNet.i18n;
using BogaNet.Helper;
using System.Globalization;

namespace BogaNet.Test.i18n;

public class LocalizerTest
{
   [OneTimeSetUp]
   public static void Init()
   {
      string testDirectory = $"{FileHelper.CurrentDirectory}Testfiles/i18n/";

      Localizer.Instance.LoadFiles($"{testDirectory}BogaNet.csv", $"{testDirectory}BogaNet_de.csv");
   }

   [Test]
   public void Localizer_Test()
   {
      Localizer.Instance.Culture = new CultureInfo("en");

      //Assert.That(Localizer.Instance.ContainsKey(null), Is.EqualTo(false));

      string key = "Greeting";
      string? text = Localizer.Instance.GetText(key);
      string refText = "Hi there!";
      Assert.That(text, Is.EqualTo(refText));

      Localizer.Instance.Culture = new CultureInfo("de");

      text = Localizer.Instance.GetText(key);
      refText = "Hallöchen zusammen!";
      Assert.That(text, Is.EqualTo(refText));

      //invalid key
      key = "InvalidKey";
      text = Localizer.Instance.GetText(key);
      refText = "???InvalidKey???";
      Assert.That(text, Is.EqualTo(refText));
   }

   [Test]
   public void Localizer_Add_Remove_Test()
   {
      Localizer.Instance.Culture = new CultureInfo("en");

      //add new key/value
      const string newKey = "GreetingNew";
      string refText = "Hello world!";
      Localizer.Instance.Add(newKey, new CultureInfo("en"), refText);

      string? text = Localizer.Instance.GetText(newKey);
      Assert.That(text, Is.EqualTo(refText));

      Localizer.Instance.Culture = new CultureInfo("de");
      text = Localizer.Instance.GetText(newKey);
      Assert.That(text, Is.EqualTo(refText));

      //remove new key/value
      Localizer.Instance.Remove(newKey);
      refText = $"???{newKey}???";
      text = Localizer.Instance.GetText(newKey);
      Assert.That(text, Is.EqualTo(refText));
   }

   [Test]
   public void Localizer_Replace_Test()
   {
      Localizer.Instance.Culture = new CultureInfo("en");

      const string key = "ReplaceMe";
      string? text = Localizer.Instance.GetTextWithReplacements(key, TextType.LABEL, "BogaNet", ".NET 8");
      const string refText = "Hello, I'm BogaNet, your awesome library for .NET 8!";
      Assert.That(text, Is.EqualTo(refText));
   }

   [Test]
   public void Localizer_MultiValue_Test()
   {
      Localizer.Instance.Culture = new CultureInfo("en");

      const string key = "Name";
      string? text = Localizer.Instance.GetText(key);
      string refText = "Name:";
      Assert.That(text, Is.EqualTo(refText));

      text = Localizer.Instance.GetText(key, TextType.TOOLTIP);
      refText = "Enter your name to access the application.";
      Assert.That(text, Is.EqualTo(refText));

      text = Localizer.Instance.GetText(key, TextType.PLACEHOLDER);
      refText = "Enter your name here...";
      Assert.That(text, Is.EqualTo(refText));
   }
}