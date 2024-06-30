using Enumerable = System.Linq.Enumerable;
using System.Text;

namespace BogaNet.CRC;

/// <summary>
/// Implementation of CRC16 (ARC).
/// NOTE: never use CRC for integrity checks, use hashes instead!
/// </summary>
public abstract class CRC16
{
   #region Variables

   private static readonly ushort[] _crc16table = new ushort[256];
   private const ushort _crc16poly = 0xA001;

   #endregion

   #region Static block

   static CRC16()
   {
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
   /// Calculate the CRC16 (ARC) for a byte-array.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC16</param>
   /// <returns>CRC16 as ushort</returns>
   public static ushort CalcCRC16(params byte[]? bytes)
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
   /// Calculate CRC16 (ARC) for a string.
   /// </summary>
   /// <param name="text">string for the CRC16</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>CRC16 as ushort</returns>
   public static ushort CalcCRC16(string text, Encoding? encoding = null)
   {
      return CalcCRC16(text.BNToByteArray(encoding));
   }

   #endregion
}