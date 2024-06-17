using Enumerable = System.Linq.Enumerable;

namespace BogaNet.Crypto;

/// <summary>
/// Helper for CRC checks.
/// NOTE: never use CRC for integrity checks, use hashes instead!
/// </summary>
public abstract class CRCHelper
{
   private static byte[] _crc8table = new byte[256];
   private const byte _crc8poly = 0x07;

   private static ushort[] _crc16table = new ushort[256];
   private const ushort _crc16poly = 0xA001;

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
      ushort value;
      ushort tempCrc16;
      for (ushort ii = 0; ii < _crc16table.Length; ii++)
      {
         value = 0;
         tempCrc16 = ii;

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

   /// <summary>
   /// Standard CRC8 implementation.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC8</param>
   /// <returns>CRC8</returns>
   public static byte CRC8(params byte[] bytes)
   {
      byte crc = 0;
      if (bytes != null && bytes.Length > 0)
      {
         foreach (byte b in bytes)
         {
            crc = _crc8table[crc ^ b];
         }
      }

      return crc;
   }

   /// <summary>
   /// Standard CRC16 (ARC) implementation.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC16</param>
   /// <returns>CRC16</returns>
   public static ushort CRC16(byte[] bytes)
   {
      ushort crc = 0;

      foreach (byte index in Enumerable.Select(bytes, b => (byte)(crc ^ b)))
      {
         crc = (ushort)((crc >> 8) ^ _crc16table[index]);
      }

      return crc;
   }

   /// <summary>
   /// Standard CRC32 implementation.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC32</param>
   /// <returns>CRC32</returns>
   public static uint CRC32(byte[] bytes)
   {
      System.IO.Hashing.Crc32 crc32 = new();

      crc32.Append(bytes);

      return crc32.GetCurrentHashAsUInt32();
   }

   /// <summary>
   /// Standard CRC64 implementation.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC64</param>
   /// <returns>CRC64</returns>
   public static ulong CRC64(byte[] bytes)
   {
      System.IO.Hashing.Crc64 crc64 = new();

      crc64.Append(bytes);

      return crc64.GetCurrentHashAsUInt64();
   }
}