using BogaNet.Crypto;

namespace BogaNet.Test;

public class ObfuscatorTest
{
   #region Tests

   [Test]
   public void Obfuscator_Test()
   {
      string plain = "BogaNet rulez!";

      string output = Obfuscator.Obfuscate(plain);
      string plain2 = Obfuscator.Deobfuscate(output);

      Assert.False(plain == output);
      Assert.That(plain, Is.EqualTo(plain2));

      plain = "Another test?!";

      output = Obfuscator.Obfuscate(plain, 123);
      plain2 = Obfuscator.Deobfuscate(output, 123);

      Assert.False(plain == output);
      Assert.That(plain, Is.EqualTo(plain2));

      plain = "And another test?!";

      output = Obfuscator.Obfuscate(plain, 10);
      plain2 = Obfuscator.Deobfuscate(output, 11);

      Assert.False(plain == output);
      Assert.False(plain == plain2);
   }

   #endregion
}