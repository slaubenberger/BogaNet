using System.Numerics;
using System;

namespace BogaNet.Extension;

/// <summary>
/// Extension methods for arrays.
/// </summary>
public static class ArrayExtension //NUnit
{
   //private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ArrayExtension));

   /// <summary>
   /// Reverses an object array.
   /// </summary>
   /// <param name="array">Array-instance</param>
   /// <returns>Reversed object array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static void BNReverse(this object[] array)
   {
      ArgumentNullException.ThrowIfNull(array);

      Array.Reverse(array);
   }

   /// <summary>
   /// Reverses a Number type array.
   /// </summary>
   /// <param name="array">Number type array-instance</param>
   /// <returns>Reversed Number type array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static void BNReverse<T>(this T[] array) where T : INumber<T>
   {
      ArgumentNullException.ThrowIfNull(array);

      Array.Reverse(array);
   }
/*
   /// <summary>
   /// Converts a byte-array to a string.
   /// </summary>
   /// <param name="bytes">Byte-array</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <param name="offset">Offset inside the byte-array (optional, default: 0)</param>
   /// <param name="length">Number of bytes (optional, default: 0 = all)</param>
   /// <returns>String from the byte-array</returns>
   public static string BNToString(this byte[] bytes, Encoding? encoding = null, int offset = 0, int length = 0)
   {
      if (bytes == null)
         return null;

      int off = offset > 0 ? offset : 0;
      int len = length > 0 ? length : bytes.Length;

      byte[] content = new byte[len];
      Buffer.BlockCopy(bytes, off, content, 0, len);
      string? res = content.BNToString(encoding);
      return res?.Trim('\0');
   }
*/

   #region Private methods

/*
   private static bool isByteArrayValidForNumber(byte[] bytes, int length, Type type)
   {
      if (bytes.Length < length)
      {
         _logger.LogWarning($"Byte array is to short for a valid number type {type}!");
         return false;
      }

      return true;
   }
*/

   #endregion
}