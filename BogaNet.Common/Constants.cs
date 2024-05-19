using System.Text.RegularExpressions;

namespace BogaNet;

/// <summary>
/// Collected constants of very general utility.
/// </summary>
public abstract class Constants
{
   #region Constant variables

   #region Coding

   /// <summary>Factor for kilo bytes.</summary>
   public const int FACTOR_KB = 1024;

   /// <summary>Factor for mega bytes.</summary>
   public const int FACTOR_MB = FACTOR_KB * 1024;

   /// <summary>Factor for giga bytes.</summary>
   public const int FACTOR_GB = FACTOR_MB * 1024;

   /// <summary>Float value of 32768.</summary>
   public const float FLOAT_32768 = 32768f;

   /// <summary>Float tolerance.</summary>
   public const float FLOAT_TOLERANCE = 0.0001f;

   /// <summary>ToString for two decimal places.</summary>
   public const string FORMAT_TWO_DECIMAL_PLACES = "0.00";

   /// <summary>ToString for no decimal places.</summary>
   public const string FORMAT_NO_DECIMAL_PLACES = "0";

   /// <summary>ToString for percent.</summary>
   public const string FORMAT_PERCENT = "0%";

   /// <summary>Path delimiter for Windows.</summary>
   public const string PATH_DELIMITER_WINDOWS = @"\";

   /// <summary>Path delimiter for Unix.</summary>
   public const string PATH_DELIMITER_UNIX = "/";

   #endregion

   #region Time

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

   #endregion

   #region Physic/Math constants

   public const float SPEED_OF_LIGHT = 299792458f; //speed of light in m/s
   public const float ABSOLUTE_ZERO = -273.15f; //absolute zero in Celsius
   public const float GRAVITY_ON_EARTH = 9.80665f; //gravity on earth in m/s^2
   public const float FACTOR_KCAL_TO_KJ = 4.1868f; //kilogram calorie to kilojoule

   public const float FACTOR_SQRT_2 = 1.41421356f; //Square root of 2
   public const float FACTOR_GOLDEN_RATIO_A_TO_B = 1.6180339887f; //golden ratio between a and b

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
   public static Regex REGEX_LINEENDINGS => _regexLineEndings ??= new Regex(@"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+");

   private static Regex? _regexEmail;
   public static Regex REGEX_EMAIL => _regexEmail ??= new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");

   private static Regex? _regexCreditCard;
   public static Regex REGEX_CREDITCARD => _regexCreditCard ??= new Regex(@"^((\d{4}[- ]?){3}\d{4})$");

   private static Regex? _regexUrlWeb;
   public static Regex REGEX_URL_WEB => _regexUrlWeb ??= new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$");

   private static Regex? _regexIPAddress;
   public static Regex REGEX_IP_ADDRESS => _regexIPAddress ??= new Regex(@"^([0-9]{1,3}\.){3}[0-9]{1,3}$");

   private static Regex? _regexInvalidChars;
   public static Regex REGEX_INVALID_CHARS => _regexInvalidChars ??= new Regex(@"[^\w\.@-]");

   private static Regex? _regexAlpha;
   public static Regex REGEX_ALPHANUMERIC => _regexAlpha ??= new Regex("([A-Za-z0-9_]+)");

   private static Regex? _regexCleanSpace;
   public static Regex REGEX_CLEAN_SPACES => _regexCleanSpace ??= new Regex(@"\s+");

   private static Regex? _regexCleanTags;
   public static Regex REGEX_CLEAN_TAGS => _regexCleanTags ??= new Regex("<.*?>");

   private static Regex? _regexDriveLetters;
   public static Regex REGEX_DRIVE_LETTERS => _regexDriveLetters ??= new Regex("^[a-zA-Z]:");

   private static Regex? _regexFile;

   //public static Regex REGEX_FILE => _regexFile ?? (_regexFile = new Regex(@"^\.[\w]+$"));
   public static Regex REGEX_FILE => _regexFile ??= new Regex(@"^.*\.[\w]+$");

   private static Regex? _regexUnsignedInteger;
   public static Regex REGEX_UNSIGNED_INTEGER => _regexUnsignedInteger ??= new Regex("([0-9]+)");

   private static Regex? _regexStartOfLine;
   public static Regex REGEX_START_OF_LINE => _regexStartOfLine ??= new Regex(@"^\s*");

   private static Regex? _regexEndOfLine;
   public static Regex REGEX_END_OF_LINE => _regexEndOfLine ??= new Regex(@"\s*$");

   private static Regex? _regexSpace;
   public static Regex REGEX_SPACE => _regexSpace ??= new Regex(@"\s*");

   private static Regex? _regexDomain;
   public static Regex REGEX_DOMAIN => _regexDomain ??= new Regex(@"^([\-\w]+\.)+[a-zA-Z]{2,4}$");

   private static Regex? _regexUUID;
   public static Regex REGEX_UUID => _regexUUID ??= new Regex("[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?");

