using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;
using BogaNet.Extension;

namespace BogaNet.Helper;

/// <summary>
/// Helper for HMAC cryptography. It contains ready-to-use Implementations of HMAC256, HMAC384 and HMAC512.
/// </summary>
public abstract class HMACHelper //NUnit
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(HMACHelper));

   #region Public methods

   /// <summary>
   /// Generates a secure secret for HMAC.
   /// </summary>
   /// <param name="length">Length of the secret (optional, default: 16)</param>
   /// <returns>Secure secret as byte-array</returns>
   public static byte[] GenerateSecret(int length = 16)
   {
      return RandomNumberGenerator.GetBytes(length);
   }

   /// <summary>
   /// Generates a HMAC-value as byte-array with given byte-array and algorithm as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="algo">HMAC-algorithm</param>
   /// <returns>HMAC-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Hash(byte[]? bytes, HMAC? algo)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      try
      {
         return algo?.ComputeHash(bytes, 0, bytes.Length) ?? [];
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
   public static byte[] Hash(string? text, HMAC? algo, Encoding? encoding = null)
   {
      return Hash(text.BNToByteArray(encoding), algo);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA256 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HMAC256(byte[]? bytes, byte[]? secret)
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
   public static byte[] HMAC256(string? text, byte[]? secret, Encoding? encoding = null)
   {
      return HMAC256(text.BNToByteArray(encoding), secret);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA384 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HMAC384(byte[]? bytes, byte[]? secret)
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
   public static byte[] HMAC384(string? text, byte[]? secret, Encoding? encoding = null)
   {
      return HMAC384(text.BNToByteArray(encoding), secret);
   }

   /// <summary>
   /// Generates a HMAC-value with SHA512 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HMAC512(byte[]? bytes, byte[]? secret)
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
   public static byte[] HMAC512(string? text, byte[]? secret, Encoding? encoding = null)
   {
      return HMAC512(text.BNToByteArray(encoding), secret);
   }

   #endregion
}