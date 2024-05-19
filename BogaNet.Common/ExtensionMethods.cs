using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace BogaNet;

/// <summary>
/// Various extension methods.
/// </summary>
public static class ExtensionMethods
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger("ExtensionMethods");

   #endregion

   #region Object

   /// <summary>
   /// Extension method for objects.
   /// Adds a generic ToString-method to objects
   /// </summary>
   /// <param name="obj">Object for the generic ToString</param>
   /// /// <param name="bindingFlags">Used binding flags (optional, default: Instance|NonPublic|Public</param>
   /// <returns>Generic ToString</returns>
   public static string CTToString(this object? obj, System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.Instance |
                                                                                                   System.Reflection.BindingFlags.NonPublic |
                                                                                                   System.Reflection.BindingFlags.Public)
   {
      if (obj == null)
         return string.Empty;

      StringBuilder sb = new();

      sb.Append(obj.GetType().Name);
      sb.Append(":[");
      sb.Append(JsonHelper.SerializeToString(obj, JsonHelper.FORMAT_NONE));

      /*
      List<object?> listValues = obj.GetType().GetFields(bindingFlags).Select(field => field.GetValue(obj)).Where(value => value != null).ToList();
      foreach (object item in listValues)
      {
         // Note that you need to cast to string on objects that don't support ToSting() native! Maybe a new method to cast.
         sb.Append($"{item?.GetType().Name}='{item}', ");
      }

      List<object?> listProperties = obj.GetType().GetProperties(bindingFlags).Select(property => property.GetValue(obj)).Where(value => value != null).ToList();
      foreach (object item in listProperties)
      {
         sb.Append($"{item?.GetType().Name}='{item}', ");
      }
*/
      sb.Append(']');

      return sb.ToString();
   }

   #endregion

   #region Strings

   /// <summary>
   /// Extension method for strings.
   /// Converts a string to title case (first letter uppercase).
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>Converted string in title case.</returns>
   public static string? CTToTitleCase(this string? str)
   {
      return str == null ? str : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
   }

   /// <summary>
   /// Extension method for strings.
   /// Reverses a string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>Reversed string.</returns>
   public static string? CTReverse(this string? str)
   {
      if (str == null)
         return str;

      char[] charArray = str.ToCharArray();
      Array.Reverse(charArray);

      return new string(charArray);
   }

   /// <summary>
   /// Extension method for strings.
   /// Default: case insensitive 'Replace'.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="oldString">String to replace.</param>
   /// <param name="newString">New replacement string.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>Replaced string.</returns>
   public static string? CTReplace(this string? str, string? oldString, string? newString, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return str;

      if (oldString == null)
         return str;

      if (newString == null)
         return str;

      bool matchFound;
      do
      {
         int index = str.IndexOf(oldString, comp);

         matchFound = index >= 0;

         if (matchFound)
         {
            str = str.Remove(index, oldString.Length);

            str = str.Insert(index, newString);
         }
      } while (matchFound);

      return str;
   }

   /// <summary>
   /// Extension method for strings.
   /// Removes characters from a string
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="removeChars">Characters to remove.</param>
   /// <returns>String without the given characters.</returns>
   public static string? CTRemoveChars(this string? str, params char[]? removeChars)
   {
      if (str == null)
         return str;

      if (removeChars == null)
         return str;

      return removeChars.Aggregate(str, (current, rmChar) => current.Replace($"{rmChar}", string.Empty));
   }

   /// <summary>
   /// Extension method for strings.
   /// Default: case insensitive 'Equals'.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string contains the given string.</returns>
   public static bool CTEquals(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      return str?.Equals(toCheck, comp) == true;
   }

   /// <summary>
   /// Extension method for strings.
   /// Default: case insensitive 'Contains'.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string contains the given string.</returns>
   public static bool CTContains(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      return toCheck != null && str?.IndexOf(toCheck, comp) >= 0;
   }

   /// <summary>
   /// Extension method for strings.
   /// Contains any given string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="searchTerms">Search terms separated by the given split-character.</param>
   /// <param name="splitChar">Split-character (optional, default: ' ')</param>
   /// <returns>True if the string contains any parts of the given string.</returns>
   public static bool CTContainsAny(this string? str, string? searchTerms, char splitChar = ' ')
   {
      if (str == null)
         return false;

      if (string.IsNullOrEmpty(searchTerms))
         return true;

      char[] split = [splitChar];

      return searchTerms.Split(split, StringSplitOptions.RemoveEmptyEntries).Any(searchTerm => str.CTContains(searchTerm));
   }

   /// <summary>
   /// Extension method for strings.
   /// Contains all given strings.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="searchTerms">Search terms separated by the given split-character.</param>
   /// <param name="splitChar">Split-character (optional, default: ' ')</param>
   /// <returns>True if the string contains all parts of the given string.</returns>
   public static bool CTContainsAll(this string? str, string? searchTerms, char splitChar = ' ')
   {
      if (str == null)
         return false;

      if (string.IsNullOrEmpty(searchTerms))
         return true;

      char[] split = [splitChar];

      return searchTerms.Split(split, StringSplitOptions.RemoveEmptyEntries).All(searchTerm => str.CTContains(searchTerm));
   }

   /// <summary>
   /// Extension method for strings.
   /// Replaces new lines with a replacement string pattern.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="replacement">Replacement string pattern (optional, default: "#nl#").</param>
   /// <param name="newLine">New line string (optional, default: Environment.NewLine).</param>
   /// <returns>Replaced string without new lines.</returns>
   public static string? CTRemoveNewLines(this string? str, string? replacement = "#nl#", string? newLine = null)
   {
      return str?.Replace(string.IsNullOrEmpty(newLine) ? Environment.NewLine : newLine, replacement);
   }

   /// <summary>
   /// Extension method for strings.
   /// Replaces a given string pattern with new lines in a string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="replacement">Replacement string pattern (optional, default: "#nl#").</param>
   /// <param name="newLine">New line string (optional, default: Environment.NewLine).</param>
   /// <returns>Replaced string with new lines.</returns>
   public static string? CTAddNewLines(this string? str, string? replacement = "#nl#", string? newLine = null)
   {
      return str?.CTReplace(replacement, string.IsNullOrEmpty(newLine) ? Environment.NewLine : newLine);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string is numeric.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is numeric.</returns>
   public static bool CTIsNumeric(this string? str)
   {
      return str != null && double.TryParse(str, out double _);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string is integer.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is integer.</returns>
   public static bool CTIsInteger(this string? str)
   {
      if (str == null)
         return false;

      return !str.Contains('.') && long.TryParse(str, out long _);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string is an email address.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is an email address.</returns>
   public static bool CTIsEmail(this string? str)
   {
      return str != null && Constants.REGEX_EMAIL.IsMatch(str);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string is a website address.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is a website address.</returns>
   public static bool CTIsWebsite(this string? str)
   {
      return str != null && Constants.REGEX_URL_WEB.IsMatch(str);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string is a creditcard.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is a creditcard.</returns>
   public static bool CTIsCreditcard(this string? str)
   {
      return str != null && Constants.REGEX_CREDITCARD.IsMatch(str);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string is an IPv4 address.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is an IPv4 address.</returns>
   public static bool CTIsIPv4(this string? str)
   {
      return str != null && NetworkHelper.isIPv4(str);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string is alphanumeric.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is alphanumeric.</returns>
   public static bool CTIsAlphanumeric(this string? str)
   {
      return str != null && Constants.REGEX_ALPHANUMERIC.IsMatch(str);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string has line endings.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string has line endings.</returns>
   public static bool CTHasLineEndings(this string? str)
   {
      return str != null && Constants.REGEX_LINEENDINGS.IsMatch(str);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string has invalid characters.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string has invalid characters.</returns>
   public static bool CTHasInvalidChars(this string? str)
   {
      return str != null && Constants.REGEX_INVALID_CHARS.IsMatch(str);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string starts with another string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string is integer.</returns>
   public static bool CTStartsWith(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return false;

      return string.IsNullOrEmpty(toCheck) || str.StartsWith(toCheck, comp);
   }

   /// <summary>
   /// Extension method for strings.
   /// Checks if the string ends with another string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string is integer.</returns>
   public static bool CTEndsWith(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return false;

      return string.IsNullOrEmpty(toCheck) || str.EndsWith(toCheck, comp);
   }

   /// <summary>
   /// Extension method for strings.
   /// Returns the index of the last occurence of a given string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String for the index.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>The index of the last occurence of the given string if the string is integer.</returns>
   public static int CTLastIndexOf(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return 0;

      return string.IsNullOrEmpty(toCheck) ? 0 : str.LastIndexOf(toCheck, comp);
   }

   /// <summary>
   /// Extension method for strings.
   /// Returns the index of the first occurence of a given string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String for the index.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>The index of the first occurence of the given string if the string is integer.</returns>
   public static int CTIndexOf(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return 0;

      return string.IsNullOrEmpty(toCheck) ? 0 : str.IndexOf(toCheck, comp);
   }

   /// <summary>
   /// Extension method for strings.
   /// Returns the index of the first occurence of a given string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String for the index.</param>
   /// <param name="startIndex">Start index for the check.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>The index of the first occurence of the given string if the string is integer.</returns>
   public static int CTIndexOf(this string? str, string? toCheck, int startIndex, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return 0;

      return string.IsNullOrEmpty(toCheck) ? 0 : str.IndexOf(toCheck, startIndex, comp);
   }

   /// <summary>
   /// Extension method for strings.
   /// Converts the value of a string to a Base64-string.
   /// </summary>
   /// <param name="str">Input string.</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8).</param>
   /// <returns>String value as converted Base64-string.</returns>
   public static string? CTToBase64(this string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return _encoding.GetBytes(str).CTToBase64();
   }

   /// <summary>
   /// Extension method for strings.
   /// Converts the value of a Base64-string to a string.
   /// </summary>
   /// <param name="str">Input Base64-string.</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8).</param>
   /// <returns>Base64-string value as converted string.</returns>
   public static string? CTFromBase64(this string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      byte[]? base64 = str.CTFromBase64ToByteArray();
      return base64 == null ? null : _encoding.GetString(base64);
   }

   /// <summary>
   /// Extension method for strings.
   /// Converts the value of a Base64-string to a byte-array.
   /// </summary>
   /// <param name="str">Input Base64-string.</param>
   /// <returns>Base64-Byte-array from the Base64-string.</returns>
   public static byte[]? CTFromBase64ToByteArray(this string? str)
   {
      return str == null ? null : Convert.FromBase64String(str);
   }

   /// <summary>
   /// Extension method for strings.
   /// Converts the value of a string to a Hex-string (with Unicode support).
   /// </summary>
   /// <param name="str">Input string.</param>
   /// <param name="addPrefix">Add "0x"-as prefix (optional, default: false).</param>
   /// <returns>String value as converted Hex-string.</returns>
   public static string? CTToHex(this string? str, bool addPrefix = false)
   {
      if (str == null)
         return null;

      StringBuilder sb = new();

      if (addPrefix)
         sb.Append("0x");

      byte[] bytes = Encoding.Unicode.GetBytes(str);
      foreach (byte t in bytes)
      {
         sb.Append(t.ToString("X2"));
      }

      return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
   }

   /// <summary>
   /// Extension method for strings.
   /// Converts the Hex-value of a string to a string (with Unicode support).
   /// </summary>
   /// <param name="hexString">Input as Hex-string.</param>
   /// <returns>Hex-string value as converted string.</returns>
   public static string? CTHexToString(this string? hexString)
   {
      if (hexString == null)
         return null;

      string _hex = hexString;

      if (_hex.StartsWith("0x"))
         _hex = _hex.Substring(2);

      if (hexString.Length % 2 != 0)
         throw new FormatException($"String seems to be an invalid hex-code: {hexString}");

      byte[] bytes = new byte[_hex.Length / 2];
      for (int ii = 0; ii < bytes.Length; ii++)
      {
         bytes[ii] = Convert.ToByte(hexString.Substring(ii * 2, 2), 16);
      }

      //return Encoding.ASCII.GetString(bytes);
      return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
   }

   /// <summary>
   /// Extension method for strings.
   /// Converts the value of a string to a byte-array.
   /// </summary>
   /// <param name="str">Input string.</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8).</param>
   /// <returns>Byte-array with the string.</returns>
   public static byte[]? CTToByteArray(this string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return _encoding.GetBytes(str);
   }

   /// <summary>
   /// Extension method for strings.
   /// Cleans a given text from tags.
   /// </summary>
   /// <param name="str">Input to clean.</param>
   /// <returns>Clean text without tags.</returns>
   public static string? CTClearTags(this string? str)
   {
      return str != null ? Constants.REGEX_CLEAN_TAGS.Replace(str, string.Empty).Trim() : null;
   }

   /// <summary>
   /// Extension method for strings.
   /// Cleans a given text from multiple spaces.
   /// </summary>
   /// <param name="str">Input to clean.</param>
   /// <returns>Clean text without multiple spaces.</returns>
   public static string? CTClearSpaces(this string? str)
   {
      return str != null ? Constants.REGEX_CLEAN_SPACES.Replace(str, " ").Trim() : null;
   }

   /// <summary>
   /// Extension method for strings.
   /// Cleans a given text from line endings.
   /// </summary>
   /// <param name="str">Input to clean.</param>
   /// <returns>Clean text without line endings.</returns>
   public static string? CTClearLineEndings(this string? str)
   {
      return str != null ? Constants.REGEX_LINEENDINGS.Replace(str, string.Empty).Trim() : null;
   }

   /// <summary>
   /// Extension method for strings.
   /// Creates a fixed length string.
   /// </summary>
   /// <param name="str">Input to fix.</param>
   /// <param name="length">Length of the string</param>
   /// <param name="padRight">Right padding (otherwise left padding)</param>
   /// <returns>Fix length string</returns>
   public static string CTFixedLength(this string? str, int length, bool padRight = true)
   {
      if (padRight)
         return str == null ? new string(' ', length) : str.PadRight(length).Substring(0, length);

      return str == null ? new string(' ', length) : str.PadLeft(length).Substring(0, length);
   }

   #endregion

   #region Byte

   /// <summary>
   /// Extension method for byte.
   /// Represents the given byte a binary string.
   /// </summary>
   /// <param name="value">The byte to represent as binary string</param>
   /// <returns>Binary string</returns>
   public static string CTToBinary(this byte value) => Convert.ToString(value).PadLeft(8, '0');

   /// <summary>
   /// Extension method for byte.
   /// Determine if the bit at the provided index is set (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte whose index to check.</param>
   /// <param name="index">The bit index to check.</param>
   /// <returns>The mutated byte value</returns>
   public static bool CTIsBitSetAtIndex(this byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (value & (1 << (7 - index))) != 0;

   /// <summary>
   /// Extension method for byte.
   /// Set the bit value at the provided index (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte value whose bit to set.</param>
   /// <param name="index">The index of the bit to set.</param>
   /// <returns>The mutated byte value</returns>
   public static byte CTSetBitAtIndex(this byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (byte)(value | (1 << (7 - index)));

   /// <summary>
   /// Extension method for byte.
   /// Clear the bit value at the provided index (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte value whose bit to clear.</param>
   /// <param name="index">The index of the bit to clear.</param>
   /// <returns>The mutated byte value</returns>
   public static byte ClearBitAtIndex(this byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (byte)(value & ~(1 << (7 - index)));

   /// <summary>
   /// Extension method for byte.
   /// Toggle the bit value at the provided index (indexed from left-to-right).
   /// </summary>
   /// <param name="value">The byte value whose bit to toggle.</param>
   /// <param name="index">The index of the bit to toggle.</param>
   /// <returns>The mutated byte value</returns>
   public static byte CTToggleBitAtIndex(this byte value, byte index) => index > 7 ? throw new IndexOutOfRangeException("Index must be between 0 and 7 inclusive.") : (byte)(value ^ (1 << (7 - index)));

   #endregion

   #region Numbers

   /// <summary>
   /// Extension method for byte.
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static byte CTClamp(this byte value, byte min, byte max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Extension method for ushort.
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static ushort CTClamp(this ushort value, ushort min, ushort max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Extension method for short.
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static short CTClamp(this short value, short min, short max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Extension method for uint.
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static uint CTClamp(this uint value, uint min, uint max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Extension method for int.
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static int CTClamp(this int value, int min, int max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Extension method for ulong.
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static ulong CTClamp(this ulong value, ulong min, ulong max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Extension method for long.
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static long CTClamp(this long value, long min, long max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Extension method for float.
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static float CTClamp(this float value, float min, float max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   /// <summary>
   /// Extension method for double.
   /// Clamps the value between min and max
   /// </summary>
   /// <param name="value">Given value</param>
   /// <param name="min">Min value</param>
   /// <param name="max">Max value</param>
   /// <returns>Clamped value</returns>
   public static double CTClamp(this double value, double min, double max)
   {
      return value < min ? min : (value > max) ? max : value;
   }

   #endregion

   #region Arrays

/*
   /// <summary>
   /// Extension method for arrays.
   /// Shuffles an array.
   /// </summary>
   /// <param name="array">Array-instance to shuffle.</param>
   /// <param name="seed">Seed for the PRNG (optional, default: 0 (=standard))</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void CTShuffle<T>(this T[]? array, int seed = 0)
   {
      if (array == null || array.Length <= 0)
         throw new ArgumentNullException(nameof(array));

      Random rnd = seed == 0 ? new Random() : new Random(seed);
      int n = array.Length;
      while (n > 1)
      {
         int k = rnd.Next(n--);
         (array[n], array[k]) = (array[k], array[n]);
      }
   }


   /// <summary>
   /// Extension method for string arrays.
   /// Default: case insensitive 'Contains'.
   /// </summary>
   /// <param name="str">String array-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparer (optional, default: StringComparer.OrdinalIgnoreCase)</param>
   /// <returns>True if the string array contains the given string.</returns>
   public static bool CTContains(this string[]? str, string? toCheck, StringComparer? comp = null)
   {
      if (str == null)
         return false;

      comp ??= StringComparer.OrdinalIgnoreCase;

      return str.Contains(toCheck, comp);
   }

   /// <summary>
   /// Extension method for arrays.
   /// Dumps an array to a string.
   /// </summary>
   /// <param name="array">Array-instance to dump.</param>
   /// <param name="prefix">Prefix for every element (optional, default: empty).</param>
   /// <param name="postfix">Postfix for every element (optional, default: empty).</param>
   /// <param name="appendNewLine">Append new line, otherwise use the given delimiter (optional, default: false).</param>
   /// <param name="delimiter">Delimiter if appendNewLine is false (optional, default: "; ").</param>
   /// <returns>String with lines for all array entries.</returns>
   public static string? CTDump<T>(this T[]? array, string? prefix = "", string? postfix = "", bool appendNewLine = true, string delimiter = "; ")
   {
      if (array == null) // || array.Length <= 0)
         return null;

      StringBuilder sb = new();

      foreach (T element in array)
      {
         if (0 < sb.Length)
         {
            sb.Append(appendNewLine ? Environment.NewLine : delimiter);
         }

         sb.Append(prefix);
         sb.Append(element);
         sb.Append(postfix);
      }

      return sb.ToString();
   }

   /// <summary>
   /// Extension method for arrays.
   /// Generates a string array with all entries (via CTToString).
   /// </summary>
   /// <param name="array">Array-instance to ToString.</param>
   /// <returns>String array with all entries (via CTToString).</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string[] CTToStringArray<T>(this T[]? array)
   {
      if (array == null) // || array.Length <= 0)
         throw new ArgumentNullException(nameof(array));

      string[] result = new string[array.Length];

      for (int ii = 0; ii < array.Length; ii++)
      {
         string line = "null";

         T content = array[ii];

         if (content != null)
            line = content.CTToString()!;

         result[ii] = line;
      }

      return result;
   }
*/
   /// <summary>
   /// Extension method for byte-arrays.
   /// Converts a byte-array to a float-array.
   /// </summary>
   /// <param name="array">Array-instance to convert.</param>
   /// <param name="count">Number of bytes to convert (optional).</param>
   /// <returns>Converted float-array.</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static float[] CTToFloatArray(this byte[]? array, int count = 0)
   {
      if (array == null) // || array.Length <= 0)
         throw new ArgumentNullException(nameof(array));

      int _count = count;

      if (_count <= 0)
         _count = array.Length;

      float[] floats = new float[_count / 2];

      int ii = 0;
      for (int zz = 0; zz < _count; zz += 2)
      {
         floats[ii] = bytesToFloat(array[zz], array[zz + 1]);
         ii++;
      }

      return floats;
   }

   /// <summary>
   /// Extension method for float-arrays.
   /// Converts a float-array to a byte-array.
   /// </summary>
   /// <param name="array">Array-instance to convert.</param>
   /// <param name="count">Number of floats to convert (optional).</param>
   /// <returns>Converted byte-array.</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] CTToByteArray(this float[]? array, int count = 0)
   {
      if (array == null) // || array.Length <= 0)
         throw new ArgumentNullException(nameof(array));

      int _count = count;

      if (_count <= 0)
         _count = array.Length;

      byte[] bytes = new byte[_count * 2];
      int byteIndex = 0;

      for (int ii = 0; ii < _count; ii++)
      {
         short outsample = (short)(array[ii] * short.MaxValue);

         bytes[byteIndex] = (byte)(outsample & 0xff);

         bytes[byteIndex + 1] = (byte)((outsample >> 8) & 0xff);

         byteIndex += 2;
      }

      return bytes;
   }

   /// <summary>
   /// Extension method for byte-arrays.
   /// Converts a byte-array to a string.
   /// </summary>
   /// <param name="data">Input string as byte-array.</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8).</param>
   /// <returns>Byte-array with the string.</returns>
   public static string? CTConvertToString(this byte[]? data, Encoding? encoding = null)
   {
      if (data == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return _encoding.GetString(data);
   }

   /// <summary>
   /// Extension method for byte-arrays.
   /// Converts a byte-array to a Base64-string.
   /// </summary>
   /// <param name="data">Input as byte-array.</param>
   /// <returns>Base64-string from the byte-array.</returns>
   public static string? CTToBase64(this byte[]? data)
   {
      return data == null ? null : Convert.ToBase64String(data);
   }

   /// <summary>
   /// Extension method for 2D-arrays.
   /// Returns the column of a 2D-array as array.
   /// </summary>
   /// <param name="matrix">Input as 2D-array.</param>
   /// <param name="columnNumber">Desired column of the 2D-array</param>
   /// <returns>Column of a 2D-array as array.</returns>
   public static T[]? GetColumn<T>(this T[,]? matrix, int columnNumber)
   {
      return matrix != null ? Enumerable.Range(0, matrix.GetLength(0)).Select(x => matrix[x, columnNumber]).ToArray() : default;
   }

   /// <summary>
   /// Extension method for 2D-arrays.
   /// Returns the row of a 2D-array as array.
   /// </summary>
   /// <param name="matrix">Input as 2D-array.</param>
   /// <param name="rowNumber">Desired row of the 2D-array</param>
   /// <returns>Row of a 2D-array as array.</returns>
   public static T[]? GetRow<T>(this T[,]? matrix, int rowNumber)
   {
      return matrix != null ? Enumerable.Range(0, matrix.GetLength(1)).Select(x => matrix[rowNumber, x]).ToArray() : default;
   }

   #endregion

   #region Lists

   /// <summary>
   /// Extension method for IList.
   /// Generates a string list with all entries (via CTToString).
   /// </summary>
   /// <param name="list">IList-instance to ToString.</param>
   /// <returns>String list with all entries (via CTToString).</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string CTToString<T>(this IList<T>? list)
   {
      if (list == null)
         throw new ArgumentNullException(nameof(list));

      StringBuilder sb = new();

      sb.Append(list.GetType().Name);
      sb.Append(":[");

      for (int ii = 0; ii < list.Count; ii++)
      {
         sb.Append(list[ii].CTToString());

         if (ii < list.Count - 1)
            sb.Append(',');
      }

      sb.Append(']');

      return sb.ToString();
   }

   /// <summary>
   /// Extension method for IList.
   /// Shuffles a List.
   /// </summary>
   /// <param name="list">IList-instance to shuffle.</param>
   /// <param name="seed">Seed for the PRNG (optional, default: 0 (=standard))</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void CTShuffle<T>(this IList<T>? list, int seed = 0)
   {
      if (list == null)
         throw new ArgumentNullException(nameof(list));

      Random rnd = seed == 0 ? new Random() : new Random(seed);
      int n = list.Count;

      while (n > 1)
      {
         int k = rnd.Next(n--);
         (list[n], list[k]) = (list[k], list[n]);
      }
   }

   /// <summary>
   /// Extension method for IList.
   /// Dumps a list to a string.
   /// </summary>
   /// <param name="list">IList-instance to dump.</param>
   /// <param name="prefix">Prefix for every element (optional, default: empty).</param>
   /// <param name="postfix">Postfix for every element (optional, default: empty).</param>
   /// <param name="appendNewLine">Append new line, otherwise use the given delimiter (optional, default: false).</param>
   /// <param name="delimiter">Delimiter if appendNewLine is false (optional, default: "; ").</param>
   /// <returns>String with lines for all list entries.</returns>
   public static string? CTDump<T>(this IList<T>? list, string? prefix = "", string? postfix = "", bool appendNewLine = true, string delimiter = "; ")
   {
      if (list == null)
         return null;

      StringBuilder sb = new();

      foreach (T element in list)
      {
         if (0 < sb.Length)
         {
            sb.Append(appendNewLine ? Environment.NewLine : delimiter);
         }

         sb.Append(prefix);
         sb.Append(element.CTToString());
         sb.Append(postfix);
      }

      return sb.ToString();
   }

   /// <summary>
   /// Extension method for IList.
   /// Generates a string list with all entries (via CTToString).
   /// </summary>
   /// <param name="list">IList-instance to ToString.</param>
   /// <returns>String list with all entries (via CTToString).</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static List<string> CTToStringList<T>(this IList<T>? list)
   {
      if (list == null)
         throw new ArgumentNullException(nameof(list));

      List<string> result = new(list.Count);
      result.AddRange(list.Select(element => null == element ? "null" : element.CTToString()));

      return result;
   }

   /// <summary>
   /// Extension method for string lists.
   /// Default: case insensitive 'Contains'.
   /// </summary>
   /// <param name="str">String list-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparer (optional, default: StringComparer.OrdinalIgnoreCase)</param>
   /// <returns>True if the string list contains the given string.</returns>
   public static bool CTContains(this List<string>? str, string? toCheck, StringComparer? comp = null)
   {
      if (str == null)
         return false;

      comp ??= StringComparer.OrdinalIgnoreCase;

      return str.Contains(toCheck, comp);
   }

   #endregion

   #region Dictionaries

   /// <summary>
   /// Extension method for IDictionary.
   /// Dumps a dictionary to a string.
   /// </summary>
   /// <param name="dict">IDictionary-instance to dump.</param>
   /// <param name="prefix">Prefix for every element (optional, default: empty).</param>
   /// <param name="postfix">Postfix for every element (optional, default: empty).</param>
   /// <param name="appendNewLine">Append new line, otherwise use the given delimiter (optional, default: false).</param>
   /// <param name="delimiter">Delimiter if appendNewLine is false (optional, default: "; ").</param>
   /// <returns>String with lines for all dictionary entries.</returns>
   public static string? CTDump<K, V>(this IDictionary<K, V>? dict, string? prefix = "", string? postfix = "", bool appendNewLine = true, string delimiter = "; ")
   {
      if (dict == null)
         return null;

      StringBuilder sb = new();

      foreach (KeyValuePair<K, V> kvp in dict)
      {
         if (0 < sb.Length)
         {
            sb.Append(appendNewLine ? Environment.NewLine : delimiter);
         }

         sb.Append(prefix);
         sb.Append("Key = ");
         sb.Append(kvp.Key);
         sb.Append(", Value = ");
         sb.Append(kvp.Value.CTToString());
         sb.Append(postfix);
      }

      return sb.ToString();
   }

   /// <summary>
   /// Extension method for IDictionary.
   /// Adds a dictionary to an existing one.
   /// </summary>
   /// <param name="dict">IDictionary-instance.</param>
   /// <param name="collection">Dictionary to add.</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void CTAddRange<K, V>(this Dictionary<K, V>? dict, IDictionary<K, V>? collection) where K : notnull
   {
      if (dict == null)
         throw new ArgumentNullException(nameof(dict));

      if (collection == null)
         throw new ArgumentNullException(nameof(collection));

      foreach (KeyValuePair<K, V> item in collection)
      {
         if (!dict.ContainsKey(item.Key))
         {
            dict.Add(item.Key, item.Value);
         }
         else
         {
            // handle duplicate key issue here
            _logger.LogWarning($"Duplicate key found: {item.Key}");
         }
      }
   }

   #endregion

   #region Streams

   /// <summary>
   /// Extension method for Stream.
   /// Reads the full content of a Stream.
   /// </summary>
   /// <param name="input">Stream-instance to read.</param>
   /// <returns>Byte-array of the Stream content.</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] CTReadFully(this Stream? input)
   {
      if (input == null)
         throw new ArgumentNullException(nameof(input));

      using MemoryStream ms = new();
      input.CopyTo(ms);
      return ms.ToArray();
   }

   #endregion

   #region Private methods

   private static float bytesToFloat(byte firstByte, byte secondByte)
   {
      // convert two bytes to one short (little endian) and convert it to range from -1 to (just below) 1
      return (short)((secondByte << 8) | firstByte) / Constants.FLOAT_32768;
   }

   #endregion
}