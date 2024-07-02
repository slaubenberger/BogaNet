using System.Security.Cryptography.X509Certificates;

namespace BogaNet.Extension;

/// <summary>
/// Extension methods for certificates.
/// </summary>
public static class CertificateExtension
{
   /// <summary>
   /// Converts a X509-certificate to a byte-array.
   /// </summary>
   /// <param name="cert">X509-certificate</param>
   /// <param name="password">Password for the certificate (optional, default: none)</param>
   /// <param name="type">Type of the content (optional, default: Cert (=public key))</param>
   /// <returns>Byte-array with the X509-certificate</returns>
   public static byte[]? BNToByteArray(this X509Certificate2? cert, string? password = null, X509ContentType type = X509ContentType.Cert)
   {
      return cert?.Export(type, password);
   }
}