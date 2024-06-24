using BogaNet.Util;

namespace BogaNet.Test.Util;

public class ObfuscatorTest
{
   #region Tests

   [Test]
   public void Obfuscator_Test()
   {
      for (byte IVgen = 0; IVgen < byte.MaxValue; IVgen++)
      {
         /*
         string testStr = "abc";
         var text2 = Obfuscator.Obfuscate(testStr, IVgen);
         var text3 = Obfuscator.DeobfuscateToString(text2, IVgen);

         Console.WriteLine($"{IVgen} - {BogaNet.Encoder.Base16.ToBase16String(testStr.BNToByteArray())} - Obf: {BogaNet.Encoder.Base16.ToBase16String(text2)}");
         Console.WriteLine($"{testStr} - {text3}");

         Assert.True(testStr.Equals(text3));
*/
         
         decimal dec = 35.8m;
         var t = dec.ToString();
         //var text2 = Obfuscator.Obfuscate(t, IVgen);
         //var text3 = Obfuscator.DeobfuscateToString(text2, IVgen);
         
         var text2 = Obfuscator.Obfuscate(dec.BNToByteArray(), IVgen);

         var text3 = Obfuscator.Deobfuscate(text2, IVgen);

         Console.WriteLine($"{IVgen} - {BogaNet.Encoder.Base16.ToBase16String(dec.BNToByteArray())} - {BogaNet.Encoder.Base16.ToBase16String(text2)}");

         //decimal decVal = decimal.Parse(text3);
         //Assert.True(t.Equals(text3));
         
         decimal decVal = text3.BNToNumber<decimal>();
         
         
         Assert.True(dec.Equals(decVal));
         
      }

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