using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;
using BogaNet.Extension;

namespace BogaNet.Helper;

/// <summary>
/// Helper for hash computations. It contains ready-to-use Implementations of SHA256, SHA384 and SHA512.
/// </summary>
public abstract class HashHelper //NUnit
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(HashHelper));

   #region Public methods

   /// <summary>
   /// Generates a hash-value as byte-array with a given byte-array and algorithm as input.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <returns>Hash-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] Hash(byte[]? bytes, HashAlgorithm? algo)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      try
      {
         return algo?.ComputeHash(bytes, 0, bytes.Length) ?? [];
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
      return Hash(text.BNToByteArray(encoding), algo);
   }

   /// <summary>
   /// Generates a SHA256-value as byte-array with a given byte-array.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA256(params byte[]? bytes)
   {
      using HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create();
      return Hash(bytes, sha256);
   }

   /// <summary>
   /// Generates a SHA256-value as byte-array with a given string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA256(string? text, Encoding? encoding = null)
   {
      return SHA256(text.BNToByteArray(encoding));
   }

   /// <summary>
   /// Generates a SHA384-value as byte-array with a given byte-array.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>SHA384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA384(params byte[]? bytes)
   {
      using HashAlgorithm sha384 = System.Security.Cryptography.SHA384.Create();
      return Hash(bytes, sha384);
   }

   /// <summary>
   /// Generates a SHA384-value as byte-array with a given string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA384-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA384(string? text, Encoding? encoding = null)
   {
      return SHA384(text.BNToByteArray(encoding));
   }

   /// <summary>
   /// Generates a SHA512-value as byte-array with a given byte-array.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>SHA512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA512(params byte[]? bytes)
   {
      using HashAlgorithm sha512 = System.Security.Cryptography.SHA512.Create();
      return Hash(bytes, sha512);
   }

   /// <summary>
   /// Generates a SHA512-value as byte-array with a given string.
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>SHA512-value as byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SHA512(string? text, Encoding? encoding = null)
   {
      return SHA512(text.BNToByteArray(encoding));
   }

   #endregion
}