﻿using System.Text;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System;

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

   /// <summary>
   /// Converts a byte-array to a string.
   /// </summary>
   /// <param name="bytes">Byte-array</param>
   /// <param name="offset">Offset inside the byte-array</param>
   /// <param name="length">Number of bytes</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String from the byte-array</returns>
   public static string? BNToString(this byte[]? bytes, int offset, int length, Encoding? encoding = null)
   {
      if (bytes == null)
         return null;

      byte[] content = new byte[length];
      Buffer.BlockCopy(bytes, offset, content, 0, length);
      string? res = content.BNToString(encoding);
      return res?.Trim('\0');
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

      switch (type)
      {
         case Type tByte when tByte == typeof(byte):
            return T.CreateTruncating(bytes[offset]);
         case Type tSbyte when tSbyte == typeof(sbyte):
            //return T.CreateTruncating(BogaNet.Helper.ArrayHelper.ByteArrayToSByteArray(bytes)[offset]);
            return T.CreateTruncating(bytes[offset]);
         case Type tShort when tShort == typeof(short) && isByteArrayValidForNumber(bytes, 2, type):
            content = new byte[2];
            Buffer.BlockCopy(bytes, offset, content, 0, 2);
            return T.CreateTruncating(BitConverter.ToInt16(content, 0));
         case Type tUshort when tUshort == typeof(ushort) && isByteArrayValidForNumber(bytes, 2, type):
            content = new byte[2];
            Buffer.BlockCopy(bytes, offset, content, 0, 2);
            return T.CreateTruncating(BitConverter.ToUInt16(content, 0));
         case Type tChar when tChar == typeof(char) && isByteArrayValidForNumber(bytes, 2, type):
            content = new byte[2];
            Buffer.BlockCopy(bytes, offset, content, 0, 2);
            return T.CreateTruncating(BitConverter.ToChar(content, 0));
         case Type tFloat when tFloat == typeof(float) && isByteArrayValidForNumber(bytes, 4, type):
            content = new byte[4];
            Buffer.BlockCopy(bytes, offset, content, 0, 4);
            return T.CreateTruncating(BitConverter.ToSingle(content, 0));
         case Type tInt when tInt == typeof(int) && isByteArrayValidForNumber(bytes, 4, type):
            content = new byte[4];
            Buffer.BlockCopy(bytes, offset, content, 0, 4);
            return T.CreateTruncating(BitConverter.ToInt32(content, 0));
         case Type tUint when tUint == typeof(uint) && isByteArrayValidForNumber(bytes, 4, type):
            content = new byte[4];
            Buffer.BlockCopy(bytes, offset, content, 0, 4);
            return T.CreateTruncating(BitConverter.ToUInt32(content, 0));
         case Type tDouble when tDouble == typeof(double) && isByteArrayValidForNumber(bytes, 8, type):
            content = new byte[8];
            Buffer.BlockCopy(bytes, offset, content, 0, 8);
            return T.CreateTruncating(BitConverter.ToDouble(content, 0));
         case Type tLong when tLong == typeof(long) && isByteArrayValidForNumber(bytes, 8, type):
            content = new byte[8];
            Buffer.BlockCopy(bytes, offset, content, 0, 8);
            return T.CreateTruncating(BitConverter.ToInt64(content, 0));
         case Type tUlong when tUlong == typeof(ulong) && isByteArrayValidForNumber(bytes, 8, type):
            content = new byte[8];
            Buffer.BlockCopy(bytes, offset, content, 0, 8);
            return T.CreateTruncating(BitConverter.ToUInt64(content, 0));
         case Type tDecimal when tDecimal == typeof(decimal) && isByteArrayValidForNumber(bytes, 16, type):
         {
            content = new byte[16];
            Buffer.BlockCopy(bytes, offset, content, 0, 16);
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

   private static bool isByteArrayValidForNumber(byte[] bytes, int length, Type type)
   {
      if (bytes.Length < length)
      {
         _logger.LogError($"Byte array is to short for a valid number type {type}!");
         return false;
      }

      return true;
   }

   #endregion
}