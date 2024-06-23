using BogaNet.Crypto;

namespace BogaNet.Test.Crypto;

public class ObfuscatorTest
{
   #region Tests

   [Test]
   public void Obfuscator_Test()
   {
      string plain = "BogaNet rulez!";

      var output = Obfuscator.Obfuscate(plain.BNToByteArray());
      var plain2 = Obfuscator.Deobfuscate(output).BNToString();

      Assert.That(plain, Is.EqualTo(plain2));

      plain = "Another test?!";

      byte IV = Obfuscator.GenerateIV();

      output = Obfuscator.Obfuscate(plain.BNToByteArray(), IV);
      plain2 = Obfuscator.Deobfuscate(output, IV).BNToString();

      Assert.That(plain, Is.EqualTo(plain2));

      plain = "And another test?!";

      output = Obfuscator.Obfuscate(plain.BNToByteArray(), 10);
      plain2 = Obfuscator.Deobfuscate(output, 11).BNToString();

      Assert.False(plain == plain2);
   }

   #endregion
}