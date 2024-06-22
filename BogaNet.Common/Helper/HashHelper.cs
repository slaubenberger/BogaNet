using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BogaNet.Helper;

/// <summary>
/// Helper for hash computations.
/// </summary>
public abstract class HashHelper
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(HashHelper));

   /// <summary>
   /// Generates a hash-value as byte-array with a given byte-array and algorithm as input.
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <returns>Hash-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Hash(byte[]? input, HashAlgorithm? algo)
   {
      if (input == null || input.Length <= 0)
         throw new ArgumentNullException(nameof(input));

      try
      {
         return algo?.ComputeHash(input, 0, input.Length) ?? [];
      }
      catch (Exception ex)
      {
         LoggerExtensions.LogError(_logger, ex, "Compute of hash failed!");
         throw;
      }
   }

   /// <summary>
   /// Generates a hash-value as byte-array with a given string and algorithm as input.
   /// </summary>
   /// <param name="input">Data as string</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Hash-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Hash(string? input, HashAlgorithm? algo, Encoding? encoding = null)
   {
      if (input == null)
         throw new ArgumentNullException(nameof(input));

      return Hash(input.BNToByteArray(encoding), algo);
   }

   /// <summary>
   /// Generates a SHA256-value as byte-array with a given byte-array.
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA256(byte[]? input)
   {
      using SHA256 sha256 = System.Security.Cryptography.SHA256.Create();
      return Hash(input, sha256);
   }

   /// <summary>
   /// Generates a SHA256-value as byte-array with a given string.
   /// </summary>
   /// <param name="input">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA256(string? input, Encoding? encoding = null)
   {
      if (input == null)
         throw new ArgumentNullException(nameof(input));

      return SHA256(input.BNToByteArray(encoding));
   }

   /// <summary>
   /// Generates a SHA384-value as byte-array with a given byte-array.
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <returns>SHA384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA384(byte[]? input)
   {
      using SHA384 sha384 = System.Security.Cryptography.SHA384.Create();
      return Hash(input, sha384);
   }

   /// <summary>
   /// Generates a SHA384-value as byte-array with a given string.
   /// </summary>
   /// <param name="input">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA384(string? input, Encoding? encoding = null)
   {
      if (input == null)
         throw new ArgumentNullException(nameof(input));

      return SHA384(input.BNToByteArray(encoding));
   }

   /// <summary>
   /// Generates a SHA512-value as byte-array with a given byte-array.
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <returns>SHA512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA512(byte[]? input)
   {
      using SHA512 sha512 = System.Security.Cryptography.SHA512.Create();
      return Hash(input, sha512);
   }

   /// <summary>
   /// Generates a SHA512-value as byte-array with a given string.
   /// </summary>
   /// <param name="input">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA512(string? input, Encoding? encoding = null)
   {
      if (input == null)
         throw new ArgumentNullException(nameof(input));

      return SHA512(input.BNToByteArray(encoding));
   }
}