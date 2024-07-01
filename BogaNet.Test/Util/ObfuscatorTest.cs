using BogaNet.Util;
using BogaNet.Extension;

namespace BogaNet.Test.Util;

public class ObfuscatorTest
{
   #region Tests

   [Test]
   public void Obfuscator_Test()
   {
      string plain;

      for (byte IVgen = 0; IVgen < byte.MaxValue; IVgen++)
      {
         plain = "abc";
         var text2 = Obfuscator.Obfuscate(plain, IVgen);
         var text3 = Obfuscator.Deobfuscate(text2, IVgen).BNToString();

         Assert.That(text3, Is.EqualTo(plain));

         decimal dec = 35.8m;

         var decBytes = Obfuscator.Obfuscate(dec.BNToByteArray(), IVgen);
         var decEncBytes = Obfuscator.Deobfuscate(decBytes, IVgen);

         decimal decVal = decEncBytes.BNToNumber<decimal>();
         Assert.That(decVal, Is.EqualTo(dec));
      }

      plain = "BogaNet rülez!";

      var output = Obfuscator.Obfuscate(plain.BNToByteArray());
      var res = Obfuscator.Deobfuscate(output).BNToString();

      Assert.That(res, Is.EqualTo(plain));

      plain = "Another test?!";

      byte IV = Obfuscator.GenerateIV();

      output = Obfuscator.Obfuscate(plain.BNToByteArray(), IV);
      res = Obfuscator.Deobfuscate(output, IV).BNToString();

      Assert.That(res, Is.EqualTo(plain));

      //wrong IV test
      plain = "And another test?!";

      output = Obfuscator.Obfuscate(plain.BNToByteArray(), 10);
      res = Obfuscator.Deobfuscate(output, 11).BNToString();

      Assert.That(plain == res, Is.False);
   }

   #endregion
}