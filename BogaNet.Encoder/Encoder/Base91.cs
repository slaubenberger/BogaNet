using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BogaNet.Extension;
using System.Threading.Tasks;
using BogaNet.Helper;

namespace BogaNet.Encoder;

/// <summary>
/// Base91 encoder class.
/// Partially based on: https://github.com/KvanTTT/BaseNcoding
/// </summary>
public static class Base91
{
   #region Variables

   private const string CHARSET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!#$%&()*+,./:;<=>?@[]^_`{|}~\"";
   private static readonly int[] _inverseCharset;

   #endregion

   #region Static block

   static Base91()
   {
      _inverseCharset = new int[CHARSET.Max() + 1];

      for (int ii = 0; ii < _inverseCharset.Length; ii++)
         _inverseCharset[ii] = -1;

      for (int ii = 0; ii < 91; ii++)
         _inverseCharset[CHARSET[ii]] = ii;
   }

   #endregion

   #region Public methods

   /// <summary>
   /// Converts a Base91-string to a byte-array.
   /// </summary>
   /// <param name="base91string">Data as Base91-string</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase91String(string base91string)
   {
      ArgumentException.ThrowIfNullOrEmpty(base91string);

      return decode(base91string);
   }

   /// <summary>
   /// Converts a byte-array to a Base91-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>Data as encoded Base91-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase91String(byte[] bytes)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      return encode(bytes);
   }

   /// <summary>
   /// Converts the value of a string to a Base91-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Base91-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase91String(string str, Encoding? encoding = null)
   {
      ArgumentException.ThrowIfNullOrEmpty(str);

      byte[] bytes = str.BNToByteArray(encoding);
      //bytes.BNReverse();
      return ToBase91String(bytes);
   }

   /// <summary>
   /// Converts a file to a Base91-string.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base91-string</returns>
   /// <exception cref="Exception"></exception>
   public static string Base91FromFile(string file)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return ToBase91String(FileHelper.ReadAllBytes(file));
   }

   /// <summary>
   /// Converts a file to a Base91-string asynchronously.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base91-string</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string> Base91FromFileAsync(string file)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return ToBase91String(await FileHelper.ReadAllBytesAsync(file));
   }

   /// <summary>
   /// Converts a Base91-string to a file.
   /// </summary>
   /// <param name="file">File to write the content of the Base91-string</param>
   /// <param name="base91string">Data as Base91-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool FileFromBase91(string file, string base91string)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return FileHelper.WriteAllBytes(file, FromBase91String(base91string));
   }

   /// <summary>
   /// Converts a Base91-string to a file asynchronously.
   /// </summary>
   /// <param name="file">File to write the content of the Base91-string</param>
   /// <param name="base91string">Data as Base91-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> FileFromBase91Async(string file, string base91string)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return await FileHelper.WriteAllBytesAsync(file, FromBase91String(base91string));
   }

   #endregion

   #region Private methods

   private static string encode(byte[] data)
   {
      StringBuilder result = new(data.Length);

      int bitQuotient = 0;
      int bitIndex = 0;

      foreach (byte theByte in data)
      {
         bitQuotient |= (theByte & 255) << bitIndex;
         bitIndex += 8;

         if (bitIndex > 13)
         {
            int encodedValue = bitQuotient & 8191;

            if (encodedValue > 88)
            {
               bitQuotient >>= 13;
               bitIndex -= 13;
            }
            else
            {
               encodedValue = bitQuotient & 16383;
               bitQuotient >>= 14;
               bitIndex -= 14;
            }

            int quotient = Math.DivRem(encodedValue, 91, out int remainder);
            result.Append(CHARSET[remainder]);
            result.Append(CHARSET[quotient]);
         }
      }

      if (bitIndex > 0)
      {
         int quotient = Math.DivRem(bitQuotient, 91, out int remainder);
         result.Append(CHARSET[remainder]);

         if (bitIndex > 7 || bitQuotient > 90)
            result.Append(CHARSET[quotient]);
      }

      return result.ToString();
   }

   private static byte[] decode(string data)
   {
      unchecked
      {
         int bitQuotient = 0;
         int bitIndex = 0;
         int decodedValue = -1;

         List<byte> result = new(data.Length);

         foreach (var theByte in data.Where(theByte => _inverseCharset[theByte] != -1))
         {
            if (decodedValue == -1)
            {
               decodedValue = _inverseCharset[theByte];
            }
            else
            {
               decodedValue += _inverseCharset[theByte] * 91;
               bitQuotient |= decodedValue << bitIndex;
               bitIndex += (decodedValue & 8191) > 88 ? 13 : 14;

               do
               {
                  result.Add((byte)bitQuotient);
                  bitQuotient >>= 8;
                  bitIndex -= 8;
               } while (bitIndex > 7);

               decodedValue = -1;
            }
         }

         if (decodedValue != -1)
            result.Add((byte)(bitQuotient | (decodedValue << bitIndex)));

         return result.ToArray();
      }
   }

   #endregion
}