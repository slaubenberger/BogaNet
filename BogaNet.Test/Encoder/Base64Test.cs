using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base64Test
{
   #region Tests

   [Test]
   public void Base64_Test()
   {
      string plain = "BogaNet rülez!";
      //plain = "Hello world!";
      //Byte-array
      string? output = Base64.ToBase64String(plain.BNToByteArray());
      string? plain2 = Base64.FromBase64String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      //String
      output = Base64.ToBase64String(plain);
      byte[] bytes = Base64.FromBase64String(output);
      plain2 = bytes.BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "Qm9nYU5ldCByw7xsZXoh";
      plain2 = Base64.FromBase64String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}