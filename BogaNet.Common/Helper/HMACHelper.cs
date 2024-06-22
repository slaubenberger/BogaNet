using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;

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
   /// Generates a HMAC-value with SHA-256 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA-256 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HMAC256(byte[]? input, byte[]? secret)
   {
      if (input == null || input.Length <= 0)
         throw new ArgumentNullException(nameof(input));
      if (secret == null || secret.Length <= 0)
         throw new ArgumentNullException(nameof(secret));

      try
      {
         using HMACSHA256 hash = new HMACSHA256(secret);
         return hash.ComputeHash(input);
      }
      catch (Exception ex)
      {
         LoggerExtensions.LogError(_logger, ex, "Compute of HMAC failed!");
         throw;
      }
   }

   /// <summary>
   /// Generates a HMAC-value with SHA-384 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA-384 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HMAC384(byte[]? input, byte[]? secret)
   {
      if (input == null || input.Length <= 0)
         throw new ArgumentNullException(nameof(input));
      if (secret == null || secret.Length <= 0)
         throw new ArgumentNullException(nameof(secret));

      try
      {
         using HMACSHA384 hash = new HMACSHA384(secret);
         return hash.ComputeHash(input);
      }
      catch (Exception ex)
      {
         LoggerExtensions.LogError(_logger, ex, "Compute of HMAC failed!");
         throw;
      }
   }

   /// <summary>
   /// Generates a HMAC-value with SHA-512 as byte-array with a given byte-array and secret as input.
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <param name="secret">Shared secret for HMAC</param>
   /// <returns>HMAC-value with SHA-512 as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] HMAC512(byte[]? input, byte[]? secret)
   {
      if (input == null || input.Length <= 0)
         throw new ArgumentNullException(nameof(input));
      if (secret == null || secret.Length <= 0)
         throw new ArgumentNullException(nameof(secret));

      try
      {
         using HMACSHA512 hash = new HMACSHA512(secret);
         return hash.ComputeHash(input);
      }
      catch (Exception ex)
      {
         LoggerExtensions.LogError(_logger, ex, "Compute of HMAC failed!");
         throw;
      }
   }
}