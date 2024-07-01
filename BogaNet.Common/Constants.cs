using System.Text.RegularExpressions;
using System.Globalization;
using System;

namespace BogaNet;

/// <summary>
/// Collected constants of very general utility.
/// </summary>
public abstract partial class Constants
{
   #region Constant variables

   #region Coding

   /// <summary>Float value of 32768.</summary>
   public const float FLOAT_32768 = 32768f;

   /// <summary>Float tolerance.</summary>
   public const float FLOAT_TOLERANCE = 0.0001f;

   /// <summary>ToString for two decimal places.</summary>
   public const string FORMAT_NUMBER_TWO_DECIMAL_PLACES = "0.00";

   /// <summary>ToString for no decimal places.</summary>
   public const string FORMAT_NUMBER_NO_DECIMAL_PLACES = "0";

   /// <summary>ToString for percent.</summary>
   public const string FORMAT_NUMBER_PERCENT = "0%";

   /// <summary>Path delimiter for Windows.</summary>
   public const string PATH_DELIMITER_WINDOWS = @"\";

   /// <summary>Path delimiter for Unix.</summary>
   public const string PATH_DELIMITER_UNIX = "/";

   #endregion

   #region Time

/*
   public const int MAX_SECOND_VALUE = 59;
   public const int MAX_MINUTE_VALUE = 59;
   public const int MAX_HOUR_VALUE = 23;
   public const int MAX_DAY_VALUE = 31;
   public const int MAX_MONTH_VALUE = 12;
   public const int MIN_YEAR_VALUE = -290000000;
   public const int MAX_YEAR_VALUE = 290000000;

   public const int HOURS_PER_DAY = 24;
   public const int DAYS_PER_WEEK = 7;
   public const int DAYS_PER_YEAR = 365;

   public const int MINUTES_PER_HOUR = 60;
   public const int MINUTES_PER_DAY = HOURS_PER_DAY * MINUTES_PER_HOUR;
   public const int MINUTES_PER_WEEK = DAYS_PER_WEEK * MINUTES_PER_DAY;
   public const int MINUTES_PER_YEAR = DAYS_PER_YEAR * MINUTES_PER_DAY;

   public const int SECONDS_PER_MINUTE = 60;
   public const int SECONDS_PER_HOUR = SECONDS_PER_MINUTE * MINUTES_PER_HOUR;
   public const int SECONDS_PER_DAY = HOURS_PER_DAY * SECONDS_PER_HOUR;
   public const int SECONDS_PER_WEEK = DAYS_PER_WEEK * SECONDS_PER_DAY;
   public const int SECONDS_PER_YEAR = DAYS_PER_YEAR * SECONDS_PER_DAY;

   public const long MILLISECONDS_PER_SECOND = 1000L;
   public const long MILLISECONDS_PER_MINUTE = SECONDS_PER_MINUTE * MILLISECONDS_PER_SECOND;
   public const long MILLISECONDS_PER_HOUR = MINUTES_PER_HOUR * MILLISECONDS_PER_MINUTE;
   public const long MILLISECONDS_PER_DAY = HOURS_PER_DAY * MILLISECONDS_PER_HOUR;
   public const long MILLISECONDS_PER_WEEK = DAYS_PER_WEEK * MILLISECONDS_PER_DAY;
   public const long MILLISECONDS_PER_YEAR = DAYS_PER_YEAR * MILLISECONDS_PER_DAY;
   public const long SECONDS_BETWEEN_1900_AND_1970 = 2208988800L;
*/
   /// <summary>Format for DateTime in ISO8601.</summary>
   public const string FORMAT_DATETIME_ISO8601 = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";

   #endregion

   #region Physic/Math constants

   /// <summary>
   /// Speed of light in m/s.
   /// </summary>
   public const decimal SPEED_OF_LIGHT = 299792458m;

   /// <summary>
   /// Absolute zero in Celsius.
   /// </summary>
   public const decimal ABSOLUTE_ZERO = -273.16m;

   /// <summary>
   /// Gravity on earth in m/s^2.
   /// </summary>
   public const decimal GRAVITY_ON_EARTH = 9.80665m;

   /// <summary>
   /// Kilogram calorie to kilojoule.
   /// </summary>
   public const decimal FACTOR_KCAL_TO_KJ = 4.1868m;

   /// <summary>
   /// Square root of 2
   /// </summary>
   public const decimal FACTOR_SQRT_2 = 1.414213562373m;

   /// <summary>
   /// Golden ratio between a and b
   /// </summary>
   public const decimal FACTOR_GOLDEN_RATIO_A_TO_B = 1.618033988749m;

   #endregion

   #region Strings

