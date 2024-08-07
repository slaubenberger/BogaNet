using Enumerable = System.Linq.Enumerable;
using System.Text;
using BogaNet.Extension;
using System.Threading.Tasks;
using BogaNet.Helper;
using System;

namespace BogaNet.CRC;

/// <summary>
/// Implementation of CRC8.
/// NOTE: never use CRC for integrity checks, use hashes instead!
/// </summary>
public abstract class CRC8 //NUnit
{
   #region Variables

   private static readonly byte[] _crc8table = new byte[256];
   private const byte CRC8_POLY = 0x07;

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
               temp = (temp << 1) ^ CRC8_POLY;
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
   /// Calculate the CRC8 for a byte-array.
   /// </summary>
   /// <param name="bytes">Bytes for the CRC8</param>
   /// <returns>CRC8 as byte</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte CalcCRC(params byte[] bytes)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      byte crc = 0;

      if (bytes.Length > 0)
         crc = Enumerable.Aggregate(bytes, crc, (current, b) => _crc8table[current ^ b]);

      return crc;
   }

   /// <summary>
   /// Calculate the CRC8 for a string.
   /// </summary>
   /// <param name="text">string for the CRC8</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>CRC8 as byte</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte CalcCRC(string text, Encoding? encoding = null)
   {
      return CalcCRC(text.BNToByteArray(encoding));
   }

   /// <summary>
   /// Calculate the CRC8 for a file.
   /// </summary>
   /// <param name="file">File to crc</param>
   /// <returns>CRC8 as byte</returns>
   /// <exception cref="Exception"></exception>
   public static byte CalcCRCFile(string file)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      byte[] bytes = FileHelper.ReadAllBytes(file);
      return CalcCRC(bytes);
   }

   /// <summary>
   /// /// Calculate the CRC8 for a file asynchronously.
   /// </summary>
   /// <param name="file">File to crc</param>
   /// <returns>CRC8 as byte</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<byte> CalcCRCFileAsync(string file)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      byte[] bytes = await FileHelper.ReadAllBytesAsync(file);
      return CalcCRC(bytes);
   }

   #endregion
}