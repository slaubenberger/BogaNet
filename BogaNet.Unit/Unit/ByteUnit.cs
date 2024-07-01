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

   /// <summary>kibibyte to bytes.</summary>
   public const decimal FACTOR_KiB_TO_BYTES = 1024;

   /// <summary>mebibyte to bytes.</summary>
   public const decimal FACTOR_MiB_TO_BYTES = FACTOR_KiB_TO_BYTES * 1024;

   /// <summary>gibibyte to bytes.</summary>
   public const decimal FACTOR_GiB_TO_BYTES = FACTOR_MiB_TO_BYTES * 1024;

   /// <summary>tebibyte to bytes.</summary>
   public const decimal FACTOR_TiB_TO_BYTES = FACTOR_GiB_TO_BYTES * 1024;

   /// <summary>pebibyte to bytes.</summary>
   public const decimal FACTOR_PiB_TO_BYTES = FACTOR_TiB_TO_BYTES * 1024;

   /// <summary>exbibyte to bytes.</summary>
   public const decimal FACTOR_EiB_TO_BYTES = FACTOR_PiB_TO_BYTES * 1024;

   /// <summary>kilobyte to bytes.</summary>
   public const decimal FACTOR_kB_TO_BYTES = 1000;

   /// <summary>megabyte to bytes.</summary>
   public const decimal FACTOR_MB_TO_BYTES = FACTOR_kB_TO_BYTES * 1000;

   /// <summary>gigabyte to bytes.</summary>
   public const decimal FACTOR_GB_TO_BYTES = FACTOR_MB_TO_BYTES * 1000;

   /// <summary>terabyte to bytes.</summary>
   public const decimal FACTOR_TB_TO_BYTES = FACTOR_GB_TO_BYTES * 1000;

   /// <summary>petabyte to bytes.</summary>
   public const decimal FACTOR_PB_TO_BYTES = FACTOR_TB_TO_BYTES * 1000;

   /// <summary>exabyte to bytes.</summary>
   public const decimal FACTOR_EB_TO_BYTES = FACTOR_PB_TO_BYTES * 1000;

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
            val *= FACTOR_KiB_TO_BYTES;
            break;
         case ByteUnit.MiB:
            val *= FACTOR_MiB_TO_BYTES;
            break;
         case ByteUnit.GiB:
            val *= FACTOR_GiB_TO_BYTES;
            break;
         case ByteUnit.TiB:
            val *= FACTOR_TiB_TO_BYTES;
            break;
         case ByteUnit.PiB:
            val *= FACTOR_PiB_TO_BYTES;
            break;
         case ByteUnit.EiB:
            val *= FACTOR_EiB_TO_BYTES;
            break;
         case ByteUnit.kB:
            val *= FACTOR_kB_TO_BYTES;
            break;
         case ByteUnit.MB:
            val *= FACTOR_MB_TO_BYTES;
            break;
         case ByteUnit.GB:
            val *= FACTOR_GB_TO_BYTES;
            break;
         case ByteUnit.TB:
            val *= FACTOR_TB_TO_BYTES;
            break;
         case ByteUnit.PB:
            val *= FACTOR_PB_TO_BYTES;
            break;
         case ByteUnit.EB:
            val *= FACTOR_EB_TO_BYTES;
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
            outVal = val / FACTOR_KiB_TO_BYTES;
            break;
         case ByteUnit.MiB:
            outVal = val / FACTOR_MiB_TO_BYTES;
            break;
         case ByteUnit.GiB:
            outVal = val / FACTOR_GiB_TO_BYTES;
            break;
         case ByteUnit.TiB:
            outVal = val / FACTOR_TiB_TO_BYTES;
            break;
         case ByteUnit.PiB:
            outVal = val / FACTOR_PiB_TO_BYTES;
            break;
         case ByteUnit.EiB:
            outVal = val / FACTOR_EiB_TO_BYTES;
            break;
         case ByteUnit.kB:
            outVal = val / FACTOR_kB_TO_BYTES;
            break;
         case ByteUnit.MB:
            outVal = val / FACTOR_MB_TO_BYTES;
            break;
         case ByteUnit.GB:
            outVal = val / FACTOR_GB_TO_BYTES;
            break;
         case ByteUnit.TB:
            outVal = val / FACTOR_TB_TO_BYTES;
            break;
         case ByteUnit.PB:
            outVal = val / FACTOR_PB_TO_BYTES;
            break;
         case ByteUnit.EB:
            outVal = val / FACTOR_EB_TO_BYTES;
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