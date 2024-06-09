using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace BogaNet.Crypto;

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
         _logger.LogError(ex, "Compute of hash failed!");
         throw;
      }
   }

   /// <summary>
   /// Generates a hash-value as byte-array with a given string and algorithm as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Hash-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Hash(string? text, HashAlgorithm? algo, Encoding? encoding = null)
   {
      if (string.IsNullOrEmpty(text))
         throw new ArgumentNullException(nameof(text));

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return Hash(_encoding.GetBytes(text), algo);
   }

   /// <summary>
   /// Generates a hash-value as string with a given byte-array and algorithm as input.
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <returns>Hash-value as string</returns>
   /// <exception cref="Exception"></exception>
   public static string HashAsString(byte[]? input, HashAlgorithm? algo)
   {
      if (input == null || input.Length <= 0)
         throw new ArgumentNullException(nameof(input));

      string hash = string.Empty;
      byte[] crypto = Hash(input, algo);

      return crypto.Aggregate(hash, (current, bit) => current + bit.ToString("x2"));
   }

   /// <summary>
   /// Generates a hash-value as string with a given string and algorithm as input.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Hash-value as string</returns>
   /// <exception cref="Exception"></exception>
   public static string HashAsString(string? text, HashAlgorithm? algo, Encoding? encoding = null)
   {
      if (string.IsNullOrEmpty(text))
         throw new ArgumentNullException(nameof(text));

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return HashAsString(_encoding.GetBytes(text), algo);
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
   /// <param name="text">Data as string</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA256(string? text)
   {
      using SHA256 sha256 = System.Security.Cryptography.SHA256.Create();
      return Hash(text, sha256);
   }

   /// <summary>
   /// Generates a SHA256-value as string with a given byte-array.
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <returns>SHA256-value as string</returns>
   /// <exception cref="Exception"></exception>
   public static string SHA256AsString(byte[]? input)
   {
      using SHA256 sha256 = System.Security.Cryptography.SHA256.Create();
      return HashAsString(input, sha256);
   }

   /// <summary>
   /// Generates a SHA256-value as string with a given string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <returns>SHA256-value as string</returns>
   /// <exception cref="Exception"></exception>
   public static string SHA256AsString(string? text)
   {
      using SHA256 sha256 = System.Security.Cryptography.SHA256.Create();
      return HashAsString(text, sha256);
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
   /// <param name="text">Data as string</param>
   /// <returns>SHA512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA512(string? text)
   {
      using SHA512 sha512 = System.Security.Cryptography.SHA512.Create();
      return Hash(text, sha512);
   }

   /// <summary>
   /// Generates a SHA512-value as string with a given byte-array.
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <returns>SHA512-value as string</returns>
   /// <exception cref="Exception"></exception>
   public static string SHA512AsString(byte[]? input)
   {
      using SHA512 sha512 = System.Security.Cryptography.SHA512.Create();
      return HashAsString(input, sha512);
   }

   /// <summary>
   /// Generates a SHA512-value as string with a given string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <returns>SHA512-value as string</returns>
   /// <exception cref="Exception"></exception>
   public static string SHA512AsString(string? text)
   {
      using SHA512 sha512 = System.Security.Cryptography.SHA512.Create();
      return HashAsString(text, sha512);
   }
}