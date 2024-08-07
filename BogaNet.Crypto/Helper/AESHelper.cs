using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using BogaNet.Extension;

namespace BogaNet.Helper;

/// <summary>
/// Helper for AES cryptography.
/// </summary>
public abstract class AESHelper //NUnit //TODO add other algorithms, key&blocksize, padding and mode?
{
   private static readonly ILogger<AESHelper> _logger = GlobalLogging.CreateLogger<AESHelper>();

   #region Public methods

   /// <summary>
   /// Generates a secure IV for AES.
   /// </summary>
   /// <param name="length">Length of the IV (optional, range: 16 - 32, default: 16)</param>
   /// <returns>IV as byte-array</returns>
   public static byte[] GenerateIV(int length = 16)
   {
      return RandomNumberGenerator.GetBytes(Math.Clamp(length, 16, 32));
   }

   /// <summary>
   /// Generates a secure key for AES.
   /// </summary>
   /// <param name="length">Length of the key (optional, range: 16 - 128, default: 16)</param>
   /// <returns>Secure key as byte-array</returns>
   public static byte[] GenerateKey(int length = 16)
   {
      return RandomNumberGenerator.GetBytes(Math.Clamp(length, 16, 128));
   }

   /// <summary>
   /// Encrypts a file with AES.
   /// </summary>
   /// <param name="file">File to encrypt</param>
   /// <param name="key">Key for the file as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool EncryptFile(string file, byte[] key, byte[] IV)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      byte[] bytes = FileHelper.ReadAllBytes(file);
      return FileHelper.WriteAllBytes(file, Encrypt(bytes, key, IV));
   }

   /// <summary>
   /// Encrypts a file with AES asynchronously.
   /// </summary>
   /// <param name="file">File to encrypt</param>
   /// <param name="key">Key for the file as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> EncryptFileAsync(string file, byte[] key, byte[] IV)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      byte[] bytes = await FileHelper.ReadAllBytesAsync(file);
      return await FileHelper.WriteAllBytesAsync(file, await EncryptAsync(bytes, key, IV));
   }

   /// <summary>
   /// Decrypts a file with AES.
   /// </summary>
   /// <param name="file">File to decrypt</param>
   /// <param name="key">Key for the file as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool DecryptFile(string file, byte[] key, byte[] IV)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      byte[] bytes = FileHelper.ReadAllBytes(file);
      return FileHelper.WriteAllBytes(file, Decrypt(bytes, key, IV));
   }

   /// <summary>
   /// Decrypts a file with AES asynchronously.
   /// </summary>
   /// <param name="file">File to decrypt</param>
   /// <param name="key">Key for the file as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> DecryptFileAsync(string file, byte[] key, byte[] IV)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      byte[] bytes = await FileHelper.ReadAllBytesAsync(file);
      return await FileHelper.WriteAllBytesAsync(file, await DecryptAsync(bytes, key, IV));
   }

   /// <summary>
   /// Encrypts a string with AES.
   /// </summary>
   /// <param name="textToEncrypt">string to encrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Encrypted byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Encrypt(string textToEncrypt, byte[] key, byte[] IV, Encoding? encoding = null)
   {
      ArgumentException.ThrowIfNullOrEmpty(textToEncrypt);

      return Encrypt(textToEncrypt.BNToByteArray(encoding), key, IV);
   }

   /// <summary>
   /// Encrypts a byte-array with AES.
   /// </summary>
   /// <param name="dataToEncrypt">byte-array to encrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>Encrypted byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] IV)
   {
      ArgumentNullException.ThrowIfNull(dataToEncrypt);
      ArgumentNullException.ThrowIfNull(key);
      ArgumentNullException.ThrowIfNull(IV);

      try
      {
         using SymmetricAlgorithm algo = Aes.Create();
         using ICryptoTransform encryptor = algo.CreateEncryptor(key, IV);
         using MemoryStream msEncrypt = new();
         using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
         csEncrypt.Write(dataToEncrypt);
         csEncrypt.FlushFinalBlock();

         return msEncrypt.ToArray();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Encrypt failed!");
         throw;
      }
   }

   /// <summary>
   /// Encrypts a byte-array with AES asynchronously.
   /// </summary>
   /// <param name="dataToEncrypt">byte-array to encrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>Encrypted byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> EncryptAsync(byte[] dataToEncrypt, byte[] key, byte[] IV)
   {
      ArgumentNullException.ThrowIfNull(dataToEncrypt);
      ArgumentNullException.ThrowIfNull(key);
      ArgumentNullException.ThrowIfNull(IV);

      try
      {
         using SymmetricAlgorithm algo = Aes.Create();
         /*
         algo.KeySize = 256;
         algo.BlockSize = 128;
         algo.Padding = PaddingMode.PKCS7;
         algo.Mode = CipherMode.CFB;
         */
         using ICryptoTransform encryptor = algo.CreateEncryptor(key, IV);
         using MemoryStream msEncrypt = new();
         await using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
         await csEncrypt.WriteAsync(dataToEncrypt);
         await csEncrypt.FlushFinalBlockAsync();

         return msEncrypt.ToArray();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Encrypt failed!");
         throw;
      }
   }

   /// <summary>
   /// Decrypts a byte-array with AES.
   /// </summary>
   /// <param name="dataToDecrypt">byte-array to decrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>Decrypted byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] IV)
   {
      ArgumentNullException.ThrowIfNull(dataToDecrypt);
      ArgumentNullException.ThrowIfNull(key);
      ArgumentNullException.ThrowIfNull(IV);

      try
      {
         using SymmetricAlgorithm algo = Aes.Create();
         using ICryptoTransform decryptor = algo.CreateDecryptor(key, IV);
         using MemoryStream msDecrypt = new(dataToDecrypt);
         using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);

         return csDecrypt.BNReadFully();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Decrypt failed!");
         throw;
      }
   }

   /// <summary>
   /// Decrypts a byte-array with AES asynchronously.
   /// </summary>
   /// <param name="dataToDecrypt">byte-array to decrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>Decrypted byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> DecryptAsync(byte[] dataToDecrypt, byte[] key, byte[] IV)
   {
      ArgumentNullException.ThrowIfNull(dataToDecrypt);
      ArgumentNullException.ThrowIfNull(key);
      ArgumentNullException.ThrowIfNull(IV);

      try
      {
         using SymmetricAlgorithm algo = Aes.Create();
         /*
         algo.KeySize = 256;
         algo.BlockSize = 128;
         algo.Padding = PaddingMode.PKCS7;
         algo.Mode = CipherMode.CFB;
         */
         using ICryptoTransform decryptor = algo.CreateDecryptor(key, IV);
         using MemoryStream msDecrypt = new(dataToDecrypt);
         await using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);

         return await csDecrypt.BNReadFullyAsync();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Decrypt failed!");
         throw;
      }
   }

   #endregion
}