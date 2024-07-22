using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using BogaNet.Encoder;
using BogaNet.Extension;

namespace BogaNet.Helper;

/// <summary>
/// Various helper functions for networking.
/// </summary>
public abstract class NetworkHelper
{
   #region Variables

   private static readonly ILogger<NetworkHelper> _logger = GlobalLogging.CreateLogger<NetworkHelper>();

   private static bool _isInternetAvailable = false;
   private static DateTime _lastInternetCheck;
   private const float INTERNET_CHECK_INTERVAL = 7; //minimum time in seconds between Internet availability checks

   #endregion

   #region Public methods

   /// <summary>
   /// Create a basic AuthenticationHeaderValue.
   /// </summary>
   /// <param name="username">Name of the user</param>
   /// <param name="password">Password of the user</param>
   /// <returns>Basic AuthenticationHeaderValue</returns>
   public static AuthenticationHeaderValue CreateBasicAuth(string username, string password)
   {
      return new AuthenticationHeaderValue(
         "Basic",
         Base64.ToBase64String($"{username}:{password}", Encoding.ASCII, false)
      );
   }

   /// <summary>
   /// Create a bearer AuthenticationHeaderValue.
   /// </summary>
   /// <param name="bearerToken">Token for bearer</param>
   /// <returns>Bearer AuthenticationHeaderValue</returns>
   public static AuthenticationHeaderValue CreateBearerAuth(string bearerToken)
   {
      return new AuthenticationHeaderValue(
         "Bearer",
         bearerToken
      );
   }

   /// <summary>
   /// Sets the global proxy for all network requests to the default.
   /// </summary>
   /// <returns>Used global proxy</returns>
   public static IWebProxy? SetGlobalProxyToDefault()
   {
      if (WebRequest.DefaultWebProxy != null)
      {
         WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
         HttpClient.DefaultProxy = WebRequest.DefaultWebProxy;
         return WebRequest.DefaultWebProxy;
      }

      return null;
   }

   /// <summary>
   /// Sets the global proxy for all network requests with the given credentials.
   /// </summary>
   /// <param name="user">User for the proxy</param>
   /// <param name="password">Password for the proxy</param>
   /// <param name="url">Url of the proxy</param>
   /// <returns>Used global proxy</returns>
   public static IWebProxy SetGlobalProxy(string user, string password, string url)
   {
      NetworkCredential credentials = new(user, password);

      WebProxy proxy = new(new Uri(url), true, null, credentials);
      HttpClient.DefaultProxy = proxy;
      return proxy;
   }

   /// <summary>
   /// Opens the given URL with the file explorer or browser.
   /// </summary>
   /// <param name="url">URL to open</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool OpenURL(string? url)
   {
      if (IsURL(url))
      {
         openURL(url);
         return true;
      }

      _logger.LogWarning($"URL was invalid: {url}");
      return false;
   }

   /// <summary>
   /// HTTPS-certification callback, which overrides the checks for invalid certificates (like dev-certs).
   /// NOTE: do not use this in production since it breaks the SSL security!
   /// </summary>
   /// <param name="sender">Sender of the callback</param>
   /// <param name="certificate">Certificate to check</param>
   /// <param name="chain">Chain to use</param>
   /// <param name="sslPolicyErrors">SSL errors</param>
   /// <returns>True if the validation of the certificate was successful</returns>
   public static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
   {
#if DEBUG
      _logger.LogWarning("Do not use this in production since it breaks the SSL security!");
#endif
      bool isOk = true;

      // If there are errors in the certificate chain, look at each error to determine the cause.
      if (sslPolicyErrors != SslPolicyErrors.None)
      {
         foreach (X509ChainStatus unused in chain.ChainStatus.Where(t => t.Status != X509ChainStatusFlags.RevocationStatusUnknown))
         {
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;

            isOk = chain.Build((X509Certificate2)certificate);
         }
      }

      return isOk;
   }

