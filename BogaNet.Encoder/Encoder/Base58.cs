using System;
using System.Collections.Generic;
using System.Text;
using BogaNet.Extension;
using System.Threading.Tasks;
using BogaNet.Helper;
using System.Numerics;

namespace BogaNet.Encoder;

/// <summary>
/// Base58 encoder class.
/// Partially based on: https://github.com/medo64/Medo
/// </summary>
public static class Base58
{
   #region Public methods

   /// <summary>
   /// Converts a Base58-string to a byte-array.
   /// </summary>
   /// <param name="base58string">Data as Base58-string</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase58String(string base58string)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(base58string);

      return Base58_intern.AsBytes(base58string);
   }

   /// <summary>
   /// Converts a byte-array to a Base58-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>Data as encoded Base58-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase58String(byte[] bytes)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      return Base58_intern.ToString(bytes);
   }

   /// <summary>
   /// Converts the value of a string to a Base58-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Base58-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase58String(string str, Encoding? encoding = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(str);

      byte[] bytes = str.BNToByteArray(encoding);
      //bytes.BNReverse();
      return ToBase58String(bytes);
   }

   /// <summary>
   /// Converts a file to a Base58-string.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base58-string</returns>
   /// <exception cref="Exception"></exception>
   public static string Base58FromFile(string file)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return ToBase58String(FileHelper.ReadAllBytes(file));
   }

   /// <summary>
   /// Converts a file to a Base58-string asynchronously.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base58-string</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string> Base58FromFileAsync(string file)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return ToBase58String(await FileHelper.ReadAllBytesAsync(file));
   }

   /// <summary>
   /// Converts a Base58-string to a file.
   /// </summary>
   /// <param name="file">File to write the content of the Base58-string</param>
   /// <param name="base58string">Data as Base58-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool FileFromBase58(string file, string base58string)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return FileHelper.WriteAllBytes(file, FromBase58String(base58string));
   }

   /// <summary>
   /// Converts a Base58-string to a file asynchronously.
   /// </summary>
   /// <param name="file">File to write the content of the Base58-string</param>
   /// <param name="base58string">Data as Base58-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> FileFromBase58Async(string file, string base58string)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return await FileHelper.WriteAllBytesAsync(file, FromBase58String(base58string));
   }

   #endregion

   #region Inner classes

   /// <summary>
   /// Base58 encoder/decoder with leading-zero preservation.
   /// </summary>
   /// <example>
   /// <code>
   /// var inString = "1LQX";
   /// var outBytes = Base58.AsBytes(inString);
   /// var outString = Base58.ToString(outBytes);
   /// </code>
   /// </example>
   private static class Base58_intern
   {
      private static readonly char[] Base58Map =
      [
         '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F',
         'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W',
         'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'm',
         'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
      ];

      private static readonly BigInteger Base58Divisor = 58;

      //private static readonly Encoding Utf8 = new UTF8Encoding(false);

      #region ToString

      /// <summary>
      /// Returns Base58 string.
      /// </summary>
      /// <param name="bytes">Bytes to convert.</param>
      /// <exception cref="NullReferenceException">Bytes cannot be null.</exception>
      public static string ToString(byte[] bytes)
      {
         ArgumentNullException.ThrowIfNull(bytes);

         if (TryToString(bytes, out var result))
            return result;

         throw new FormatException("Cannot convert.");
      }

/*
      /// <summary>
      /// Returns Base58 string.
      /// </summary>
      /// <param name="text">UTF-8 text to convert.</param>
      /// <exception cref="NullReferenceException">Text cannot be null.</exception>
      public static string ToString(string text)
      {
         ArgumentNullException.ThrowIfNullOrEmpty(text);

         return ToString(Utf8.GetBytes(text));
      }
*/
      /// <summary>
      /// Returns true of conversion succeeded.
      /// </summary>
      /// <param name="bytes">Bytes to convert.</param>
      /// <param name="result">Conversion result as Base58 string.</param>
      /// <exception cref="NullReferenceException">Bytes cannot be null.</exception>
      public static bool TryToString(byte[] bytes, out string result)
      {
         ArgumentNullException.ThrowIfNull(bytes);

         if (bytes.Length == 0)
         {
            //don't bother if there is nothing in array
            result = "";
            return true;
         }

         var input = new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
         var remainders = new List<int>();

         while (input > 0)
         {
            input = BigInteger.DivRem(input, Base58Divisor, out var remainder);
            remainders.Add((int)remainder);
         }

         //preserve leading zeros
         foreach (var b in bytes)
         {
            if (b == 0)
            {
               remainders.Add(0);
            }
            else
            {
               break;
            }
         }

         remainders.Reverse();

         var sbOutput = new StringBuilder();

         foreach (var remainder in remainders)
         {
            sbOutput.Append(Base58Map[remainder]);
         }

         result = sbOutput.ToString();

         return true;
      }

/*
      /// <summary>
      /// Returns true of conversion succeeded.
      /// </summary>
      /// <param name="text">Text to convert.</param>
      /// <param name="result">Conversion result as Base58 string.</param>
      /// <exception cref="NullReferenceException">Text cannot be null.</exception>
      public static bool TryToString(string text, out string result)
      {
         ArgumentNullException.ThrowIfNullOrEmpty(text);

         return TryToString(Utf8.GetBytes(text), out result);
      }
*/

      #endregion ToString


      #region AsBytes

      /// <summary>
      /// Returns bytes based on their Base58 encoding.
      /// </summary>
      /// <param name="base58">Base58 encoded string.</param>
      /// <exception cref="NullReferenceException">Base58 string cannot be null.</exception>
      /// <exception cref="FormatException">Unknown character.</exception>
      public static byte[] AsBytes(string base58)
      {
         ArgumentNullException.ThrowIfNullOrEmpty(base58);

         if (TryAsBytes(base58, out var result))
            return result;

         throw new FormatException("Cannot convert.");
      }

      /// <summary>
      /// Returns true of conversion succeeded.
      /// </summary>
      /// <param name="base58">Base58 encoded string.</param>
      /// <param name="result">Conversion result as bytes.</param>
      /// <exception cref="NullReferenceException">Base58 string cannot be null.</exception>
      /// <exception cref="FormatException">Unknown character.</exception>
      public static bool TryAsBytes(string base58, out byte[] result)
      {
         ArgumentNullException.ThrowIfNullOrEmpty(base58);

         if (string.IsNullOrEmpty(base58))
         {
            result = [];
            return true;
         }

         var inStarting = true;
         var startingZeros = 0;
         var indices = new List<int>();

         foreach (var c in base58)
         {
            var index = Array.IndexOf(Base58Map, c);
            if (index >= 0)
            {
               if (inStarting && (index == 0))
               {
                  startingZeros += 1;
               }
               else
               {
                  inStarting = false;
               }

               indices.Add(index);
            }
            else
            {
               throw new FormatException("Unknown character.");
            }
         }

         var output = new BigInteger();

         foreach (var index in indices)
         {
            output = BigInteger.Multiply(output, Base58Divisor);
            output = BigInteger.Add(output, index);
         }

         var outputBytes = output.ToByteArray(isUnsigned: false, isBigEndian: true);

         try
         {
            var extraZeros = (outputBytes[0] == 0x00) ? startingZeros - 1 : startingZeros;
            var buffer = new byte[outputBytes.Length + extraZeros];

            if (extraZeros >= 0)
            {
               Buffer.BlockCopy(outputBytes, 0, buffer, extraZeros, outputBytes.Length);
            }
            else
            {
               Buffer.BlockCopy(outputBytes, -extraZeros, buffer, 0, buffer.Length);
            }

            result = buffer;
            return true;
         }
         finally
         {
            Array.Clear(outputBytes, 0, outputBytes.Length);
         }
      }

      #endregion AsBytes

      #region AsString

/*
      /// <summary>
      /// Returns UTF-8 string based on it's Base58 encoding.
      /// </summary>
      /// <param name="base58">Base58 encoded string.</param>
      /// <exception cref="NullReferenceException">Base58 string cannot be null.</exception>
      /// <exception cref="FormatException">Unknown character.</exception>
      public static string? AsString(string base58)
      {
         ArgumentNullException.ThrowIfNullOrEmpty(base58);

         if (TryAsString(base58, out var result))
            return result;

         throw new FormatException("Cannot convert.");
      }

      /// <summary>
      /// Returns true of conversion succeeded.
      /// </summary>
      /// <param name="base58">Base58 encoded string.</param>
      /// <param name="result">Conversion result as UTF-8 string.</param>
      /// <exception cref="NullReferenceException">Base58 string cannot be null.</exception>
      /// <exception cref="FormatException">Unknown character.</exception>
      public static bool TryAsString(string base58, out string? result)
      {
         ArgumentNullException.ThrowIfNullOrEmpty(base58);

         if (TryAsBytes(base58, out var bytes))
         {
            result = Utf8.GetString(bytes);
            return true;
         }

         result = null;
         return false;
      }
*/

      #endregion AsString
   }

   #endregion
}