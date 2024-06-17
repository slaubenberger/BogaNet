using System;
using System.Security.Cryptography;

namespace BogaNet.Crypto;

/// <summary>
/// Obfuscator for strings and byte-arrays.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public abstract class Obfuscator
{
   private const byte DEFAULT_IV = 76; //TODO change the value in every project!

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
}