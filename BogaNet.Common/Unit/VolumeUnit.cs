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
   PINT,
   GALLON,
   BARREL
   //TODO add more exotic volumes?
}

/// <summary>
/// Extension methods for VolumeUnit.
/// </summary>
public static class VolumeUnitExtension
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(VolumeUnitExtension));

   public static bool IgnoreSameUnit = true;

   public const decimal FACTOR_MM3_TO_CM3 = 1000; //millimeters^3 to centimeters^3
   public const decimal FACTOR_CM3_TO_L = 1000; //centimeters^3 to liter
   public const decimal FACTOR_L_TO_M3 = 1000; //liter to m^3
   public const decimal FACTOR_INCH3_TO_L = 0.016387064m; //cubic inch to liter
   public const decimal FACTOR_FOOT3_TO_L = 28.316864592m; //cubic foot to liter
   public const decimal FACTOR_PINT_TO_L = 0.473176473m; //pint to liter
   public const decimal FACTOR_GALLON_US_TO_L = 3.785411784m; //gallon to liter
   public const decimal FACTOR_BARREL_TO_L = 158.987294928m; //barrel to liter

   public static decimal FACTOR_MM3_TO_L => FACTOR_MM3_TO_CM3 * FACTOR_CM3_TO_L;

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromVolumeUnit">Source unit</param>
   /// <param name="toVolumeUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value in the target unit</returns>
   public static T Convert<T>(this VolumeUnit fromVolumeUnit, VolumeUnit toVolumeUnit, T inVal) where T : INumber<T>
   {
      if (IgnoreSameUnit && fromVolumeUnit == toVolumeUnit)
         return inVal;

      decimal val = System.Convert.ToDecimal(inVal);
      decimal outVal = 0; // = inVal;

      //Convert to liter
      switch (fromVolumeUnit)
      {
         case VolumeUnit.LITER:
            //val = inVal;
            break;
         case VolumeUnit.MM3:
            val = val / FACTOR_MM3_TO_L;
            break;
         case VolumeUnit.CM3:
            val = val / FACTOR_CM3_TO_L;
            break;
         case VolumeUnit.M3:
            val = val * FACTOR_L_TO_M3;
            break;
         case VolumeUnit.INCH3:
            val = val * FACTOR_INCH3_TO_L;
            break;
         case VolumeUnit.FOOT3:
            val = val * FACTOR_FOOT3_TO_L;
            break;
         case VolumeUnit.PINT:
            val = val * FACTOR_PINT_TO_L;
            break;
         case VolumeUnit.GALLON:
            val = val * FACTOR_GALLON_US_TO_L;
            break;
         case VolumeUnit.BARREL:
            val = val * FACTOR_BARREL_TO_L;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromVolumeUnit}");
            break;
      }

      //Convert from m
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
         case VolumeUnit.PINT:
            outVal = val / FACTOR_PINT_TO_L;
            break;
         case VolumeUnit.GALLON:
            outVal = val / FACTOR_GALLON_US_TO_L;
            break;
         case VolumeUnit.BARREL:
            outVal = val / FACTOR_BARREL_TO_L;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toVolumeUnit}");
            break;
      }

      return T.CreateTruncating(outVal);
   }
}