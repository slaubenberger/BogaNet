using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base85Test
{
   #region Tests

   [Test]
   public void Base85_Test()
   {
      const string plain = "BogaNet rülez!";
      //plain = "Hello world!";
      //Byte-array
      string output = Base85.ToBase85String(plain.BNToByteArray());
      string plain2 = Base85.FromBase85String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      Base85.EnforceMarks = false;
      //String
      output = Base85.ToBase85String(plain);
      byte[] bytes = Base85.FromBase85String(output);
      plain2 = bytes.BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "<~6>pLF:1\\MiEl5P+AU5L~>";
      plain2 = Base85.FromBase85String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}