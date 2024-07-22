using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base62Test
{
   #region Tests

   [Test]
   public void Base62_Test()
   {
      const string plain = Constants.SIGNS_EXT;
      string output;
      string plain2;

      //BogaNet.Util.StopWatch watch = new();
      //watch.Start();
      for (int ii = 0; ii < 10000; ii++)
      {
         //Byte-array
         output = Base62.ToBase62String(plain.BNToByteArray());
         plain2 = Base62.FromBase62String(output).BNToString();
         Assert.That(plain2, Is.EqualTo(plain));
      }

      //watch.Stop();
      //Console.WriteLine(watch.ElapsedTime);

      //String
      output = Base62.ToBase62String(plain);
      byte[] bytes = Base62.FromBase62String(output);
      plain2 = bytes.BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "qp5OyuFRUJMmdgTRQQ4B3V6uOEAssFgCAtUsAnQYut3dRsK5wDOTGrWSBVsgwZRFo1bkj5H7DlUuiGXcfq0zoHvrkLQXKyFoHDnjgPOJhvzs2YUommthOdBQjWpsYFfeER1NlFslanpmAkVegIKKgEFBuqIxxJZlwy6zatKQr";
      plain2 = Base62.FromBase62String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   [Test]
   public void Base62_NonLatinTest()
   {
      const string plain = TestConstants.NonLatinText;

      string output = Base62.ToBase62String(plain);
      string plain2 = Base62.FromBase62String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "LGDgSqhDel2rYvNLtVtbFKJoXkgSMqx4kgRdOYxcsVGteUzNpIVlkBvnZCD97gHgA0lDHLesmhAOo6WxEV1nWJHVjfSdK7RaUhnCj";
      plain2 = Base62.FromBase62String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}