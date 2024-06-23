using Microsoft.Extensions.Logging;
using System.Numerics;

namespace BogaNet.Unit;

/// <summary>
/// Units for bytes.
/// </summary>
public enum ByteUnit
{
   BYTE,
   kB,
   MB,
   GB,
   TB,
   PB,
   EB,
   KiB,
   MiB,
   GiB,
   TiB,
   PiB,
   EiB
}

/// <summary>
/// Extension methods for ByteUnit.
/// </summary>
public static class ByteUnitExtension
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ByteUnitExtension));

   public static bool IgnoreSameUnit = true;

   /// <summary>Factor bytes to kibibyte.</summary>
   public const decimal FACTOR_BYTES_TO_KiB = 1024;

   /// <summary>Factor bytes to mebibyte.</summary>
   public const decimal FACTOR_BYTES_TO_MiB = FACTOR_BYTES_TO_KiB * 1024;

   /// <summary>Factor bytes to gibibyte.</summary>
   public const decimal FACTOR_BYTES_TO_GiB = FACTOR_BYTES_TO_MiB * 1024;

   /// <summary>Factor bytes to tebibyte.</summary>
   public const decimal FACTOR_BYTES_TO_TiB = FACTOR_BYTES_TO_GiB * 1024;

   /// <summary>Factor bytes to pebibyte.</summary>
   public const decimal FACTOR_BYTES_TO_PiB = FACTOR_BYTES_TO_TiB * 1024;

   /// <summary>Factor bytes to exbibyte.</summary>
   public const decimal FACTOR_BYTES_TO_EiB = FACTOR_BYTES_TO_PiB * 1024;

   /// <summary>Factor bytes to kilobyte.</summary>
   public const decimal FACTOR_BYTES_TO_kB = 1000;

   /// <summary>Factor bytes to megabyte.</summary>
   public const decimal FACTOR_BYTES_TO_MB = FACTOR_BYTES_TO_kB * 1000;

   /// <summary>Factor bytes to gigabyte.</summary>
   public const decimal FACTOR_BYTES_TO_GB = FACTOR_BYTES_TO_MB * 1000;

   /// <summary>Factor bytes to terabyte.</summary>
   public const decimal FACTOR_BYTES_TO_TB = FACTOR_BYTES_TO_GB * 1000;

   /// <summary>Factor bytes to petabyte.</summary>
   public const decimal FACTOR_BYTES_TO_PB = FACTOR_BYTES_TO_TB * 1000;

   /// <summary>Factor bytes to exabyte.</summary>
   public const decimal FACTOR_BYTES_TO_EB = FACTOR_BYTES_TO_PB * 1000;

   /// <summary>
   /// Converts a value from one unit to another.
   /// </summary>
   /// <param name="fromByteUnit">Source unit</param>
   /// <param name="toByteUnit">Target unit</param>
   /// <param name="inVal">Value of the source unit</param>
   /// <returns>Value as decimal in the target unit</returns>
   public static decimal Convert<T>(this ByteUnit fromByteUnit, ByteUnit toByteUnit, T inVal) where T : INumber<T>
   {
      decimal val = inVal.BNToDecimal();

      if (IgnoreSameUnit && fromByteUnit == toByteUnit)
         return val;

      decimal outVal = 0; // = inVal;

      //Convert to Byte
      switch (fromByteUnit)
      {
         case ByteUnit.BYTE:
            //val = inVal;
            break;
         case ByteUnit.KiB:
            val *= FACTOR_BYTES_TO_KiB;
            break;
         case ByteUnit.MiB:
            val *= FACTOR_BYTES_TO_MiB;
            break;
         case ByteUnit.GiB:
            val *= FACTOR_BYTES_TO_GiB;
            break;
         case ByteUnit.TiB:
            val *= FACTOR_BYTES_TO_TiB;
            break;
         case ByteUnit.PiB:
            val *= FACTOR_BYTES_TO_PiB;
            break;
         case ByteUnit.EiB:
            val *= FACTOR_BYTES_TO_EiB;
            break;
         case ByteUnit.kB:
            val *= FACTOR_BYTES_TO_kB;
            break;
         case ByteUnit.MB:
            val *= FACTOR_BYTES_TO_MB;
            break;
         case ByteUnit.GB:
            val *= FACTOR_BYTES_TO_GB;
            break;
         case ByteUnit.TB:
            val *= FACTOR_BYTES_TO_TB;
            break;
         case ByteUnit.PB:
            val *= FACTOR_BYTES_TO_PB;
            break;
         case ByteUnit.EB:
            val *= FACTOR_BYTES_TO_EB;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the fromUnit: {fromByteUnit}");
            break;
      }

      //Convert from Byte
      switch (toByteUnit)
      {
         case ByteUnit.BYTE:
            outVal = val;
            break;
         case ByteUnit.KiB:
            outVal = val / FACTOR_BYTES_TO_KiB;
            break;
         case ByteUnit.MiB:
            outVal = val / FACTOR_BYTES_TO_MiB;
            break;
         case ByteUnit.GiB:
            outVal = val / FACTOR_BYTES_TO_GiB;
            break;
         case ByteUnit.TiB:
            outVal = val / FACTOR_BYTES_TO_TiB;
            break;
         case ByteUnit.PiB:
            outVal = val / FACTOR_BYTES_TO_PiB;
            break;
         case ByteUnit.EiB:
            outVal = val / FACTOR_BYTES_TO_EiB;
            break;
         case ByteUnit.kB:
            outVal = val / FACTOR_BYTES_TO_kB;
            break;
         case ByteUnit.MB:
            outVal = val / FACTOR_BYTES_TO_MB;
            break;
         case ByteUnit.GB:
            outVal = val / FACTOR_BYTES_TO_GB;
            break;
         case ByteUnit.TB:
            outVal = val / FACTOR_BYTES_TO_TB;
            break;
         case ByteUnit.PB:
            outVal = val / FACTOR_BYTES_TO_PB;
            break;
         case ByteUnit.EB:
            outVal = val / FACTOR_BYTES_TO_EB;
            break;
         default:
            _logger.LogWarning($"There is no conversion for the toUnit: {toByteUnit}");
            break;
      }

      return outVal;
   }

   /// <summary>
   /// Converts a ByteUnit to a BitUnit.
   /// </summary>
   /// <param name="fromByteUnit">Source BitUnit</param>
   /// <param name="toBitUnit">Target ByteUnit</param>
   /// <param name="inVal">Value of the source BitUnit</param>
   /// <returns>Value as decimal in the target ByteUnit</returns>
   public static decimal Convert<T>(this ByteUnit fromByteUnit, BitUnit toBitUnit, T inVal) where T : INumber<T>
   {
      decimal val = System.Convert.ToDecimal(inVal);
      decimal bits = fromByteUnit.Convert(ByteUnit.BYTE, val) * 8;

      return BitUnit.BIT.Convert(toBitUnit, bits);
   }
}