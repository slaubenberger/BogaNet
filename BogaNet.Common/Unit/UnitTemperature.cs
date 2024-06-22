using Microsoft.Extensions.Logging;
using System.Numerics;
using System;

namespace BogaNet.Unit;

/// <summary>
/// Units for temperatures.
/// </summary>
public enum UnitTemperature
{
   KELVIN,
   CELSIUS,
   FAHRENHEIT,
}

/// <summary>
/// Extension methods for UnitTemperature.
/// </summary>
public static class ExtensionUnitTemperature
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ExtensionUnitTemperature));

   public static bool IgnoreSameUnit = true;

   public const decimal FACTOR_CELSIUS_TO_KELVIN = -Constants.ABSOLUTE_ZERO; //Celsius to Kelvin

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromUnit">Source unit</param>
   /// <param name="toUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value in the target unit</returns>
   public static T Convert<T>(this UnitTemperature fromUnit, UnitTemperature toUnit, T inVal) where T : INumber<T>
   {
      if (IgnoreSameUnit && fromUnit == toUnit)
         return inVal;

      decimal val = System.Convert.ToDecimal(inVal);
      decimal outVal = 0; // = inVal;
      decimal fcDiv = 1.8m;

      //Convert to Kelvin
      switch (fromUnit)
      {
         case UnitTemperature.KELVIN:
            //val = inVal;
            break;
         case UnitTemperature.CELSIUS:
            val = val + FACTOR_CELSIUS_TO_KELVIN;
            break;
         case UnitTemperature.FAHRENHEIT:
            val = ((val - 32) / fcDiv) + FACTOR_CELSIUS_TO_KELVIN;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromUnit}");
            break;
      }

      //Convert from Kelvin
      switch (toUnit)
      {
         case UnitTemperature.KELVIN:
            outVal = val;
            break;
         case UnitTemperature.CELSIUS:
            outVal = val - FACTOR_CELSIUS_TO_KELVIN;
            break;
         case UnitTemperature.FAHRENHEIT:
            outVal = ((val - FACTOR_CELSIUS_TO_KELVIN) * fcDiv) + 32;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toUnit}");
            break;
      }

      return T.CreateTruncating(outVal);
   }
}