﻿using System.Text;

namespace BogaNet;

/// <summary>
/// Extension methods for arrays.
/// </summary>
public static class ExtensionArray
{
/*
   /// <summary>
   /// Extension method for arrays.
   /// Shuffles an array.
   /// </summary>
   /// <param name="array">Array-instance to shuffle.</param>
   /// <param name="seed">Seed for the PRNG (optional, default: 0 (=standard))</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void BNShuffle<T>(this T[]? array, int seed = 0)
   {
      if (array == null || array.Length <= 0)
         throw new ArgumentNullException(nameof(array));

      Random rnd = seed == 0 ? new Random() : new Random(seed);
      int n = array.Length;
      while (n > 1)
      {
         int k = rnd.Next(n--);
         (array[n], array[k]) = (array[k], array[n]);
      }
   }

   /// <summary>
   /// Case insensitive 'Contains' per default.
   /// </summary>
   /// <param name="str">String array-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparer (optional, default: StringComparer.OrdinalIgnoreCase)</param>
   /// <returns>True if the string array contains the given string.</returns>
   public static bool BNContains(this string[]? str, string? toCheck, StringComparer? comp = null)
   {
      if (str == null)
         return false;

      comp ??= StringComparer.OrdinalIgnoreCase;

      return str.Contains(toCheck, comp);
   }

   /// <summary>
   /// Dumps an array to a string.
   /// </summary>
   /// <param name="array">Array-instance to dump.</param>
   /// <param name="prefix">Prefix for every element (optional, default: empty).</param>
   /// <param name="postfix">Postfix for every element (optional, default: empty).</param>
   /// <param name="appendNewLine">Append new line, otherwise use the given delimiter (optional, default: false).</param>
   /// <param name="delimiter">Delimiter if appendNewLine is false (optional, default: "; ").</param>
   /// <returns>String with lines for all array entries.</returns>
   public static string? BNDump<T>(this T[]? array, string? prefix = "", string? postfix = "", bool appendNewLine = true, string delimiter = "; ")
   {
      if (array == null) // || array.Length <= 0)
         return null;

      StringBuilder sb = new();

      foreach (T element in array)
      {
         if (0 < sb.Length)
         {
            sb.Append(appendNewLine ? Environment.NewLine : delimiter);
         }

         sb.Append(prefix);
         sb.Append(element);
         sb.Append(postfix);
      }

      return sb.ToString();
   }

   /// <summary>
   /// Generates a string array with all entries (via CTToString).
   /// </summary>
   /// <param name="array">Array-instance to ToString.</param>
   /// <returns>String array with all entries (via CTToString).</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string[] BNToStringArray<T>(this T[]? array)
   {
      if (array == null) // || array.Length <= 0)
         throw new ArgumentNullException(nameof(array));

      string[] result = new string[array.Length];

      for (int ii = 0; ii < array.Length; ii++)
      {
         string line = "null";

         T content = array[ii];

         if (content != null)
            line = content.CTToString()!;

         result[ii] = line;
      }

      return result;
   }
*/
   /// <summary>
   /// Converts a byte-array to a float-array.
   /// </summary>
   /// <param name="array">Array-instance to convert.</param>
   /// <param name="count">Number of bytes to convert (optional).</param>
   /// <returns>Converted float-array.</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static float[] BNToFloatArray(this byte[]? array, int count = 0)
   {
      if (array == null) // || array.Length <= 0)
         throw new ArgumentNullException(nameof(array));

      int _count = count;

      if (_count <= 0)
         _count = array.Length;

      float[] floats = new float[_count / 2];

      int ii = 0;
      for (int zz = 0; zz < _count; zz += 2)
      {
         floats[ii] = bytesToFloat(array[zz], array[zz + 1]);
         ii++;
      }

      return floats;
   }

   /// <summary>
   /// Converts a float-array to a byte-array.
   /// </summary>
   /// <param name="array">Array-instance to convert.</param>
   /// <param name="count">Number of floats to convert (optional).</param>
   /// <returns>Converted byte-array.</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] BNToByteArray(this float[]? array, int count = 0)
   {
      if (array == null) // || array.Length <= 0)
         throw new ArgumentNullException(nameof(array));

      int _count = count;

      if (_count <= 0)
         _count = array.Length;

      byte[] bytes = new byte[_count * 2];
      int byteIndex = 0;

      for (int ii = 0; ii < _count; ii++)
      {
         short outsample = (short)(array[ii] * short.MaxValue);

         bytes[byteIndex] = (byte)(outsample & 0xff);

         bytes[byteIndex + 1] = (byte)((outsample >> 8) & 0xff);

         byteIndex += 2;
      }

      return bytes;
   }

   /// <summary>
   /// Converts a byte-array to a string.
   /// </summary>
   /// <param name="data">Input string as byte-array.</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8).</param>
   /// <returns>Byte-array with the string.</returns>
   public static string? BNConvertToString(this byte[]? data, Encoding? encoding = null)
   {
      if (data == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return _encoding.GetString(data);
   }

   /// <summary>
   /// Converts a byte-array to a Base64-string.
   /// </summary>
   /// <param name="data">Input as byte-array.</param>
   /// <returns>Base64-string from the byte-array.</returns>
   public static string? BNToBase64(this byte[]? data)
   {
      return data == null ? null : Convert.ToBase64String(data);
   }

   /// <summary>
   /// Returns the column of a 2D-array as array.
   /// </summary>
   /// <param name="matrix">Input as 2D-array.</param>
   /// <param name="columnNumber">Desired column of the 2D-array</param>
   /// <returns>Column of a 2D-array as array.</returns>
   public static T[]? BNGetColumn<T>(this T[,]? matrix, int columnNumber)
   {
      return matrix != null ? Enumerable.Range(0, matrix.GetLength(0)).Select(x => matrix[x, columnNumber]).ToArray() : default;
   }

   /// <summary>
   /// Returns the row of a 2D-array as array.
   /// </summary>
   /// <param name="matrix">Input as 2D-array.</param>
   /// <param name="rowNumber">Desired row of the 2D-array</param>
   /// <returns>Row of a 2D-array as array.</returns>
   public static T[]? BNGetRow<T>(this T[,]? matrix, int rowNumber)
   {
      return matrix != null ? Enumerable.Range(0, matrix.GetLength(1)).Select(x => matrix[rowNumber, x]).ToArray() : default;
   }

   #region Private methods

   private static float bytesToFloat(byte firstByte, byte secondByte)
   {
      // convert two bytes to one short (little endian) and convert it to range from -1 to (just below) 1
      return (short)((secondByte << 8) | firstByte) / Constants.FLOAT_32768;
   }

   #endregion
}