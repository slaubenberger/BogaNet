using BogaNet.SecureType;
using BogaNet.Test.Testfiles;

namespace BogaNet.Test.SecureType;

public class SecureTypeTest
{
   #region Tests

   [Test]
   public void Byte_Test()
   {
      ByteSec age = 35;
      byte years = 7;
      age += years;

      byte res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Bool_Test()
   {
      BoolSec isOk = true;

      bool res = isOk;
      Assert.True(isOk == res);
   }

   [Test]
   public void Char_Test()
   {
      CharSec ch = 'A';

      char res = ch;
      Assert.True(ch == res);
   }

   [Test]
   public void Decimal_Test()
   {
      DecimalSec age = 35.8m;
      decimal years = 7;
      age += years;

      decimal res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Double_Test()
   {
      DoubleSec age = 35.8;
      double years = 7;
      age += years;

      double res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Float_Test()
   {
      FloatSec age = 35.8f;
      float years = 7;
      age += years;

      float res = age;
      Assert.True(age.Equals(res));
   }

   [Test]
   public void Int_Test()
   {
      IntSec age = 35;
      int years = 7;
      age += years;

      int res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Long_Test()
   {
      LongSec age = 35;
      long years = 7;
      age += years;

      long res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Nint_Test()
   {
      NintSec age = 35;
      nint years = 7;
      age += years;

      nint res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Nuint_Test()
   {
      NuintSec age = 35;
      nuint years = 7;
      age += years;

      nuint res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Sbyte_Test()
   {
      SbyteSec age = 35;
      sbyte years = 7;
      age += years;

      sbyte res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Short_Test()
   {
      ShortSec age = 35;
      short years = 7;
      age += years;

      short res = age;
      Assert.True(age == res);
   }

   [Test]
   public void String_Test()
   {
      StringSec text = $"Hello everybody! {DateTime.Now}";
      string frag = " BYE";
      text += frag;

      string textB = text;

      Assert.True(text.Equals(textB));
   }

   [Test]
   public void Uint_Test()
   {
      UintSec age = 35;
      uint years = 7;
      age += years;

      uint res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Ulong_Test()
   {
      UlongSec age = 35;
      ulong years = 7;
      age += years;

      ulong res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Ushort_Test()
   {
      UshortSec age = 35;
      ushort years = 7;
      age += years;

      ushort res = age;
      Assert.True(age == res);
   }

   [Test]
   public void Object_Test()
   {
      TestModel refObj = new();
      ObjectSec<TestModel> obj = refObj;
      TestModel tm = obj;
      Assert.True(obj.Equals(refObj));
      Assert.True(tm.Equals(refObj));
   }
   #endregion
}