   /// <summary>
   /// Returns the URL of a given file.
   /// </summary>
   /// <param name="path">File path</param>
   /// <returns>URL of the file path</returns>
   public static string GetURLForFile(string path) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(path);

      string validFile = FileHelper.ValidateFile(path);

      if (!IsURL(path))
         return Constants.PREFIX_FILE + StringHelper.EscapeURL(validFile.Replace('\\', '/'));

      return StringHelper.EscapeURL(validFile.Replace('\\', '/'));
   }

   /// <summary>
   /// Validates a given URL.
   /// </summary>
   /// <param name="url">URL to validate</param>
   /// <param name="removeProtocol">Remove the protocol, e.g. http:// (optional, default: false)</param>
   /// <param name="removeWWW">Remove www (optional, default: true)</param>
   /// <param name="removeSlash">Remove slash at the end (optional, default: true)</param>
   /// <returns>Validated URL</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ValidateURL(string url, bool removeProtocol = false, bool removeWWW = true, bool removeSlash = true) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(url);

      if (IsURL(url))
      {
         string result = url.Trim().Replace('\\', '/');

         if (removeWWW)
            result = result.BNReplace("www.", string.Empty)!;

         if (removeSlash && result.BNEndsWith(Constants.PATH_DELIMITER_UNIX))
            result = result.Substring(0, result.Length - 1);

         if (!string.IsNullOrEmpty(result))
         {
            if (removeProtocol)
            {
               int split = result.BNIndexOf("//");
               //string? protocol = result?.Substring(0, split + 2);
               string data = result.Substring(split > 1 ? split + 2 : 0);

               return $"{StringHelper.EscapeURL(data)}";
            }

            return $"{StringHelper.EscapeURL(result)}";
         }
      }

      return url;
   }

   /// <summary>
   /// Checks if the input is an URL.
   /// </summary>
   /// <param name="url">Input as possible URL</param>
   /// <returns>True if the given path is an URL</returns>
   public static bool IsURL(string? url) //NUnit
   {
      return !string.IsNullOrEmpty(url) &&
             (url.BNStartsWith(Constants.PREFIX_FILE) ||
              url.BNStartsWith(Constants.PREFIX_HTTP) ||
              url.BNStartsWith(Constants.PREFIX_HTTPS) ||
              url.BNStartsWith("www."));
   }

   /// <summary>
   /// Checks if the input is an IPv4 address.
   /// </summary>
   /// <param name="ip">Input as possible IPv4</param>
   /// <returns>True if the given input is an IPv4 address</returns>
   public static bool IsIPv4(string? ip) //NUnit
   {
      return Uri.CheckHostName(ip) == UriHostNameType.IPv4;
   }

   /// <summary>
   /// Checks if the input is an IPv6 address.
   /// </summary>
   /// <param name="ip">Input as possible IPv6</param>
   /// <returns>True if the given input is an IPv6 address</returns>
   public static bool IsIPv6(string? ip) //NUnit
   {
      return Uri.CheckHostName(ip) == UriHostNameType.IPv6;
   }

   /// <summary>
   /// Returns the IP of a given host name.
   /// </summary>
   /// <param name="host">Host name</param>
   /// <returns>IP of a given host name</returns>
   /// <exception cref="Exception"></exception>
   public static string GetIP(string host) //NUnit
   {
      ArgumentNullException.ThrowIfNullOrEmpty(host);

      if (IsIPv4(host) || IsIPv6(host))
         return host;

      string validHost = ValidateURL(host, IsURL(host));

      try
      {
         string ip = Dns.GetHostAddresses(validHost)[0].ToString();
         return ip == "::1" ? "127.0.0.1" : ip;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not resolve host '{host}");
         throw;
      }
   }

   /// <summary>
   /// Returns the public IP of the Internet connection.
   /// </summary>
   /// <param name="checkUrl">External url to check the ip (optional, default: https://checkip.amazonaws.com/</param>
   /// <returns>Public IP of the Internet connection.</returns>
   public static string GetPublicIP(string checkUrl = "https://checkip.amazonaws.com/") //alternatives: "https://icanhazip.com", "https://ipinfo.io/ip"  //NUnit
   {
      return Task.Run(() => GetPublicIPAsync(checkUrl)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Returns the public IP of the Internet connection asynchronously.
   /// </summary>
   /// <param name="checkUrl">External url to check the ip (optional, default: https://checkip.amazonaws.com/</param>
   /// <returns>Public IP of the Internet connection.</returns>
   public static async Task<string> GetPublicIPAsync(string checkUrl = "https://checkip.amazonaws.com/") //alternatives: "https://icanhazip.com", "https://ipinfo.io/ip"
   {
#if !BROWSER
      ArgumentNullException.ThrowIfNullOrEmpty(checkUrl);

      try
      {
         string content = await ReadAllTextAsync(checkUrl);

         _logger.LogDebug($"Content: {content}");

         return content.Trim();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not determine the public IP!");
      }
#else
         _logger.LogWarning("'GetPublicIPAsync' is not supported under the current platform!");
#endif
      return "unknown";
   }

   /// <summary>
   /// Returns a list of all available network interfaces.
   /// </summary>
   /// <param name="activeOnly">Search only for active network interfaces (optional, default: true)</param>
   /// <returns>List of network interfaces.</returns>
   public static List<NetworkAdapter> GetNetworkAdapters(bool activeOnly = true) //NUnit
   {
      List<NetworkAdapter> adapters = [];

#if !BROWSER
      NetworkInterface[] networkInterfaces = activeOnly ? NetworkInterface.GetAllNetworkInterfaces().Where(ni => ni.OperationalStatus == OperationalStatus.Up).ToArray() : NetworkInterface.GetAllNetworkInterfaces();

      foreach (NetworkInterface networkInterface in networkInterfaces)
      {
         PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();

         string macAddress = string.Join(":", physicalAddress.GetAddressBytes().Select(delegate(byte val)
         {
            string sign = val.ToString("X");
            if (sign.Length == 1)
               sign = $"0{sign}";

            return sign;
         }).ToArray());

         IPInterfaceProperties ipInterfaceProperties = networkInterface.GetIPProperties();
         UnicastIPAddressInformation? unicastAddressIP =
            ipInterfaceProperties.UnicastAddresses.FirstOrDefault(ua =>
               ua.Address.AddressFamily == AddressFamily.InterNetwork);

         IPAddress? gateway = ipInterfaceProperties.GatewayAddresses.Select(g => g.Address).FirstOrDefault();

         if (unicastAddressIP != null)
         {
            adapters.Add(new NetworkAdapter(networkInterface.Id, networkInterface.Name, networkInterface.NetworkInterfaceType,
               unicastAddressIP.Address, unicastAddressIP.IPv4Mask, macAddress, gateway, networkInterface.Speed,
               networkInterface.OperationalStatus));
         }
      }
#else
      _logger.LogWarning("'GetNetworkAdapters' is not supported under the current platform!");
#endif
      return adapters;
   }

   /// <summary>
   /// Checks the availability of the Internet (aka "Captive Portal Detection").
   /// </summary>
   /// <returns>True if a connection to the Internet is available</returns>
   public static bool CheckInternetAvailability() //NUnit
   {
      return Task.Run(CheckInternetAvailabilityAsync).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Checks the availability of the Internet (aka "Captive Portal Detection") asynchronously.
   /// </summary>
   /// <returns>True if a connection to the Internet is available</returns>
   public static async Task<bool> CheckInternetAvailabilityAsync()
   {
      if ((DateTime.UtcNow - _lastInternetCheck).TotalSeconds < INTERNET_CHECK_INTERVAL)
         return _isInternetAvailable;

      const string microsoftUrl = "http://www.msftncsi.com/ncsi.txt";
      const string appleUrl = "https://www.apple.com/library/test/success.html";
      const string ubuntuUrl = "https://start.ubuntu.com/connectivity-check";

      const string microsoftText = "Microsoft NCSI";
      const string appleText = "<TITLE>Success</TITLE>";
      const string ubuntuText = "<TITLE>Lorem Ipsum</TITLE>";

      const string windowsDesc = "Microsoft";
      const string appleDesc = "Apple";
      const string ubuntuDesc = "Ubuntu";

      const bool microsoftEquals = true;
      const bool appleEquals = false;
      const bool ubuntuEquals = false;

      bool available = false;

      // Microsoft check
      if (!Constants.IsIOS)
      {
         available = await checkAsync(microsoftUrl, microsoftText, microsoftEquals, windowsDesc);

         _logger.LogDebug($"{windowsDesc} check: {available}");
      }

      // Apple check
      if (!available && (!Constants.IsIOS || !Constants.IsOSX))
      {
         available = await checkAsync(appleUrl, appleText, appleEquals, appleDesc);

         _logger.LogDebug($"{appleDesc} check: {available}");
      }

      // Ubuntu check
      if (!available)
      {
         available = await checkAsync(ubuntuUrl, ubuntuText, ubuntuEquals, ubuntuDesc);

         _logger.LogDebug($"{ubuntuDesc} check: {available}");
      }

      _isInternetAvailable = available;
      _lastInternetCheck = DateTime.UtcNow;

      return available;
   }

   /// <summary>
   /// Pings a given host and returns the Roundtrip-Time.
   /// </summary>
   /// <param name="hostname">Host/IP to ping</param>
   /// <returns>Roundtrip-Time in milliseconds</returns>
   /// <exception cref="Exception"></exception>
   public static long Ping(string hostname) //NUnit
   {
      return Task.Run(() => PingAsync(hostname)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Pings a given host and returns the Roundtrip-Time asynchronously.
   /// </summary>
   /// <param name="hostname">Host/IP to ping</param>
   /// <returns>Roundtrip-Time in milliseconds</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<long> PingAsync(string hostname)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(hostname);

      try
      {
         string ip = GetIP(hostname);

         Ping ping = new();
         var reply = await ping.SendPingAsync(ip);

         return reply.RoundtripTime;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Ping failed!");
         throw;
      }
   }

   /// <summary>
   /// Reads the text of a file from a given URL.
   /// </summary>
   /// <param name="url">URL to the file</param>
   /// <returns>Text-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static string ReadAllText(string url)
   {
      return Task.Run(() => ReadAllTextAsync(url)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Reads the text of a file from a given URL asynchronously.
   /// </summary>
   /// <param name="url">URL to the file</param>
   /// <returns>Text-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string> ReadAllTextAsync(string url)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(url);

      try
      {
         using HttpClient client = new();
         using HttpResponseMessage response = client.GetAsync(url).Result;
         //response.EnsureSuccessStatusCode();

         if (!response.IsSuccessStatusCode)
            throw new Exception($"Could not download data: {response.StatusCode} - {response.ReasonPhrase}");

         return await response.Content.ReadAsStringAsync();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not read file '{url}'");
         throw;
      }
   }

   /// <summary>
   /// Reads all lines of text from a file from a given URL.
   /// </summary>
   /// <param name="url">URL to the file</param>
   /// <returns>Array of text lines from the file</returns>
   /// <exception cref="Exception"></exception>
   public static string[] ReadAllLines(string url)
   {
      return Task.Run(() => ReadAllLinesAsync(url)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Reads all lines of text from a file from a given URL asynchronously.
   /// </summary>
   /// <param name="url">URL to the file</param>
   /// <returns>Array of text lines from the file</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string[]> ReadAllLinesAsync(string url)
   {
      string text = await ReadAllTextAsync(url);

      return StringHelper.SplitToLines(text).ToArray();
   }

   /// <summary>
   /// Reads the bytes of a file from a given URL.
   /// </summary>
   /// <param name="url">URL to the file</param>
   /// <returns>Byte-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] ReadAllBytes(string url)
   {
      return Task.Run(() => ReadAllBytesAsync(url)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Reads the bytes of a file from a given URL asynchronously.
   /// </summary>
   /// <param name="url">URL to the file</param>
   /// <returns>Byte-content of the file</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> ReadAllBytesAsync(string url)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(url);

      try
      {
         using HttpClient client = new();
         using HttpResponseMessage response = client.GetAsync(url).Result;
         //response.EnsureSuccessStatusCode();

         if (!response.IsSuccessStatusCode)
            throw new Exception($"Could not download data: {response.StatusCode} - {response.ReasonPhrase}");

         return await response.Content.ReadAsByteArrayAsync();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not read file '{url}'");
         throw;
      }
   }

   #endregion

   #region Private methods

   private static void openURL(string? url)
   {
      using Process process = new();
      process.StartInfo.FileName = url;
      process.Start();
   }

   private static async Task<bool> checkAsync(string url, string data, bool equals, string type, bool showError = false)
   {
      bool available = false;

      try
      {
         string content = await ReadAllTextAsync(urlAntiCacheRandomizer(url));

         _logger.LogTrace($"Content from {type}: {content}");

         available = equals ? !string.IsNullOrEmpty(content) && content.Equals(data) : !string.IsNullOrEmpty(content) && content.Contains(data);
      }
      catch (Exception ex)
      {
         if (showError)
            _logger.LogError(ex, $"Error getting content from {type}!");
      }

      return available;
   }

   private static string urlAntiCacheRandomizer(string url) //TODO make public?
   {
      Random rnd = new();
      return $"{url}?p={rnd.NextInt64(9999, 99999999)}";
   }

   #endregion
}

/// <summary>
/// Network adapter (interface) from the current device.
/// </summary>
public class NetworkAdapter
{
   #region Properties

   /// <summary>Id of the network adapter.</summary>
   public string Id { get; private set; }

   /// <summary>Name of the network adapter.</summary>
   public string Name { get; private set; }

   /// <summary>Type of the network adapter.</summary>
   public NetworkInterfaceType Type { get; private set; }

   /// <summary>Address of the network adapter.</summary>
   public IPAddress Address { get; private set; }

   /// <summary>Mask of the network adapter.</summary>
   public IPAddress Mask { get; private set; }

   /// <summary>MAC address of the network adapter.</summary>
   public string MacAddress { get; private set; }

   /// <summary>Gateway of the network adapter.</summary>
   public IPAddress? Gateway { get; private set; }

   /// <summary>Speed of the network adapter in bits-per-second (bps).</summary>
   public long Speed { get; private set; }

   /// <summary>Status of the network adapter.</summary>
   public OperationalStatus Status { get; private set; }

   #endregion

   #region Constructor

   public NetworkAdapter(string id, string name, NetworkInterfaceType type,
      IPAddress address, IPAddress mask, string macAddress, IPAddress? gateway,
      long speed, OperationalStatus status)
   {
      Id = id;
      Name = name;
      Type = type;
      Address = address;
      Mask = mask;
      MacAddress = macAddress;
      Gateway = gateway;
      Speed = speed;
      Status = status;
   }

   #endregion

   #region Overridden methods

   public override string ToString()
   {
      System.Text.StringBuilder result = new();

      result.Append("• ");
      result.Append(Name);

      result.Append(" (");
      result.Append(Type);
      result.Append("), ");

      result.Append("Address: ");
      result.Append(Address);

      if (Status == OperationalStatus.Up)
      {
         result.Append(" (");
         result.Append(Mask);
         result.Append("), ");
      }
      else
      {
         result.Append(", ");
      }

      result.Append("Mac: ");
      result.Append(MacAddress);
      result.Append(", ");

      if (Status == OperationalStatus.Up)
      {
         result.Append("Gateway: ");
         result.Append(Gateway);
         result.Append(", ");

         result.Append("Speed: ");
         result.Append(GeneralHelper.FormatBitrateToHRF(Speed));
         result.Append(", ");
      }

      result.Append("Status: ");
      result.Append(Status);

      return result.ToString();
   }

   #endregion
}