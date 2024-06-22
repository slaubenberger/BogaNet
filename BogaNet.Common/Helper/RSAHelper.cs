using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Security.Cryptography;
using System.Net;
using System.Threading.Tasks;

namespace BogaNet.Helper;

/// <summary>
/// Helper for RSA cryptography.
/// </summary>
public abstract class RSAHelper
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(RSAHelper));

   /// <summary>
   /// Generates a self-signed X509-certificate.
   /// </summary>
   /// <param name="certName">Name of the certificate</param>
   /// <param name="keyLength">Key length of the certificate (optional, default: 2048)</param>
   /// <param name="oid">OID for the certificate (optional, default: "1.2.840.10045.3.1.7"</param>
   /// <returns>X509-certificate</returns>
   public static X509Certificate2 GenerateSelfSignedCertificate(string certName, int keyLength = 2048, string oid = "1.2.840.10045.3.1.7")
   {
      SubjectAlternativeNameBuilder sanBuilder = new();
      sanBuilder.AddIpAddress(IPAddress.Loopback);
      sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
      sanBuilder.AddDnsName("localhost");
      sanBuilder.AddDnsName(Environment.MachineName);

      X500DistinguishedName distinguishedName = new($"CN={certName}");

      using RSA rsa = RSA.Create(keyLength);
      CertificateRequest request = new(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

      request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));

      request.CertificateExtensions.Add(
         new X509EnhancedKeyUsageExtension(
            new OidCollection { new(oid) },
            //new OidCollection { new("1.3.6.1.5.5.7.3.1") },
            false));

      request.CertificateExtensions.Add(sanBuilder.Build());

      X509Certificate2 certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));

#if WINDOWS
      certificate.FriendlyName = certName;
