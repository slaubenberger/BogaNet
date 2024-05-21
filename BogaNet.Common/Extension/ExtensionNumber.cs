namespace BogaNet;

/// <summary>
/// Extension method for various number formats.
/// </summary>
public static class ExtensionNumber
{
   /// <summary>
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static byte BNClamp(this byte value, byte min, byte max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static ushort BNClamp(this ushort value, ushort min, ushort max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static short BNClamp(this short value, short min, short max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static uint BNClamp(this uint value, uint min, uint max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static int BNClamp(this int value, int min, int max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static ulong BNClamp(this ulong value, ulong min, ulong max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static long BNClamp(this long value, long min, long max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static float BNClamp(this float value, float min, float max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static double BNClamp(this double value, double min, double max)
   {
      return value < min ? min : (value > max) ? max : value;
   }
}