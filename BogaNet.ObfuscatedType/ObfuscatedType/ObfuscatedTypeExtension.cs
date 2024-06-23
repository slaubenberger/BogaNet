using System;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Extension methods for ObfuscatedType.
/// </summary>
public static class ObfuscatedTypeExtension
{
   /// <summary>
   /// Converts a byte-array to a ByteObf-array.
   /// </summary>
   /// <param name="array">Array-instance to convert</param>
   /// <returns>Converted ByteObf-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static ByteObf[] BNToBNbyteArray(this byte[]? array)
   {
      if (array == null)
         throw new ArgumentNullException(nameof(array));

      ByteObf[] bNbytes = new ByteObf[array.Length];

      for (int ii = 0; ii < array.Length; ii++)
      {
         bNbytes[ii] = array[ii];
      }

      return bNbytes;
   }

   /// <summary>
   /// Converts a ByteObf-array to a byte-array.
   /// </summary>
   /// <param name="array">Array-instance to convert</param>
   /// <returns>Converted byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] BNToByteArray(this ByteObf[]? array)
   {
      if (array == null)
         throw new ArgumentNullException(nameof(array));

      byte[] bytes = new byte[array.Length];

      for (int ii = 0; ii < array.Length; ii++)
      {
         bytes[ii] = array[ii];
      }

      return bytes;
   }
}