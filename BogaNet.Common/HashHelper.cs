﻿using System.Security.Cryptography;
using System.Text;

namespace BogaNet;

/// <summary>
/// Helper for hash functions.
/// </summary>
public abstract class HashHelper
{
   //public static readonly System.Security.Cryptography.SHA512 ALGO_SHA512 = System.Security.Cryptography.SHA512.Create();
   public static readonly System.Security.Cryptography.SHA256 ALGO_SHA256 = System.Security.Cryptography.SHA256.Create();

   /// <summary>
   /// Generates a hash-value as byte-array with a given byte-array and algorithm as input
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <returns>Hash-value as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] Hash(byte[]? input, HashAlgorithm? algo)
   {
      if (input == null || input.Length <= 0)
         throw new ArgumentNullException(nameof(input));

      return algo?.ComputeHash(input, 0, input.Length) ?? [];
   }

   /// <summary>
   /// Generates a hash-value as byte-array with a given string and algorithm as input
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8).</param>
   /// <returns>Hash-value as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] Hash(string? text, HashAlgorithm? algo, Encoding? encoding = null)
   {
      if (string.IsNullOrEmpty(text))
         throw new ArgumentNullException(nameof(text));

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return Hash(_encoding.GetBytes(text), algo);
   }

   /// <summary>
   /// Generates a hash-value as string with a given byte-array and algorithm as input
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <returns>Hash-value as string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string HashAsString(byte[]? input, HashAlgorithm? algo)
   {
      if (input == null || input.Length <= 0)
         throw new ArgumentNullException(nameof(input));

      string hash = string.Empty;
      byte[] crypto = Hash(input, algo);

      return crypto.Aggregate(hash, (current, bit) => current + bit.ToString("x2"));
   }

   /// <summary>
   /// Generates a hash-value as string with a given string and algorithm as input
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <param name="algo">Hash-algorithm</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8).</param>
   /// <returns>Hash-value as string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string HashAsString(string? text, HashAlgorithm? algo, Encoding? encoding = null)
   {
      if (string.IsNullOrEmpty(text))
         throw new ArgumentNullException(nameof(text));

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return HashAsString(_encoding.GetBytes(text), algo);
   }

   /// <summary>
   /// Generates a SHA256-value as byte-array with a given byte-array
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] SHA256(byte[]? input)
   {
      return Hash(input, ALGO_SHA256);
   }

   /// <summary>
   /// Generates a SHA256-value as byte-array with a given string
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <returns>SHA256-value as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] SHA256(string? text)
   {
      return Hash(text, ALGO_SHA256);
   }

   /// <summary>
   /// Generates a SHA256-value as string with a given byte-array
   /// </summary>
   /// <param name="input">Data as byte-array</param>
   /// <returns>SHA256-value as string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string SHA256AsString(byte[]? input)
   {
      return HashAsString(input, ALGO_SHA256);
   }

   /// <summary>
   /// Generates a SHA256-value as string with a given string
   /// </summary>
   /// <param name="text">Data as string</param>
   /// <returns>SHA256-value as string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string SHA256AsString(string? text)
   {
      return HashAsString(text, ALGO_SHA256);
   }
}