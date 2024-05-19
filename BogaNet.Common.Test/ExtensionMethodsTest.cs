namespace BogaNet.Test;

public class ExtensionMethodsTest
{
   [Test]
   public void CTToTitleCaseOk()
   {
      //Test: input good
      const string input = "testing tiTle cASE";
      string? result = input.CTToTitleCase();

      Assert.That(result, Is.EqualTo("Testing Title Case"));
   }

   [Test]
   public void CTToTitleCaseNotOk()
   {
      //Test: input empty
      const string input = "";
      string? result = input.CTToTitleCase();

      Assert.IsEmpty(result);
   }

   [Test]
   public void CTReverseOk()
   {
      //Test: input good
      const string input = "crosstales LLC";
      string? result = input.CTReverse();

      Assert.That(result, Is.EqualTo("CLL selatssorc"));
   }

   [Test]
   public void CTReverseNotOk()
   {
      //Test: input empty
      const string input = "";
      string? result = input.CTReverse();

      Assert.IsEmpty(result);
   }

   [Test]
   public void CTReplaceOk()
   {
      //Test: input good
      const string input = "crosstales GmbH";
      const string oldString = "gmbh";
      const string newString = "LLC";
      string? result = input.CTReplace(oldString, newString);

      Assert.That(result, Is.EqualTo("crosstales LLC"));
   }

   [Test]
   public void CTReplaceNotOk()
   {
      //Test: input empty
      const string input = "";
      const string oldString = "gmbh";
      const string newString = "LLC";
      string? result = input.CTReplace(oldString, newString);

      Assert.IsEmpty(result);
/*
         //Test: newString null
         input = "crosstales GmbH";
         oldString = "gmbh";
         newString = null;

         try
         {
            result = input.CTReplace(oldString, newString);
            Assert.True(false);
         }
         catch (System.ArgumentNullException)
         {
            Assert.True(true);
         }
*/
   }
}