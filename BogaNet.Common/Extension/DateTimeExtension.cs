using System;
using System.Globalization;

namespace BogaNet.Extension;

/// <summary>
/// Extension methods for DateTime.
/// </summary>
public static class DateTimeExtension //NUnit
{
   #region Public methods

   /// <summary>
   /// Converts the specified ISO 8601 representation of a date and time to its DateTime equivalent.
   /// </summary>
   /// <param name="isoString">ISO 8601 string representation to convert</param>
   /// <returns>The DateTime equivalent</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static DateTime BNFromISO8601(this string isoString)
   {
      ArgumentException.ThrowIfNullOrEmpty(isoString);

      return DateTime.ParseExact(isoString, Constants.FORMAT_DATETIME_ISO8601, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
   }

   /// <summary>
   /// Formats a date in the standard ISO 8601 format.
   /// </summary>
   /// <param name="date">Date to format</param>
   /// <returns>The formatted date</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string BNToISO8601(this DateTime date)
   {
      ArgumentNullException.ThrowIfNull(date);

      return date.ToUniversalTime().ToString(Constants.FORMAT_DATETIME_ISO8601, CultureInfo.InvariantCulture);
   }

   /// <summary>
   /// Converts a DateTime from a given time zone to UTC.
   /// </summary>
   /// <param name="date">Date to convert</param>
   /// <param name="fromTZ">Origin time zone (optional, default: local)</param>
   /// <returns>Converted DateTime as UTC</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static DateTime BNConvertTimeZoneToUtc(this DateTime date, TimeZoneInfo? fromTZ = null)
   {
      ArgumentNullException.ThrowIfNull(date);

      return TimeZoneInfo.ConvertTimeToUtc(date, fromTZ ?? (date.Kind == DateTimeKind.Utc ? TimeZoneInfo.Utc : TimeZoneInfo.Local));
   }

   /// <summary>
   /// Converts a DateTime from UTC/local to a given time zone.
   /// </summary>
   /// <param name="date">Date to convert</param>
   /// <param name="toTZ">Destination time zone (optional, default: local)</param>
   /// <returns>Converted DateTime in the given time zone</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static DateTime BNConvertToTimeZone(this DateTime date, TimeZoneInfo? toTZ = null)
   {
      ArgumentNullException.ThrowIfNull(date);

      return date.Kind == DateTimeKind.Utc ? TimeZoneInfo.ConvertTimeFromUtc(date, toTZ ?? TimeZoneInfo.Local) : TimeZoneInfo.ConvertTime(date, toTZ ?? TimeZoneInfo.Local);
   }

   /// <summary>
   /// Specifies a kind (UTC/local) for a given DateTime.
   /// </summary>
   /// <param name="date">Date to specify the kind</param>
   /// <param name="kind">Kind for the DateTime (optional, default: UTC)</param>
   /// <returns>DateTime with the specified kind</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static DateTime BNSpecifyKind(this DateTime date, DateTimeKind? kind = null)
   {
      ArgumentNullException.ThrowIfNull(date);

      return DateTime.SpecifyKind(date, kind ?? DateTimeKind.Utc);
   }

   #endregion
}