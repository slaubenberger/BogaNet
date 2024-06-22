using Microsoft.Extensions.Logging;
using System.Numerics;

namespace BogaNet.Unit;

/// <summary>
/// Units for weight.
/// </summary>
public enum WeightUnit
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
/// Extension methods for WeightUnit.
/// </summary>
public static class WeightUnitExtension
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(WeightUnitExtension));

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
   /// <param name="fromWeightUnit">Source unit</param>
   /// <param name="toWeightUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value in the target unit</returns>
   public static T Convert<T>(this WeightUnit fromWeightUnit, WeightUnit toWeightUnit, T inVal) where T : INumber<T>
   {
      if (IgnoreSameUnit && fromWeightUnit == toWeightUnit)
         return inVal;

      decimal val = System.Convert.ToDecimal(inVal);
      decimal outVal = 0; // = inVal;

      //Convert to kg
      switch (fromWeightUnit)
      {
         case WeightUnit.KILOGRAM:
            //val = inVal;
            break;
         case WeightUnit.MILLIGRAM:
            val = val / FACTOR_MILLIGRAM_TO_KILOGRAM;
            break;
         case WeightUnit.GRAM:
            val = val / FACTOR_GRAM_TO_KILOGRAM;
            break;
         case WeightUnit.OUNCE:
            val = val * FACTOR_OUNCE_TO_KILOGRAM;
            break;
         case WeightUnit.POUND:
            val = val * FACTOR_POUND_TO_KILOGRAM;
            break;
         case WeightUnit.TON:
            val = val * FACTOR_TON_TO_KILOGRAM;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromWeightUnit}");
            break;
      }

      //Convert from kg
      switch (toWeightUnit)
      {
         case WeightUnit.KILOGRAM:
            outVal = val;
            break;
         case WeightUnit.MILLIGRAM:
            outVal = val * FACTOR_MILLIGRAM_TO_KILOGRAM;
            break;
         case WeightUnit.GRAM:
            outVal = val * FACTOR_GRAM_TO_KILOGRAM;
            break;
         case WeightUnit.OUNCE:
            outVal = val / FACTOR_OUNCE_TO_KILOGRAM;
            break;
         case WeightUnit.POUND:
            outVal = val / FACTOR_POUND_TO_KILOGRAM;
            break;
         case WeightUnit.TON:
            outVal = val / FACTOR_TON_TO_KILOGRAM;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toWeightUnit}");
            break;
      }

      return T.CreateTruncating(outVal);
   }
}