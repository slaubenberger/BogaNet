using BogaNet.ObfuscatedType;
using BogaNet.Test.Testfiles;

namespace BogaNet.Test.ObfuscatedType;

public class ObfuscatedTypeTest
{
   #region Tests

   [Test]
   public void Byte_Test()
   {
      ByteObf age = 35;
      const byte years = 7;
      age += years;

      byte res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Bool_Test()
   {
      BoolObf isOk = true;

      bool res = isOk;
      Assert.True(isOk == res);
   }

   [Test]
   public void Char_Test()
   {
      CharObf ch = 'A';

      char res = ch;
      Assert.True(ch == res);
   }

   [Test]
   public void Decimal_Test()
   {
      DecimalObf age = 35.8m;
      const decimal years = 7;
      age += years;

      decimal res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Double_Test()
   {
      DoubleObf age = 35.8;
      const double years = 7;
      age += years;

      double res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Float_Test()
   {
      FloatObf age = 35.8f;
      const float years = 7;
      age += years;

      float res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Int_Test()
   {
      IntObf age = 35;
      const int years = 7;
      age += years;

      int res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Long_Test()
   {
      LongObf age = 35;
      const long years = 7;
      age += years;

      long res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Nint_Test()
   {
      NintObf age = 35;
      const IntPtr years = 7;
      age += years;

      nint res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Nuint_Test()
   {
      NuintObf age = 35;
      const UIntPtr years = 7;
      age += years;

      nuint res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Sbyte_Test()
   {
      SbyteObf age = 35;
      const sbyte years = 7;
      age += years;

      sbyte res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Short_Test()
   {
      ShortObf age = 35;
      const short years = 7;
      age += years;

      short res = age;
      Assert.True(age == res);
   }

   [Test]
   public void String_Test()
   {
      StringObf text = $"Hellö everybödy! {DateTime.Now}";
      const string frag = " BYE";
      text += frag;

      string textB = text;

      Assert.That(text.Equals(textB), Is.True);
   }

   [Test]
   public void Uint_Test()
   {
      UintObf age = 35;
      const uint years = 7;
      age += years;

      uint res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Ulong_Test()
   {
      UlongObf age = 35;
      const ulong years = 7;
      age += years;

      ulong res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Ushort_Test()
   {
      UshortObf age = 35;
      const ushort years = 7;
      age += years;

      ushort res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Object_Test()
   {
      TestClass refObj = new();
      ObjectObf<TestClass> obj = refObj;
      TestClass tm = obj;
      Assert.True(obj.Equals(refObj));
      Assert.That(tm, Is.EqualTo(refObj));
   }

   #endregion
}