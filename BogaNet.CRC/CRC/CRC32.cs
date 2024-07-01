using System.IO.Hashing;
using System.Text;
using BogaNet.Extension;

namespace BogaNet.CRC;

/// <summary>
/// Implementation of CRC32.
/// NOTE: never use CRC for integrity checks, use hashes instead!
/// </summary>
public abstract class CRC32 //NUnit
{
   #region Public methods

   /// <summary>
   /// Calculate the CRC32 for a byte-array.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC32</param>
   /// <returns>CRC32 as uint</returns>
   public static uint CalcCRC32(params byte[]? bytes)
   {
      if (bytes == null)
         return 0;

      Crc32 crc32 = new();

      crc32.Append(bytes);

      return crc32.GetCurrentHashAsUInt32();
   }

   /// <summary>
   /// Calculate the CRC32 for a string.
   /// </summary>
   /// <param name="text">string for the CRC32</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>CRC32 as uint</returns>
   public static uint CalcCRC32(string text, Encoding? encoding = null)
   {
      return CalcCRC32(text.BNToByteArray(encoding));
   }

   #endregion
}