using Microsoft.Extensions.Logging;
using System.Numerics;

namespace BogaNet.Unit;

/// <summary>
/// Units for lengths.
/// </summary>
public enum UnitLength
{
   M,
   MM,
   CM,
   KM,
   INCH,
   FOOT,
   YARD,
   MILE,
   NAUTICAL_MILE
   //TODO add more exotic lengths?
}

/// <summary>
/// Extension methods for UnitLength.
/// </summary>
public static class ExtensionUnitLength
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ExtensionUnitLength));

   public static bool IgnoreSameUnit = true;

   public const decimal FACTOR_INCH_TO_CM = 2.54m; //inch to centimeters
   public const decimal FACTOR_FOOT_TO_M = 0.3048m; //foot to meters
   public const decimal FACTOR_YARD_TO_M = 0.9144m; //yard to meters
   public const decimal FACTOR_MILE_TO_M = 1609.344m; //mile (terrestrial) to meters
   public const decimal FACTOR_NAUTICAL_MILE_TO_M = 1852m; //nautical mile to meters
   public const decimal FACTOR_MM_TO_CM = 10; //millimeters to centimeters
   public const decimal FACTOR_CM_TO_M = 100; //centimeters to meters
   public const decimal FACTOR_M_TO_KM = 1000; //meters to kilometers

   public static decimal FACTOR_MM_TO_M => FACTOR_MM_TO_CM * FACTOR_CM_TO_M;
   public static decimal FACTOR_INCH_TO_M => FACTOR_INCH_TO_CM / FACTOR_CM_TO_M;

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromUnit">Source unit</param>
   /// <param name="toUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value in the target unit</returns>
   public static T BNConvert<T>(this UnitLength fromUnit, UnitLength toUnit, T inVal) where T : INumber<T>
   {
      if (IgnoreSameUnit && fromUnit == toUnit)
         return inVal;

      decimal val = Convert.ToDecimal(inVal);
      decimal outVal = 0; // = inVal;

      //Convert to m
      switch (fromUnit)
      {
         case UnitLength.M:
            //val = inVal;
            break;
         case UnitLength.MM:
            val = val / FACTOR_MM_TO_M;
            break;
         case UnitLength.CM:
            val = val / FACTOR_CM_TO_M;
            break;
         case UnitLength.KM:
            val = val * FACTOR_M_TO_KM;
            break;
         case UnitLength.INCH:
            val = val * FACTOR_INCH_TO_M;
            break;
         case UnitLength.FOOT:
            val = val * FACTOR_FOOT_TO_M;
            break;
         case UnitLength.YARD:
            val = val * FACTOR_YARD_TO_M;
            break;
         case UnitLength.MILE:
            val = val * FACTOR_MILE_TO_M;
            break;
         case UnitLength.NAUTICAL_MILE:
            val = val * FACTOR_NAUTICAL_MILE_TO_M;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromUnit}");
            break;
      }

      //Convert from m
      switch (toUnit)
      {
         case UnitLength.M:
            outVal = val;
            break;
         case UnitLength.MM:
            outVal = val * FACTOR_MM_TO_M;
            break;
         case UnitLength.CM:
            outVal = val * FACTOR_CM_TO_M;
            break;
         case UnitLength.KM:
            outVal = val / FACTOR_M_TO_KM;
            break;
         case UnitLength.INCH:
            outVal = val / FACTOR_INCH_TO_M;
            break;
         case UnitLength.FOOT:
            outVal = val / FACTOR_FOOT_TO_M;
            break;
         case UnitLength.YARD:
            outVal = val / FACTOR_YARD_TO_M;
            break;
         case UnitLength.MILE:
            outVal = val / FACTOR_MILE_TO_M;
            break;
         case UnitLength.NAUTICAL_MILE:
            outVal = val / FACTOR_NAUTICAL_MILE_TO_M;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toUnit}");
            break;
      }

      return T.CreateTruncating(outVal);
   }
}