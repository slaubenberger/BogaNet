using BogaNet.Helper;

namespace BogaNet.Test.Helper;

public class StringHelperTest
{
   //TODO improve tests

   [Test]
   public void ToTitleCaseOk()
   {
      //Test: input good
      const string input = "testing tiTle cASE";
      string? result = StringHelper.ToTitleCase(input);

      Assert.That(result, Is.EqualTo("Testing Title Case"));
   }

   [Test]
   public void ToTitleCaseNotOk()
   {
      //Test: input empty
      const string input = "";
      string? result = StringHelper.ToTitleCase(input);

      Assert.That(result, Is.Empty);
   }
}