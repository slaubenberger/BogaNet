using System.Text;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System;
using BogaNet.Helper;

namespace BogaNet;

/// <summary>
/// Extension methods for arrays.
/// </summary>
public static class ArrayExtension //NUnit
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ArrayExtension));

   /// <summary>
   /// Converts a byte-array to a string.
   /// </summary>
   /// <param name="bytes">Input string as byte-array</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String from the byte-array</returns>
   public static string? BNToString(this byte[]? bytes, Encoding? encoding = null)
   {
      if (bytes == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return _encoding.GetString(bytes);
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
   public static string? BNToString(this byte[]? bytes, Encoding? encoding = null, int offset = 0, int length = 0)
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
   private static byte[] readNumberData(int len, int off, byte[] bytes)
   {
      byte[] data = new byte[len];
      int dstOff = Math.Clamp(len - bytes.Length, 0, len);
      int length = Math.Clamp(bytes.Length, 1, len);
      Buffer.BlockCopy(bytes, off, data, dstOff, length);

      if (BitConverter.IsLittleEndian)
         data.BNReverse();

      return data;
   }

   /// <summary>
   /// Converts a byte-array to a Number.
   /// </summary>
   /// <param name="bytes">Byte-array</param>
   /// <param name="offset">Offset inside the byte-array (optional, default: 0)</param>
   /// <returns>Number from the byte-array</returns>
   public static T? BNToNumber<T>(this byte[]? bytes, int offset = 0) where T : INumber<T>
   {
      if (bytes == null || bytes.Length == 0)
         return default;

      Type type = typeof(T);
      byte[] content;
      int off = offset > 0 ? offset : 0;
      switch (type)
      {
         case Type tByte when tByte == typeof(byte):
            return T.CreateTruncating(bytes[off]);
         case Type tSbyte when tSbyte == typeof(sbyte):
            return T.CreateTruncating(bytes[off]);
         case Type tShort when tShort == typeof(short):
            content = readNumberData(2, off, bytes);
            return T.CreateTruncating(BitConverter.ToInt16(content));
         case Type tUshort when tUshort == typeof(ushort):
            content = readNumberData(2, off, bytes);
            return T.CreateTruncating(BitConverter.ToUInt16(content));
         case Type tChar when tChar == typeof(char):
            content = readNumberData(2, off, bytes);
            return T.CreateTruncating(BitConverter.ToChar(content));
         case Type tFloat when tFloat == typeof(float):
            content = readNumberData(4, off, bytes);
            return T.CreateTruncating(BitConverter.ToSingle(content));
         case Type tInt when tInt == typeof(int):
            content = readNumberData(4, off, bytes);
            return T.CreateTruncating(BitConverter.ToInt32(content));
         case Type tUint when tUint == typeof(uint):
            content = readNumberData(4, off, bytes);
            return T.CreateTruncating(BitConverter.ToUInt32(content));
         case Type tDouble when tDouble == typeof(double):
            content = readNumberData(8, off, bytes);
            return T.CreateTruncating(BitConverter.ToDouble(content));
         case Type tLong when tLong == typeof(long):
            content = readNumberData(8, off, bytes);
            return T.CreateTruncating(BitConverter.ToInt64(content));
         case Type tUlong when tUlong == typeof(ulong):
            content = readNumberData(8, off, bytes);
            return T.CreateTruncating(BitConverter.ToUInt64(content));
         case Type tDecimal when tDecimal == typeof(decimal):
         {
            content = readNumberData(16, off, bytes);
            int i1 = BitConverter.ToInt32(content, 0);
            int i2 = BitConverter.ToInt32(content, 4);
            int i3 = BitConverter.ToInt32(content, 8);
            int i4 = BitConverter.ToInt32(content, 12);

            decimal result = new(new[] { i1, i2, i3, i4 });

            return T.CreateTruncating(result);
         }
         default:
            _logger.LogError($"Number type {type} is not supported!");
            break;
      }

      return default;
   }

   /// <summary>
   /// Converts a byte-array (as JSON) to an object.
   /// </summary>
   /// <param name="bytes">Byte-array</param>
   /// <returns>Object from the byte-array</returns>
   public static T? BNToObject<T>(this byte[]? bytes)
   {
      return JsonHelper.DeserializeFromString<T>(bytes.BNToString(), JsonHelper.FORMAT_NONE);
   }

   /// <summary>
   /// Reverses an object array.
   /// </summary>
   /// <param name="array">Array-instance</param>
   /// <returns>Reversed object array</returns>
   public static void BNReverse(this object[]? array)
   {
      if (array == null)
         return;

      Array.Reverse(array);
   }

   /// <summary>
   /// Reverses a Number type array.
   /// </summary>
   /// <param name="array">Number type array-instance</param>
   /// <returns>Reversed Number type array</returns>
   public static void BNReverse<T>(this T[]? array) where T : INumber<T>
   {
      if (array == null)
         return;

      Array.Reverse(array);
   }

   #region Private methods

/*
   private static bool isByteArrayValidForNumber(byte[] bytes, int length, Type type)
   {
      if (bytes.Length < length)
      {
         _logger.LogError($"Byte array is to short for a valid number type {type}!");
         return false;
      }

      return true;
   }
*/

   #endregion
}