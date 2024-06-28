using System;
using System.Security.Cryptography;
using System.Text;

namespace BogaNet.Util;

/// <summary>
/// Obfuscator for strings and byte-arrays.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public abstract class Obfuscator //NUnit
{
   private const byte DEFAULT_IV = 76;

   /// <summary>
   /// Generates a secure IV for the obfuscation.
   /// </summary>
   /// <returns>IV as byte</returns>
   public static byte GenerateIV()
   {
      return RandomNumberGenerator.GetBytes(1)[0];
   }

   /// <summary>
   /// Obfuscate a byte-array.
   /// </summary>
   /// <param name="data">byte-array to obfuscate</param>
   /// <param name="IV">Initial-Vector byte (optional)</param>
   /// <returns>Obfuscated byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] Obfuscate(byte[]? data, byte IV = DEFAULT_IV)
   {
      ArgumentNullException.ThrowIfNull(data);

      Array.Reverse(data);

      byte[] result = new byte[data.Length];
      byte lastByte = 0;

      for (int ii = 0; ii < data.Length; ii++)
      {
         byte currentByte = data[ii];
         lastByte = ii == 0 ? (byte)(currentByte + IV) : (byte)(currentByte + lastByte + IV);

         result[ii] = lastByte;
      }

      return result;
   }

   /// <summary>
   /// Obfuscate a string.
   /// </summary>
   /// <param name="data">string to obfuscate</param>
   /// <param name="IV">Initial-Vector byte (optional)</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Obfuscated string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[]? Obfuscate(string? data, byte IV = DEFAULT_IV, Encoding? encoding = null)
   {
      return Obfuscate(data.BNToByteArray(encoding), IV);
   }

   /// <summary>
   /// De-obfuscate a byte-array.
   /// </summary>
   /// <param name="obfuscatedData">byte-array to de-obfuscate</param>
   /// <param name="IV">Initial-Vector byte (optional)</param>
   /// <returns>De-obfuscated byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] Deobfuscate(byte[]? obfuscatedData, byte IV = DEFAULT_IV)
   {
      ArgumentNullException.ThrowIfNull(obfuscatedData);

      byte[] result = new byte[obfuscatedData.Length];

      for (int ii = obfuscatedData.Length - 1; ii >= 0; ii--)
      {
         byte currentByte = obfuscatedData[ii];
         byte lastByte = ii == 0 ? (byte)(currentByte - IV) : (byte)(currentByte - obfuscatedData[ii - 1] - IV);
         //byte lastByte = ii == 0 ? (byte)(currentByte) : (byte)(currentByte - obfuscatedData[ii - 1]); //simulate someone who hasn't got the IV

         result[ii] = lastByte;
      }

      Array.Reverse(result);

      return result;
   }

   /// <summary>
   /// De-obfuscate a byte-array to a string.
   /// </summary>
   /// <param name="obfuscatedData">byte-array to de-obfuscate</param>
   /// <param name="IV">Initial-Vector byte (optional)</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>De-obfuscated byte-array as string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string? DeobfuscateToString(byte[]? obfuscatedData, byte IV = DEFAULT_IV, Encoding? encoding = null)
   {
      return Deobfuscate(obfuscatedData, IV).BNToString(encoding);
   }
}