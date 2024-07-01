using System.IO.Hashing;
using System.Text;
using BogaNet.Extension;

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
   public static ulong CalcCRC64(params byte[]? bytes)
   {
      if (bytes == null)
         return 0;

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
   public static ulong CalcCRC64(string text, Encoding? encoding = null)
   {
      return CalcCRC64(text.BNToByteArray(encoding));
   }

   #endregion
}