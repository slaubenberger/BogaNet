using System.Numerics;
using System;

namespace BogaNet.Helper;

/// <summary>
/// Various helper functions of very general utility.
/// </summary>
public abstract class GeneralHelper
{
   #region Variables

   //private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(Helper));

   #endregion

   #region Public methods

   /// <summary>
   /// Format byte (B) to Human-Readable-Form.
   /// </summary>
   /// <param name="bytes">Value in bytes</param>
   /// <param name="useSI">Use SI-system (optional, default: true)</param>
   /// <returns>Formatted byte-value in Human-Readable-Form</returns>
   public static string FormatBytesToHRF(long bytes, bool useSI = true)
   {
      return formatToHRF(bytes, useSI, "B");
   }

   /// <summary>
   /// Format bitrate (bit/s) to Human-Readable-Form.
   /// </summary>
   /// <param name="bits">Bitrate in bit/s</param>
   /// <param name="useSI">Use SI-system (optional, default: true)</param>
   /// <returns>Formatted bitrate in Human-Readable-Form</returns>
   public static string FormatBitrateToHRF(long bits, bool useSI = true)
   {
      return formatToHRF(bits, useSI, "bit/s");
   }

   /// <summary>
   /// Format seconds to Human-Readable-Form.
   /// </summary>
   /// <param name="seconds">Value in seconds</param>
   /// <returns>Formatted seconds in Human-Readable-Form</returns>
   public static string FormatSecondsToHRF<T>(T seconds) where T : INumber<T>
   {
      long val = Convert.ToInt64(seconds);

      bool wasMinus = val < 0;

      long totalSeconds = Math.Abs(val);
      long calcSeconds = totalSeconds % 60;

      if (val >= 86400)
      {
         long calcDays = totalSeconds / 86400;
         long calcHours = (totalSeconds -= calcDays * 86400) / 3600;
         long calcMinutes = (totalSeconds - calcHours * 3600) / 60;

         return $"{(wasMinus ? "-" : "")}{calcDays}d {calcHours}:{addLeadingZero(calcMinutes)}:{addLeadingZero(calcSeconds)}";
      }

      if (val >= 3600)
      {
         long calcHours = totalSeconds / 3600;
         long calcMinutes = (totalSeconds - calcHours * 3600) / 60;

         return $"{(wasMinus ? "-" : "")}{calcHours}:{addLeadingZero(calcMinutes)}:{addLeadingZero(calcSeconds)}";
      }
      else
      {
         long calcMinutes = totalSeconds / 60;

         return $"{(wasMinus ? "-" : "")}{calcMinutes}:{addLeadingZero(calcSeconds)}";
      }
   }

   /// <summary>
   /// Returns a CLI argument for a name from the command line.
   /// </summary>
   /// <param name="name">Name for the argument</param>
   /// <param name="args">Arguments to search for (optional)</param>
   /// <returns>Argument for a name from the command line</returns>
   public static string? GetCLIArgument(string? name, string[]? args = null)
   {
      if (!string.IsNullOrEmpty(name))
      {
         string[] cliArguments = args ?? GetCLIArguments();

         for (int ii = 0; ii < cliArguments.Length; ii++)
         {
            if (name.BNEquals(cliArguments[ii]) && cliArguments.Length > ii + 1)
               return cliArguments[ii + 1];
         }
      }

      return null;
   }

   /// <summary>
   /// Returns all CLI arguments.
   /// </summary>
   /// <returns>Arguments from the command line</returns>
   public static string[] GetCLIArguments()
   {
      return Environment.GetCommandLineArgs();
   }

/*
         /// <summary>Generates a string of all latin latin characters (ABC...xyz).</summary>
         /// <returns>"String of all latin latin characters</returns>
         public static string GenerateLatinABC()
         {
            return GenerateLatinUppercaseABC() + GenerateLatinLowercaseABC();
         }

         /// <summary>Generates a string of all latin latin characters in uppercase (ABC...XYZ).</summary>
         /// <returns>"String of all latin latin characters in uppercase</returns>
         public static string GenerateLatinUppercaseABC()
         {
            Text.StringBuilder result = new Text.StringBuilder();

            for (int ii = 65; ii <= 90; ii++)
            {
               result.Append((char)ii);
            }

            return result.ToString();
         }

         /// <summary>Generates a string of all latin latin characters in lowercase (abc...xyz).</summary>
         /// <returns>"String of all latin latin characters in lowercase</returns>
         public static string GenerateLatinLowercaseABC()
         {
            Text.StringBuilder result = new Text.StringBuilder();

            for (int ii = 97; ii <= 122; ii++)
            {
               result.Append((char)ii);
            }

            return result.ToString();
         }
   */

   #endregion

   #region Private methods

   private static string addLeadingZero(long value)
   {
      return value < 10 ? "0" + value : value.ToString();
   }

   private static string formatToHRF(long bits, bool useSI, string unit)
   {
      const string siIndex = "kMGTPE";
      const string binIndex = "KMGTPE";

      int index = 0;

      if (useSI)
      {
         if (bits is > -1000 and < 1000)
            return $"{bits} {unit}";

         while (bits is <= -999_950 or >= 999_950)
         {
            bits /= 1000;
            index++;
         }

         return $"{(bits / 1000.0):N2} {siIndex[index]}{unit}";
      }

      long absB = bits == long.MinValue ? long.MaxValue : Math.Abs(bits);
      if (absB < 1024)
         return $"{bits} {unit}";

      long value = absB;

      for (int ii = 40; ii >= 0 && absB > 0xfffccccccccccccL >> ii; ii -= 10)
      {
         value >>= 10;
         index++;
      }

      value *= Math.Sign(bits);

      return $"{(value / 1024f):N2} {binIndex[index]}i{unit}";
   }

   #endregion
}