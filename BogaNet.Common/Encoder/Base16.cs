using System;
using System.Text;
using System.Numerics;
using BogaNet.Helper;

namespace BogaNet.Encoder;

/// <summary>
/// Base16 (aka Hex) encoder class.
/// </summary>
public static class Base16 //NUnit
{
   #region Public methods

   /// <summary>
   /// Converts a Base16-string to a byte-array.
   /// </summary>
   /// <param name="base16string">Data as Base16-string</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase16String(string? base16string)
   {
      ArgumentNullException.ThrowIfNull(base16string);

      return Convert.FromHexString(base16string.BNStartsWith("0x") ? base16string[2..] : base16string);
   }

   /// <summary>
   /// Converts a byte-array to a Base16-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="addPrefix">Add "0x"-as prefix (optional, default: false)</param>
   /// <returns>Data as encoded Base16-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase16String(byte[]? bytes, bool addPrefix = false)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      return addPrefix ? $"0x{Convert.ToHexString(bytes)}" : Convert.ToHexString(bytes);
   }

   /// <summary>
   /// Converts the value of a Number to a Base16-string.
   /// </summary>
   /// <param name="number">Given value</param>
   /// <param name="addPrefix">Add "0x"-as prefix (optional, default: false)</param>
   /// <param name="useFullLength">Use the full length of the Number type (optional, default: false)</param>
   /// <returns>Number as converted Base16-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase16String<T>(T? number, bool addPrefix = false, bool useFullLength = false) where T : INumber<T>
   {
      ArgumentNullException.ThrowIfNull(number);

      Type type = typeof(T);
      int pairs = 8;

      bool isInteger = true;

      switch (type)
      {
         case Type t when t == typeof(double):
            isInteger = false;
            break;
         case Type t when t == typeof(float):
            pairs = 4;
            isInteger = false;
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
            isInteger = false;
            break;
      }

      //string hex = number.ToString($"x{pairs}", null);
      //float number2 = number.BNTO;

      string hex;

      hex = isInteger ? $"{number:X}" : ToBase16String(number.BNToByteArray());

      string res = useFullLength ? StringHelper.CreateFixedLengthString(hex, 2 * pairs, '0', false) : hex;

      return addPrefix ? $"0x{res}" : res;
   }

   /// <summary>
   /// Converts the value of a string to a Base16-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="addPrefix">Add "0x"-as prefix (optional, default: false)</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Base16-string</returns>
   public static string? ToBase16String(string? str, bool addPrefix = false, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return ToBase16String(_encoding.GetBytes(str), addPrefix);
   }

   /// <summary>
   /// Converts the value of a Base16-string to a string.
   /// </summary>
   /// <param name="base16string">String as Base16-string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Base16-string value as converted string</returns>
   public static string? StringFromBase16String(string? base16string, Encoding? encoding = null)
   {
      if (base16string == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      byte[]? base16 = FromBase16String(base16string);
      return base16 == null ? null : _encoding.GetString(base16);
   }

   /// <summary>
   /// Converts value of a Base16-string to a Number.
   /// </summary>
   /// <param name="base16string">Number as Base16-string</param>
   /// <returns>Base16-string value as converted number</returns>
   public static T? NumberFromBase16String<T>(string? base16string) where T : INumber<T>
   {
      if (base16string == null)
         return default;

      Type type = typeof(T);

      bool isInteger = true;

      switch (type)
      {
         case Type t when t == typeof(double):
            isInteger = false;
            break;
         case Type t when t == typeof(float):
            isInteger = false;
            break;
         case Type t when t == typeof(decimal):
            isInteger = false;
            break;
      }

      if (isInteger)
         return T.Parse(base16string.BNStartsWith("0x") ? base16string[2..] : base16string, System.Globalization.NumberStyles.HexNumber, null);

      string hexVal = base16string.BNStartsWith("0x") ? base16string[2..] : base16string;
      byte[] data = new byte[hexVal.Length / 2];

      for (int ii = 0; ii < data.Length; ++ii)
      {
         data[ii] = Convert.ToByte(hexVal.Substring(ii * 2, 2), 16);
      }

      return data.BNToNumber<T>();
   }

   #endregion
}