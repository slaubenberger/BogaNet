using System.Numerics;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace BogaNet;

/// <summary>
/// Extension methods for numbers.
/// </summary>
public static class NumberExtension
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(NumberExtension));

   /// <summary>
   /// Clamps a value between min and max.
   /// </summary>
   /// <param name="number">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>t
   public static T BNClamp<T>(this T number, T min, T max) where T : INumber<T>
   {
      return number < min ? min : number > max ? max : number;
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
         case Type t when t == typeof(sbyte):
            sbyte sbyteVal = sbyte.CreateTruncating(number);
            return [(byte)sbyteVal];
         //return [Convert.ToByte(sbyteVal + 127)];
         case Type t when t == typeof(char):
            ushort charVal = ushort.CreateTruncating(number);
            return BitConverter.GetBytes(charVal);
         case Type t when t == typeof(decimal):
            decimal decVal = decimal.CreateTruncating(number);
            int[] int64s = decimal.GetBits(decVal);
            byte[] bytes = int64s.Take(4).SelectMany(BitConverter.GetBytes).ToArray();
            //byte[] bytes = int64s.Take(4).SelectMany(BitConverter.GetBytes).Reverse().ToArray();
            return bytes;
         default:
            _logger.LogWarning("Number type is not supported!");
            break;
      }

      return default;
   }

   /// <summary>
   /// Converts a byte to the given Number type.
   /// </summary>
   /// <param name="number">Given value as byte</param>
   /// <returns>Number value from the given byte.</returns>
   public static T BNToNumber<T>(this byte number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a sbyte to the given Number type.
   /// </summary>
   /// <param name="number">Given value as sbyte</param>
   /// <returns>Number value from the given sbyte.</returns>
   public static T BNToNumber<T>(this sbyte number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a short to the given Number type.
   /// </summary>
   /// <param name="number">Given value as short</param>
   /// <returns>Number value from the given short.</returns>
   public static T BNToNumber<T>(this short number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a ushort to the given Number type.
   /// </summary>
   /// <param name="number">Given value as ushort</param>
   /// <returns>Number value from the given ushort.</returns>
   public static T BNToNumber<T>(this ushort number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a char to the given Number type.
   /// </summary>
   /// <param name="number">Given value as char</param>
   /// <returns>Number value from the given char.</returns>
   public static T BNToNumber<T>(this char number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a float to the given Number type.
   /// </summary>
   /// <param name="number">Given value as float</param>
   /// <returns>Number value from the given float.</returns>
   public static T BNToNumber<T>(this float number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a int to the given Number type.
   /// </summary>
   /// <param name="number">Given value as int</param>
   /// <returns>Number value from the given int.</returns>
   public static T BNToNumber<T>(this int number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a uint to the given Number type.
   /// </summary>
   /// <param name="number">Given value as uint</param>
   /// <returns>Number value from the given uint.</returns>
   public static T BNToNumber<T>(this uint number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a double to the given Number type.
   /// </summary>
   /// <param name="number">Given value as double</param>
   /// <returns>Number value from the given double.</returns>
   public static T BNToNumber<T>(this double number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a long to the given Number type.
   /// </summary>
   /// <param name="number">Given value as long</param>
   /// <returns>Number value from the given long.</returns>
   public static T BNToNumber<T>(this long number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a ulong to the given Number type.
   /// </summary>
   /// <param name="number">Given value as ulong</param>
   /// <returns>Number value from the given ulong.</returns>
   public static T BNToNumber<T>(this ulong number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a decimal to the given Number type.
   /// </summary>
   /// <param name="number">Given value as decimal</param>
   /// <returns>Number value from the given decimal.</returns>
   public static T BNToNumber<T>(this decimal number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a nint to the given Number type.
   /// </summary>
   /// <param name="number">Given value as nint</param>
   /// <returns>Number value from the given nint.</returns>
   public static T BNToNumber<T>(this nint number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a nuint to the given Number type.
   /// </summary>
   /// <param name="number">Given value as nuint</param>
   /// <returns>Number value from the given nuint.</returns>
   public static T BNToNumber<T>(this nuint number) where T : INumber<T>
   {
      return T.CreateTruncating(number);
   }

   /// <summary>
   /// Converts a Number type to decimal.
   /// </summary>
   /// <param name="number">Given value as Number type</param>
   /// <returns>Decimal value from the given type.</returns>
   public static decimal BNToDecimal<T>(this T number) where T : INumber<T>
   {
      return System.Convert.ToDecimal(number);
   }
}