using BogaNet.Crypto;

namespace BogaNet.Test;

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

      output = Obfuscator.Obfuscate(plain.BNToByteArray(), 123);
      plain2 = Obfuscator.Deobfuscate(output, 123).BNToString();

      Assert.That(plain, Is.EqualTo(plain2));

      plain = "And another test?!";

      output = Obfuscator.Obfuscate(plain.BNToByteArray(), 10);
      plain2 = Obfuscator.Deobfuscate(output, 11).BNToString();

      Assert.False(plain == plain2);
   }

   #endregion
}