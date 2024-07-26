using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;
using BogaNet.Extension;
using System.Threading.Tasks;

namespace BogaNet.Helper;

/// <summary>
/// Helper for HMAC cryptography. It contains ready-to-use implementations of HMAC-SHA256, HMAC-SHA384, HMAC-SHA512, HMAC-SHA3-256, HMAC-SHA3-384 and HMAC-SHA3-512.
/// </summary>
public abstract class HMACHelper //NUnit
{
   private static readonly ILogger<HMACHelper> _logger = GlobalLogging.CreateLogger<HMACHelper>();

   #region Public methods

   /// <summary>
   /// Generates a secure secret for HMAC.
   /// </summary>
   /// <param name="length">Length of the secret (optional, default: 16)</param>
   /// <returns>Secure secret as byte-array</returns>
   public static byte[] GenerateSecret(int length = 16)
   {
      return RandomNumberGenerator.GetBytes(Math.Abs(length));
   }

   #region Generic

   /// <summary>
   /// Generates a HMAC-value as byte-array with given byte-array and algorithm as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="algo">HMAC-algorithm</param>
   /// <returns>HMAC-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Hash(byte[] bytes, HMAC algo)
   {
      ArgumentNullException.ThrowIfNull(bytes);
      ArgumentNullException.ThrowIfNull(algo);

      try
      {
         return algo.ComputeHash(bytes, 0, bytes.Length);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Compute of HMAC failed!");
         throw;
      }
   }

   /// <summary>
   /// Generates a HMAC-value as byte-array with a given string and algorithm as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="algo">HMAC-algorithm</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>HMAC-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Hash(string? text, HMAC algo, Encoding? encoding = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(text);

      return Hash(text.BNToByteArray(encoding), algo);
   }

   /// <summary>
   /// Generates a HMAC-value as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <returns>HMAC-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashFile(string file, HMAC algo)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      byte[] bytes = FileHelper.ReadAllBytes(file);
      return Hash(bytes, algo);
   }

   /// <summary>
   /// Generates a HMAC-value as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <returns>HMAC-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashFileAsync(string file, HMAC algo)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      byte[] bytes = await FileHelper.ReadAllBytesAsync(file);
      return Hash(bytes, algo);
   }

   #endregion

   #region HMAC SHA256

   /// <summary>
   /// Generates a HMAC-value with SHA256 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA256(byte[] bytes, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA256(secret);
      return Hash(bytes, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA256 as byte-array with a given string and secret as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>HMAC-value with SHA256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA256(string text, byte[] secret, Encoding? encoding = null)
   {
      return HashHMACSHA256(text.BNToByteArray(encoding), secret);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA256 as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA256File(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA256(secret);
      return HashFile(file, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA256 as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashHMACSHA256FileAsync(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA256(secret);
      return await HashFileAsync(file, hash);
   }

   #endregion

   #region HMAC SHA384

   /// <summary>
   /// Generates a HMAC-value with SHA384 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA384(byte[] bytes, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA384(secret);
      return Hash(bytes, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA384 as byte-array with a given string and secret as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>HMAC-value with SHA384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA384(string text, byte[] secret, Encoding? encoding = null)
   {
      return HashHMACSHA384(text.BNToByteArray(encoding), secret);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA384 as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA384File(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA384(secret);
      return HashFile(file, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA384 as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashHMACSHA384FileAsync(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA384(secret);
      return await HashFileAsync(file, hash);
   }

   #endregion

   #region HMAC SHA512

   /// <summary>
   /// Generates a HMAC-value with SHA512 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA512(byte[] bytes, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA512(secret);
      return Hash(bytes, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA512 as byte-array with a given string and secret as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>HMAC-value with SHA512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA512(string text, byte[] secret, Encoding? encoding = null)
   {
      return HashHMACSHA512(text.BNToByteArray(encoding), secret);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA512 as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA512File(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA512(secret);
      return HashFile(file, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA512 as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashHMACSHA512FileAsync(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA512(secret);
      return await HashFileAsync(file, hash);
   }

   #endregion

   #region HMAC SHA3-256

   /// <summary>
   /// Generates a HMAC-value with SHA3-256 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA3-256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA3_256(byte[] bytes, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA3_256(secret);
      return Hash(bytes, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA3-256 as byte-array with a given string and secret as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>HMAC-value with SHA3-256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA3_256(string text, byte[] secret, Encoding? encoding = null)
   {
      return HashHMACSHA3_256(text.BNToByteArray(encoding), secret);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA3-256 as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA3-256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA3_256File(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA3_256(secret);
      return HashFile(file, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA3-256 as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA3-256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashHMACSHA3_256FileAsync(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA3_256(secret);
      return await HashFileAsync(file, hash);
   }

   #endregion

   #region HMAC SHA3-384

   /// <summary>
   /// Generates a HMAC-value with SHA3-384 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA3-384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA3_384(byte[] bytes, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA3_384(secret);
      return Hash(bytes, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA3-384 as byte-array with a given string and secret as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>HMAC-value with SHA3-384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA3_384(string text, byte[] secret, Encoding? encoding = null)
   {
      return HashHMACSHA3_384(text.BNToByteArray(encoding), secret);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA3-384 as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA3-384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA3_384File(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA3_384(secret);
      return HashFile(file, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA3-384 as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA3-384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashHMACSHA3_384FileAsync(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA3_384(secret);
      return await HashFileAsync(file, hash);
   }

   #endregion

   #region HMAC SHA3-512

   /// <summary>
   /// Generates a HMAC-value with SHA3-512 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA3-512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA3_512(byte[] bytes, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA3_512(secret);
      return Hash(bytes, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA3-512 as byte-array with a given string and secret as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>HMAC-value with SHA3-512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA3_512(string text, byte[] secret, Encoding? encoding = null)
   {
      return HashHMACSHA3_512(text.BNToByteArray(encoding), secret);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA3-512 as byte-array from a file.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA3-512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HashHMACSHA3_512File(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA3_512(secret);
      return HashFile(file, hash);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA3-512 as byte-array from a file asynchronously.
   /// </summary>
   /// <param name="file">File to hash</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA3-512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte[]> HashHMACSHA3_512FileAsync(string file, byte[] secret)
   {
      ArgumentNullException.ThrowIfNull(secret);

      using HMAC hash = new HMACSHA3_512(secret);
      return await HashFileAsync(file, hash);
   }

   #endregion

   #endregion
}