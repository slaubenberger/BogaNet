﻿using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace BogaNet.Helper;

/// <summary>
/// Helper for AES cryptography.
/// </summary>
public abstract class AESHelper //TODO add other algorithms, key&blocksize, padding and mode?
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(AESHelper));

   /// <summary>
   /// Generates a secure IV for AES.
   /// </summary>
   /// <returns>IV as byte-array</returns>
   public static byte[] GenerateIV()
   {
      byte[] buffer = new byte[16];
      using RandomNumberGenerator rng = RandomNumberGenerator.Create();
      rng.GetBytes(buffer);
      return buffer;
   }

   /// <summary>
   /// Generates a secure key for AES.
   /// </summary>
   /// <param name="length">Length of the key (optional, default: 16)</param>
   /// <returns>Secure key as byte-array</returns>
   public static byte[] GenerateKey(int length = 16)
   {
      byte[] buffer = new byte[length.BNClamp(16, 32)];
      using RandomNumberGenerator rng = RandomNumberGenerator.Create();
      rng.GetBytes(buffer);
      return buffer;
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
   /// Encrypts a string with AES.
   /// </summary>
   /// <param name="textToEncrypt">string to encrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Encrypted byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Encrypt(string textToEncrypt, byte[]? key, byte[]? IV, Encoding? encoding = null)
   {
      if (textToEncrypt == null)
         throw new ArgumentNullException(nameof(textToEncrypt));

      return Encrypt(textToEncrypt.BNToByteArray(encoding), key, IV);
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
         await csEncrypt.WriteAsync(dataToEncrypt, 0, dataToEncrypt.Length);
         await csEncrypt.FlushFinalBlockAsync();

         return msEncrypt.ToArray();
      }
      catch (Exception ex)
      {
         LoggerExtensions.LogError(_logger, ex, "Encrypt failed!");
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
   /// Decrypts a byte-array with AES to a string.
   /// </summary>
   /// <param name="dataToDecrypt">byte-array to decrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Decrypted byte-array as string</returns>
   /// <exception cref="Exception"></exception>
   public static string? DecryptToString(byte[]? dataToDecrypt, byte[]? key, byte[]? IV, Encoding? encoding = null)
   {
      return Decrypt(dataToDecrypt, key, IV).BNToString(encoding);
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
         LoggerExtensions.LogError(_logger, ex, "Decrypt failed!");
         throw;
      }
   }
}