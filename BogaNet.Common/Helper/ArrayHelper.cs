using System;
using System.Collections.Generic;
using System.Linq;

namespace BogaNet.Helper;

/// <summary>
/// Helper methods for arrays.
/// </summary>
public static class ArrayHelper
{
   //private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ArrayHelper));

   #region Public methods

   /// <summary>
   /// Converts a byte-array to a float-array.
   /// </summary>
   /// <param name="bytes">Byte-array to convert</param>
   /// <param name="count">Number of bytes to convert (optional, default: 0 = all)</param>
   /// <returns>Converted float-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static float[] ByteArrayToFloatArray(IList<byte> bytes, int count = 0)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      if (count <= 0)
         count = bytes.Count;

      float[] floats = new float[count / 2];

      int ii = 0;
      for (int zz = 0; zz < count; zz += 2)
      {
         floats[ii] = bytesToFloat(bytes[zz], bytes[zz + 1]);
         ii++;
      }

      return floats;
   }

   /// <summary>
   /// Converts a float-array to a byte-array.
   /// </summary>
   /// <param name="array">Float-array to convert</param>
   /// <param name="count">Number of floats to convert (optional, default: 0 = all)</param>
   /// <returns>Converted byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FloatArrayToByteArray(IList<float> array, int count = 0)
   {
      ArgumentNullException.ThrowIfNull(array);

      if (count <= 0)
         count = array.Count;

      byte[] bytes = new byte[count * 2];
      int byteIndex = 0;

      for (int ii = 0; ii < count; ii++)
      {
         short outsample = (short)(array[ii] * short.MaxValue);

         bytes[byteIndex] = (byte)(outsample & 0xff);

         bytes[byteIndex + 1] = (byte)((outsample >> 8) & 0xff);

         byteIndex += 2;
      }

      return bytes;
   }

   /// <summary>
   /// Converts a byte-array to a sbyte-array.
   /// </summary>
   /// <param name="bytes">Byte-array to convert</param>
   /// <param name="count">Number of bytes to convert (optional, default: 0 = all)</param>
   /// <returns>Converted sbyte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static sbyte[] ByteArrayToSByteArray(IList<byte> bytes, int count = 0)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      if (count <= 0)
         count = bytes.Count;

      sbyte[] sbytes = new sbyte[count];

      for (int ii = 0; ii < count; ii++)
      {
         sbytes[ii] = Convert.ToSByte(bytes[ii] - 128);
      }

      return sbytes;
   }

   /// <summary>
   /// Converts a sbyte-array to a byte-array.
   /// </summary>
   /// <param name="sbytes">SByte-array to convert</param>
   /// <param name="count">Number of sbytes to convert (optional, default: 0 = all)</param>
   /// <returns>Converted byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] SByteArrayToByteArray(IList<sbyte> sbytes, int count = 0)
   {
      ArgumentNullException.ThrowIfNull(sbytes);

      if (count <= 0)
         count = sbytes.Count;

      byte[] bytes = new byte[count];

      for (int ii = 0; ii < count; ii++)
      {
         bytes[ii] = Convert.ToByte(sbytes[ii] + 128);
      }

      return bytes;
   }

   /// <summary>
   /// Returns the column of a 2D-array as array.
   /// </summary>
   /// <param name="matrix">Input as 2D-array</param>
   /// <param name="columnNumber">Desired column of the 2D-array</param>
   /// <returns>Column of a 2D-array as array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static T[] GetColumn<T>(T[,] matrix, int columnNumber)
   {
      ArgumentNullException.ThrowIfNull(matrix);

      return Enumerable.Range(0, matrix.GetLength(0)).Select(x => matrix[x, Math.Abs(columnNumber)]).ToArray();
   }

   /// <summary>
   /// Returns the row of a 2D-array as array.
   /// </summary>
   /// <param name="matrix">Input as 2D-array</param>
   /// <param name="rowNumber">Desired row of the 2D-array</param>
   /// <returns>Row of a 2D-array as array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static T[] GetRow<T>(T[,] matrix, int rowNumber)
   {
      ArgumentNullException.ThrowIfNull(matrix);

      return Enumerable.Range(0, matrix.GetLength(1)).Select(x => matrix[Math.Abs(rowNumber), x]).ToArray();
   }

   #endregion

   #region Private methods

   private static float bytesToFloat(byte firstByte, byte secondByte)
   {
      // convert two bytes to one short (little endian) and convert it to range from -1 to (just below) 1
      return (short)((secondByte << 8) | firstByte) / Constants.FLOAT_32768;
   }

   #endregion
}