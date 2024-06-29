using System;
using System.Text;
using System.Numerics;
using BogaNet.Helper;
using Microsoft.Extensions.Primitives;

namespace BogaNet.Encoder;

/// <summary>
/// Base2 (aka Binary) encoder class.
/// </summary>
public static class Base2 //NUnit
{
   #region Public methods

   /// <summary>
   /// Converts a Base2-string to a byte-array.
   /// </summary>
   /// <param name="base2string">Data as Base2-string</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase2String(string? base2string)
   {
      ArgumentNullException.ThrowIfNull(base2string);

      int diff = base2string.Length % 8;

      if (diff != 0)
         base2string = StringHelper.CreateString(diff, "0") + base2string;

      int numOfBytes = base2string.Length / 8;

      byte[] bytes = new byte[numOfBytes];

      for (int ii = 0; ii < numOfBytes; ++ii)
      {
         bytes[ii] = Convert.ToByte(base2string.Substring(8 * ii, 8), 2);
      }

      return bytes;
   }

   /// <summary>
   /// Converts a byte-array to a Base2-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>Data as encoded Base2-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase2String(byte[]? bytes)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      StringBuilder sb = new();

      foreach (byte b in bytes)
      {
         sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
      }

      return sb.ToString();
   }

   /// <summary>
   /// Converts the value of a Number to a Base2-string.
   /// </summary>
   /// <param name="number">Given value</param>
   /// <returns>Number as converted Base2-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase2String<T>(T? number) where T : INumber<T>
   {
      ArgumentNullException.ThrowIfNull(number);

      Type type = typeof(T);
      int pairs = 8;

      switch (type)
      {
         case Type t when t == typeof(double):
            break;
         case Type t when t == typeof(float):
            pairs = 4;
            break;
         case Type t when t == typeof(int):
            pairs = 4;
            break;
         case Type t when t == typeof(uint):
            pairs = 4;
            break;
         case Type t when t == typeof(short):
            pairs = 2;
            break;
         case Type t when t == typeof(ushort):
            pairs = 2;
            break;
         case Type t when t == typeof(char):
            pairs = 2;
            break;
         //TODO needs unsafe...
/*
         case Type t when t == typeof(nint):
            length = sizeof(nint);
            break;
         case Type t when t == typeof(nuint):
            length = sizeof(nint);
            break;
*/
         case Type t when t == typeof(byte):
            pairs = 1;
            break;
         case Type t when t == typeof(sbyte):
            pairs = 1;
            break;
         case Type t when t == typeof(decimal):
            pairs = 16;
            break;
      }

      return ToBase2String(number.BNToByteArray());
   }

   /// <summary>
   /// Converts the value of a string to a Base2-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Base2-string</returns>
   public static string? ToBase2String(string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return ToBase2String(_encoding.GetBytes(str));
   }

   /// <summary>
   /// Converts the value of a Base2-string to a string.
   /// </summary>
   /// <param name="base2string">String as Base2-string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Base2-string value as converted string</returns>
   public static string? StringFromBase2String(string? base2string, Encoding? encoding = null)
   {
      if (base2string == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      byte[]? base2 = FromBase2String(base2string);
      return base2 == null ? null : _encoding.GetString(base2);
   }

   /// <summary>
   /// Converts value of a Base2-string to a Number.
   /// </summary>
   /// <param name="base2string">Number as Base2-string</param>
   /// <returns>Base2-string value as converted number</returns>
   public static T? NumberFromBase2String<T>(string? base2string) where T : INumber<T>
   {
      if (base2string == null)
         return default;

      return FromBase2String(base2string).BNToNumber<T>();
   }

   #endregion
}