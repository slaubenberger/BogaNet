using Microsoft.Extensions.Logging;
using System.Numerics;
using BogaNet.Extension;

namespace BogaNet.Unit;

/// <summary>
/// Units for lengths.
/// </summary>
public enum LengthUnit //NUnit
{
   M,
   MM,
   CM,
   DECIMETER,
   DECAMETER,
   HECTOMETER,
   KM,
   INCH,
   FOOT,
   YARD,
   MILE,
   NAUTICAL_MILE,
   POINT
}

/// <summary>
/// Extension methods for LengthUnit.
/// </summary>
public static class LengthUnitExtension
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(LengthUnitExtension));

   public static bool IgnoreSameUnit = true;

   /// <summary>
   /// Millimeter to meters.
   /// </summary>
   public const decimal FACTOR_MM_TO_M = 0.001m;

   /// <summary>
   /// Centimeter to meters.
   /// </summary>
   public const decimal FACTOR_CM_TO_M = 0.01m;

   /// <summary>
   /// Decimeter to meters.
   /// </summary>
   public const decimal FACTOR_DECIMETER_TO_M = 0.1m;

   /// <summary>
   /// Decameter to meters.
   /// </summary>
   public const decimal FACTOR_DECAMETER_TO_M = 10m;

   /// <summary>
   /// Hectometer to meters.
   /// </summary>
   public const decimal FACTOR_HECTOMETER_TO_M = 100m;

   /// <summary>
   /// Kilometers to meters.
   /// </summary>
   public const decimal FACTOR_KM_TO_M = 1000m;

   /// <summary>
   /// Inch to meters.
   /// </summary>
   public const decimal FACTOR_INCH_TO_M = 0.0254m;

   /// <summary>
   /// Foot to meters.
   /// </summary>
   public const decimal FACTOR_FOOT_TO_M = 0.3048m;

   /// <summary>
   /// Yard to meters.
   /// </summary>
   public const decimal FACTOR_YARD_TO_M = 0.9144m;

   /// <summary>
   /// Mile (terrestrial) to meters.
   /// </summary>
   public const decimal FACTOR_MILE_TO_M = 1609.344m;

   /// <summary>
   /// Nautical mile to meters.
   /// </summary>
   public const decimal FACTOR_NAUTICAL_MILE_TO_M = 1852m;

   /// <summary>
   /// Point to meters.
   /// </summary>
   public const decimal FACTOR_POINT_TO_M = 0.0003527778m;
   
   #endregion

   #region Public methods

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromLengthUnit">Source unit</param>
   /// <param name="toLengthUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value as decimal in the target unit</returns>
   public static decimal Convert<T>(this LengthUnit fromLengthUnit, LengthUnit toLengthUnit, T inVal) where T : INumber<T>
   {
      decimal val = inVal.BNToDecimal();

      if (IgnoreSameUnit && fromLengthUnit == toLengthUnit)
         return val;

      decimal outVal = 0; // = inVal;

      //Convert to m
      switch (fromLengthUnit)
      {
         case LengthUnit.M:
            //val = inVal;
            break;
         case LengthUnit.MM:
            val *= FACTOR_MM_TO_M;
            break;
         case LengthUnit.CM:
            val *= FACTOR_CM_TO_M;
            break;
         case LengthUnit.DECIMETER:
            val *= FACTOR_DECIMETER_TO_M;
            break;
         case LengthUnit.DECAMETER:
            val *= FACTOR_DECAMETER_TO_M;
            break;
         case LengthUnit.HECTOMETER:
            val *= FACTOR_HECTOMETER_TO_M;
            break;
         case LengthUnit.KM:
            val *= FACTOR_KM_TO_M;
            break;
         case LengthUnit.INCH:
            val *= FACTOR_INCH_TO_M;
            break;
         case LengthUnit.FOOT:
            val *= FACTOR_FOOT_TO_M;
            break;
         case LengthUnit.YARD:
            val *= FACTOR_YARD_TO_M;
            break;
         case LengthUnit.MILE:
            val *= FACTOR_MILE_TO_M;
            break;
         case LengthUnit.NAUTICAL_MILE:
            val *= FACTOR_NAUTICAL_MILE_TO_M;
            break;
         case LengthUnit.POINT:
            val *= FACTOR_POINT_TO_M;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromLengthUnit}");
            break;
      }

      //Convert from m
      switch (toLengthUnit)
      {
         case LengthUnit.M:
            outVal = val;
            break;
         case LengthUnit.MM:
            outVal = val / FACTOR_MM_TO_M;
            break;
         case LengthUnit.CM:
            outVal = val / FACTOR_CM_TO_M;
            break;
         case LengthUnit.DECIMETER:
            outVal = val / FACTOR_DECIMETER_TO_M;
            break;
         case LengthUnit.DECAMETER:
            outVal = val / FACTOR_DECAMETER_TO_M;
            break;
         case LengthUnit.HECTOMETER:
            outVal = val / FACTOR_HECTOMETER_TO_M;
            break;
         case LengthUnit.KM:
            outVal = val / FACTOR_KM_TO_M;
            break;
         case LengthUnit.INCH:
            outVal = val / FACTOR_INCH_TO_M;
            break;
         case LengthUnit.FOOT:
            outVal = val / FACTOR_FOOT_TO_M;
            break;
         case LengthUnit.YARD:
            outVal = val / FACTOR_YARD_TO_M;
            break;
         case LengthUnit.MILE:
            outVal = val / FACTOR_MILE_TO_M;
            break;
         case LengthUnit.NAUTICAL_MILE:
            outVal = val / FACTOR_NAUTICAL_MILE_TO_M;
            break;
         case LengthUnit.POINT:
            outVal = val / FACTOR_POINT_TO_M;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toLengthUnit}");
            break;
      }

      return outVal;
   }

   #endregion
}