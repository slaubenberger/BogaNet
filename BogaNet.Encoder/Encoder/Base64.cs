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
   #region Public methods

   /// <summary>
   /// Converts a Base64-string to a byte-array.
   /// </summary>
   /// <param name="base64string">Data as Base64-string</param>
   /// <param name="useSaveFormat">Use safe format for Base64, suitable for URLs and files (optional, default: true)</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase64String(string base64string, bool useSaveFormat = true)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(base64string);

      return Convert.FromBase64String(useSaveFormat ? base64string.Replace("_", "/").Replace("-", "+") : base64string);
   }

   /// <summary>
   /// Converts a byte-array to a Base64-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="useSaveFormat">Use safe format for Base64, suitable for URLs and files (optional, default: true)</param>
   /// <returns>Data as encoded Base64-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase64String(byte[] bytes, bool useSaveFormat = true)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      return useSaveFormat
         ? Convert.ToBase64String(bytes).Replace("/", "_").Replace("+", "-")
         : Convert.ToBase64String(bytes);
   }

   /// <summary>
   /// Converts the value of a string to a Base64-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <param name="useSaveFormat">Use safe format for Base64, suitable for URLs and files (optional, default: true)</param>
   /// <returns>String value as converted Base64-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase64String(string str, Encoding? encoding = null, bool useSaveFormat = true)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(str);

      byte[] bytes = str.BNToByteArray(encoding);
      //bytes.BNReverse();
      return ToBase64String(bytes, useSaveFormat);
   }

   /// <summary>
   /// Converts a file to a Base64-string.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <param name="useSaveFormat">Use safe format for Base64, suitable for URLs and files (optional, default: true)</param>
   /// <returns>File content as converted Base64-string</returns>
   /// <exception cref="Exception"></exception>
   public static string Base64FromFile(string file, bool useSaveFormat = true)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return ToBase64String(FileHelper.ReadAllBytes(file), useSaveFormat);
   }

   /// <summary>
   /// Converts a file to a Base64-string asynchronously.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <param name="useSaveFormat">Use safe format for Base64, suitable for URLs and files (optional, default: true)</param>
   /// <returns>File content as converted Base64-string</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string> Base64FromFileAsync(string file, bool useSaveFormat = true)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return ToBase64String(await FileHelper.ReadAllBytesAsync(file), useSaveFormat);
   }

   /// <summary>
   /// Converts a Base64-string to a file.
   /// </summary>
   /// <param name="file">File to write the content of the Base64-string</param>
   /// <param name="base64string">Data as Base64-string</param>
   /// <param name="useSaveFormat">Use safe format for Base64, suitable for URLs and files (optional, default: true)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool FileFromBase64(string file, string base64string, bool useSaveFormat = true)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return FileHelper.WriteAllBytes(file, FromBase64String(base64string, useSaveFormat));
   }

   /// <summary>
   /// Converts a Base64-string to a file asynchronously.
   /// </summary>
   /// <param name="file">File to write the content of the Base64-string</param>
   /// <param name="base64string">Data as Base64-string</param>
   /// <param name="useSaveFormat">Use safe format for Base64, suitable for URLs and files (optional, default: true)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> FileFromBase64Async(string file, string base64string, bool useSaveFormat = true)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return await FileHelper.WriteAllBytesAsync(file, FromBase64String(base64string, useSaveFormat));
   }

   #endregion
}