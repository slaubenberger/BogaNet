using System;
using System.Text;
using BogaNet.Extension;

namespace BogaNet.Encoder;

/// <summary>
/// Base64 encoder class.
/// </summary>
public static class Base64 //NUnit
{
   #region Public methods

   /// <summary>
   /// Converts a Base64-string to a byte-array.
   /// </summary>
   /// <param name="base64string">Data as Base64-string</param>
   /// <param name="useSave">Use non-standard, but safe version of Base64 for URLs and files</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase64String(string? base64string, bool useSave = false)
   {
      ArgumentNullException.ThrowIfNull(base64string);

      return Convert.FromBase64String(useSave ? base64string.Replace("_", "/").Replace("-", "+") : base64string);
   }

   /// <summary>
   /// Converts a byte-array to a Base64-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="useSave">Use non-standard, but safe version of Base64 for URLs and files</param>
   /// <returns>Data as encoded Base64-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase64String(byte[]? bytes, bool useSave = false)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      //bytes.BNReverse();
      return useSave ? Convert.ToBase64String(bytes).Replace("/", "_").Replace("+", "-") : Convert.ToBase64String(bytes);
   }

   /// <summary>
   /// Converts the value of a string to a Base64-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <param name="useSave">Use non-standard, but safe version of Base64 for URLs and files</param>
   /// <returns>String value as converted Base64-string</returns>
   public static string? ToBase64String(string? str, bool useSave = false, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      byte[]? bytes = str.BNToByteArray(encoding);
      //bytes.BNReverse();
      return ToBase64String(bytes, useSave);
   }

   #endregion
}