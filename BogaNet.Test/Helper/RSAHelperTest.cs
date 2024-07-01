using BogaNet.Helper;
using System.Security.Cryptography.X509Certificates;
using BogaNet.Extension;

namespace BogaNet.Test.Helper;

public class RSAHelperTest
{
   #region Tests

   [Test]
   public void RSAHelper_Test()
   {
      const string plain = "BogaNet rulez!";

      X509Certificate2 cert = RSAHelper.GenerateSelfSignedCertificate("BogaNet");

      X509Certificate2 certPrivate = RSAHelper.GetPrivateCertificate(cert);
      X509Certificate2 certPublic = RSAHelper.GetPublicCertificate(cert);

      /*
      string certPrivateFile = FileHelper.TempFile;
      RSAHelper.WritePrivateCertificateToFile(certPrivateFile, cert, "W3NeedASaf3rPassw0rd");
      X509Certificate2 certPrivate = RSAHelper.ReadCertificateFromFile(certPrivateFile, "W3NeedASaf3rPassw0rd");

      string certPublicFile = FileHelper.TempFile;
      RSAHelper.WritePublicCertificateToFile(certPublicFile, cert);
      X509Certificate2 certPublic = RSAHelper.ReadCertificateFromFile(certPublicFile);
      */

      byte[]? enc = RSAHelper.Encrypt(plain.BNToByteArray(), certPublic);
      byte[]? dec = RSAHelper.Decrypt(enc, certPrivate);

      string? result = dec.BNToString();

      Assert.That(result, Is.EqualTo(plain));
   }

   #endregion
}