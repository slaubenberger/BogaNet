using System;
using System.Linq;

namespace BogaNet.Helper;

/// <summary>
/// Helper methods for arrays.
/// </summary>
public static class ArrayHelper
{
   //private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ArrayHelper));

   /// <summary>
   /// Converts a byte-array to a float-array.
   /// </summary>
   /// <param name="array">Array-instance to convert</param>
   /// <param name="count">Number of bytes to convert (optional)</param>
   /// <returns>Converted float-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static float[] ByteArrayToFloatArray(byte[]? array, int count = 0)
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
   /// <param name="array">Array-instance to convert</param>
   /// <param name="count">Number of floats to convert (optional)</param>
   /// <returns>Converted byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FloatArrayToByteArray(float[]? array, int count = 0)
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
   /// Returns the column of a 2D-array as array.
   /// </summary>
   /// <param name="matrix">Input as 2D-array</param>
   /// <param name="columnNumber">Desired column of the 2D-array</param>
   /// <returns>Column of a 2D-array as array</returns>
   public static T[]? GetColumn<T>(T[,]? matrix, int columnNumber)
   {
      return matrix != null ? Enumerable.Range(0, matrix.GetLength(0)).Select(x => matrix[x, columnNumber]).ToArray() : default;
   }

   /// <summary>
   /// Returns the row of a 2D-array as array.
   /// </summary>
   /// <param name="matrix">Input as 2D-array</param>
   /// <param name="rowNumber">Desired row of the 2D-array</param>
   /// <returns>Row of a 2D-array as array</returns>
   public static T[]? GetRow<T>(T[,]? matrix, int rowNumber)
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