using System;

namespace BogaNet;

/// <summary>
/// Extension methods for byte.
/// </summary>
public static class ByteExtension
{
   /// <summary>
   /// Represents the given byte a binary string.
   /// </summary>
   /// <param name="value">The byte to represent as binary string</param>
   /// <returns>Binary string</returns>
   public static string BNToBinary(this byte value) => Convert.ToString(value).PadLeft(8, '0');
}