using System;
using System.Security.Cryptography;
using System.Text;

namespace BogaNet.Crypto;

/// <summary>
/// Obfuscator for strings and byte-arrays.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public abstract class Obfuscator
{
   private const byte DEFAULT_IV = 76;

   /// <summary>
   /// Generates a secure IV for the obfuscation.
   /// </summary>
   /// <returns>IV as byte</returns>
   public static byte GenerateIV()
   {
      byte[] buffer = new byte[1];
      using RandomNumberGenerator rng = RandomNumberGenerator.Create();
      rng.GetBytes(buffer);
      return buffer[0];
   }

   /// <summary>
   /// Obfuscate a byte-array.
   /// </summary>
   /// <param name="data">byte-array to obfuscate</param>
   /// <param name="IV">Initial-Vector byte (optional)</param>
   /// <returns>Obfuscated byte-array</returns>
   public static byte[]? Obfuscate(byte[]? data, byte IV = DEFAULT_IV)
   {
      if (data == null)
         return null;

      Array.Reverse(data);

      byte[] result = new byte[data.Length];
      byte lastByte = 0;

      for (int ii = 0; ii < data.Length; ii++)
      {
         byte currentByte = data[ii];
         lastByte = ii == 0 ? (byte)(currentByte + IV) : (byte)(currentByte + lastByte);

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
   public static byte[]? Obfuscate(string data, byte IV = DEFAULT_IV, Encoding? encoding = null)
   {
      if (data == null)
         return null;

      return Obfuscate(data.BNToByteArray(encoding));
   }

   /// <summary>
   /// De-obfuscate a byte-array.
   /// </summary>
   /// <param name="obfuscatedData">byte-array to de-obfuscate</param>
   /// <param name="IV">Initial-Vector byte (optional)</param>
   /// <returns>De-obfuscated byte-array</returns>
   public static byte[]? Deobfuscate(byte[]? obfuscatedData, byte IV = DEFAULT_IV)
   {
      if (obfuscatedData == null)
         return null;

      byte[] result = new byte[obfuscatedData.Length];

      for (int ii = obfuscatedData.Length - 1; ii >= 0; ii--)
      {
         byte currentByte = obfuscatedData[ii];
         byte lastByte = ii == 0 ? (byte)(currentByte - IV) : (byte)(currentByte - obfuscatedData[ii - 1]);

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
   public static string? DeobfuscateToString(byte[]? obfuscatedData, byte IV = DEFAULT_IV, Encoding? encoding = null)
   {
      if (obfuscatedData == null)
         return null;

      return Deobfuscate(obfuscatedData, IV).BNToString(encoding);
   }
}