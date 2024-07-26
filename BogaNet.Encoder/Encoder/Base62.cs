using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BogaNet.Extension;
using System.Threading.Tasks;
using BogaNet.Helper;

namespace BogaNet.Encoder;

/// <summary>
/// Base62 encoder class.
/// Partially based on: https://github.com/JoyMoe/Base62.Net
/// </summary>
public static class Base62
{
   #region Public methods

   /// <summary>
   /// Converts a Base62-string to a byte-array.
   /// </summary>
   /// <param name="base62string">Data as Base62-string</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase62String(string base62string)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(base62string);

      return Base62_intern.FromBase62(base62string);
   }

   /// <summary>
   /// Converts a byte-array to a Base62-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>Data as encoded Base62-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase62String(byte[] bytes)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      return Base62_intern.ToBase62(bytes);
   }

   /// <summary>
   /// Converts the value of a string to a Base62-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Base62-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase62String(string str, Encoding? encoding = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(str);

      byte[] bytes = str.BNToByteArray(encoding);
      //bytes.BNReverse();
      return ToBase62String(bytes);
   }

   /// <summary>
   /// Converts a file to a Base62-string.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base62-string</returns>
   /// <exception cref="Exception"></exception>
   public static string Base62FromFile(string file)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return ToBase62String(FileHelper.ReadAllBytes(file));
   }

   /// <summary>
   /// Converts a file to a Base62-string asynchronously.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base62-string</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string> Base62FromFileAsync(string file)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return ToBase62String(await FileHelper.ReadAllBytesAsync(file));
   }

   /// <summary>
   /// Converts a Base62-string to a file.
   /// </summary>
   /// <param name="file">File to write the content of the Base62-string</param>
   /// <param name="base62string">Data as Base62-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool FileFromBase62(string file, string base62string)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return FileHelper.WriteAllBytes(file, FromBase62String(base62string));
   }

   /// <summary>
   /// Converts a Base62-string to a file asynchronously.
   /// </summary>
   /// <param name="file">File to write the content of the Base62-string</param>
   /// <param name="base62string">Data as Base62-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> FileFromBase62Async(string file, string base62string)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return await FileHelper.WriteAllBytesAsync(file, FromBase62String(base62string));
   }

   #endregion

   #region Inner classes

   /// <summary>
   /// Encoder based on: https://github.com/JoyMoe/Base62.Net/blob/dev/src/Base62/EncodingExtensions.cs
   /// </summary>
   private static class Base62_intern
   {
      private const string DefaultCharacterSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

      private const string InvertedCharacterSet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

/*
        /// <summary>
        /// Encode a 2-byte number with Base62
        /// </summary>
        /// <param name="original">String</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this short original, bool inverted = false)
        {
            var array = BitConverter.GetBytes(original);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }

            return array.ToBase62(inverted);
        }

        /// <summary>
        /// Encode a 4-byte number with Base62
        /// </summary>
        /// <param name="original">String</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this int original, bool inverted = false)
        {
            var array = BitConverter.GetBytes(original);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }

            return array.ToBase62(inverted);
        }

        /// <summary>
        /// Encode a 8-byte number with Base62
        /// </summary>
        /// <param name="original">String</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this long original, bool inverted = false)
        {
            var array = BitConverter.GetBytes(original);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }

            return array.ToBase62(inverted);
        }

        /// <summary>
        /// Encode a string with Base62
        /// </summary>
        /// <param name="original">String</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this string original, bool inverted = false)
        {
            return Encoding.UTF8.GetBytes(original).ToBase62(inverted);
        }
*/
      /// <summary>
      /// Encode a byte array with Base62
      /// </summary>
      /// <param name="original">Byte array</param>
      /// <param name="inverted">Use inverted character set</param>
      /// <returns>Base62 string</returns>
      public static string ToBase62(byte[] original, bool inverted = false)
      {
         var characterSet = inverted ? InvertedCharacterSet : DefaultCharacterSet;
         var arr = Array.ConvertAll(original, t => (int)t);

         var converted = BaseConvert(arr, 256, 62);
         var builder = new StringBuilder();

         foreach (var t in converted)
         {
            builder.Append(characterSet[t]);
         }

         return builder.ToString();
      }

/*
        /// <summary>
        /// Decode a base62-encoded string
        /// </summary>
        /// <param name="base62">Base62 string</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Byte array</returns>
        public static T FromBase62<T>(this string base62, bool inverted = false)
        {
            var array = base62.FromBase62(inverted);

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.String:
                    return (T)Convert.ChangeType(Encoding.UTF8.GetString(array), typeof(T), CultureInfo.InvariantCulture);
                case TypeCode.Int16:
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(array);
                    }

                    return (T)Convert.ChangeType(BitConverter.ToInt16(array, 0), typeof(T), CultureInfo.InvariantCulture);
                case TypeCode.Int32:
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(array);
                    }

                    return (T)Convert.ChangeType(BitConverter.ToInt32(array, 0), typeof(T), CultureInfo.InvariantCulture);
                case TypeCode.Int64:
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(array);
                    }

                    return (T)Convert.ChangeType(BitConverter.ToInt64(array, 0), typeof(T), CultureInfo.InvariantCulture);
                default:
                    throw new Exception($"Type of {typeof(T)} does not support.");
            }
        }
*/
      /// <summary>
      /// Decode a base62-encoded string
      /// </summary>
      /// <param name="base62">Base62 string</param>
      /// <param name="inverted">Use inverted character set</param>
      /// <returns>Byte array</returns>
      public static byte[] FromBase62(string base62, bool inverted = false)
      {
         ArgumentNullException.ThrowIfNullOrEmpty(base62);

         var characterSet = inverted ? InvertedCharacterSet : DefaultCharacterSet;
         var arr = Array.ConvertAll(base62.ToCharArray(), characterSet.IndexOf);

         var converted = BaseConvert(arr, 62, 256);
         return Array.ConvertAll(converted, Convert.ToByte);
      }

      private static int[] BaseConvert(int[] source, int sourceBase, int targetBase)
      {
         var result = new List<int>();
         var leadingZeroCount = Math.Min(source.TakeWhile(x => x == 0).Count(), source.Length - 1);
         int count;

         while ((count = source.Length) > 0)
         {
            var quotient = new List<int>();
            var remainder = 0;

            for (var i = 0; i != count; i++)
            {
               var accumulator = source[i] + remainder * sourceBase;
               var digit = accumulator / targetBase;
               remainder = accumulator % targetBase;

               if (quotient.Count > 0 || digit > 0)
                  quotient.Add(digit);
            }

            result.Insert(0, remainder);
            source = quotient.ToArray();
         }

         result.InsertRange(0, Enumerable.Repeat(0, leadingZeroCount));
         return result.ToArray();
      }
   }

   #endregion
}