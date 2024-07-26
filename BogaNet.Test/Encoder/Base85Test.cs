using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base85Test
{
   #region Tests

   [Test]
   public void Base85_Test()
   {
      const string plain = Constants.SIGNS_EXT;
      string output;
      string plain2;

      //BogaNet.Util.StopWatch watch = new();
      //watch.Start();
      // for (int ii = 0; ii < 10000; ii++)
      // {
      //plain = "Hello world!";
      //Byte-array
      output = Base85.ToBase85String(plain.BNToByteArray());
      plain2 = Base85.FromBase85String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
      // }

      //watch.Stop();
      //Console.WriteLine(watch.ElapsedTime);

      //String
      output = Base85.ToBase85String(plain);
      byte[] bytes = Base85.FromBase85String(output);
      plain2 = bytes.BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "5sdq,77Kd<8P2WL9hnJ\\;,U=l<E<1'=^&^n_h,dZ_hQ'a_hc3e_hu?i_iDWq_j&-'_jSE3_jk.E@q9._B4u!oCM[j*DfB]:F*)PJGBeCZ_k=oA_kb2I_l(DN_l:PR_lL\\X_lptb`KS3M_n3h!0JP==1c70M3&p";
      plain2 = Base85.FromBase85String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   [Test]
   public void Base85_NonLatinTest()
   {
      const string plain = TestConstants.NonLatinText;

      string output = Base85.ToBase85String(plain);
      string plain2 = Base85.FromBase85String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "j+EEQK<=0d]]4XJj+G#)K<+$bM'4#&\\<c<CN95_jje]ZZ]Rf\"NX5LLci4-&M\\<Pp]Pi0'Zi4+[&\\?b&&Vr5+;i4,WA\\:Ge";
      plain2 = Base85.FromBase85String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}