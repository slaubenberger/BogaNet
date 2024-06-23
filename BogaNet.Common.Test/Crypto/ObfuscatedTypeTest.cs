using BogaNet.Crypto.ObfuscatedType;

namespace BogaNet.Test.Crypto;

public class ObfuscatedTypeTest
{
   #region Tests

   [Test]
   public void Byte_Test()
   {
      ByteObf age = 35;
      byte years = 7;
      age += years;

      byte res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Char_Test()
   {
      CharObf ch = 'A';

      char res = ch;
      Assert.True(ch.Equals(res));
   }

   [Test]
   public void Decimal_Test()
   {
      DecimalObf age = 35.8m;
      decimal years = 7;
      age += years;

      decimal res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Double_Test()
   {
      DoubleObf age = 35.8;
      double years = 7;
      age += years;

      double res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Float_Test()
   {
      FloatObf age = 35.8f;
      float years = 7;
      age += years;

      float res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Int_Test()
   {
      IntObf age = 35;
      int years = 7;
      age += years;

      int res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Long_Test()
   {
      LongObf age = 35;
      long years = 7;
      age += years;

      long res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Nint_Test()
   {
      NintObf age = 35;
      nint years = 7;
      age += years;

      nint res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Nuint_Test()
   {
      NuintObf age = 35;
      nuint years = 7;
      age += years;

      nuint res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Sbyte_Test()
   {
      SbyteObf age = 35;
      sbyte years = 7;
      age += years;

      sbyte res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Short_Test()
   {
      ShortObf age = 35;
      short years = 7;
      age += years;

      short res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void String_Test()
   {
      StringObf text = $"Hello everybody! {DateTime.Now}";
      string frag = " BYE";
      text += frag;

      string textB = text;

      Assert.True(text.Equals(textB));
   }

   [Test]
   public void Uint_Test()
   {
      UintObf age = 35;
      uint years = 7;
      age += years;

      uint res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Ulong_Test()
   {
      UlongObf age = 35;
      ulong years = 7;
      age += years;

      ulong res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Ushort_Test()
   {
      UshortObf age = 35;
      ushort years = 7;
      age += years;

      ushort res = age;
      Assert.True(age.Equals(res));
   }

   #endregion
}