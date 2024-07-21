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
    /// <param name="useSave">Use non-standard, but safe version of Base64 for URLs and files (optional, default: false)</param>
    /// <returns>Data as byte-array</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static byte[] FromBase64String(string base64string, bool useSave = false)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(base64string);

        return Convert.FromBase64String(useSave ? base64string.Replace("_", "/").Replace("-", "+") : base64string);
    }

    /// <summary>
    /// Converts a byte-array to a Base64-string.
    /// </summary>
    /// <param name="bytes">Data as byte-array</param>
    /// <param name="useSave">Use non-standard, but safe version of Base64 for URLs and files (optional, default: false)</param>
    /// <returns>Data as encoded Base64-string</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ToBase64String(byte[] bytes, bool useSave = false)
    {
        ArgumentNullException.ThrowIfNull(bytes);

        //bytes.BNReverse();
        return useSave
            ? Convert.ToBase64String(bytes).Replace("/", "_").Replace("+", "-")
            : Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Converts the value of a string to a Base64-string.
    /// </summary>
    /// <param name="str">Input string</param>
    /// <param name="useSave">Use non-standard, but safe version of Base64 for URLs and files (optional, default: false)</param>
    /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
    /// <returns>String value as converted Base64-string</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ToBase64String(string str, bool useSave = false, Encoding? encoding = null)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(str);

        byte[] bytes = str.BNToByteArray(encoding);
        //bytes.BNReverse();
        return ToBase64String(bytes, useSave);
    }

    /// <summary>
    /// Converts a file to a Base64-string.
    /// </summary>
    /// <param name="file">File to convert</param>
    /// <param name="useSave">Use non-standard, but safe version of Base64 for URLs and files</param>
    /// <returns>File content as converted Base64-string</returns>
    /// <exception cref="Exception"></exception>
    public static string Base64FromFile(string file, bool useSave = false)
    {
        return Task.Run(() => Base64FromFileAsync(file, useSave)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Converts a file to a Base64-string asynchronously.
    /// </summary>
    /// <param name="file">File to convert</param>
    /// <param name="useSave">Use non-standard, but safe version of Base64 for URLs and files (optional, default: false)</param>
    /// <returns>File content as converted Base64-string</returns>
    /// <exception cref="Exception"></exception>
    public static async Task<string> Base64FromFileAsync(string file, bool useSave = false)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(file);

        return ToBase64String(await FileHelper.ReadAllBytesAsync(file), useSave);
    }

    /// <summary>
    /// Converts a Base64-string to a file.
    /// </summary>
    /// <param name="file">File to write the content of the Base64-string</param>
    /// <param name="base64string">Data as Base64-string</param>
    /// <param name="useSave">Use non-standard, but safe version of Base64 for URLs and files (optional, default: false)</param>
    /// <returns>True if the operation was successful</returns>
    /// <exception cref="Exception"></exception>
    public static bool FileFromBase64(string file, string base64string, bool useSave = false)
    {
        return Task.Run(() => FileFromBase64Async(file, base64string, useSave)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Converts a Base64-string to a file asynchronously.
    /// </summary>
    /// <param name="file">File to write the content of the Base64-string</param>
    /// <param name="base64string">Data as Base64-string</param>
    /// <param name="useSave">Use non-standard, but safe version of Base64 for URLs and files (optional, default: false)</param>
    /// <returns>True if the operation was successful</returns>
    /// <exception cref="Exception"></exception>
    public static async Task<bool> FileFromBase64Async(string file, string base64string, bool useSave = false)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(file);

        return await FileHelper.WriteAllBytesAsync(file, FromBase64String(base64string, useSave));
    }

    #endregion
}