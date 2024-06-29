namespace BogaNet.Test.Extension;

public class ByteExtensionTest
{
   [Test]
   public void BNToBinary_Test()
   {
      byte input = 0;
      string expected = "00000000";
      string? result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));

      input = 1;
      expected = "00000001";
      result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));

      input = 2;
      expected = "00000010";
      result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));

      input = 4;
      expected = "00000100";
      result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));

      input = 8;
      expected = "00001000";
      result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));

      input = 16;
      expected = "00010000";
      result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));

      input = 32;
      expected = "00100000";
      result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));

      input = 64;
      expected = "01000000";
      result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));

      input = 128;
      expected = "10000000";
      result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));

      input = 255;
      expected = "11111111";
      result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));

      input = 113;
      expected = "01110001";
      result = input.BNToBinary();

      Assert.That(result, Is.EqualTo(expected));
   }
}