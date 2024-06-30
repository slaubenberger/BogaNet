using Microsoft.Extensions.Logging;
using System.Numerics;

namespace BogaNet.Unit;

/// <summary>
/// Units for volumes.
/// </summary>
public enum VolumeUnit
{
   LITER,
   MM3,
   CM3, //aka "milli liter"
   M3,
   INCH3,
   FOOT3,
   YARD3,
   PINT_US,
   GALLON_US,
   BARREL,
   CUP_US,
   TABLESPOON_US,
   TEASPOON_US,

   QUART_US
   //TODO add more exotic volumes?
}

/// <summary>
/// Extension methods for VolumeUnit.
/// </summary>
public static class VolumeUnitExtension
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(VolumeUnitExtension));

   public static bool IgnoreSameUnit = true;

   private const decimal FACTOR_MM3_TO_CM3 = 1000; //millimeters^3 to centimeters^3

   /// <summary>
   /// Liter to meters³.
   /// </summary>
   public const decimal FACTOR_L_TO_M3 = 1000;

   /// <summary>
   /// Centimeter³ (aka milliliter) to liters.
   /// </summary>
   public const decimal FACTOR_CM3_TO_L = 1000;

   /// <summary>
   /// Inch³ to liters.
   /// </summary>
   public const decimal FACTOR_INCH3_TO_L = 0.016387064m;

   /// <summary>
   /// Foot³ to liters.
   /// </summary>
   public const decimal FACTOR_FOOT3_TO_L = 28.316864592m;

   /// <summary>
   /// Yard³ to liters.
   /// </summary>
   public const decimal FACTOR_YARD3_TO_L = 764.5549m;

   /// <summary>
   /// Pint (US) to liters.
   /// </summary>
   public const decimal FACTOR_PINT_US_TO_L = 0.473176473m;

   /// <summary>
   /// Gallon (US) to liters.
   /// </summary>
   public const decimal FACTOR_GALLON_US_TO_L = 3.785411784m;

   /// <summary>
   /// Barrel to liters.
   /// </summary>
   public const decimal FACTOR_BARREL_TO_L = 158.987294928m;

   /// <summary>
   /// Cup (US) to liters.
   /// </summary>
   public const decimal FACTOR_CUP_US_TO_L = 0.24m;

   /// <summary>
   /// Tablespoon (US) to liters.
   /// </summary>
   public const decimal FACTOR_TABLESPOON_US_TO_L = 0.01478676m;

   /// <summary>
   /// Teaspoon (US) to liters.
   /// </summary>
   public const decimal FACTOR_TEASPOON_US_TO_L = 0.004928922m;

   /// <summary>
   /// Quart (US) to liters.
   /// </summary>
   public const decimal FACTOR_QUART_US_TO_L = 0.946353m;

   /// <summary>
   /// Millimeter³ to liters.
   /// </summary>
   public static decimal FACTOR_MM3_TO_L => FACTOR_MM3_TO_CM3 * FACTOR_CM3_TO_L;

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromVolumeUnit">Source unit</param>
   /// <param name="toVolumeUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value as decimal in the target unit</returns>
   public static decimal Convert<T>(this VolumeUnit fromVolumeUnit, VolumeUnit toVolumeUnit, T inVal) where T : INumber<T>
   {
      decimal val = inVal.BNToDecimal();

      if (IgnoreSameUnit && fromVolumeUnit == toVolumeUnit)
         return val;

      decimal outVal = 0; // = inVal;

      //Convert to liter
      switch (fromVolumeUnit)
      {
         case VolumeUnit.LITER:
            //val = inVal;
            break;
         case VolumeUnit.MM3:
            val /= FACTOR_MM3_TO_L;
            break;
         case VolumeUnit.CM3:
            val /= FACTOR_CM3_TO_L;
            break;
         case VolumeUnit.M3:
            val *= FACTOR_L_TO_M3;
            break;
         case VolumeUnit.INCH3:
            val *= FACTOR_INCH3_TO_L;
            break;
         case VolumeUnit.FOOT3:
            val *= FACTOR_FOOT3_TO_L;
            break;
         case VolumeUnit.YARD3:
            val *= FACTOR_YARD3_TO_L;
            break;
         case VolumeUnit.PINT_US:
            val *= FACTOR_PINT_US_TO_L;
            break;
         case VolumeUnit.GALLON_US:
            val *= FACTOR_GALLON_US_TO_L;
            break;
         case VolumeUnit.BARREL:
            val *= FACTOR_BARREL_TO_L;
            break;
         case VolumeUnit.CUP_US:
            val *= FACTOR_CUP_US_TO_L;
            break;
         case VolumeUnit.TABLESPOON_US:
            val *= FACTOR_TABLESPOON_US_TO_L;
            break;
         case VolumeUnit.TEASPOON_US:
            val *= FACTOR_TEASPOON_US_TO_L;
            break;
         case VolumeUnit.QUART_US:
            val *= FACTOR_QUART_US_TO_L;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromVolumeUnit}");
            break;
      }

      //Convert from liter
      switch (toVolumeUnit)
      {
         case VolumeUnit.LITER:
            outVal = val;
            break;
         case VolumeUnit.MM3:
            outVal = val * FACTOR_MM3_TO_L;
            break;
         case VolumeUnit.CM3:
            outVal = val * FACTOR_CM3_TO_L;
            break;
         case VolumeUnit.M3:
            outVal = val / FACTOR_L_TO_M3;
            break;
         case VolumeUnit.INCH3:
            outVal = val / FACTOR_INCH3_TO_L;
            break;
         case VolumeUnit.FOOT3:
            outVal = val / FACTOR_FOOT3_TO_L;
            break;
         case VolumeUnit.YARD3:
            outVal = val / FACTOR_YARD3_TO_L;
            break;
         case VolumeUnit.PINT_US:
            outVal = val / FACTOR_PINT_US_TO_L;
            break;
         case VolumeUnit.GALLON_US:
            outVal = val / FACTOR_GALLON_US_TO_L;
            break;
         case VolumeUnit.BARREL:
            outVal = val / FACTOR_BARREL_TO_L;
            break;
         case VolumeUnit.CUP_US:
            outVal = val / FACTOR_CUP_US_TO_L;
            break;
         case VolumeUnit.TABLESPOON_US:
            outVal = val / FACTOR_TABLESPOON_US_TO_L;
            break;
         case VolumeUnit.TEASPOON_US:
            outVal = val / FACTOR_TEASPOON_US_TO_L;
            break;
         case VolumeUnit.QUART_US:
            outVal = val / FACTOR_QUART_US_TO_L;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toVolumeUnit}");
            break;
      }

      return outVal;
   }
}