using Enumerable = System.Linq.Enumerable;
using System.Text;
using BogaNet.Extension;
using System.Threading.Tasks;
using BogaNet.Helper;
using System;

namespace BogaNet.CRC;

/// <summary>
/// Implementation of CRC16 (ARC).
/// NOTE: never use CRC for integrity checks, use hashes instead!
/// </summary>
public abstract class CRC16 //NUnit
{
   #region Variables

   private static readonly ushort[] _crc16table = new ushort[256];
   private const ushort CRC16_POLY = 0xA001;

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
               value = (ushort)((value >> 1) ^ CRC16_POLY);
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
   /// <exception cref="ArgumentNullException"></exception>
   public static ushort CalcCRC(params byte[] bytes)
   {
      ArgumentNullException.ThrowIfNull(bytes);

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
   /// <exception cref="ArgumentNullException"></exception>
   public static ushort CalcCRC(string text, Encoding? encoding = null)
   {
      return CalcCRC(text.BNToByteArray(encoding));
   }

   /// <summary>
   /// Calculate the CRC16 for a file.
   /// </summary>
   /// <param name="file">File to crc</param>
   /// <returns>CRC16 as ushort</returns>
   /// <exception cref="Exception"></exception>
   public static ushort CalcCRCFile(string file)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      byte[] bytes = FileHelper.ReadAllBytes(file);
      return CalcCRC(bytes);
   }

   /// <summary>
   /// /// Calculate the CRC16 for a file asynchronously.
   /// </summary>
   /// <param name="file">File to crc</param>
   /// <returns>CRC16 as ushort</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<ushort> CalcCRCFileAsync(string file)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      byte[] bytes = await FileHelper.ReadAllBytesAsync(file);
      return CalcCRC(bytes);
   }

   #endregion
}