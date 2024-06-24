using Enumerable = System.Linq.Enumerable;
using System.IO.Hashing;
using System.Text;

namespace BogaNet.Helper;

/// <summary>
/// Helper for CRC checks.
/// NOTE: never use CRC for integrity checks, use hashes instead!
/// </summary>
public abstract class CRCHelper
{
   #region Variables

   private static readonly byte[] _crc8table = new byte[256];
   private const byte _crc8poly = 0x07;

   private static readonly ushort[] _crc16table = new ushort[256];
   private const ushort _crc16poly = 0xA001;

   #endregion

   #region Static block

   static CRCHelper()
   {
      //fill table for CRC8
      for (int ii = 0; ii < _crc8table.Length; ii++)
      {
         int temp = ii;
         for (int yy = 0; yy < 8; yy++)
         {
            if ((temp & 0x80) != 0)
            {
               temp = (temp << 1) ^ _crc8poly;
            }
            else
            {
               temp <<= 1;
            }
         }

         _crc8table[ii] = (byte)temp;
      }

      //fill table for CRC16
      for (ushort ii = 0; ii < _crc16table.Length; ii++)
      {
         ushort value = 0;
         ushort tempCrc16 = ii;

         for (byte yy = 0; yy < 8; yy++)
         {
            if (((value ^ tempCrc16) & 0x0001) != 0)
            {
               value = (ushort)((value >> 1) ^ _crc16poly);
            }
            else
            {
               value >>= 1;
            }

            tempCrc16 >>= 1;
         }

         _crc16table[ii] = value;
      }
   }

   #endregion

   #region Public methods

   /// <summary>
   /// Standard CRC8 implementation for byte-array.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC8</param>
   /// <returns>CRC8 as byte</returns>
   public static byte CRC8(params byte[]? bytes)
   {
      byte crc = 0;

      if (bytes != null && bytes.Length > 0)
      {
         crc = Enumerable.Aggregate(bytes, crc, (current, b) => _crc8table[current ^ b]);
      }

      return crc;
   }

   /// <summary>
   /// Standard CRC8 implementation for string.
   /// </summary>
   /// <param name="text">string for the CRC8</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>CRC8 as byte</returns>
   public static byte CRC8(string text, Encoding? encoding = null)
   {
      return CRC8(text.BNToByteArray(encoding));
   }

   /// <summary>
   /// Standard CRC16 (ARC) implementation for byte-array.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC16</param>
   /// <returns>CRC16 as ushort</returns>
   public static ushort CRC16(params byte[]? bytes)
   {
      if (bytes == null)
         return 0;

      ushort crc = 0;

      foreach (byte index in Enumerable.Select(bytes, b => (byte)(crc ^ b)))
      {
         crc = (ushort)((crc >> 8) ^ _crc16table[index]);
      }

      return crc;
   }

   /// <summary>
   /// Standard CRC16 (ARC) implementation for string.
   /// </summary>
   /// <param name="text">string for the CRC16</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>CRC16 as ushort</returns>
   public static ushort CRC16(string text, Encoding? encoding = null)
   {
      return CRC16(text.BNToByteArray(encoding));
   }

   /// <summary>
   /// Standard CRC32 implementation for byte-array.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC32</param>
   /// <returns>CRC32 as uint</returns>
   public static uint CRC32(params byte[]? bytes)
   {
      if (bytes == null)
         return 0;

      Crc32 crc32 = new();

      crc32.Append(bytes);

      return crc32.GetCurrentHashAsUInt32();
   }

   /// <summary>
   /// Standard CRC32 implementation for string.
   /// </summary>
   /// <param name="text">string for the CRC32</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>CRC32 as uint</returns>
   public static uint CRC32(string text, Encoding? encoding = null)
   {
      return CRC32(text.BNToByteArray(encoding));
   }

   /// <summary>
   /// Standard CRC64 implementation for byte-array.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC64</param>
   /// <returns>CRC64 as ulong</returns>
   public static ulong CRC64(params byte[]? bytes)
   {
      if (bytes == null)
         return 0;

      Crc64 crc64 = new();

      crc64.Append(bytes);

      return crc64.GetCurrentHashAsUInt64();
   }

   /// <summary>
   /// Standard CRC64 implementation for string.
   /// </summary>
   /// <param name="text">string for the CRC64</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>CRC64 as ulong</returns>
   public static ulong CRC64(string text, Encoding? encoding = null)
   {
      return CRC64(text.BNToByteArray(encoding));
   }

   #endregion
}