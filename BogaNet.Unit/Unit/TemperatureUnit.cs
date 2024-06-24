using Microsoft.Extensions.Logging;
using System.Numerics;

namespace BogaNet.Unit;

/// <summary>
/// Units for temperatures.
/// </summary>
public enum TemperatureUnit
{
   KELVIN,
   CELSIUS,
   FAHRENHEIT
}

/// <summary>
/// Extension methods for TemperatureUnit.
/// </summary>
public static class TemperatureUnitExtension
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(TemperatureUnitExtension));

   public static bool IgnoreSameUnit = true;

   /// <summary>
   /// Celsius to Kelvin.
   /// </summary>
   public const decimal FACTOR_CELSIUS_TO_KELVIN = -Constants.ABSOLUTE_ZERO;

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromTemperatureUnit">Source unit</param>
   /// <param name="toTemperatureUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value as decimal in the target unit</returns>
   public static decimal Convert<T>(this TemperatureUnit fromTemperatureUnit, TemperatureUnit toTemperatureUnit, T inVal) where T : INumber<T>
   {
      decimal val = inVal.BNToDecimal();

      if (IgnoreSameUnit && fromTemperatureUnit == toTemperatureUnit)
         return val;

      decimal outVal = 0; // = inVal;
      const decimal fcDiv = 1.8m;

      //Convert to Kelvin
      switch (fromTemperatureUnit)
      {
         case TemperatureUnit.KELVIN:
            //val = inVal;
            break;
         case TemperatureUnit.CELSIUS:
            val += FACTOR_CELSIUS_TO_KELVIN;
            break;
         case TemperatureUnit.FAHRENHEIT:
            val = ((val - 32) / fcDiv) + FACTOR_CELSIUS_TO_KELVIN;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromTemperatureUnit}");
            break;
      }

      //Convert from Kelvin
      switch (toTemperatureUnit)
      {
         case TemperatureUnit.KELVIN:
            outVal = val;
            break;
         case TemperatureUnit.CELSIUS:
            outVal = val - FACTOR_CELSIUS_TO_KELVIN;
            break;
         case TemperatureUnit.FAHRENHEIT:
            outVal = ((val - FACTOR_CELSIUS_TO_KELVIN) * fcDiv) + 32;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toTemperatureUnit}");
            break;
      }

      return outVal;
   }
}