namespace BogaNet;

/// <summary>
/// Extension methods for byte.
/// </summary>
public static class ExtensionByte
{
   /// <summary>
   /// Represents the given byte a binary string.
   /// </summary>
   /// <param name="value">The byte to represent as binary string</param>
   /// <returns>Binary string</returns>
   public static string BNToBinary(this byte value) => Convert.ToString(value).PadLeft(8, '0');

   /// <summary>
   /// Determine if the bit at the provided index is set (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte whose index to check</param>
   /// <param name="index">The bit index to check</param>
   /// <returns>The mutated byte value</returns>
   public static bool BNIsBitSetAtIndex(this byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (value & (1 << (7 - index))) != 0;

   /// <summary>
   /// Set the bit value at the provided index (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte value whose bit to set</param>
   /// <param name="index">The index of the bit to set</param>
   /// <returns>The mutated byte value</returns>
   public static byte BNSetBitAtIndex(this byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (byte)(value | (1 << (7 - index)));

   /// <summary>
   /// Clear the bit value at the provided index (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte value whose bit to clear</param>
   /// <param name="index">The index of the bit to clear</param>
   /// <returns>The mutated byte value</returns>
   public static byte BNClearBitAtIndex(this byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (byte)(value & ~(1 << (7 - index)));

   /// <summary>
   /// Toggle the bit value at the provided index (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte value whose bit to toggle</param>
   /// <param name="index">The index of the bit to toggle</param>
   /// <returns>The mutated byte value</returns>
   public static byte BNToggleBitAtIndex(this byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (byte)(value ^ (1 << (7 - index)));
}