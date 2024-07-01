using BogaNet.Extension;

namespace BogaNet.Test.Extension;

public class StringExtensionTest
{
   //TODO improve tests

   [Test]
   public void BNReverseOk()
   {
      //Test: input good
      const string input = "crosstales LLC";
      string? result = input.BNReverse();

      Assert.That(result, Is.EqualTo("CLL selatssorc"));
   }

   [Test]
   public void BNReverseNotOk()
   {
      //Test: input empty
      const string input = "";
      string? result = input.BNReverse();

      Assert.IsEmpty(result);
   }

   [Test]
   public void BNReplaceOk()
   {
      //Test: input good
      const string input = "crosstales GmbH";
      const string oldString = "gmbh";
      const string newString = "LLC";
      string? result = input.BNReplace(oldString, newString);

      Assert.That(result, Is.EqualTo("crosstales LLC"));
   }

   [Test]
   public void BNReplaceNotOk()
   {
      //Test: input empty
      const string input = "";
      const string oldString = "gmbh";
      const string newString = "LLC";
      string? result = input.BNReplace(oldString, newString);

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