using System.Security.Cryptography;
using BogaNet.IO;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.IO;

namespace BogaNet.Crypto;

/// <summary>
/// Helper for AES cryptography.
/// </summary>
public abstract class AESHelper
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(AESHelper));

   /// <summary>
   /// Encrypts a file with AES.
   /// </summary>
   /// <param name="file">File to encrypt</param>
   /// <param name="key">Key for the file as string</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool EncryptFile(string? file, string? key, byte[]? IV)
   {
      return Task.Run(() => EncryptFileAsync(file, key, IV)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Encrypts a file with AES asynchronously.
   /// </summary>
   /// <param name="file">File to encrypt</param>
   /// <param name="key">Key for the file as string</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> EncryptFileAsync(string? file, string? key, byte[]? IV)
   {
      return await FileHelper.WriteAllBytesAsync(file, await EncryptAsync(await FileHelper.ReadAllBytesAsync(file), HashHelper.SHA256(key), IV));
   }

   /// <summary>
   /// Encrypts a file with AES.
   /// </summary>
   /// <param name="file">File to encrypt</param>
   /// <param name="key">Key for the file as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool EncryptFile(string? file, byte[]? key, byte[]? IV)
   {
      return Task.Run(() => EncryptFileAsync(file, key, IV)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Encrypts a file with AES asynchronously.
   /// </summary>
   /// <param name="file">File to encrypt</param>
   /// <param name="key">Key for the file as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> EncryptFileAsync(string? file, byte[]? key, byte[]? IV)
   {
      return await FileHelper.WriteAllBytesAsync(file, await EncryptAsync(await FileHelper.ReadAllBytesAsync(file), key, IV));
   }

   /// <summary>
   /// Decrypts a file with AES.
   /// </summary>
   /// <param name="file">File to decrypt</param>
   /// <param name="key">Key for the file as string</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool DecryptFile(string? file, string? key, byte[]? IV)
   {
      return Task.Run(() => DecryptFileAsync(file, key, IV)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Decrypts a file with AES asynchronously.
   /// </summary>
   /// <param name="file">File to decrypt</param>
   /// <param name="key">Key for the file as string</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> DecryptFileAsync(string? file, string? key, byte[]? IV)
   {
      return await FileHelper.WriteAllBytesAsync(file, await DecryptAsync(await FileHelper.ReadAllBytesAsync(file), HashHelper.SHA256(key), IV));
   }

   /// <summary>
   /// Decrypts a file with AES.
   /// </summary>
   /// <param name="file">File to decrypt</param>
   /// <param name="key">Key for the file as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool DecryptFile(string? file, byte[]? key, byte[]? IV)
   {
      return Task.Run(() => DecryptFileAsync(file, key, IV)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Decrypts a file with AES asynchronously.
   /// </summary>
   /// <param name="file">File to decrypt</param>
   /// <param name="key">Key for the file as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> DecryptFileAsync(string? file, byte[]? key, byte[]? IV)
   {
      return await FileHelper.WriteAllBytesAsync(file, await DecryptAsync(await FileHelper.ReadAllBytesAsync(file), key, IV));
   }

   /// <summary>
   /// Encrypts a byte-array with AES.
   /// </summary>
   /// <param name="dataToEncrypt">byte-array to encrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>Encrypted byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Encrypt(byte[]? dataToEncrypt, byte[]? key, byte[]? IV)
   {
      return Task.Run(() => EncryptAsync(dataToEncrypt, key, IV)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Encrypts a byte-array with AES asynchronously.
   /// </summary>
   /// <param name="dataToEncrypt">byte-array to encrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>Encrypted byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> EncryptAsync(byte[]? dataToEncrypt, byte[]? key, byte[]? IV)
   {
      if (dataToEncrypt == null || dataToEncrypt.Length <= 0)
         throw new ArgumentNullException(nameof(dataToEncrypt));
      if (key == null || key.Length <= 0)
         throw new ArgumentNullException(nameof(key));
      if (IV == null || IV.Length <= 0)
         throw new ArgumentNullException(nameof(IV));

      try
      {
         using Aes algo = Aes.Create();
         ICryptoTransform encryptor = algo.CreateEncryptor(key, IV);

         using MemoryStream msEncrypt = new();
         await using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
         await csEncrypt.WriteAsync(dataToEncrypt, 0, dataToEncrypt.Length);
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
   public static byte[] Decrypt(byte[]? dataToDecrypt, byte[]? key, byte[]? IV)
   {
      return Task.Run(() => DecryptAsync(dataToDecrypt, key, IV)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Decrypts a byte-array with AES asynchronously.
   /// </summary>
   /// <param name="dataToDecrypt">byte-array to decrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <returns>Decrypted byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> DecryptAsync(byte[]? dataToDecrypt, byte[]? key, byte[]? IV)
   {
      if (dataToDecrypt == null || dataToDecrypt.Length <= 0)
         throw new ArgumentNullException(nameof(dataToDecrypt));
      if (key == null || key.Length <= 0)
         throw new ArgumentNullException(nameof(key));
      if (IV == null || IV.Length <= 0)
         throw new ArgumentNullException(nameof(IV));

      try
      {
         using Aes algo = Aes.Create();
         ICryptoTransform decryptor = algo.CreateDecryptor(key, IV);

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
}