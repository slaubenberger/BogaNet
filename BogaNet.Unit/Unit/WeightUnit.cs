using Microsoft.Extensions.Logging;
using System.Numerics;
using BogaNet.Extension;

namespace BogaNet.Unit;

/// <summary>
/// Units for weight.
/// </summary>
public enum WeightUnit //NUnit
{
   KILOGRAM,
   MILLIGRAM,
   CENTIGRAM,
   DECIGRAM,
   GRAM,
   METRIC_TON,
   OUNCE,
   POUND,
   TON,
   STONE
}

/// <summary>
/// Extension methods for WeightUnit.
/// </summary>
public static class WeightUnitExtension
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(WeightUnitExtension));

   public static bool IgnoreSameUnit = true;

   /// <summary>
   /// Milligram to kilograms.
   /// </summary>
   public const decimal FACTOR_MILLIGRAM_TO_KILOGRAM = 0.000001m;

   /// <summary>
   /// Centigram to kilograms.
   /// </summary>
   public const decimal FACTOR_CENTIGRAM_TO_KILOGRAM = 0.00001m;

   /// <summary>
   /// Decigram to kilograms.
   /// </summary>
   public const decimal FACTOR_DECIGRAM_TO_KILOGRAM = 0.0001m;

   /// <summary>
   /// Gram to kilograms.
   /// </summary>
   public const decimal FACTOR_GRAM_TO_KILOGRAM = 0.001m;

   /// <summary>
   /// Metric ton to kilograms.
   /// </summary>
   public const decimal FACTOR_METRIC_TON_TO_KILOGRAM = 1000;

   /// <summary>
   /// Ounce to kilograms.
   /// </summary>
   public const decimal FACTOR_OUNCE_TO_KILOGRAM = 0.02834952m;

   /// <summary>
   /// Pound to kilograms.
   /// </summary>
   public const decimal FACTOR_POUND_TO_KILOGRAM = 0.453592m;

   /// <summary>
   /// Ton to kilograms.
   /// </summary>
   public const decimal FACTOR_TON_TO_KILOGRAM = 907.1847m;

   /// <summary>
   /// Stone to kilograms.
   /// </summary>
   public const decimal FACTOR_STONE_TO_KILOGRAM = 6.350293m;

   #endregion

   #region Public methods

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromWeightUnit">Source unit</param>
   /// <param name="toWeightUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value as decimal in the target unit</returns>
   public static decimal Convert<T>(this WeightUnit fromWeightUnit, WeightUnit toWeightUnit, T inVal) where T : INumber<T>
   {
      decimal val = inVal.BNToDecimal();

      if (IgnoreSameUnit && fromWeightUnit == toWeightUnit)
         return val;

      decimal outVal = 0; // = inVal;

      //Convert to kg
      switch (fromWeightUnit)
      {
         case WeightUnit.KILOGRAM:
            //val = inVal;
            break;
         case WeightUnit.MILLIGRAM:
            val *= FACTOR_MILLIGRAM_TO_KILOGRAM;
            break;
         case WeightUnit.CENTIGRAM:
            val *= FACTOR_CENTIGRAM_TO_KILOGRAM;
            break;
         case WeightUnit.DECIGRAM:
            val *= FACTOR_DECIGRAM_TO_KILOGRAM;
            break;
         case WeightUnit.GRAM:
            val *= FACTOR_GRAM_TO_KILOGRAM;
            break;
         case WeightUnit.METRIC_TON:
            val *= FACTOR_METRIC_TON_TO_KILOGRAM;
            break;
         case WeightUnit.OUNCE:
            val *= FACTOR_OUNCE_TO_KILOGRAM;
            break;
         case WeightUnit.POUND:
            val *= FACTOR_POUND_TO_KILOGRAM;
            break;
         case WeightUnit.TON:
            val *= FACTOR_TON_TO_KILOGRAM;
            break;
         case WeightUnit.STONE:
            val *= FACTOR_STONE_TO_KILOGRAM;
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
            outVal = val / FACTOR_MILLIGRAM_TO_KILOGRAM;
            break;
         case WeightUnit.CENTIGRAM:
            outVal = val / FACTOR_CENTIGRAM_TO_KILOGRAM;
            break;
         case WeightUnit.DECIGRAM:
            outVal = val / FACTOR_DECIGRAM_TO_KILOGRAM;
            break;
         case WeightUnit.GRAM:
            outVal = val / FACTOR_GRAM_TO_KILOGRAM;
            break;
         case WeightUnit.METRIC_TON:
            outVal = val / FACTOR_METRIC_TON_TO_KILOGRAM;
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
         case WeightUnit.STONE:
            outVal = val / FACTOR_STONE_TO_KILOGRAM;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toWeightUnit}");
            break;
      }

      return outVal;
   }

   #endregion
}