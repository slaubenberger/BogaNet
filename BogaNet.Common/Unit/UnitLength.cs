using Microsoft.Extensions.Logging;

namespace BogaNet;

/// <summary>
/// Units for lengths
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
/// Extension methods for UnitLength
/// </summary>
public static class ExtensionUnitLength
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger("ExtensionUnitArea");

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

   public static decimal Convert(this UnitLength fromUnit, UnitLength toUnit, decimal inVal)
   {
      if (IgnoreSameUnit && fromUnit == toUnit)
         return inVal;

      decimal val = inVal;
      decimal outVal = 0; // = inVal;

      //Convert to m
      switch (fromUnit)
      {
         case UnitLength.M:
            val = inVal;
            break;
         case UnitLength.MM:
            val = inVal / FACTOR_MM_TO_M;
            break;
         case UnitLength.CM:
            val = inVal / FACTOR_CM_TO_M;
            break;
         case UnitLength.KM:
            val = inVal * FACTOR_M_TO_KM;
            break;
         case UnitLength.INCH:
            val = inVal * FACTOR_INCH_TO_M;
            break;
         case UnitLength.FOOT:
            val = inVal * FACTOR_FOOT_TO_M;
            break;
         case UnitLength.YARD:
            val = inVal * FACTOR_YARD_TO_M;
            break;
         case UnitLength.MILE:
            val = inVal * FACTOR_MILE_TO_M;
            break;
         case UnitLength.NAUTICAL_MILE:
            val = inVal * FACTOR_NAUTICAL_MILE_TO_M;
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

      return outVal;
   }
}