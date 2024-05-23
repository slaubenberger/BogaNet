using System.Numerics;

namespace BogaNet;

/// <summary>
/// Extension methods for numbers.
/// </summary>
public static class ExtensionNumber
{
   /// <summary>
   /// Clamps a value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>t
   public static T BNClamp<T>(this T value, T min, T max) where T : INumber<T>
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Converts the value of a ushort Hex-string.
   /// </summary>
   /// <param name="val">Value</param>
   /// <param name="pairs">Number of pairs in the Hex-string</param>
   /// <param name="addPrefix">Add "0x"-as prefix (optional, default: false).</param>
   /// <returns>ushort as converted Hex-string.</returns>
   public static string BNToHex<T>(this T val, int pairs = 8, bool addPrefix = false) where T : INumber<T>
   {
      string res = val.ToString("x2", null).BNFixedLength(pairs * 2, '0', false);

      return addPrefix ? $"0x{res}" : res;
   }
}