   public const string ALPHABET_LATIN_UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
   public const string ALPHABET_LATIN_LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
   public const string ALPHABET_EXT_UPPERCASE = "ÀÂÄÆÇÈÉÊËÎÏÔŒÙÛÜ";
   public const string ALPHABET_EXT_LOWERCASE = "àâäæçèéêëîïôœùûü";
   public const string ALPHABET_LATIN = $"{ALPHABET_LATIN_UPPERCASE}{ALPHABET_LATIN_LOWERCASE}";
   public const string ALPHABET_LATIN_EXT = $"{ALPHABET_LATIN_UPPERCASE}{ALPHABET_EXT_UPPERCASE}{ALPHABET_LATIN_LOWERCASE}{ALPHABET_EXT_LOWERCASE}";

   public const string NUMBERS = "0123456789";

   public const string SIGNS = $"{ALPHABET_LATIN}{NUMBERS}";
   public const string SIGNS_EXT = $"{ALPHABET_LATIN_EXT}{NUMBERS}";

   #endregion

   #region Regex

   private static Regex? _regexLineEndings;
   public static Regex REGEX_LINEENDINGS => _regexLineEndings ??= lineEndingRegex();

   private static Regex? _regexEmail;
   public static Regex REGEX_EMAIL => _regexEmail ??= emailRegex();

   private static Regex? _regexCreditCard;
   public static Regex REGEX_CREDITCARD => _regexCreditCard ??= creditcardRegex();

   private static Regex? _regexUrlWeb;

   public static Regex REGEX_URL_WEB => _regexUrlWeb ??= urlRegex();

/*
   private static Regex? _regexInvalidChars;
   public static Regex REGEX_INVALID_CHARS => _regexInvalidChars ??= new Regex(@"[^\w\.@-]");
*/
   private static Regex? _regexAlpha;
   public static Regex REGEX_ALPHANUMERIC => _regexAlpha ??= alphanumericRegex();

   private static Regex? _regexCleanSpace;
   public static Regex REGEX_CLEAN_SPACES => _regexCleanSpace ??= cleanSpaceRegex();

   private static Regex? _regexCleanTags;
   public static Regex REGEX_CLEAN_TAGS => _regexCleanTags ??= claenTagsRegex();

   private static Regex? _regexDriveLetters;
   public static Regex REGEX_DRIVE_LETTERS => _regexDriveLetters ??= driveRegex();

   private static Regex? _regexFile;

   //public static Regex REGEX_FILE => _regexFile ?? (_regexFile = new Regex(@"^\.[\w]+$"));
   public static Regex REGEX_FILE => _regexFile ??= fileRegex();

   private static Regex? _regexUnsignedInteger;
   public static Regex REGEX_UNSIGNED_INTEGER => _regexUnsignedInteger ??= uintRegex();

   private static Regex? _regexStartOfLine;
   public static Regex REGEX_START_OF_LINE => _regexStartOfLine ??= startLineRegex();

   private static Regex? _regexEndOfLine;
   public static Regex REGEX_END_OF_LINE => _regexEndOfLine ??= endLineRegex();

   private static Regex? _regexSpace;
   public static Regex REGEX_SPACE => _regexSpace ??= spaceRegex();

   private static Regex? _regexDomain;
   public static Regex REGEX_DOMAIN => _regexDomain ??= domainRegex();

   private static Regex? _regexUUID;
   public static Regex REGEX_UUID => _regexUUID ??= uuidRegex();

   //public static readonly Regex asciiOnlyRegex = new Regex(@"[^\u0000-\u00FF]+");
   //public static readonly Regex REGEX_REALNUMBER = new Regex(@"([-+]?[0-9]*\.?[0-9]+)");
   //public static readonly Regex REGEX_SIGNED_INTEGER = new Regex(@"([-+]?[0-9]+)");
   //public static readonly Regex cleanStringRegex = new Regex(@"([^a-zA-Z0-9 ]|[ ]{2,})");

   #endregion

   #endregion

   #region Properties

   /// <summary>
   /// True if the current platform is Windows.
   /// </summary>
   public static bool IsWindows => OperatingSystem.IsWindows();

   /// <summary>
   /// True if the current platform is Mac.
   /// </summary>
   public static bool IsOSX => OperatingSystem.IsMacOS();

   /// <summary>
   /// True if the current platform is Linux.
   /// </summary>
   public static bool IsLinux => OperatingSystem.IsLinux();

   /// <summary>
   /// True if the current platform is FreeBSD.
   /// </summary>
   public static bool IsFreeBSD => OperatingSystem.IsFreeBSD();

   /// <summary>
   /// True if the current platform is iOS.
   /// </summary>
   public static bool IsIOS => OperatingSystem.IsIOS();

