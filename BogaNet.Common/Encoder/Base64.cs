using System;
using System.Text;
using System.Threading.Tasks;
using BogaNet.Extension;
using BogaNet.Helper;

namespace BogaNet.Encoder;

/// <summary>
/// Base64 encoder class.
/// </summary>
public static class Base64 //NUnit
{
   /// <summary>
   /// Use non-standard, but safe format of Base64 suitable for URLs and files (default: false).
   /// </summary>
   public static bool UseSaveFormat = false;

   #region Public methods

   /// <summary>
   /// Converts a Base64-string to a byte-array.
   /// </summary>
   /// <param name="base64string">Data as Base64-string</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase64String(string base64string)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(base64string);

      return Convert.FromBase64String(UseSaveFormat ? base64string.Replace("_", "/").Replace("-", "+") : base64string);
   }

   /// <summary>
   /// Converts a byte-array to a Base64-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>Data as encoded Base64-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase64String(byte[] bytes)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      return UseSaveFormat
         ? Convert.ToBase64String(bytes).Replace("/", "_").Replace("+", "-")
         : Convert.ToBase64String(bytes);
   }

   /// <summary>
   /// Converts the value of a string to a Base64-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Base64-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase64String(string str, Encoding? encoding = null)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(str);

      byte[] bytes = str.BNToByteArray(encoding);
      //bytes.BNReverse();
      return ToBase64String(bytes);
   }

   /// <summary>
   /// Converts a file to a Base64-string.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base64-string</returns>
   /// <exception cref="Exception"></exception>
   public static string Base64FromFile(string file)
   {
      return Task.Run(() => Base64FromFileAsync(file)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Converts a file to a Base64-string asynchronously.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base64-string</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string> Base64FromFileAsync(string file)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return ToBase64String(await FileHelper.ReadAllBytesAsync(file));
   }

   /// <summary>
   /// Converts a Base64-string to a file.
   /// </summary>
   /// <param name="file">File to write the content of the Base64-string</param>
   /// <param name="base64string">Data as Base64-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool FileFromBase64(string file, string base64string)
   {
      return Task.Run(() => FileFromBase64Async(file, base64string)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Converts a Base64-string to a file asynchronously.
   /// </summary>
   /// <param name="file">File to write the content of the Base64-string</param>
   /// <param name="base64string">Data as Base64-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> FileFromBase64Async(string file, string base64string)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return await FileHelper.WriteAllBytesAsync(file, FromBase64String(base64string));
   }

   #endregion
}