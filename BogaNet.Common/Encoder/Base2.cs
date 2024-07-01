using System;
using System.Text;
using System.Numerics;
using BogaNet.Extension;
using BogaNet.Helper;
using Microsoft.Extensions.Logging;

namespace BogaNet.Encoder;

/// <summary>
/// Base2 (aka Binary) encoder class.
/// </summary>
public static class Base2 //NUnit
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(Base2));

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
      {
         _logger.LogWarning("Input was not a multiple of 8 - filling the missing positions with leading zeros.");
         base2string = $"{StringHelper.CreateString(8 - diff, '0')}{base2string}";
      }

      //remove leading zeros
      if (base2string.Length > 8)
      {
         do
         {
            if (base2string.Length % 8 != 0)
            {
               base2string = base2string.Substring(1);
            }
            else
            {
               if (base2string.StartsWith("0000000") && base2string.Length > 8)
               {
                  base2string = base2string.Substring(8);
               }
               else
               {
                  break;
               }
            }
         } while (true);
      }

      int numOfBytes = base2string.Length / 8;

      byte[] bytes = new byte[numOfBytes];

      for (int ii = 0; ii < numOfBytes; ii++)
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
   public static string ToBase2String(params byte[]? bytes)
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

      return ToBase2String(str.BNToByteArray(encoding));
   }

   #endregion
}