using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;

namespace BogaNet;

/// <summary>
/// Various helper functions for networking.
/// </summary>
public abstract class NetworkHelper
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger("NetworkHelper");

   #endregion

   #region Properties

/*
        /// <summary>Checks if an Internet connection is available.</summary>
        /// <returns>True if an Internet connection is available.</returns>
        public static bool isInternetAvailable
        {
            get
            {
                throw new NotImplementedException();
                //return true; //TODO implement captive portal detection!
            }
        }
*/

   #endregion

   #region Public methods

   /// <summary>
   /// Create a basic AuthenticationHeaderValue
   /// </summary>
   /// <param name="username">Name of the user</param>
   /// <param name="password">Password of the user</param>
   /// <returns>Basic AuthenticationHeaderValue</returns>
   public static AuthenticationHeaderValue CreateBasicAuth(string username, string password)
   {
      return new AuthenticationHeaderValue(
         "Basic",
         Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}")
         )
      );
   }

   /// <summary>
   /// Create a bearer AuthenticationHeaderValue
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
   /// Escape the data in an URL (like spaces etc.)
   /// </summary>
   /// <param name="url"></param>
   /// <returns></returns>
   public static string? EscapeURLString(string? url)
   {
      return url == null ? null : Uri.EscapeDataString(url).Replace("%2F", "/").Replace("%3A", ":");
   }

   /// <summary>Opens the given URL with the file explorer or browser.</summary>
   /// <param name="url">URL to open</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool OpenURL(string? url)
   {
      if (isURL(url))
      {
         openURL(url);

         return true;
      }

      _logger.LogWarning($"URL was invalid: {url}");
      return false;
   }

   /// <summary>HTTPS-certification callback.</summary>
   public static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
   {
      bool isOk = true;

      // If there are errors in the certificate chain, look at each error to determine the cause.
      if (sslPolicyErrors != SslPolicyErrors.None)
      {
         foreach (X509ChainStatus t in chain.ChainStatus.Where(t => t.Status != X509ChainStatusFlags.RevocationStatusUnknown))
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

   /// <summary>Returns the URL of a given file.</summary>
   /// <param name="path">File path</param>
   /// <returns>URL of the file path</returns>
   public static string? GetURLFromFile(string? path) //NUnit
   {
      if (!string.IsNullOrEmpty(path))
      {
         string? _path = FileHelper.ValidateFile(path);

         if (_path != null)
         {
            if (!isURL(path))
               return Constants.PREFIX_FILE + EscapeURLString(_path.Replace('\\', '/'));

            return EscapeURLString(_path.Replace('\\', '/'));
         }
      }

      return path;
   }

   /// <summary>Validates a given URL.</summary>
   /// <param name="url">URL to validate</param>
   /// <param name="removeProtocol">Remove the protocol, e.g. http:// (optional, default: false)</param>
   /// <param name="removeWWW">Remove www (optional, default: true)</param>
   /// <param name="removeSlash">Remove slash at the end (optional, default: true)</param>
   /// <returns>Clean URL</returns>
   public static string? ValidateURL(string? url, bool removeProtocol = false, bool removeWWW = true, bool removeSlash = true) //NUnit
   {
      if (isURL(url))
      {
         string? result = url?.Trim().Replace('\\', '/');

         if (removeWWW)
            result = result?.CTReplace("www.", string.Empty);

         if (removeSlash && result.CTEndsWith(Constants.PATH_DELIMITER_UNIX))
            result = result?.Substring(0, result.Length - 1);

         if (!string.IsNullOrEmpty(result))
         {
            if (removeProtocol)
            {
               int split = result.CTIndexOf("//");
               //string? protocol = result?.Substring(0, split + 2);
               string data = result.Substring(split > 1 ? split + 2 : 0);

               return $"{EscapeURLString(data)}";
            }

            return $"{EscapeURLString(result)}";
         }
      }

      return url;
   }

   /// <summary>Checks if the input is an URL.</summary>
   /// <param name="url">Input as possible URL</param>
   /// <returns>True if the given path is an URL</returns>
   public static bool isURL(string? url) //NUnit
   {
      return !string.IsNullOrEmpty(url) &&
             (url.CTStartsWith(Constants.PREFIX_FILE) ||
              url.CTStartsWith(Constants.PREFIX_HTTP) ||
              url.CTStartsWith(Constants.PREFIX_HTTPS) ||
              url.CTStartsWith("www."));
   }

   /// <summary>Checks if the input is an IPv4 address.</summary>
   /// <param name="ip">Input as possible IPv4</param>
   /// <returns>True if the given path is an IPv4 address</returns>
   public static bool isIPv4(string? ip) //NUnit
   {
      if (!string.IsNullOrEmpty(ip) && Constants.REGEX_IP_ADDRESS.IsMatch(ip))
      {
         string[] ipBytes = ip.Split('.');

         foreach (string ipByte in ipBytes)
         {
            if (int.TryParse(ipByte, out int val) && val is > 255 or < 0)
               return false;
         }

         return true;
      }

      return false;
   }

   /// <summary>Returns the IP of a given host name.</summary>
   /// <param name="host">Host name</param>
   /// <returns>IP of a given host name.</returns>
   /// <exception cref="Exception"></exception>
   public static string? GetIP(string? host) //NUnit
   {
      string? validHost = ValidateURL(host, isURL(host));

      if (!string.IsNullOrEmpty(validHost))
      {
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

      _logger.LogWarning("Host name is null or empty - can't resolve to IP!");

      return host;
   }

   #endregion

   #region Private methods

   private static void openURL(string? url)
   {
      using Process process = new();
      process.StartInfo.FileName = url;
      process.Start();
   }

   #endregion
}