using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BogaNet.Helper;

/// <summary>
/// Helper for HMAC cryptography.
/// </summary>
public abstract class HMACHelper
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(HMACHelper));

   /// <summary>
   /// Generates a secure secret for HMAC.
   /// </summary>
   /// <param name="length">Length of the secret (optional, default: 16)</param>
   /// <returns>Secure secret as byte-array</returns>
   public static byte[] GenerateSecret(int length = 16)
   {
      byte[] buffer = new byte[length];
      using RandomNumberGenerator rng = RandomNumberGenerator.Create();
      rng.GetBytes(buffer);
      return buffer;
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
      if (bytes == null || bytes.Length <= 0)
         throw new ArgumentNullException(nameof(bytes));
      if (secret == null || secret.Length <= 0)
         throw new ArgumentNullException(nameof(secret));

      try
      {
         using HMACSHA256 hash = new HMACSHA256(secret);
         return hash.ComputeHash(bytes);
      }
      catch (Exception ex)
      {
         LoggerExtensions.LogError(_logger, ex, "Compute of HMAC failed!");
         throw;
      }
   }

   /// <summary>
   /// Generates a HMAC-value with SHA256 as byte-array with a given string and secret as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HMAC256(string? text, byte[]? secret, Encoding? encoding = null)
   {
      if (text == null)
         throw new ArgumentNullException(nameof(text));

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
      if (bytes == null || bytes.Length <= 0)
         throw new ArgumentNullException(nameof(bytes));
      if (secret == null || secret.Length <= 0)
         throw new ArgumentNullException(nameof(secret));

      try
      {
         using HMACSHA384 hash = new HMACSHA384(secret);
         return hash.ComputeHash(bytes);
      }
      catch (Exception ex)
      {
         LoggerExtensions.LogError(_logger, ex, "Compute of HMAC failed!");
         throw;
      }
   }

   /// <summary>
   /// Generates a HMAC-value with SHA384 as byte-array with a given string and secret as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HMAC384(string? text, byte[]? secret, Encoding? encoding = null)
   {
      if (text == null)
         throw new ArgumentNullException(nameof(text));

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
      if (bytes == null || bytes.Length <= 0)
         throw new ArgumentNullException(nameof(bytes));
      if (secret == null || secret.Length <= 0)
         throw new ArgumentNullException(nameof(secret));

      try
      {
         using HMACSHA512 hash = new HMACSHA512(secret);
         return hash.ComputeHash(bytes);
      }
      catch (Exception ex)
      {
         LoggerExtensions.LogError(_logger, ex, "Compute of HMAC failed!");
         throw;
      }
   }

   /// <summary>
   /// Generates a HMAC-value with SHA512 as byte-array with a given string and secret as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HMAC512(string? text, byte[]? secret, Encoding? encoding = null)
   {
      if (text == null)
         throw new ArgumentNullException(nameof(text));

      return HMAC512(text.BNToByteArray(encoding), secret);
   }
}