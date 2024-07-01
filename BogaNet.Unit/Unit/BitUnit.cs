using Microsoft.Extensions.Logging;
using System.Numerics;
using BogaNet.Extension;

namespace BogaNet.Unit;

/// <summary>
/// Units for bits.
/// </summary>
public enum BitUnit
{
   BIT,
   kbit,
   Mbit,
   Gbit,
   Tbit,
   Pbit,
   Ebit,
   Kibit,
   Mibit,
   Gibit,
   Tibit,
   Pibit,
   Eibit
}

/// <summary>
/// Extension methods for BitUnit.
/// </summary>
public static class BitUnitExtension
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(BitUnitExtension));

   public static bool IgnoreSameUnit = true;

   /// <summary>kibibit to Bit.</summary>
   public const decimal FACTOR_Kibit_TO_BIT = 1024;

   /// <summary>mebibit to Bit.</summary>
   public const decimal FACTOR_Mibit_TO_BIT = FACTOR_Kibit_TO_BIT * 1024;

   /// <summary>gibibit to Bit.</summary>
   public const decimal FACTOR_Gibit_TO_BIT = FACTOR_Mibit_TO_BIT * 1024;

   /// <summary>tebibit to Bit.</summary>
   public const decimal FACTOR_Tibit_TO_BIT = FACTOR_Gibit_TO_BIT * 1024;

   /// <summary>pebibit to Bit.</summary>
   public const decimal FACTOR_Pibit_TO_BIT = FACTOR_Tibit_TO_BIT * 1024;

   /// <summary>exbibit to Bit.</summary>
   public const decimal FACTOR_Eibit_TO_BIT = FACTOR_Pibit_TO_BIT * 1024;

   /// <summary>kilobit to Bit.</summary>
   public const decimal FACTOR_kbit_TO_BIT = 1000;

   /// <summary>megabit to Bit.</summary>
   public const decimal FACTOR_Mbit_TO_BIT = FACTOR_kbit_TO_BIT * 1000;

   /// <summary>gigabit to Bit.</summary>
   public const decimal FACTOR_Gbit_TO_BIT = FACTOR_Mbit_TO_BIT * 1000;

   /// <summary>terabit to Bit.</summary>
   public const decimal FACTOR_Tbit_TO_BIT = FACTOR_Gbit_TO_BIT * 1000;

   /// <summary>petabit to Bit.</summary>
   public const decimal FACTOR_Pbit_TO_BIT = FACTOR_Tbit_TO_BIT * 1000;

   /// <summary>exabit to Bit.</summary>
   public const decimal FACTOR_Ebit_TO_BIT = FACTOR_Pbit_TO_BIT * 1000;

   #endregion

   #region Public methods

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromBitUnit">Source unit</param>
   /// <param name="toBitUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value as decimal in the target unit</returns>
   public static decimal Convert<T>(this BitUnit fromBitUnit, BitUnit toBitUnit, T inVal) where T : INumber<T>
   {
      decimal val = inVal.BNToDecimal();

      if (IgnoreSameUnit && fromBitUnit == toBitUnit)
         return val;

      decimal outVal = 0; // = inVal;

      //Convert to Bit
      switch (fromBitUnit)
      {
         case BitUnit.BIT:
            //val = inVal;
            break;
         case BitUnit.Kibit:
            val *= FACTOR_Kibit_TO_BIT;
            break;
         case BitUnit.Mibit:
            val *= FACTOR_Mibit_TO_BIT;
            break;
         case BitUnit.Gibit:
            val *= FACTOR_Gibit_TO_BIT;
            break;
         case BitUnit.Tibit:
            val *= FACTOR_Tibit_TO_BIT;
            break;
         case BitUnit.Pibit:
            val *= FACTOR_Pibit_TO_BIT;
            break;
         case BitUnit.Eibit:
            val *= FACTOR_Eibit_TO_BIT;
            break;
         case BitUnit.kbit:
            val *= FACTOR_kbit_TO_BIT;
            break;
         case BitUnit.Mbit:
            val *= FACTOR_Mbit_TO_BIT;
            break;
         case BitUnit.Gbit:
            val *= FACTOR_Gbit_TO_BIT;
            break;
         case BitUnit.Tbit:
            val *= FACTOR_Tbit_TO_BIT;
            break;
         case BitUnit.Pbit:
            val *= FACTOR_Pbit_TO_BIT;
            break;
         case BitUnit.Ebit:
            val *= FACTOR_Ebit_TO_BIT;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromBitUnit}");
            break;
      }

      //Convert from Bit
      switch (toBitUnit)
      {
         case BitUnit.BIT:
            outVal = val;
            break;
         case BitUnit.Kibit:
            outVal = val / FACTOR_Kibit_TO_BIT;
            break;
         case BitUnit.Mibit:
            outVal = val / FACTOR_Mibit_TO_BIT;
            break;
         case BitUnit.Gibit:
            outVal = val / FACTOR_Gibit_TO_BIT;
            break;
         case BitUnit.Tibit:
            outVal = val / FACTOR_Tibit_TO_BIT;
            break;
         case BitUnit.Pibit:
            outVal = val / FACTOR_Pibit_TO_BIT;
            break;
         case BitUnit.Eibit:
            outVal = val / FACTOR_Eibit_TO_BIT;
            break;
         case BitUnit.kbit:
            outVal = val / FACTOR_kbit_TO_BIT;
            break;
         case BitUnit.Mbit:
            outVal = val / FACTOR_Mbit_TO_BIT;
            break;
         case BitUnit.Gbit:
            outVal = val / FACTOR_Gbit_TO_BIT;
            break;
         case BitUnit.Tbit:
            outVal = val / FACTOR_Tbit_TO_BIT;
            break;
         case BitUnit.Pbit:
            outVal = val / FACTOR_Pbit_TO_BIT;
            break;
         case BitUnit.Ebit:
            outVal = val / FACTOR_Ebit_TO_BIT;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toBitUnit}");
            break;
      }

      return outVal;
   }

   /// <summary>
   /// Converts a BitUnit to a ByteUnit.
   /// </summary>
   /// <param name="fromBitUnit">Source BitUnit</param>
   /// <param name="toByteUnit">Target ByteUnit</param>
   /// <param name="inVal">Value of the source BitUnit</param>
   /// <returns>Value as decimal in the target unit</returns>
   public static decimal Convert<T>(this BitUnit fromBitUnit, ByteUnit toByteUnit, T inVal) where T : INumber<T>
   {
      decimal val = System.Convert.ToDecimal(inVal);
      decimal bytes = fromBitUnit.Convert(BitUnit.BIT, val) / 8;

      return ByteUnit.BYTE.Convert(toByteUnit, bytes);
   }

   #endregion
}