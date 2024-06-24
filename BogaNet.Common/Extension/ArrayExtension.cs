using System.Text;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System;

namespace BogaNet;

/// <summary>
/// Extension methods for arrays.
/// </summary>
public static class ArrayExtension
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
   /// <param name="length">Length of the string</param>
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
   //public unsafe static T BNToNumber<T>(this byte[]? bytes, int offset = 0) where T : INumber<T>
   {
      if (bytes == null)
         return default;

      Type type = typeof(T);
      byte[] content;

      if (type is Type t1 && t1 == typeof(double))
      {
         content = new byte[8];
         Buffer.BlockCopy(bytes, offset, content, 0, 8);
         return T.CreateTruncating(BitConverter.ToDouble(content, 0));
      }
      else if (type is Type t2 && t2 == typeof(float))
      {
         content = new byte[4];
         Buffer.BlockCopy(bytes, offset, content, 0, 4);
         return T.CreateTruncating(BitConverter.ToSingle(content, 0));
      }
      else if (type is Type t3 && t3 == typeof(long))
      {
         content = new byte[8];
         Buffer.BlockCopy(bytes, offset, content, 0, 8);
         return T.CreateTruncating(BitConverter.ToInt64(content, 0));
      }
      else if (type is Type t4 && t4 == typeof(ulong))
      {
         content = new byte[8];
         Buffer.BlockCopy(bytes, offset, content, 0, 8);
         return T.CreateTruncating(BitConverter.ToUInt64(content, 0));
      }
      else if (type is Type t5 && t5 == typeof(int))
      {
         content = new byte[4];
         Buffer.BlockCopy(bytes, offset, content, 0, 4);
         return T.CreateTruncating(BitConverter.ToInt32(content, 0));
      }
      else if (type is Type t6 && t6 == typeof(uint))
      {
         content = new byte[4];
         Buffer.BlockCopy(bytes, offset, content, 0, 4);
         return T.CreateTruncating(BitConverter.ToUInt32(content, 0));
      }
      else if (type is Type t7 && t7 == typeof(short))
      {
         content = new byte[2];
         Buffer.BlockCopy(bytes, offset, content, 0, 2);
         return T.CreateTruncating(BitConverter.ToInt16(content, 0));
      }
      else if (type is Type t8 && t8 == typeof(ushort))
      {
         content = new byte[2];
         Buffer.BlockCopy(bytes, offset, content, 0, 2);
         return T.CreateTruncating(BitConverter.ToUInt16(content, 0));
      }
      else if (type is Type t9 && t9 == typeof(char))
      {
         content = new byte[2];
         Buffer.BlockCopy(bytes, offset, content, 0, 2);
         return T.CreateTruncating(BitConverter.ToChar(content, 0));
      }
      //TODO needs unsafe...
      /*
         case Type t when t == typeof(nint):
            int sizeInt = sizeof(nint);
            content = new byte[sizeInt];
            Buffer.BlockCopy(bytes, offset, content, 0, sizeInt);
            return T.CreateTruncating(BitConverter.ToUInt16(content, 0));
         case Type t when t == typeof(nuint):
            int size = sizeof(nuint);
            content = new byte[size];
            Buffer.BlockCopy(bytes, offset, content, 0, size);
            return T.CreateTruncating(BitConverter.ToUInt16(content, 0));
         */
      else if (type is Type t10 && t10 == typeof(byte))
         return T.CreateTruncating(bytes[offset]);
      else if (type is Type t11 && t11 == typeof(sbyte))
         return T.CreateTruncating(bytes[offset]);
      else if (type is Type t12 && t12 == typeof(decimal))
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
      else
         _logger.LogWarning("Number type is not supported!");

      return default;
   }
}