using System;

namespace BogaNet.SecureType;

/// <summary>
/// Extension methods for SecureType.
/// </summary>
public static class SecureTypeExtension
{
   /// <summary>
   /// Converts a byte-array to a ByteSec-array.
   /// </summary>
   /// <param name="array">Array-instance to convert</param>
   /// <returns>Converted ByteSec-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static ByteSec[] BNToByteSecArray(this byte[]? array)
   {
      if (array == null)
         throw new ArgumentNullException(nameof(array));

      ByteSec[] bNbytes = new ByteSec[array.Length];

      for (int ii = 0; ii < array.Length; ii++)
      {
         bNbytes[ii] = array[ii];
      }

      return bNbytes;
   }

   /// <summary>
   /// Converts a ByteSec-array to a byte-array.
   /// </summary>
   /// <param name="array">Array-instance to convert</param>
   /// <returns>Converted byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] ToByteArray(this ByteSec[]? array)
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