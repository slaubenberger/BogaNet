using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;
using BogaNet.Extension;
using System.Threading.Tasks;

namespace BogaNet.Helper;

/// <summary>
/// Helper for hash computations. It contains ready-to-use implementations of SHA256, SHA384, SHA512, SHA3-256, SHA3-384 and SHA3-512.
/// </summary>
public abstract class HashHelper //NUnit
{
   private static readonly ILogger<HashHelper> _logger = GlobalLogging.CreateLogger<HashHelper>();

   #region Public methods

   #region Generic

   /// <summary>
   /// Generates a hash-value as byte-array from a given byte-array and algorithm as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <returns>Hash-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Hash(byte[] bytes, HashAlgorithm algo)
   {
      ArgumentNullException.ThrowIfNull(bytes);
      ArgumentNullException.ThrowIfNull(algo);

      try
      {
         return algo.ComputeHash(bytes, 0, bytes.Length);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Compute of hash failed!");
         throw;
      }
   }

   /// <summary>
   /// Generates a hash-value as byte-array from a given string and algorithm as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Hash-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Hash(string text, HashAlgorithm algo, Encoding? encoding = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(text);

      return Hash(text.BNToByteArray(encoding), algo);
   }

   /// <summary>
   /// Generates a hash-value as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <returns>Hash-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashFile(string file, HashAlgorithm algo)
   {
      return Task.Run(() => HashFileAsync(file, algo)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Generates a hash-value as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <returns>Hash-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashFileAsync(string file, HashAlgorithm algo)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return Hash(await FileHelper.ReadAllBytesAsync(file), algo);
   }

   #endregion

   #region SHA256

   /// <summary>
   /// Generates a SHA256-value as byte-array from a byte-array.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA256(params byte[] bytes)
   {
      using HashAlgorithm sha256 = SHA256.Create();
      return Hash(bytes, sha256);
   }

   /// <summary>
   /// Generates a SHA256-value as byte-array from a string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA256(string text, Encoding? encoding = null)
   {
      using HashAlgorithm sha256 = SHA256.Create();
      return Hash(text, sha256, encoding);
   }

   /// <summary>
   /// Generates a SHA256-value as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA256File(string file)
   {
      return Task.Run(() => HashSHA256FileAsync(file)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Generates a SHA256-value as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashSHA256FileAsync(string file)
   {
      using HashAlgorithm sha256 = SHA256.Create();
      return await HashFileAsync(file, sha256);
   }

   #endregion

   #region SHA384

   /// <summary>
   /// Generates a SHA384-value as byte-array from a given byte-array.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>SHA384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA384(params byte[] bytes)
   {
      using HashAlgorithm sha384 = SHA384.Create();
      return Hash(bytes, sha384);
   }

   /// <summary>
   /// Generates a SHA384-value as byte-array from a given string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA384(string text, Encoding? encoding = null)
   {
      using HashAlgorithm sha = SHA384.Create();
      return Hash(text, sha, encoding);
   }

   /// <summary>
   /// Generates a SHA384-value as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA384File(string file)
   {
      return Task.Run(() => HashSHA384FileAsync(file)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Generates a SHA384-value as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashSHA384FileAsync(string file)
   {
      using HashAlgorithm sha384 = SHA384.Create();
      return await HashFileAsync(file, sha384);
   }

   #endregion

   #region SHA512

   /// <summary>
   /// Generates a SHA512-value as byte-array from a given byte-array.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>SHA512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA512(params byte[] bytes)
   {
      using HashAlgorithm sha512 = SHA512.Create();
      return Hash(bytes, sha512);
   }

   /// <summary>
   /// Generates a SHA512-value as byte-array from a given string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA512(string text, Encoding? encoding = null)
   {
      using HashAlgorithm sha = SHA512.Create();
      return Hash(text, sha, encoding);
   }

   /// <summary>
   /// Generates a SHA512-value as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA512File(string file)
   {
      return Task.Run(() => HashSHA512FileAsync(file)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Generates a SHA512-value as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashSHA512FileAsync(string file)
   {
      using HashAlgorithm sha512 = SHA512.Create();
      return await HashFileAsync(file, sha512);
   }

   #endregion

   #region SHA3-256

   /// <summary>
   /// Generates a SHA3-256-value as byte-array from a byte-array.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>SHA3-256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA3_256(params byte[] bytes)
   {
      using HashAlgorithm sha256 = SHA3_256.Create();
      return Hash(bytes, sha256);
   }

   /// <summary>
   /// Generates a SHA3-256-value as byte-array from a string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA3-256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA3_256(string text, Encoding? encoding = null)
   {
      using HashAlgorithm sha3 = SHA3_256.Create();
      return Hash(text, sha3, encoding);
   }

   /// <summary>
   /// Generates a SHA3-256-value as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA3-256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA3_256File(string file)
   {
      return Task.Run(() => HashSHA3_256FileAsync(file)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Generates a SHA3-256-value as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA3-256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashSHA3_256FileAsync(string file)
   {
      using HashAlgorithm sha256 = SHA3_256.Create();
      return await HashFileAsync(file, sha256);
   }

   #endregion

   #region SHA3-384

   /// <summary>
   /// Generates a SHA3-384-value as byte-array from a given byte-array.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>SHA3-384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA3_384(params byte[] bytes)
   {
      using HashAlgorithm sha384 = SHA3_384.Create();
      return Hash(bytes, sha384);
   }

   /// <summary>
   /// Generates a SHA3-384-value as byte-array from a given string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA3-384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA3_384(string text, Encoding? encoding = null)
   {
      using HashAlgorithm sha3 = SHA3_384.Create();
      return Hash(text, sha3, encoding);
   }

   /// <summary>
   /// Generates a SHA3-384-value as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA3-384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA3_384File(string file)
   {
      return Task.Run(() => HashSHA3_384FileAsync(file)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Generates a SHA3-384-value as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA3-384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashSHA3_384FileAsync(string file)
   {
      using HashAlgorithm sha384 = SHA3_384.Create();
      return await HashFileAsync(file, sha384);
   }

   #endregion

   #region SHA3-512

   /// <summary>
   /// Generates a SHA3-512-value as byte-array from a given byte-array.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>SHA3-512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA3_512(params byte[] bytes)
   {
      using HashAlgorithm sha512 = SHA3_512.Create();
      return Hash(bytes, sha512);
   }

   /// <summary>
   /// Generates a SHA3-512-value as byte-array from a given string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA3-512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA3_512(string text, Encoding? encoding = null)
   {
      using HashAlgorithm sha3 = SHA3_512.Create();
      return Hash(text, sha3, encoding);
   }

   /// <summary>
   /// Generates a SHA3-512-value as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA3-512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashSHA3_512File(string file)
   {
      return Task.Run(() => HashSHA3_512FileAsync(file)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Generates a SHA3-512-value as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <returns>SHA3-512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashSHA3_512FileAsync(string file)
   {
      using HashAlgorithm sha512 = SHA3_512.Create();
      return await HashFileAsync(file, sha512);
   }

   #endregion

   #endregion
}