using System;
using Enumerable = System.Linq.Enumerable;
using System.Text;

namespace BogaNet.Encoder;

/// <summary>
/// Base64 encoder class.
/// </summary>
public static class Base64
{
   #region Public methods

   /// <summary>
   /// Converts a Base64-string to a byte-array.
   /// </summary>
   /// <param name="base64string">Data as Base64-string</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase64String(string base64string)
   {
      if (string.IsNullOrEmpty(base64string))
         throw new ArgumentNullException(nameof(base64string));

      return Convert.FromBase64String(base64string);
   }

   /// <summary>
   /// Converts a byte-array to a Base64-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>Data as encoded Base64-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase64String(byte[] bytes)
   {
      if (bytes == null || bytes.Length == 0)
         throw new ArgumentNullException(nameof(bytes));

      return Convert.ToBase64String(bytes);
   }

   /// <summary>
   /// Converts the value of a string to a Base64-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Base64-string</returns>
   public static string? StringToBase64String(string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return ToBase64String(_encoding.GetBytes(str));
   }

   /// <summary>
   /// Converts the value of a Base64-string to a string.
   /// </summary>
   /// <param name="str">Input Base64-string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Base64-string value as converted string</returns>
   public static string? StringFromBase64String(this string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      byte[]? base32 = FromBase64String(str);
      return base32 == null ? null : _encoding.GetString(base32);
   }

   #endregion
}