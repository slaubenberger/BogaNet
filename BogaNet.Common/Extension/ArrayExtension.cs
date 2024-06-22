using System.Text;
using Microsoft.Extensions.Logging;
using System.Numerics;
using System;
using System.Linq;
using BogaNet.Crypto.ObfuscatedType;

namespace BogaNet;

/// <summary>
/// Extension methods for arrays.
/// </summary>
public static class ArrayExtension
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ArrayExtension));

   /// <summary>
   /// Converts a byte-array to a BNbyte-array.
   /// </summary>
   /// <param name="array">Array-instance to convert</param>
   /// <returns>Converted BNbyte-array</returns>
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
   /// Converts a BNbyte-array to a byte-array.
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

      switch (type)
      {
         case Type t when t == typeof(double):
            content = new byte[8];
            Buffer.BlockCopy(bytes, offset, content, 0, 8);
            return T.CreateTruncating(BitConverter.ToDouble(content, 0));
         case Type t when t == typeof(float):
            content = new byte[4];
            Buffer.BlockCopy(bytes, offset, content, 0, 4);
            return T.CreateTruncating(BitConverter.ToSingle(content, 0));
         case Type t when t == typeof(long):
            content = new byte[8];
            Buffer.BlockCopy(bytes, offset, content, 0, 8);
            return T.CreateTruncating(BitConverter.ToInt64(content, 0));
         case Type t when t == typeof(ulong):
            content = new byte[8];
            Buffer.BlockCopy(bytes, offset, content, 0, 8);
            return T.CreateTruncating(BitConverter.ToUInt64(content, 0));
         case Type t when t == typeof(int):
            content = new byte[4];
            Buffer.BlockCopy(bytes, offset, content, 0, 4);
            return T.CreateTruncating(BitConverter.ToInt32(content, 0));
         case Type t when t == typeof(uint):
            content = new byte[4];
            Buffer.BlockCopy(bytes, offset, content, 0, 4);
            return T.CreateTruncating(BitConverter.ToUInt32(content, 0));
         case Type t when t == typeof(short):
            content = new byte[2];
            Buffer.BlockCopy(bytes, offset, content, 0, 2);
            return T.CreateTruncating(BitConverter.ToInt16(content, 0));
         case Type t when t == typeof(ushort):
            content = new byte[2];
            Buffer.BlockCopy(bytes, offset, content, 0, 2);
            return T.CreateTruncating(BitConverter.ToUInt16(content, 0));
         case Type t when t == typeof(char):
            content = new byte[2];
            Buffer.BlockCopy(bytes, offset, content, 0, 2);
            return T.CreateTruncating(BitConverter.ToChar(content, 0));
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
         case Type t when t == typeof(byte):
            return T.CreateTruncating(bytes[offset]);
         case Type t when t == typeof(sbyte):
            return T.CreateTruncating(bytes[offset]);
         default:
            _logger.LogWarning("Number type is not supported!");
            break;
      }

      return default;
   }

   /// <summary>
   /// Converts a byte-array to a Hex-string.
   /// </summary>
   /// <param name="data">Input as byte-array</param>
   /// <returns>Hex-string from the byte-array</returns>
   public static string? BNToHex(this byte[]? data)
   {
      string hex = string.Empty;
      return data == null ? null : data.Aggregate(hex, (current, bit) => current + bit.ToString("x2"));
   }
}