   //public static readonly Regex asciiOnlyRegex = new Regex(@"[^\u0000-\u00FF]+");
   //public static readonly Regex REGEX_REALNUMBER = new Regex(@"([-+]?[0-9]*\.?[0-9]+)");
   //public static readonly Regex REGEX_SIGNED_INTEGER = new Regex(@"([-+]?[0-9]+)");
   //public static readonly Regex cleanStringRegex = new Regex(@"([^a-zA-Z0-9 ]|[ ]{2,})");

   #endregion

   #endregion

   #region Changable variables

   /// <summary>Company owning the software.</summary>
   public static string COMPANY = "crosstales LLC";

   /// <summary>URL of the company.</summary>
   public static string COMPANY_URL = "https://www.crosstales.com/";

   /// <summary>URL of the company.</summary>
   public static string COMPANY_EMAIL = "welcome@crosstales.com";

   // Prefixes for URLs and paths
   public const string PREFIX_HTTP = "http://";

   public const string PREFIX_HTTPS = "https://";
   //public const string PREFIX_SMB = "smb://";
   //public const string PREFIX_FTP = "ftp://";

   /// <summary>Path to the cmd under Windows.</summary>
   public static string CMD_WINDOWS_PATH = @"C:\Windows\system32\cmd.exe";

   #endregion

   #region Properties

   /// <summary>URL prefix for files.</summary>
   public static string PREFIX_FILE
   {
      get
      {
         //TODO determine platform

         //if ("Windows")
         //{
         //   return "file:///";
         //}

         return "file://";
      }
   }

   #endregion

   #region old, but interesting Java-stuff :-)

   //TODO convert it when being used or free-time is available...

