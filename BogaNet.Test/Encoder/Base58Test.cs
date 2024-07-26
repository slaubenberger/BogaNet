using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Test.Encoder;

public class Base58Test
{
   #region Tests

   [Test]
   public void Base58_Test()
   {
      const string plain = Constants.SIGNS_EXT;
      string output;
      string plain2;

      //BogaNet.Util.StopWatch watch = new();
      //watch.Start();
      // for (int ii = 0; ii < 10000; ii++)
      // {
      //Byte-array
      output = Base58.ToBase58String(plain.BNToByteArray());
      plain2 = Base58.FromBase58String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
      // }

      //watch.Stop();
      //Console.WriteLine(watch.ElapsedTime);

      //String
      output = Base58.ToBase58String(plain);
      byte[] bytes = Base58.FromBase58String(output);
      plain2 = bytes.BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "Lt8CwJ1Phxv8ubJs7TW1Roa3eQLLCpBUm15Dg8TW8t1qtZgGJZY4MkmJ6vRLjXKhUQoknRnfrS7u4eRdVMUEedS2JtbVDWMA1H5DDqzrL3LBvEgFEdVkh6p7wM47nFygpnSmwg7dWg7FaS1RHY3AkSnuqynFgvhorQx7r9ZMzA6k";
      plain2 = Base58.FromBase58String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   [Test]
   public void Base58_NonLatinTest()
   {
      const string plain = TestConstants.NonLatinText;

      string output = Base58.ToBase58String(plain);
      string plain2 = Base58.FromBase58String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));

      output = "5ym7uUMfeHwUdYwDK1TCNLBLscPha8GfwixAyrchj51azcFHUkaiMMaY6uk4WwcrHd7npu6mNRo5BMhGgnaFo8uXKmPcxujJW62ibqa";
      plain2 = Base58.FromBase58String(output).BNToString();
      Assert.That(plain2, Is.EqualTo(plain));
   }

   #endregion
}