#endif

      return certificate;
   }

   /// <summary>
   /// Gets a X509-certificate from the key store.
   /// </summary>
   /// <param name="certName">Name of the certificate</param>
   /// <param name="storeLocation">Location in the key store (optional, default: LocalMachine)</param>
   /// <returns>X509-certificate</returns>
   public static X509Certificate2? GetCertificateFromStore(string certName, StoreLocation storeLocation = StoreLocation.LocalMachine)
   {
      // Get the certificate store for the current user.
      using X509Store store = new(storeLocation);
      try
      {
         store.Open(OpenFlags.ReadOnly);

         // Place all certificates in an X509Certificate2Collection object.
         X509Certificate2Collection certCollection = store.Certificates;
         // If using a certificate with a trusted root you do not need to FindByTimeValid, instead:
         // currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, true);
         X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
         X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);

         if (signingCert.Count == 0)
            return null;

         // Return the first certificate in the collection, has the right name and is current.
         return signingCert[0];
      }
      finally
      {
         store.Close();
      }
   }

   /// <summary>
   /// Adds a X509-certificate to the key store.
   /// </summary>
   /// <param name="cert">X509-certificate</param>
   /// <param name="storeName">Name of the key store (optional, default: TrustedPublisher)</param>
   /// <param name="storeLocation">Location in the key store (optional, default: LocalMachine)</param>
   public static void AddCertificateToStore(X509Certificate2 cert, StoreName storeName = StoreName.TrustedPublisher, StoreLocation storeLocation = StoreLocation.LocalMachine)
   {
      using X509Store store = new(storeName, storeLocation);

      try
      {
         store.Open(OpenFlags.ReadWrite);
         store.Add(cert);
      }
      finally
      {
         store.Close();
      }
   }

   /// <summary>
   /// Gets the public key from a X509-certificate.
   /// </summary>
   /// <param name="cert">X509-certificate</param>
   /// <param name="password">Password for the file (optional, default: none)</param>
   /// <returns>X509-certificate as byte-array</returns>
   public static byte[] GetPublicCertificate(X509Certificate2 cert, string password = "")
   {
      return cert.Export(X509ContentType.Cert, password);
   }

   /// <summary>
   /// Gets the private and public key from a X509-certificate.
   /// NOTE: never share this data with people outside of your organisation!
   /// </summary>
   /// <param name="cert">X509-certificate</param>
   /// <param name="password">Password for the file (optional, default: none)</param>
   /// <returns>X509-certificate as byte-array</returns>
   public static byte[] GetPrivateCertificate(X509Certificate2 cert, string password = "")
   {
      return cert.Export(X509ContentType.Pfx, password);
   }

   /// <summary>
   /// Gets a X509-certificate from a byte-array.
   /// </summary>
   /// <param name="data">X509-certificate as byte-array</param>
   /// <param name="password">Password for the file</param>
   /// <returns>X509-certificate</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static X509Certificate2 GetCertificate(byte[]? data, string password = "")
   {
      if (data == null || data.Length <= 0)
         throw new ArgumentNullException(nameof(data));

      return new X509Certificate2(data, password);
   }

   /// <summary>
   /// Writes the public key as X509-certificate to a file.
   /// </summary>
   /// <param name="filename">Name of the file</param>
   /// <param name="cert">X509-certificate</param>
   /// <param name="password">Password for the file (optional, default: none)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool WritePublicCertificateToFile(string filename, X509Certificate2 cert, string password = "")
   {
      return Task.Run(() => WritePublicCertificateToFileAsync(filename, cert, password)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Writes the public key as X509-certificate to a file asynchronously.
   /// </summary>
   /// <param name="filename">Name of the file</param>
   /// <param name="cert">X509-certificate</param>
   /// <param name="password">Password for the file (optional, default: none)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> WritePublicCertificateToFileAsync(string filename, X509Certificate2 cert, string password = "")
   {
      return await FileHelper.WriteAllBytesAsync(filename, GetPublicCertificate(cert, password));
   }

   /// <summary>
   /// Writes the private and public key as X509-certificate to a file.
   /// NOTE: never share this file with people outside of your organisation!
   /// </summary>
   /// <param name="filename">Name of the file for the certificate</param>
   /// <param name="cert">X509-certificate</param>
   /// <param name="password">Password for the file</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool WritePrivateCertificateToFile(string filename, X509Certificate2 cert, string password)
   {
      return Task.Run(() => WritePrivateCertificateToFileAsync(filename, cert, password)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Writes the private and public key as X509-certificate to a file asynchronously.
   /// NOTE: never share this file with people outside of your organisation!
   /// </summary>
   /// <param name="filename">Name of the file for the certificate</param>
   /// <param name="cert">X509-certificate</param>
   /// <param name="password">Password for the file</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> WritePrivateCertificateToFileAsync(string filename, X509Certificate2 cert, string password)
   {
      return await FileHelper.WriteAllBytesAsync(filename, GetPrivateCertificate(cert, password));
   }

   /// <summary>
   /// Reads a X509-certificate from a file.
   /// </summary>
   /// <param name="filename">Name of the file with the certificate</param>
   /// <param name="password">Password for the file</param>
   /// <returns>X509-certificate</returns>
   /// <exception cref="Exception"></exception>
   public static X509Certificate2 ReadCertificateFromFile(string filename, string password = "")
   {
      return Task.Run(() => ReadCertificateFromFileAsync(filename, password)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Reads a X509-certificate from a file asynchronously.
   /// </summary>
   /// <param name="filename">Name of the file with the certificate</param>
   /// <param name="password">Password for the file</param>
   /// <returns>X509-certificate</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<X509Certificate2> ReadCertificateFromFileAsync(string filename, string password = "")
   {
      return GetCertificate(await FileHelper.ReadAllBytesAsync(filename), password);
   }
   
   /// <summary>
   /// Encrypts a byte-array with a X509-certificate.
   /// </summary>
   /// <param name="dataToEncrypt">byte-array to encrypt</param>
   /// <param name="cert">X509-certificate</param>
   /// <param name="padding">Padding for the asymmetric encryption (optional, default: OaepSHA256)</param>
   /// <returns>Encrypted byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[]? Encrypt(byte[]? dataToEncrypt, X509Certificate2? cert, RSAEncryptionPadding? padding = null)
   {
      if (dataToEncrypt == null || dataToEncrypt.Length <= 0)
         throw new ArgumentNullException(nameof(dataToEncrypt));
      if (cert == null)
         throw new ArgumentNullException(nameof(cert));

      try
      {
         using RSA? publicKey = RSACertificateExtensions.GetRSAPublicKey(cert);
         return publicKey?.Encrypt(dataToEncrypt, padding ?? RSAEncryptionPadding.OaepSHA256);
      }
      catch (CryptographicException ex)
      {
         LoggerExtensions.LogError(_logger, ex, "Encrypt failed!");
         throw;
      }
   }

   /// <summary>
   /// Decrypts a byte-array with a X509-certificate.
   /// </summary>
   /// <param name="dataToDecrypt">byte-array to decrypt</param>
   /// <param name="cert">X509-certificate</param>
   /// <param name="padding">Padding for the asymmetric encryption (optional, default: OaepSHA256)</param>
   /// <returns>Decrypted byte-array</returns>
   /// <returns>Decrypted byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[]? Decrypt(byte[] dataToDecrypt, X509Certificate2 cert, RSAEncryptionPadding? padding = null)
   {
      if (dataToDecrypt == null || dataToDecrypt.Length <= 0)
         throw new ArgumentNullException(nameof(dataToDecrypt));
      if (cert == null)
         throw new ArgumentNullException(nameof(cert));

      try
      {
         using RSA? privateKey = RSACertificateExtensions.GetRSAPrivateKey(cert);
         return privateKey?.Decrypt(dataToDecrypt, padding ?? RSAEncryptionPadding.OaepSHA256);
      }
      catch (Exception ex)
      {
         LoggerExtensions.LogError(_logger, ex, "Decrypt failed!");
         throw;
      }
   }

/*
   // Encrypt a file using a public key.
   private static void EncryptFile(string inFile, RSA rsaPublicKey)
   {
      using (Aes aes = Aes.Create())
      {
         // Create instance of Aes for
         // symmetric encryption of the data.
         aes.KeySize = 256;
         aes.Mode = CipherMode.CBC;
         using (ICryptoTransform transform = aes.CreateEncryptor())
         {
            RSAPKCS1KeyExchangeFormatter keyFormatter = new RSAPKCS1KeyExchangeFormatter(rsaPublicKey);
            byte[] keyEncrypted = keyFormatter.CreateKeyExchange(aes.Key, aes.GetType());

            // Create byte arrays to contain
            // the length values of the key and IV.
            byte[] LenK = new byte[4];
            byte[] LenIV = new byte[4];

            int lKey = keyEncrypted.Length;
            LenK = BitConverter.GetBytes(lKey);
            int lIV = aes.IV.Length;
            LenIV = BitConverter.GetBytes(lIV);

            // Write the following to the FileStream
            // for the encrypted file (outFs):
            // - length of the key
            // - length of the IV
            // - encrypted key
            // - the IV
            // - the encrypted cipher content

            int startFileName = inFile.LastIndexOf("\\") + 1;
            // Change the file's extension to ".enc"
            string outFile = encrFolder + inFile.Substring(startFileName, inFile.LastIndexOf(".") - startFileName) + ".enc";
            IO.Directory.CreateDirectory(encrFolder);

            using (IO.FileStream outFs = new IO.FileStream(outFile, IO.FileMode.Create))
            {
               outFs.Write(LenK, 0, 4);
               outFs.Write(LenIV, 0, 4);
               outFs.Write(keyEncrypted, 0, lKey);
               outFs.Write(aes.IV, 0, lIV);

               // Now write the cipher text using
               // a CryptoStream for encrypting.
               using (CryptoStream outStreamEncrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
               {
                  // By encrypting a chunk at
                  // a time, you can save memory
                  // and accommodate large files.
                  int count = 0;

                  // blockSizeBytes can be any arbitrary size.
                  int blockSizeBytes = aes.BlockSize / 8;
                  byte[] data = new byte[blockSizeBytes];
                  int bytesRead = 0;

                  using (IO.FileStream inFs = new IO.FileStream(inFile, IO.FileMode.Open))
                  {
                     do
                     {
                        count = inFs.Read(data, 0, blockSizeBytes);
                        outStreamEncrypted.Write(data, 0, count);
                        bytesRead += count;
                     } while (count > 0);

                     inFs.Close();
                  }

                  outStreamEncrypted.FlushFinalBlock();
                  outStreamEncrypted.Close();
               }

               outFs.Close();
            }
         }
      }
   }


   // Decrypt a file using a private key.
   private static void DecryptFile(string inFile, RSA rsaPrivateKey)
   {
      // Create instance of Aes for
      // symmetric decryption of the data.
      using (Aes aes = Aes.Create())
      {
         aes.KeySize = 256;
         aes.Mode = CipherMode.CBC;

         // Create byte arrays to get the length of
         // the encrypted key and IV.
         // These values were stored as 4 bytes each
         // at the beginning of the encrypted package.
         byte[] LenK = new byte[4];
         byte[] LenIV = new byte[4];

         // Construct the file name for the decrypted file.
         string outFile = decrFolder + inFile.Substring(0, inFile.LastIndexOf(".")) + ".txt";

         // Use FileStream objects to read the encrypted
         // file (inFs) and save the decrypted file (outFs).
         using (IO.FileStream inFs = new IO.FileStream(encrFolder + inFile, IO.FileMode.Open))
         {
            inFs.Seek(0, IO.SeekOrigin.Begin);
            inFs.Seek(0, IO.SeekOrigin.Begin);
            inFs.Read(LenK, 0, 3);
            inFs.Seek(4, IO.SeekOrigin.Begin);
            inFs.Read(LenIV, 0, 3);

            // Convert the lengths to integer values.
            int lenK = BitConverter.ToInt32(LenK, 0);
            int lenIV = BitConverter.ToInt32(LenIV, 0);

            // Determine the start position of
            // the cipher text (startC)
            // and its length(lenC).
            int startC = lenK + lenIV + 8;
            int lenC = (int)inFs.Length - startC;

            // Create the byte arrays for
            // the encrypted Aes key,
            // the IV, and the cipher text.
            byte[] KeyEncrypted = new byte[lenK];
            byte[] IV = new byte[lenIV];

            // Extract the key and IV
            // starting from index 8
            // after the length values.
            inFs.Seek(8, IO.SeekOrigin.Begin);
            inFs.Read(KeyEncrypted, 0, lenK);
            inFs.Seek(8 + lenK, IO.SeekOrigin.Begin);
            inFs.Read(IV, 0, lenIV);
            IO.Directory.CreateDirectory(decrFolder);
            // Use RSA
            // to decrypt the Aes key.
            byte[] KeyDecrypted = rsaPrivateKey.Decrypt(KeyEncrypted, RSAEncryptionPadding.Pkcs1);

            // Decrypt the key.
            using (ICryptoTransform transform = aes.CreateDecryptor(KeyDecrypted, IV))
            {
               // Decrypt the cipher text from
               // from the FileSteam of the encrypted
               // file (inFs) into the FileStream
               // for the decrypted file (outFs).
               using (IO.FileStream outFs = new IO.FileStream(outFile, IO.FileMode.Create))
               {
                  int count = 0;

                  int blockSizeBytes = aes.BlockSize / 8;
                  byte[] data = new byte[blockSizeBytes];

                  // By decrypting a chunk a time,
                  // you can save memory and
                  // accommodate large files.

                  // Start at the beginning
                  // of the cipher text.
                  inFs.Seek(startC, IO.SeekOrigin.Begin);
                  using (CryptoStream outStreamDecrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                  {
                     do
                     {
                        count = inFs.Read(data, 0, blockSizeBytes);
                        outStreamDecrypted.Write(data, 0, count);
                     } while (count > 0);

                     outStreamDecrypted.FlushFinalBlock();
                     outStreamDecrypted.Close();
                  }

                  outFs.Close();
               }

               inFs.Close();
            }
         }
      }

   }
   */
}