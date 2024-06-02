using System.Numerics;
using Microsoft.Extensions.Logging;

namespace BogaNet;

/// <summary>
/// Extension methods for numbers.
/// </summary>
public static class ExtensionNumber
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ExtensionNumber));

   /// <summary>
   /// Clamps a value between min and max.
   /// </summary>
   /// <param name="number">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>t
   public static T BNClamp<T>(this T number, T min, T max) where T : INumber<T>
   {
      return number < min ? min : (number > max) ? max : number;
   }

   /// <summary>
   /// Converts the value of a Number to a Hex-string.
   /// </summary>
   /// <param name="number">Given value</param>
   /// <param name="addPrefix">Add "0x"-as prefix (optional, default: false)</param>
   /// <returns>Number as converted Hex-string</returns>
   public static string BNToHex<T>(this T number, bool addPrefix = false) where T : INumber<T>
   //public unsafe static string BNToHex<T>(this T val, bool addPrefix = false) where T : INumber<T>
   {
      Type type = typeof(T);
      int pairs = 8;

      switch (type)
      {
         case Type t when t == typeof(float):
            pairs = 4;
            break;
         case Type t when t == typeof(int):
            pairs = 4;
            break;
         case Type t when t == typeof(uint):
            pairs = 4;
            break;
         case Type t when t == typeof(short):
            pairs = 2;
            break;
         case Type t when t == typeof(ushort):
            pairs = 2;
            break;
/*
         case Type t when t == typeof(nint):
            length = sizeof(nint);
            break;
         case Type t when t == typeof(nuint):
            length = sizeof(nint);
            break;
*/
         case Type t when t == typeof(byte):
            pairs = 1;
            break;
         case Type t when t == typeof(sbyte):
            pairs = 1;
            break;
      }

      string res = number.ToString("x2", null).BNFixedLength(2 * pairs, '0', false);

      return addPrefix ? $"0x{res}" : res;
   }

   /// <summary>
   /// Converts the value of a Number to a byte-array.
   /// </summary>
   /// <param name="number">Given value</param>
   /// <returns>Byte-array with the Number</returns>
   public static byte[]? BNToByteArray<T>(this T? number) where T : INumber<T>
   {
      if (number == null)
         return null;

      Type type = typeof(T);

      switch (type)
      {
         case Type t when t == typeof(double):
            double doubleVal = double.CreateTruncating(number);
            return BitConverter.GetBytes(doubleVal);
         case Type t when t == typeof(float):
            float floatVal = float.CreateTruncating(number);
            return BitConverter.GetBytes(floatVal);
         case Type t when t == typeof(long):
            long longVal = long.CreateTruncating(number);
            return BitConverter.GetBytes(longVal);
         case Type t when t == typeof(ulong):
            ulong ulongVal = ulong.CreateTruncating(number);
            return BitConverter.GetBytes(ulongVal);
         case Type t when t == typeof(int):
            int intVal = int.CreateTruncating(number);
            return BitConverter.GetBytes(intVal);
         case Type t when t == typeof(uint):
            uint uintVal = uint.CreateTruncating(number);
            return BitConverter.GetBytes(uintVal);
         case Type t when t == typeof(short):
            short shortVal = short.CreateTruncating(number);
            return BitConverter.GetBytes(shortVal);
         case Type t when t == typeof(ushort):
            ushort ushortVal = ushort.CreateTruncating(number);
            return BitConverter.GetBytes(ushortVal);
         case Type t when t == typeof(nint):
            nint nintVal = nint.CreateTruncating(number);
            return BitConverter.GetBytes(nintVal);
         case Type t when t == typeof(nuint):
            nint nuintVal = nint.CreateTruncating(number);
            return BitConverter.GetBytes(nuintVal);
         case Type t when t == typeof(byte):
            byte byteVal = byte.CreateTruncating(number);
            return [byteVal];
         /*
         case Type t when t == typeof(sbyte):
            sbyte sbyteVal = sbyte.CreateTruncating(number);
            return [sbyteVal];
         */
         default:
            _logger.LogWarning("Number type is not supported!");
            break;
      }

      return default;
   }
}