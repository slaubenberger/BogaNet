using Enumerable = System.Linq.Enumerable;
using System.Text;

namespace BogaNet.CRC;

/// <summary>
/// Implementation of CRC8.
/// NOTE: never use CRC for integrity checks, use hashes instead!
/// </summary>
public abstract class CRC8
{
   #region Variables

   private static readonly byte[] _crc8table = new byte[256];
   private const byte _crc8poly = 0x07;

   #endregion

   #region Static block

   static CRC8()
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
   }

   #endregion

   #region Public methods

   /// <summary>
   /// Standard CRC8 implementation for byte-array.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC8</param>
   /// <returns>CRC8 as byte</returns>
   public static byte CalcCRC8(params byte[]? bytes)
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
   public static byte CalcCRC8(string text, Encoding? encoding = null)
   {
      return CalcCRC8(text.BNToByteArray(encoding));
   }

   #endregion
}