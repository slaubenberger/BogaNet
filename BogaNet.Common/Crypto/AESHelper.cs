using System.Security.Cryptography;

namespace BogaNet.Crypto;

/// <summary>
/// Helper for AES cryptography.
/// </summary>
public abstract class AESHelper
{
   /// <summary>
   /// Encrypts a file
   /// </summary>
   /// <param name="file">File to encrypt</param>
   /// <param name="key">Key for the file as string</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <exception cref="Exception"></exception>
   public static void EncryptFile(string? file, string? key, byte[]? IV)
   {
      BogaNet.IO.FileHelper.WriteAllBytes(file, Encrypt(BogaNet.IO.FileHelper.ReadAllBytes(file), HashHelper.SHA256(key), IV));
   }

   /// <summary>
   /// Encrypts a file
   /// </summary>
   /// <param name="file">File to encrypt</param>
   /// <param name="key">Key for the file as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <exception cref="Exception"></exception>
   public static void EncryptFile(string? file, byte[]? key, byte[]? IV)
   {
      BogaNet.IO.FileHelper.WriteAllBytes(file, Encrypt(BogaNet.IO.FileHelper.ReadAllBytes(file), key, IV));
   }

   /// <summary>
   /// Decrypts a file
   /// </summary>
   /// <param name="file">File to decrypt</param>
   /// <param name="key">Key for the file as string</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <exception cref="Exception"></exception>
   public static void DecryptFile(string? file, string? key, byte[]? IV)
   {
      BogaNet.IO.FileHelper.WriteAllBytes(file, Decrypt(BogaNet.IO.FileHelper.ReadAllBytes(file), HashHelper.SHA256(key), IV));
   }

   /// <summary>
   /// Decrypts a file
   /// </summary>
   /// <param name="file">File to decrypt</param>
   /// <param name="key">Key for the file as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <exception cref="Exception"></exception>
   public static void DecryptFile(string? file, byte[]? key, byte[]? IV)
   {
      BogaNet.IO.FileHelper.WriteAllBytes(file, Decrypt(BogaNet.IO.FileHelper.ReadAllBytes(file), key, IV));
   }

   /// <summary>
   /// Encrypts a byte-array
   /// </summary>
   /// <param name="dataToEncrypt">byte-array to encrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] Encrypt(byte[]? dataToEncrypt, byte[]? key, byte[]? IV)
   {
      if (dataToEncrypt == null || dataToEncrypt.Length <= 0)
         throw new ArgumentNullException(nameof(dataToEncrypt));
      if (key == null || key.Length <= 0)
         throw new ArgumentNullException(nameof(key));
      if (IV == null || IV.Length <= 0)
         throw new ArgumentNullException(nameof(IV));

      using Aes algo = Aes.Create();
      ICryptoTransform encryptor = algo.CreateEncryptor(key, IV);

      using MemoryStream msEncrypt = new();
      using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
      csEncrypt.Write(dataToEncrypt, 0, dataToEncrypt.Length);
      csEncrypt.FlushFinalBlock();

      byte[] encrypted = msEncrypt.ToArray();

      return encrypted;
   }

   /// <summary>
   /// Decrypts a byte-array
   /// </summary>
   /// <param name="dataToDecrypt">byte-array to decrypt</param>
   /// <param name="key">Key for the byte-array as byte-array</param>
   /// <param name="IV">IV (initial vector) for AES</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] Decrypt(byte[]? dataToDecrypt, byte[]? key, byte[]? IV)
   {
      if (dataToDecrypt == null || dataToDecrypt.Length <= 0)
         throw new ArgumentNullException(nameof(dataToDecrypt));
      if (key == null || key.Length <= 0)
         throw new ArgumentNullException(nameof(key));
      if (IV == null || IV.Length <= 0)
         throw new ArgumentNullException(nameof(IV));

      using Aes algo = Aes.Create();
      ICryptoTransform decryptor = algo.CreateDecryptor(key, IV);

      using MemoryStream msDecrypt = new(dataToDecrypt);
      using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
      byte[] decrypted = csDecrypt.BNReadFully();

      return decrypted;
   }
}