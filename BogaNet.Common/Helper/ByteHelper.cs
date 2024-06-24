using System;

namespace BogaNet.Helper;

/// <summary>
/// Helper methods for byte.
/// </summary>
public static class ByteHelper
{
   /// <summary>
   /// Determine if the bit at the provided index is set (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte whose index to check</param>
   /// <param name="index">The bit index to check</param>
   /// <returns>The mutated byte value</returns>
   public static bool IsBitSetAtIndex(byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (value & (1 << (7 - index))) != 0;

   /// <summary>
   /// Set the bit value at the provided index (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte value whose bit to set</param>
   /// <param name="index">The index of the bit to set</param>
   /// <returns>The mutated byte value</returns>
   public static byte SetBitAtIndex(byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (byte)(value | (1 << (7 - index)));

   /// <summary>
   /// Clear the bit value at the provided index (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte value whose bit to clear</param>
   /// <param name="index">The index of the bit to clear</param>
   /// <returns>The mutated byte value</returns>
   public static byte ClearBitAtIndex(byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (byte)(value & ~(1 << (7 - index)));

   /// <summary>
   /// Toggle the bit value at the provided index (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte value whose bit to toggle</param>
   /// <param name="index">The index of the bit to toggle</param>
   /// <returns>The mutated byte value</returns>
   public static byte ToggleBitAtIndex(byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (byte)(value ^ (1 << (7 - index)));
/*
   /// <summary>
   /// Shift the bits of the provided byte array right by the provided distance. Bits will shift across byte
   /// boundaries in the array. Zero will be shifted in from the left. The sign of the byte array isn't considered.
   /// </summary>
   /// <param name="bytes">The byte array to shift.</param>
   /// <param name="distance">The distance to shift the bits.</param>
   /// <returns>The right-shifted byte array.</returns>
   public static byte[] ShiftRight(byte[] bytes, uint distance = 1)
   {
      // For each index distance to shift the bits.
      for (var d = 0; d < distance; d++)
      {
         // For every byte in the array (MSB <- LSB).
         for (var i = bytes.Length - 1; i >= 0; i--)
         {
            // Shift the current index right by one bit.
            bytes[i] = (byte)(bytes[i] >> 1);

            // Set the most significant bit if necessary.
            if (i >= 1 && (bytes[i - 1] & 0x01) != 0)
            {
               bytes[i] |= (byte)(bytes[i] | 0x80);
            }
         }
      }

      return bytes;
   }

   /// <summary>
   /// Shift the bits of the provided byte array left by the provided distance. Bits will shift across byte
   /// boundaries in the array. The sign of the byte array isn't considered.
   /// </summary>
   /// <param name="bytes">The byte array to shift.</param>
   /// <param name="distance">The distance to shift the bits.</param>
   /// <returns>The left-shifted byte array.</returns>
   public static byte[] ShiftLeft(byte[] bytes
      , uint distance = 1
   )
   {
      // For each index distance to shift the bits.
      for (var d = 0; d < distance; d++)
      {
         // For every byte in the array (MSB <- LSB).
         for (var i = bytes.Length - 1; i >= 0; i--)
         {
            // Shift the current index right by one bit.
            bytes[i] = (byte)(bytes[i] << 1);

            // Set the most significant bit if necessary.
            if (i >= 1 && (bytes[i - 1] & 0x01) != 0)
            {
               bytes[i] |= (byte)(bytes[i] | 0x80);
            }
         }
      }

      return bytes;
   }

   /// <summary>
   /// Mask off the the nth bit position of the byte starting from the left. Indexing is 0-based.
   /// 
   /// e.g. A value of 0xFF will produce the following:
   /// 
   /// 11111111 (0) -> 01111111
   /// 11111111 (1) -> 00111111
   /// 11111111 (2) -> 00011111
   /// 11111111 (3) -> 00001111
   /// 11111111 (4) -> 00000111
   /// 11111111 (5) -> 00000011
   /// 11111111 (6) -> 00000001
   /// 11111111 (7) -> 00000000
   /// </summary>
   /// <param name="value">The byte to mask.</param>
   /// <param name="maskIndex"></param>
   /// <returns>The masked byte value.</returns>
   public static byte MaskLeft(byte value, byte maskIndex) => (byte)(value & (0xFF >> (maskIndex + 1)));

   /// <summary>
   /// Mask off the the nth bit position of the byte starting from the right. Indexing is 0-based.
   /// 
   /// e.g. A value of 0xFF will produce the following:
   /// 
   /// 11111111 (0) -> 11111110
   /// 11111111 (1) -> 11111100
   /// 11111111 (2) -> 11111000
   /// 11111111 (3) -> 11110000
   /// 11111111 (4) -> 11100000
   /// 11111111 (5) -> 11000000
   /// 11111111 (6) -> 10000000
   /// 11111111 (7) -> 00000000
   /// </summary>
   /// <param name="value"></param>
   /// <param name="maskIndex"></param>
   /// <returns></returns>
   public static byte MaskRight(byte value, byte maskIndex) => (byte)(value & (0xFF << (maskIndex + 1)));
   */
}