using BogaNet.Helper;

namespace BogaNet.Test.Helper;

public class NetworkHelperTest
{
   //TODO improve tests

   #region Variables

   private static readonly string _testUrl = Constants.COMPANY_URL;
   private const string _ipCT = "207.154.226.218"; //this may change
   private const string _ipLocalhost = "127.0.0.1";
   private const string _ipValid = "207.154.226";
   private const string _ipWrong1 = "256.154.226.218";
   private const string _ipWrong2 = "-1.154.226.218";
   private const string _ipWrong3 = "207.154.226.218.218";
   private const string _ipV6 = "2345:0425:2CA1:0000:0000:0567:5673:23b5";
   private const string _ipV6Complex = "2345:425:2CA1:0000:0000:567:5673:23b5/64";

   #endregion

   #region Tests

   [Test]
   public void GetURLFromFile_Test()
   {
      string file = FileHelper.TempFile;

      string? url = NetworkHelper.GetURLForFile(file);
      Assert.That(url, Is.EqualTo($"{Constants.PREFIX_FILE}{file.Replace('\\', '/')}"));

      url = NetworkHelper.GetURLForFile(_testUrl);
      Assert.That(url, Is.EqualTo(_testUrl));

      url = NetworkHelper.GetURLForFile("");
      Assert.That(url, Is.EqualTo(""));

      url = NetworkHelper.GetURLForFile(null);
      Assert.That(url, Is.EqualTo(null));
   }

   [Test]
   public void ValidateURL_Test()
   {
      string inUrl = "https://www.crosstales.com/";
      string outUrl = "crosstales.com";

      string url = NetworkHelper.ValidateURL(inUrl, true);
      Assert.That(url, Is.EqualTo(outUrl));

      outUrl = "https://crosstales.com";
      url = NetworkHelper.ValidateURL(inUrl);
      Assert.That(url, Is.EqualTo(outUrl));

      outUrl = "https://www.crosstales.com";
      url = NetworkHelper.ValidateURL(inUrl, false, false);
      Assert.That(url, Is.EqualTo(outUrl));

      url = NetworkHelper.ValidateURL(inUrl, false, false, false);
      Assert.That(url, Is.EqualTo(inUrl));

      inUrl = "https://www.crosstales.com/images\\ct logo.png";
      outUrl = "https://crosstales.com/images/ct%20logo.png";
      url = NetworkHelper.ValidateURL(inUrl);
      Assert.That(url, Is.EqualTo(outUrl));

      url = NetworkHelper.ValidateURL(_ipCT);
      Assert.That(url, Is.EqualTo(_ipCT));

      inUrl = "www.crosstales.com/";
      outUrl = "crosstales.com";

      url = NetworkHelper.ValidateURL(inUrl, true);
      Assert.That(url, Is.EqualTo(outUrl));
   }

   [Test]
   public void isURL_Test()
   {
      string file = FileHelper.TempFile;

      Assert.Multiple(() =>
      {
         Assert.That(NetworkHelper.IsURL(_testUrl), Is.True);
         Assert.That(NetworkHelper.IsURL(file), Is.False);
         Assert.That(NetworkHelper.IsURL(""), Is.False);
         Assert.That(NetworkHelper.IsURL(null), Is.False);
         Assert.That(NetworkHelper.IsURL("www.crosstales.com"), Is.True);
      });
   }

   [Test]
   public void isIPv4_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(NetworkHelper.IsIPv4(_ipCT), Is.True);
         Assert.That(NetworkHelper.IsIPv4(_ipLocalhost), Is.True);
         Assert.That(NetworkHelper.IsIPv4(_ipValid), Is.True);
         Assert.That(NetworkHelper.IsIPv4(_ipWrong1), Is.False);
         Assert.That(NetworkHelper.IsIPv4(_ipWrong2), Is.False);
         Assert.That(NetworkHelper.IsIPv4(_ipWrong3), Is.False);
         Assert.That(NetworkHelper.IsIPv4(_testUrl), Is.False);
         Assert.That(NetworkHelper.IsIPv4("ueli"), Is.False);
         Assert.That(NetworkHelper.IsIPv4(""), Is.False);
         Assert.That(NetworkHelper.IsIPv4(null), Is.False);
      });
   }

   [Test]
   public void isIPv6_Test()
   {
      Assert.Multiple(() =>
      {
         Assert.That(NetworkHelper.IsIPv6(_ipV6), Is.True);
         Assert.That(NetworkHelper.IsIPv6(_ipV6Complex), Is.True);
         Assert.That(NetworkHelper.IsIPv6(_ipCT), Is.False);
         Assert.That(NetworkHelper.IsIPv6(_ipLocalhost), Is.False);
         Assert.That(NetworkHelper.IsIPv6(_ipWrong1), Is.False);
         Assert.That(NetworkHelper.IsIPv6(_ipWrong2), Is.False);
         Assert.That(NetworkHelper.IsIPv6(_ipWrong3), Is.False);
         Assert.That(NetworkHelper.IsIPv6(_ipValid), Is.False);
         Assert.That(NetworkHelper.IsIPv6(_testUrl), Is.False);
         Assert.That(NetworkHelper.IsIPv6("ueli"), Is.False);
         Assert.That(NetworkHelper.IsIPv6(""), Is.False);
         Assert.That(NetworkHelper.IsIPv6(null), Is.False);
      });
   }

   [Test]
   public void GetIP_Test()
   {
      string ip = NetworkHelper.GetIP(_testUrl);
      Assert.That(ip, Is.EqualTo(_ipCT));

      ip = NetworkHelper.GetIP("localhost");
      Assert.That(ip, Is.EqualTo(_ipLocalhost));
   }

   [Test]
   public void GetPublicIP_Test()
   {
      string ip = NetworkHelper.GetPublicIP();
      Assert.That(ip, Is.EqualTo("188.63.132.68")); //TODO change for every run
   }

   [Test]
   public void GetNetworkAdapters_Test()
   {
      var adapters = NetworkHelper.GetNetworkAdapters();
      Assert.That(adapters, Has.Count.GreaterThan(0));
   }

   [Test]
   public void CheckInternetAvailability_Test()
   {
      var isOnline = NetworkHelper.CheckInternetAvailability();
      Assert.That(isOnline, Is.EqualTo(true));
   }

   [Test]
   public void Ping_Test()
   {
      var ping = NetworkHelper.Ping("google.com");
      Assert.That(ping, Is.GreaterThan(0));
   }

   #endregion
}