using BogaNet.IO;

namespace BogaNet.Test;

public class NetworkHelperTest
{
    #region Variables

    private static readonly string testUrl = Constants.COMPANY_URL;
    private const string ipCT = "207.154.226.218"; //this may change
    private const string ipLocalhost = "127.0.0.1";
    private const string ipWrong1 = "256.154.226.218";
    private const string ipWrong2 = "-1.154.226.218";
    private const string ipWrong3 = "207.154.226.218.218";
    private const string ipWrong4 = "207.154.226";

    #endregion

    #region Tests

    [Test]
    public void GetURLFromFile_Test()
    {
        string file = FileHelper.TempFile;

        string? url = NetworkHelper.GetURLFromFile(file);
        Assert.That(url, Is.EqualTo($"{Constants.PREFIX_FILE}{file.Replace('\\', '/')}"));

        url = NetworkHelper.GetURLFromFile(testUrl);
        Assert.That(url, Is.EqualTo(testUrl));

        url = NetworkHelper.GetURLFromFile("");
        Assert.That(url, Is.EqualTo(""));

        url = NetworkHelper.GetURLFromFile(null);
        Assert.That(url, Is.EqualTo(null));
    }

    [Test]
    public void ValidateURL_Test()
    {
        string inUrl = "https://www.crosstales.com/";
        string outUrl = "crosstales.com";

        string? url = NetworkHelper.ValidateURL(inUrl, true);
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

        url = NetworkHelper.ValidateURL(ipCT);
        Assert.That(url, Is.EqualTo(ipCT));

        url = NetworkHelper.ValidateURL("");
        Assert.That(url, Is.EqualTo(""));

        url = NetworkHelper.ValidateURL(null);
        Assert.That(url, Is.EqualTo(null));

        inUrl = "www.crosstales.com/";
        outUrl = "crosstales.com";

        url = NetworkHelper.ValidateURL(inUrl, true);
        Assert.That(url, Is.EqualTo(outUrl));
    }

    [Test]
    public void isURL_Test()
    {
        string file = FileHelper.TempFile;

        Assert.True(NetworkHelper.isURL(testUrl));

        Assert.False(NetworkHelper.isURL(file));
        Assert.False(NetworkHelper.isURL(""));
        Assert.False(NetworkHelper.isURL(null));
        Assert.True(NetworkHelper.isURL("www.crosstales.com"));
    }

    [Test]
    public void isIPv4_Test()
    {
        Assert.True(NetworkHelper.isIPv4(ipCT));
        Assert.True(NetworkHelper.isIPv4(ipLocalhost));

        Assert.False(NetworkHelper.isIPv4(ipWrong1));
        Assert.False(NetworkHelper.isIPv4(ipWrong2));
        Assert.False(NetworkHelper.isIPv4(ipWrong3));
        Assert.False(NetworkHelper.isIPv4(ipWrong4));
        Assert.False(NetworkHelper.isIPv4(testUrl));
        Assert.False(NetworkHelper.isIPv4("ueli"));
        Assert.False(NetworkHelper.isIPv4(""));
        Assert.False(NetworkHelper.isIPv4(null));
    }

    [Test]
    public void GetIP_Test()
    {
        string? ip = NetworkHelper.GetIP(testUrl);
        Assert.That(ip, Is.EqualTo(ipCT));

        ip = NetworkHelper.GetIP("localhost");
        Assert.That(ip, Is.EqualTo(ipLocalhost));

        ip = NetworkHelper.GetIP("");
        Assert.That(ip, Is.EqualTo(""));

        ip = NetworkHelper.GetIP(null);
        Assert.That(ip, Is.EqualTo(null));
    }

    #endregion

    /*
          #region Cleanup

          [TearDown]
          public void CleanUp()
          {
          }

          #endregion
    */
}