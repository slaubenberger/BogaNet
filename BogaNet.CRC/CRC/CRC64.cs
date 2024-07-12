using System.IO.Hashing;
using System.Text;
using BogaNet.Extension;
using System.Threading.Tasks;
using BogaNet.Helper;
using System;

namespace BogaNet.CRC;

/// <summary>
/// Implementation of CRC64.
/// NOTE: never use CRC for integrity checks, use hashes instead!
/// </summary>
public abstract class CRC64 //NUnit
{
   #region Public methods

   /// <summary>
   /// Calculate the CRC64 for a byte-array.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC64</param>
   /// <returns>CRC64 as ulong</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static ulong CalcCRC(params byte[] bytes)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      Crc64 crc64 = new();

      crc64.Append(bytes);

      return crc64.GetCurrentHashAsUInt64();
   }

   /// <summary>
   /// Calculate the CRC64 for a string.
   /// </summary>
   /// <param name="text">string for the CRC64</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>CRC64 as ulong</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static ulong CalcCRC(string text, Encoding? encoding = null)
   {
      return CalcCRC(text.BNToByteArray(encoding));
   }

   /// <summary>
   /// Calculate the CRC64 for a file.
   /// </summary>
   /// <param name="file">File to crc</param>
   /// <returns>CRC64 as ulong</returns>
   /// <exception cref="Exception"></exception>
   public static ulong CalcCRCFile(string file)
   {
      return Task.Run(() => CalcCRCFileAsync(file)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// /// Calculate the CRC64 for a file asynchronously.
   /// </summary>
   /// <param name="file">File to crc</param>
   /// <returns>CRC64 as ulong</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<ulong> CalcCRCFileAsync(string file)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      byte[] bytes = await FileHelper.ReadAllBytesAsync(file);
      return CalcCRC(bytes);
   }

   #endregion
}