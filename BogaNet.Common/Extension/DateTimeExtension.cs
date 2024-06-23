using System;
using System.Globalization;

namespace BogaNet;

/// <summary>
/// Extension methods for DateTime.
/// </summary>
public static class DateTimeExtension
{
   /// <summary>
   /// Converts the specified ISO 8601 representation of a date and time to its DateTime equivalent.
   /// </summary>
   /// <param name="isoString">ISO 8601 string representation to convert</param>
   /// <returns>The DateTime equivalent</returns>
   public static DateTime BNFromISO8601(this string isoString)
   {
      return DateTime.ParseExact(isoString, Constants.FORMAT_DATETIME_ISO8601, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
   }

   /// <summary>
   /// Formats a date in the standard ISO 8601 format.
   /// </summary>
   /// <param name="date">Date to format</param>
   /// <returns>The formatted date</returns>
   public static string BNToISO8601(this DateTime date)
   {
      return date.ToUniversalTime().ToString(Constants.FORMAT_DATETIME_ISO8601, CultureInfo.InvariantCulture);
   }
}