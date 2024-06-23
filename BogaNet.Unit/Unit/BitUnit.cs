using Microsoft.Extensions.Logging;
using System.Numerics;

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
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(BitUnitExtension));

   public static bool IgnoreSameUnit = true;

   /// <summary>Factor bit to kibibit.</summary>
   public const decimal FACTOR_BIT_TO_Kibit = 1024;

   /// <summary>Factor bit to mebibit.</summary>
   public const decimal FACTOR_BIT_TO_Mibit = FACTOR_BIT_TO_Kibit * 1024;

   /// <summary>Factor bit to gibibit.</summary>
   public const decimal FACTOR_BIT_TO_Gibit = FACTOR_BIT_TO_Mibit * 1024;

   /// <summary>Factor bit to tebibit.</summary>
   public const decimal FACTOR_BIT_TO_Tibit = FACTOR_BIT_TO_Gibit * 1024;

   /// <summary>Factor bit to pebibit.</summary>
   public const decimal FACTOR_BIT_TO_Pibit = FACTOR_BIT_TO_Tibit * 1024;

   /// <summary>Factor bit to exbibit.</summary>
   public const decimal FACTOR_BIT_TO_Eibit = FACTOR_BIT_TO_Pibit * 1024;

   /// <summary>Factor bit to kilobit.</summary>
   public const decimal FACTOR_BIT_TO_kbit = 1000;

   /// <summary>Factor bit to megabit.</summary>
   public const decimal FACTOR_BIT_TO_Mbit = FACTOR_BIT_TO_kbit * 1000;

   /// <summary>Factor bit to gigabit.</summary>
   public const decimal FACTOR_BIT_TO_Gbit = FACTOR_BIT_TO_Mbit * 1000;

   /// <summary>Factor bytes to terabit.</summary>
   public const decimal FACTOR_BIT_TO_Tbit = FACTOR_BIT_TO_Gbit * 1000;

   /// <summary>Factor bit to petabit.</summary>
   public const decimal FACTOR_BIT_TO_Pbit = FACTOR_BIT_TO_Tbit * 1000;

   /// <summary>Factor bit to exabit.</summary>
   public const decimal FACTOR_BIT_TO_Ebit = FACTOR_BIT_TO_Pbit * 1000;

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
            val *= FACTOR_BIT_TO_Kibit;
            break;
         case BitUnit.Mibit:
            val *= FACTOR_BIT_TO_Mibit;
            break;
         case BitUnit.Gibit:
            val *= FACTOR_BIT_TO_Gibit;
            break;
         case BitUnit.Tibit:
            val *= FACTOR_BIT_TO_Tibit;
            break;
         case BitUnit.Pibit:
            val *= FACTOR_BIT_TO_Pibit;
            break;
         case BitUnit.Eibit:
            val *= FACTOR_BIT_TO_Eibit;
            break;
         case BitUnit.kbit:
            val *= FACTOR_BIT_TO_kbit;
            break;
         case BitUnit.Mbit:
            val *= FACTOR_BIT_TO_Mbit;
            break;
         case BitUnit.Gbit:
            val *= FACTOR_BIT_TO_Gbit;
            break;
         case BitUnit.Tbit:
            val *= FACTOR_BIT_TO_Tbit;
            break;
         case BitUnit.Pbit:
            val *= FACTOR_BIT_TO_Pbit;
            break;
         case BitUnit.Ebit:
            val *= FACTOR_BIT_TO_Ebit;
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
            outVal = val / FACTOR_BIT_TO_Kibit;
            break;
         case BitUnit.Mibit:
            outVal = val / FACTOR_BIT_TO_Mibit;
            break;
         case BitUnit.Gibit:
            outVal = val / FACTOR_BIT_TO_Gibit;
            break;
         case BitUnit.Tibit:
            outVal = val / FACTOR_BIT_TO_Tibit;
            break;
         case BitUnit.Pibit:
            outVal = val / FACTOR_BIT_TO_Pibit;
            break;
         case BitUnit.Eibit:
            outVal = val / FACTOR_BIT_TO_Eibit;
            break;
         case BitUnit.kbit:
            outVal = val / FACTOR_BIT_TO_kbit;
            break;
         case BitUnit.Mbit:
            outVal = val / FACTOR_BIT_TO_Mbit;
            break;
         case BitUnit.Gbit:
            outVal = val / FACTOR_BIT_TO_Gbit;
            break;
         case BitUnit.Tbit:
            outVal = val / FACTOR_BIT_TO_Tbit;
            break;
         case BitUnit.Pbit:
            outVal = val / FACTOR_BIT_TO_Pbit;
            break;
         case BitUnit.Ebit:
            outVal = val / FACTOR_BIT_TO_Ebit;
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
}