   /*
    * factors
    */
//		//area
//		public static final BigDecimal FACTOR_MM2_TO_CM2 = HelperNumber.NUMBER_100; //millimeters^2 to centimeters^2
//		public static final BigDecimal FACTOR_CM2_TO_M2 = HelperNumber.NUMBER_10000; //centimeters^2 to meters^2
//		public static final BigDecimal FACTOR_M2_TO_AREA = HelperNumber.NUMBER_100; //meters^2 to area
//		public static final BigDecimal FACTOR_AREA_TO_HECTARE = HelperNumber.NUMBER_100; //area to hectare
//		public static final BigDecimal FACTOR_HECTARE_TO_KM2 = HelperNumber.NUMBER_100; //hectare to kilometers^2
//		public static final BigDecimal FACTOR_FOOT2_TO_M2 = new BigDecimal("0.09290304"); //square foot to meters^2
//		public static final BigDecimal FACTOR_YARD2_TO_M2 = new BigDecimal("0.83612736"); //square yard to meters^2
//		public static final BigDecimal FACTOR_PERCH_TO_M2 = new BigDecimal("25.2928526"); //square perch to meters^2
//		public static final BigDecimal FACTOR_ACRE_TO_M2 = new BigDecimal("4046.8564224"); //acre to meters^2
//		public static final BigDecimal FACTOR_MILE2_TO_KM2 = new BigDecimal("2.5899881103"); //square mile (terrestrial) to kilometers^2
//		
//		//bit
//		public static final BigDecimal FACTOR_BIT_TO_BYTE = HelperNumber.NUMBER_8; //bit to byte
//		public static final BigDecimal FACTOR_BIT_TO_KILOBIT = new BigDecimal("10E2");
//		public static final BigDecimal FACTOR_BIT_TO_MEGABIT = new BigDecimal("10E5");
//		public static final BigDecimal FACTOR_BIT_TO_GIGABIT = new BigDecimal("10E8");
//		public static final BigDecimal FACTOR_BIT_TO_TERABIT = new BigDecimal("10E11");
//		public static final BigDecimal FACTOR_BIT_TO_PETABIT = new BigDecimal("10E14");
//		public static final BigDecimal FACTOR_BIT_TO_EXABIT = new BigDecimal("10E17");
//		public static final BigDecimal FACTOR_BIT_TO_ZETTABIT = new BigDecimal("10E20");
//		public static final BigDecimal FACTOR_BIT_TO_YOTTABIT = new BigDecimal("10E23");
//		public static final BigDecimal FACTOR_BIT_TO_KILOBYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_KILOBIT);
//		public static final BigDecimal FACTOR_BIT_TO_MEGABYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_MEGABIT);
//		public static final BigDecimal FACTOR_BIT_TO_GIGABYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_GIGABIT);
//		public static final BigDecimal FACTOR_BIT_TO_TERABYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_TERABIT);
//		public static final BigDecimal FACTOR_BIT_TO_PETABYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_PETABIT);
//		public static final BigDecimal FACTOR_BIT_TO_EXABYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_EXABIT);
//		public static final BigDecimal FACTOR_BIT_TO_ZETTABYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_ZETTABIT);
//		public static final BigDecimal FACTOR_BIT_TO_YOTTABYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_YOTTABIT);
//		public static final BigDecimal FACTOR_BIT_TO_KIBIBIT = HelperNumber.NUMBER_1024;
//		public static final BigDecimal FACTOR_BIT_TO_MEBIBIT = FACTOR_BIT_TO_KIBIBIT.multiply(HelperNumber.NUMBER_1024);
//		public static final BigDecimal FACTOR_BIT_TO_GIBIBIT = FACTOR_BIT_TO_MEBIBIT.multiply(HelperNumber.NUMBER_1024);
//		public static final BigDecimal FACTOR_BIT_TO_TEBIBIT = FACTOR_BIT_TO_GIBIBIT.multiply(HelperNumber.NUMBER_1024);
//		public static final BigDecimal FACTOR_BIT_TO_PEBIBIT = FACTOR_BIT_TO_TEBIBIT.multiply(HelperNumber.NUMBER_1024);
//		public static final BigDecimal FACTOR_BIT_TO_EXBIBIT = FACTOR_BIT_TO_PEBIBIT.multiply(HelperNumber.NUMBER_1024);
//		public static final BigDecimal FACTOR_BIT_TO_ZEBIBIT = FACTOR_BIT_TO_EXBIBIT.multiply(HelperNumber.NUMBER_1024);
//		public static final BigDecimal FACTOR_BIT_TO_YOBIBIT = FACTOR_BIT_TO_ZEBIBIT.multiply(HelperNumber.NUMBER_1024);
//		public static final BigDecimal FACTOR_BIT_TO_KIBIBYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_KIBIBIT);
//		public static final BigDecimal FACTOR_BIT_TO_MEBIBYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_MEBIBIT);
//		public static final BigDecimal FACTOR_BIT_TO_GIBIBYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_GIBIBIT);
//		public static final BigDecimal FACTOR_BIT_TO_TEBIBYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_TEBIBIT);
//		public static final BigDecimal FACTOR_BIT_TO_PEBIBYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_PEBIBIT);
//		public static final BigDecimal FACTOR_BIT_TO_EXBIBYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_EXBIBIT);
//		public static final BigDecimal FACTOR_BIT_TO_ZEBIBYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_ZEBIBIT);
//		public static final BigDecimal FACTOR_BIT_TO_YOBIBYTE = FACTOR_BIT_TO_BYTE.multiply(FACTOR_BIT_TO_YOBIBIT);
//		
//		//length
//		public static final BigDecimal FACTOR_INCH_TO_CM = new BigDecimal("2.54"); //inch to centimeters
//		public static final BigDecimal FACTOR_FOOT_TO_M = new BigDecimal("0.3048"); //foot to meters
//		public static final BigDecimal FACTOR_YARD_TO_M = new BigDecimal("0.9144"); //yard to meters
//		public static final BigDecimal FACTOR_MILE_TO_M = new BigDecimal("1609.344"); //mile (terrestrial) to meters
//		public static final BigDecimal FACTOR_NAUTICAL_MILE_TO_M = new BigDecimal("1852"); //nautical mile to meters
//		public static final BigDecimal FACTOR_MM_TO_CM = BigDecimal.TEN; //millimeters to centimeters
//		public static final BigDecimal FACTOR_CM_TO_M = HelperNumber.NUMBER_100; //centimeters to meters
//		public static final BigDecimal FACTOR_M_TO_KM = HelperNumber.NUMBER_1000; //meters to kilometers
//		
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
//		
//		//volume
//		public static final BigDecimal FACTOR_MM3_TO_CM3 = HelperNumber.NUMBER_1000; //millimeters^3 to centimeters^3
//		public static final BigDecimal FACTOR_CM3_TO_L = HelperNumber.NUMBER_1000; //centimeters^3 to liter
//		public static final BigDecimal FACTOR_L_TO_M3 = HelperNumber.NUMBER_1000; //liter to m^3
//		public static final BigDecimal FACTOR_PINT_TO_CM3 = new BigDecimal("473.176473"); //pObscuredInt to centimeters^3
//		public static final BigDecimal FACTOR_QUART_TO_L = new BigDecimal("0.946326"); //quart to liter
//		public static final BigDecimal FACTOR_GALLON_US_TO_L = new BigDecimal("3.785411784"); //gallon to liter
//		public static final BigDecimal FACTOR_BARREL_TO_L = new BigDecimal("158.987294928"); //barrel to liter
//		
//		//weight
//		public static final BigDecimal FACTOR_MILLIGRAM_TO_GRAM = HelperNumber.NUMBER_1000; //milligram to gram
//		public static final BigDecimal FACTOR_GRAM_TO_KILOGRAM = HelperNumber.NUMBER_1000; //gram to kilogram
//		public static final BigDecimal FACTOR_OUNCE_TO_GRAM = new BigDecimal("28.34952"); //ounce to gram
//		public static final BigDecimal FACTOR_POUND_TO_KILOGRAM = new BigDecimal("0.453592"); //pound to kilogram
//		public static final BigDecimal FACTOR_TON_TO_KILOGRAM = new BigDecimal("907.1847"); //ton to kilogram

   #endregion
}