   /// <summary>
   /// True if the current platform is Android.
   /// </summary>
   public static bool IsAndroid => OperatingSystem.IsAndroid();

   /// <summary>
   /// True if the current platform is a web browser.
   /// </summary>
   public static bool IsBrowser => OperatingSystem.IsBrowser();

   /// <summary>
   /// True if the current platform is Unix-based (=Linux, FreeBSD and OSX).
   /// </summary>
   public static bool IsUnix => IsLinux || IsFreeBSD || IsOSX;

   /// <summary>
   /// True if the current platform is PC-based (=Linux, FreeBSD, OSX and Windows).
   /// </summary>
   public static bool IsPC => IsLinux || IsWindows;

   /// <summary>
   /// True if the current platform is mobile-based (=iOS/Android).
   /// </summary>
   public static bool IsMobile => IsIOS || IsAndroid;

   // Prefixes for URLs and paths
   public const string PREFIX_HTTP = "http://";
   public const string PREFIX_HTTPS = "https://";
   public const string PREFIX_SMB = "smb://";
   public const string PREFIX_FTP = "ftp://";

   #endregion

   #region Changable variables

   /// <summary>The current culture of the application./// </summary>
   public static CultureInfo CurrentCulture = CultureInfo.CurrentCulture;

   /// <summary>Company owning the software.</summary>
   public static string COMPANY = "crosstales LLC";

   /// <summary>URL of the company.</summary>
   public static string COMPANY_URL = "https://www.crosstales.com/";

   /// <summary>URL of the company.</summary>
   public static string COMPANY_EMAIL = "welcome@crosstales.com";

   /// <summary>Path to the cmd under Windows.</summary>
   public static string CMD_WINDOWS_PATH = @"C:\Windows\system32\cmd.exe";

   #endregion

   #region Properties

   /// <summary>URL prefix for files.</summary>
   public static string PREFIX_FILE => IsWindows ? "file:///" : "file://";

   [GeneratedRegex(@"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+")]
   private static partial Regex lineEndingRegex();

   [GeneratedRegex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")]
   private static partial Regex emailRegex();

   [GeneratedRegex(@"^((\d{4}[- ]?){3}\d{4})$")]
   private static partial Regex creditcardRegex();

   [GeneratedRegex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$")]
   private static partial Regex urlRegex();

   [GeneratedRegex("([A-Za-z0-9_]+)")]
   private static partial Regex alphanumericRegex();

   [GeneratedRegex(@"\s+")]
   private static partial Regex cleanSpaceRegex();

   [GeneratedRegex("<.*?>")]
   private static partial Regex claenTagsRegex();

   [GeneratedRegex("^[a-zA-Z]:")]
   private static partial Regex driveRegex();

   [GeneratedRegex(@"^.*\.[\w]+$")]
   private static partial Regex fileRegex();

   [GeneratedRegex("([0-9]+)")]
   private static partial Regex uintRegex();

   [GeneratedRegex(@"^\s*")]
   private static partial Regex startLineRegex();

   [GeneratedRegex(@"\s*$")]
   private static partial Regex endLineRegex();

   [GeneratedRegex(@"\s*")]
   private static partial Regex spaceRegex();

   [GeneratedRegex(@"^([\-\w]+\.)+[a-zA-Z]{2,4}$")]
   private static partial Regex domainRegex();

   [GeneratedRegex("[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?")]
   private static partial Regex uuidRegex();

   #endregion

   #region some old Java-stuff...

   /*
    * factors
    */
   //		//time
   //		public static final BigDecimal FACTOR_NANOSECOND_TO_SECOND = new BigDecimal("1000000000"); //nanoseconds to seconds
   //		public static final BigDecimal FACTOR_MICROSECOND_TO_SECOND = HelperNumber.NUMBER_1000000; //microseconds to seconds
   //		public static final BigDecimal FACTOR_MILLISECOND_TO_SECOND = HelperNumber.NUMBER_1000; //milliseconds to seconds
   //		public static final BigDecimal FACTOR_SECOND_TO_MINUTE = new BigDecimal("60"); //seconds to minutes
   //		public static final BigDecimal FACTOR_MINUTE_TO_HOUR = new BigDecimal("60"); //minutes to hours
   //		public static final BigDecimal FACTOR_HOUR_TO_DAY = new BigDecimal("24"); //hours to days
   //		public static final BigDecimal FACTOR_DAY_TO_WEEK = new BigDecimal("7"); //days to weeks
   //		public static final BigDecimal FACTOR_DAY_TO_MONTH = new BigDecimal("30"); //days to months
   //		public static final BigDecimal FACTOR_DAY_TO_YEAR = new BigDecimal("365"); //days to years

   #endregion
}