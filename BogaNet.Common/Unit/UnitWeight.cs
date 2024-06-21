using Microsoft.Extensions.Logging;
using System.Numerics;
using System;

namespace BogaNet.Unit;

/// <summary>
/// Units for weight.
/// </summary>
public enum UnitWeight
{
   MILLIGRAM,
   GRAM,
   KILOGRAM,
   OUNCE,
   POUND,
   TON,
   //TODO add more exotic weights?
}

/// <summary>
/// Extension methods for UnitWeight.
/// </summary>
public static class ExtensionUnitWeight
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ExtensionUnitWeight));

   public static bool IgnoreSameUnit = true;

   public const decimal FACTOR_MILLIGRAM_TO_GRAM = 1000; //milligram to gram
   public const decimal FACTOR_GRAM_TO_KILOGRAM = 1000; //gram to kilogram
   public const decimal FACTOR_OUNCE_TO_GRAM = 28.34952m; //ounce to gram
   public const decimal FACTOR_POUND_TO_KILOGRAM = 0.453592m; //pound to kilogram
   public const decimal FACTOR_TON_TO_KILOGRAM = 907.1847m; //ton to kilogram

   public static decimal FACTOR_MILLIGRAM_TO_KILOGRAM => FACTOR_MILLIGRAM_TO_GRAM * FACTOR_GRAM_TO_KILOGRAM;

   public static decimal FACTOR_OUNCE_TO_KILOGRAM =>  FACTOR_OUNCE_TO_GRAM / FACTOR_GRAM_TO_KILOGRAM;

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromUnit">Source unit</param>
   /// <param name="toUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value in the target unit</returns>
   public static T BNConvert<T>(this UnitWeight fromUnit, UnitWeight toUnit, T inVal) where T : INumber<T>
   {
      if (IgnoreSameUnit && fromUnit == toUnit)
         return inVal;

      decimal val = Convert.ToDecimal(inVal);
      decimal outVal = 0; // = inVal;

      //Convert to kg
      switch (fromUnit)
      {
         case UnitWeight.KILOGRAM:
            //val = inVal;
            break;
         case UnitWeight.MILLIGRAM:
            val = val / FACTOR_MILLIGRAM_TO_KILOGRAM;
            break;
         case UnitWeight.GRAM:
            val = val / FACTOR_GRAM_TO_KILOGRAM;
            break;
         case UnitWeight.OUNCE:
            val = val * FACTOR_OUNCE_TO_KILOGRAM;
            break;
         case UnitWeight.POUND:
            val = val * FACTOR_POUND_TO_KILOGRAM;
            break;
         case UnitWeight.TON:
            val = val * FACTOR_TON_TO_KILOGRAM;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromUnit}");
            break;
      }

      //Convert from kg
      switch (toUnit)
      {
         case UnitWeight.KILOGRAM:
            outVal = val;
            break;
         case UnitWeight.MILLIGRAM:
            outVal = val * FACTOR_MILLIGRAM_TO_KILOGRAM;
            break;
         case UnitWeight.GRAM:
            outVal = val * FACTOR_GRAM_TO_KILOGRAM;
            break;
         case UnitWeight.OUNCE:
            outVal = val / FACTOR_OUNCE_TO_KILOGRAM;
            break;
         case UnitWeight.POUND:
            outVal = val / FACTOR_POUND_TO_KILOGRAM;
            break;
         case UnitWeight.TON:
            outVal = val / FACTOR_TON_TO_KILOGRAM;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toUnit}");
            break;
      }

      return T.CreateTruncating(outVal);
   }
}