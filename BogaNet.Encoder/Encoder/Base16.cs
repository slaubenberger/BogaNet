using System;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using BogaNet.Extension;
using BogaNet.Helper;
using Microsoft.Extensions.Logging;

namespace BogaNet.Encoder;

/// <summary>
/// Base16 (aka Hex) encoder class.
/// </summary>
public static class Base16 //NUnit
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(Base16));

   #region Public methods

   /// <summary>
   /// Converts a Base16-string to a byte-array.
   /// </summary>
   /// <param name="base16string">Data as Base16-string</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase16String(string base16string)
   {
      ArgumentException.ThrowIfNullOrEmpty(base16string);

      int diff = base16string.Length % 2;

      if (diff != 0)
      {
         _logger.LogWarning("Input was not a multiple of 2 - filling the missing position with a leading zero.");
         base16string = $"0{base16string}";
      }

      string hexVal = base16string.BNStartsWith("0x") ? base16string[2..] : base16string;

      //remove leading zeros
      if (hexVal.Length <= 2) return Convert.FromHexString(hexVal);
      
      do
      {
         if (hexVal.BNStartsWith("00") && hexVal.Length > 2)
         {
            hexVal = hexVal[2..];
         }
         else
         {
            break;
         }
      } while (true);

      return Convert.FromHexString(hexVal);
   }

   /// <summary>
   /// Converts a byte-array to a Base16-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="addPrefix">Add "0x"-as prefix (optional, default: false)</param>
   /// <returns>Data as encoded Base16-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase16String(byte[] bytes, bool addPrefix = false)
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
   public static string ToBase16String<T>(T number, bool addPrefix = false, bool useFullLength = false) where T : INumber<T>
   {
      ArgumentNullException.ThrowIfNull(number);

      Type type = typeof(T);
      int pairs = 8;

      switch (type)
      {
         case not null when type == typeof(byte):
            pairs = 1;
            break;
         case not null when type == typeof(sbyte):
            pairs = 1;
            break;
         case not null when type == typeof(short):
            pairs = 2;
            break;
         case not null when type == typeof(ushort):
            pairs = 2;
            break;
         case not null when type == typeof(char):
            pairs = 2;
            break;
         case not null when type == typeof(float):
            pairs = 4;
            break;
         case not null when type == typeof(int):
            pairs = 4;
            break;
         case not null when type == typeof(uint):
            pairs = 4;
            break;
         case not null when type == typeof(double):
            pairs = 8;
            break;
         case not null when type == typeof(long):
            pairs = 8;
            break;
         case not null when type == typeof(ulong):
            pairs = 8;
            break;
         //needs unsafe...
/*
         case Type t when t == typeof(nint):
            length = sizeof(nint);
            break;
         case Type t when t == typeof(nuint):
            length = sizeof(nint);
            break;
*/
         case not null when type == typeof(decimal):
            pairs = 16;
            break;
         default:
            _logger.LogWarning($"Number type {type} is not supported!");
            break;
      }

      byte[] bytes = number.BNToByteArray();
      string hex = ToBase16String(bytes);
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
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase16String(string str, bool addPrefix = false, Encoding? encoding = null)
   {
      return ToBase16String(str.BNToByteArray(encoding), addPrefix);
   }

   /// <summary>
   /// Converts a file to a Base16-string.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base16-string</returns>
   /// <exception cref="Exception"></exception>
   public static string Base16FromFile(string file)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return ToBase16String(FileHelper.ReadAllBytes(file));
   }

   /// <summary>
   /// Converts a file to a Base16-string asynchronously.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base16-string</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string> Base16FromFileAsync(string file)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return ToBase16String(await FileHelper.ReadAllBytesAsync(file));
   }

   /// <summary>
   /// Converts a Base16-string to a file.
   /// </summary>
   /// <param name="file">File to write the content of the Base16-string</param>
   /// <param name="base16string">Data as Base16-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool FileFromBase16(string file, string base16string)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return FileHelper.WriteAllBytes(file, FromBase16String(base16string));
   }

   /// <summary>
   /// Converts a Base16-string to a file asynchronously.
   /// </summary>
   /// <param name="file">File to write the content of the Base16-string</param>
   /// <param name="base16string">Data as Base16-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> FileFromBase16Async(string file, string base16string)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return await FileHelper.WriteAllBytesAsync(file, FromBase16String(base16string));
   }

   #endregion
}