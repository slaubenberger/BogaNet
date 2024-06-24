using Microsoft.Extensions.Logging;
using System.Numerics;

namespace BogaNet.Unit;

/// <summary>
/// Units for areas.
/// </summary>
public enum AreaUnit
{
   M2,
   MM2,
   CM2,
   AREA,
   HECTARE,
   KM2,
   INCH2,
   FOOT2,
   YARD2,
   PERCH,
   ACRE,
   MILE2
   //TODO add more exotic areas?
}

/// <summary>
/// Extension methods for AreaUnit.
/// </summary>
public static class AreaUnitExtension
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(AreaUnitExtension));

   public static bool IgnoreSameUnit = true;

   private const decimal FACTOR_MM2_TO_CM2 = 100; //millimeters² to centimeters²
   private const decimal FACTOR_AREA_TO_HECTARE = 100; //area to hectare
   private const decimal FACTOR_HECTARE_TO_KM2 = 100; //hectare to kilometers²
   private const decimal FACTOR_INCH_TO_CM2 = 6.4516m; //inch² to centimeters²
   private const decimal FACTOR_MILE2_TO_KM2 = 2.5899881103m; //mile² (terrestrial) to kilometers²

   /// <summary>
   /// Centimeter² to meters².
   /// </summary>
   public const decimal FACTOR_CM2_TO_M2 = 10000;

   /// <summary>
   /// Meter² to areas.
   /// </summary>
   public const decimal FACTOR_M2_TO_AREA = 100;

   /// <summary>
   /// Foot² to meters².
   /// </summary>
   public const decimal FACTOR_FOOT2_TO_M2 = 0.09290304m;

   /// <summary>
   /// Yard² to meters².
   /// </summary>
   public const decimal FACTOR_YARD2_TO_M2 = 0.83612736m;

   /// <summary>
   /// Perch/rod² to meters².
   /// </summary>
   public const decimal FACTOR_PERCH_TO_M2 = 25.2928526m;

   /// <summary>
   /// Acre to meters².
   /// </summary>
   public const decimal FACTOR_ACRE_TO_M2 = 4046.8564224m;

   /// <summary>
   /// Millimeter² to meters².
   /// </summary>
   public static decimal FACTOR_MM2_TO_M2 => FACTOR_MM2_TO_CM2 * FACTOR_CM2_TO_M2;

   /// <summary>
   /// Meter² to hectares.
   /// </summary>
   public static decimal FACTOR_M2_TO_HECTARE => FACTOR_M2_TO_AREA * FACTOR_AREA_TO_HECTARE;

   /// <summary>
   /// Meter² to kilometers².
   /// </summary>
   public static decimal FACTOR_M2_TO_KM2 => FACTOR_M2_TO_AREA * FACTOR_AREA_TO_HECTARE * FACTOR_HECTARE_TO_KM2;

   /// <summary>
   /// Inch² to meters².
   /// </summary>
   public static decimal FACTOR_INCH2_TO_M2 => FACTOR_INCH_TO_CM2 * FACTOR_CM2_TO_M2;

   /// <summary>
   /// Meter² to miles².
   /// </summary>
   public static decimal FACTOR_M2_TO_MILE2 => FACTOR_M2_TO_KM2 * FACTOR_MILE2_TO_KM2;

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromAreaUnit">Source unit</param>
   /// <param name="toAreaUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value as decimal in the target unit</returns>
   public static decimal Convert<T>(this AreaUnit fromAreaUnit, AreaUnit toAreaUnit, T inVal) where T : INumber<T>
   {
      decimal val = inVal.BNToDecimal();

      if (IgnoreSameUnit && fromAreaUnit == toAreaUnit)
         return val;

      decimal outVal = 0; // = inVal;

      //Convert to m²
      switch (fromAreaUnit)
      {
         case AreaUnit.M2:
            //val = inVal;
            break;
         case AreaUnit.MM2:
            val /= FACTOR_MM2_TO_M2;
            break;
         case AreaUnit.CM2:
            val /= FACTOR_CM2_TO_M2;
            break;
         case AreaUnit.AREA:
            val *= FACTOR_M2_TO_AREA;
            break;
         case AreaUnit.HECTARE:
            val *= FACTOR_M2_TO_HECTARE;
            break;
         case AreaUnit.KM2:
            val *= FACTOR_M2_TO_KM2;
            break;
         case AreaUnit.INCH2:
            val /= FACTOR_INCH2_TO_M2;
            break;
         case AreaUnit.FOOT2:
            val *= FACTOR_FOOT2_TO_M2;
            break;
         case AreaUnit.YARD2:
            val *= FACTOR_YARD2_TO_M2;
            break;
         case AreaUnit.PERCH:
            val *= FACTOR_PERCH_TO_M2;
            break;
         case AreaUnit.ACRE:
            val *= FACTOR_ACRE_TO_M2;
            break;
         case AreaUnit.MILE2:
            val *= FACTOR_M2_TO_MILE2;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromAreaUnit}");
            break;
      }

      //Convert from m²
      switch (toAreaUnit)
      {
         case AreaUnit.M2:
            outVal = val;
            break;
         case AreaUnit.MM2:
            outVal = val * FACTOR_MM2_TO_M2;
            break;
         case AreaUnit.CM2:
            outVal = val * FACTOR_CM2_TO_M2;
            break;
         case AreaUnit.AREA:
            outVal = val / FACTOR_M2_TO_AREA;
            break;
         case AreaUnit.HECTARE:
            outVal = val / FACTOR_M2_TO_HECTARE;
            break;
         case AreaUnit.KM2:
            outVal = val / FACTOR_M2_TO_KM2;
            break;
         case AreaUnit.INCH2:
            outVal = val * FACTOR_INCH2_TO_M2;
            break;
         case AreaUnit.FOOT2:
            outVal = val / FACTOR_FOOT2_TO_M2;
            break;
         case AreaUnit.YARD2:
            outVal = val / FACTOR_YARD2_TO_M2;
            break;
         case AreaUnit.PERCH:
            outVal = val / FACTOR_PERCH_TO_M2;
            break;
         case AreaUnit.ACRE:
            outVal = val / FACTOR_ACRE_TO_M2;
            break;
         case AreaUnit.MILE2:
            outVal = val / FACTOR_M2_TO_MILE2;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toAreaUnit}");
            break;
      }

      return outVal;
   }
}