namespace BogaNet.Test.Extension;

public class ArrayExtensionTest
{
   [Test]
   public void BNToString_Test()
   {
      const string input = "crosstales LLC";
      byte[]? bytes = input.BNToByteArray();
      string? result = bytes.BNToString();

      Assert.That(input, Is.EqualTo(result));
   }
}