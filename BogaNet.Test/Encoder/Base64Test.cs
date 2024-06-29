using BogaNet.Encoder;

namespace BogaNet.Test.Encoder;

public class Base64Test
{
   #region Tests

   [Test]
   public void Base64_Test()
   {
      string plain = "BogaNet rülez!";

      //Byte-array
      string? output = Base64.ToBase64String(plain.BNToByteArray());
      string? plain2 = Base64.FromBase64String(output).BNToString();
      Assert.That(plain, Is.EqualTo(plain2));

      //String
      output = Base64.ToBase64String(plain);
      plain2 = Base64.FromBase64String(output).BNToString();
      Assert.That(plain, Is.EqualTo(plain2));
   }

   #endregion
}