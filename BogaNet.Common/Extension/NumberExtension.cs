using System.Numerics;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace BogaNet.Extension;

/// <summary>
/// Extension methods for numbers.
/// </summary>
public static class NumberExtension
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(NumberExtension));

   #region Public methods

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
      byte[]? bytes = null;

      switch (type)
      {
         case Type when type == typeof(byte):
            byte byteVal = byte.CreateTruncating(number);
            bytes = [byteVal];
            break;
         case Type when type == typeof(sbyte):
            sbyte sbyteVal = sbyte.CreateTruncating(number);
            bytes = [(byte)sbyteVal];
            break;
         case Type when type == typeof(short):
            short shortVal = short.CreateTruncating(number);
            bytes = BitConverter.GetBytes(shortVal);
            break;
         case Type when type == typeof(ushort):
            ushort ushortVal = ushort.CreateTruncating(number);
            bytes = BitConverter.GetBytes(ushortVal);
            break;
         case Type when type == typeof(char):
            ushort charVal = ushort.CreateTruncating(number);
            bytes = BitConverter.GetBytes(charVal);
            break;
         case Type when type == typeof(float):
            float floatVal = float.CreateTruncating(number);
            bytes = BitConverter.GetBytes(floatVal);
            break;
         case Type when type == typeof(int):
            int intVal = int.CreateTruncating(number);
            bytes = BitConverter.GetBytes(intVal);
            break;
         case Type when type == typeof(uint):
            uint uintVal = uint.CreateTruncating(number);
            bytes = BitConverter.GetBytes(uintVal);
            break;
         case Type when type == typeof(double):
            double doubleVal = double.CreateTruncating(number);
            bytes = BitConverter.GetBytes(doubleVal);
            break;
         case Type when type == typeof(long):
            long longVal = long.CreateTruncating(number);
            bytes = BitConverter.GetBytes(longVal);
            break;
         case Type when type == typeof(ulong):
            ulong ulongVal = ulong.CreateTruncating(number);
            bytes = BitConverter.GetBytes(ulongVal);
            break;
         case Type when type == typeof(nint):
            nint nintVal = nint.CreateTruncating(number);
            bytes = BitConverter.GetBytes(nintVal);
            break;
         case Type when type == typeof(nuint):
            nint nuintVal = nint.CreateTruncating(number);
            bytes = BitConverter.GetBytes(nuintVal);
            break;
         case Type when type == typeof(decimal):
            decimal decVal = decimal.CreateTruncating(number);
            int[] int64s = decimal.GetBits(decVal);
            bytes = int64s.Take(4).SelectMany(BitConverter.GetBytes).ToArray();
            //byte[] bytes = int64s.Take(4).SelectMany(BitConverter.GetBytes).Reverse().ToArray();
            break;
         default:
            _logger.LogWarning($"Number type {type} is not supported!");
            break;
      }

      if (BitConverter.IsLittleEndian)
         bytes.BNReverse();

      return bytes;
   }

   /// <summary>
   /// Converts a byte-array to a Number.
   /// </summary>
   /// <param name="bytes">Byte-array</param>
   /// <param name="offset">Offset inside the byte-array (optional, default: 0)</param>
   /// <returns>Number from the byte-array</returns>
   public static T? BNToNumber<T>(this byte[]? bytes, int offset = 0) where T : INumber<T>
   {
      if (bytes == null || bytes.Length == 0)
         return default;

      Type type = typeof(T);
      byte[] content;
      int off = offset > 0 ? offset : 0;
      switch (type)
      {
         case Type when type == typeof(byte):
            return T.CreateTruncating(bytes[off]);
         case Type when type == typeof(sbyte):
            return T.CreateTruncating(bytes[off]);
         case Type when type == typeof(short):
            content = readNumberData(2, off, bytes);
            return T.CreateTruncating(BitConverter.ToInt16(content));
         case Type when type == typeof(ushort):
            content = readNumberData(2, off, bytes);
            return T.CreateTruncating(BitConverter.ToUInt16(content));
         case Type when type == typeof(char):
            content = readNumberData(2, off, bytes);
            return T.CreateTruncating(BitConverter.ToChar(content));
         case Type when type == typeof(float):
            content = readNumberData(4, off, bytes);
            return T.CreateTruncating(BitConverter.ToSingle(content));
         case Type when type == typeof(int):
            content = readNumberData(4, off, bytes);
            return T.CreateTruncating(BitConverter.ToInt32(content));
         case Type when type == typeof(uint):
            content = readNumberData(4, off, bytes);
            return T.CreateTruncating(BitConverter.ToUInt32(content));
         case Type when type == typeof(double):
            content = readNumberData(8, off, bytes);
            return T.CreateTruncating(BitConverter.ToDouble(content));
         case Type when type == typeof(long):
            content = readNumberData(8, off, bytes);
            return T.CreateTruncating(BitConverter.ToInt64(content));
         case Type when type == typeof(ulong):
            content = readNumberData(8, off, bytes);
            return T.CreateTruncating(BitConverter.ToUInt64(content));
         case Type when type == typeof(decimal):
         {
            content = readNumberData(16, off, bytes);
            int i1 = BitConverter.ToInt32(content, 0);
            int i2 = BitConverter.ToInt32(content, 4);
            int i3 = BitConverter.ToInt32(content, 8);
            int i4 = BitConverter.ToInt32(content, 12);

            decimal result = new(new[] { i1, i2, i3, i4 });

            return T.CreateTruncating(result);
         }
         default:
            _logger.LogError($"Number type {type} is not supported!");
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

   /*
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
*/

   #endregion

   #region Private methods

   private static byte[] readNumberData(int len, int off, byte[] bytes)
   {
      byte[] data = new byte[len];
      int dstOff = Math.Clamp(len - bytes.Length, 0, len);
      int length = Math.Clamp(bytes.Length, 1, len);
      Buffer.BlockCopy(bytes, off, data, dstOff, length);

      if (BitConverter.IsLittleEndian)
         data.BNReverse();

      return data;
   }

   #endregion
}