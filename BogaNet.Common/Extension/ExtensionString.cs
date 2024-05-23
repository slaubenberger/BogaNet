using System.Globalization;
using System.Text;
using System.Numerics;

namespace BogaNet;

/// <summary>
/// Extension methods for strings.
/// </summary>
public static class ExtensionString
{
   /// <summary>
   /// Converts a string to title case (first letter uppercase).
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>Converted string in title case.</returns>
   public static string? BNToTitleCase(this string? str)
   {
      return str == null ? str : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
   }

   /// <summary>
   /// Reverses a string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>Reversed string.</returns>
   public static string? BNReverse(this string? str)
   {
      if (str == null)
         return str;

      char[] charArray = str.ToCharArray();
      Array.Reverse(charArray);

      return new string(charArray);
   }

   /// <summary>
   /// Case insensitive 'Replace' per default.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="oldString">String to replace.</param>
   /// <param name="newString">New replacement string.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>Replaced string.</returns>
   public static string? BNReplace(this string? str, string? oldString, string? newString, StringComparison comp = StringComparison.OrdinalIgnoreCase)
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
   /// Removes characters from a string
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="removeChars">Characters to remove.</param>
   /// <returns>String without the given characters.</returns>
   public static string? BNRemoveChars(this string? str, params char[]? removeChars)
   {
      if (str == null)
         return str;

      if (removeChars == null)
         return str;

      return removeChars.Aggregate(str, (current, rmChar) => current.Replace($"{rmChar}", string.Empty));
   }

   /// <summary>
   /// Case insensitive 'Equals' per default.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string contains the given string.</returns>
   public static bool BNEquals(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      return str?.Equals(toCheck, comp) == true;
   }

   /// <summary>
   /// Case insensitive 'Contains' per default.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string contains the given string.</returns>
   public static bool BNContains(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      return toCheck != null && str?.IndexOf(toCheck, comp) >= 0;
   }

   /// <summary>
   /// Contains any given string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="searchTerms">Search terms separated by the given split-character.</param>
   /// <param name="splitChar">Split-character (optional, default: ' ')</param>
   /// <returns>True if the string contains any parts of the given string.</returns>
   public static bool BNContainsAny(this string? str, string? searchTerms, char splitChar = ' ')
   {
      if (str == null)
         return false;

      if (string.IsNullOrEmpty(searchTerms))
         return true;

      char[] split = [splitChar];

      return searchTerms.Split(split, StringSplitOptions.RemoveEmptyEntries).Any(searchTerm => str.BNContains(searchTerm));
   }

   /// <summary>
   /// Contains all given strings.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="searchTerms">Search terms separated by the given split-character.</param>
   /// <param name="splitChar">Split-character (optional, default: ' ')</param>
   /// <returns>True if the string contains all parts of the given string.</returns>
   public static bool BNContainsAll(this string? str, string? searchTerms, char splitChar = ' ')
   {
      if (str == null)
         return false;

      if (string.IsNullOrEmpty(searchTerms))
         return true;

      char[] split = [splitChar];

      return searchTerms.Split(split, StringSplitOptions.RemoveEmptyEntries).All(searchTerm => str.BNContains(searchTerm));
   }

   /// <summary>
   /// Replaces new lines with a replacement string pattern.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="replacement">Replacement string pattern (optional, default: "#nl#").</param>
   /// <param name="newLine">New line string (optional, default: Environment.NewLine).</param>
   /// <returns>Replaced string without new lines.</returns>
   public static string? BNRemoveNewLines(this string? str, string? replacement = "#nl#", string? newLine = null)
   {
      return str?.Replace(string.IsNullOrEmpty(newLine) ? Environment.NewLine : newLine, replacement);
   }

   /// <summary>
   /// Replaces a given string pattern with new lines in a string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="replacement">Replacement string pattern (optional, default: "#nl#").</param>
   /// <param name="newLine">New line string (optional, default: Environment.NewLine).</param>
   /// <returns>Replaced string with new lines.</returns>
   public static string? BNAddNewLines(this string? str, string? replacement = "#nl#", string? newLine = null)
   {
      return str?.BNReplace(replacement, string.IsNullOrEmpty(newLine) ? Environment.NewLine : newLine);
   }

   /// <summary>
   /// Checks if the string is numeric.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is numeric.</returns>
   public static bool BNIsNumeric(this string? str)
   {
      return str != null && double.TryParse(str, out double _);
   }

   /// <summary>
   /// Checks if the string is integer.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is integer.</returns>
   public static bool BNIsInteger(this string? str)
   {
      if (str == null)
         return false;

      return !str.Contains('.') && long.TryParse(str, out long _);
   }

   /// <summary>
   /// Checks if the string is an email address.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is an email address.</returns>
   public static bool BNIsEmail(this string? str)
   {
      return str != null && Constants.REGEX_EMAIL.IsMatch(str);
   }

   /// <summary>
   /// Checks if the string is a website address.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is a website address.</returns>
   public static bool BNIsWebsite(this string? str)
   {
      return str != null && Constants.REGEX_URL_WEB.IsMatch(str);
   }

   /// <summary>
   /// Checks if the string is a creditcard.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is a creditcard.</returns>
   public static bool BNIsCreditcard(this string? str)
   {
      return str != null && Constants.REGEX_CREDITCARD.IsMatch(str);
   }

   /// <summary>
   /// Checks if the string is an IPv4 address.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is an IPv4 address.</returns>
   public static bool BNIsIPv4(this string? str)
   {
      return BogaNet.IO.NetworkHelper.isIPv4(str);
   }

   /// <summary>
   /// Checks if the string is an IPv6 address.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is an IPv6 address.</returns>
   public static bool BNIsIPv6(this string? str)
   {
      return BogaNet.IO.NetworkHelper.isIPv6(str);
   }

   /// <summary>
   /// Checks if the string is alphanumeric.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string is alphanumeric.</returns>
   public static bool BNIsAlphanumeric(this string? str)
   {
      return str != null && Constants.REGEX_ALPHANUMERIC.IsMatch(str);
   }

   /// <summary>
   /// Checks if the string has line endings.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string has line endings.</returns>
   public static bool BNHasLineEndings(this string? str)
   {
      return str != null && Constants.REGEX_LINEENDINGS.IsMatch(str);
   }

   /// <summary>
   /// Checks if the string has invalid characters.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <returns>True if the string has invalid characters.</returns>
   public static bool BNHasInvalidChars(this string? str)
   {
      return str != null && Constants.REGEX_INVALID_CHARS.IsMatch(str);
   }

   /// <summary>
   /// Checks if the string starts with another string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string is integer.</returns>
   public static bool BNStartsWith(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return false;

      return string.IsNullOrEmpty(toCheck) || str.StartsWith(toCheck, comp);
   }

   /// <summary>
   /// Checks if the string ends with another string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string is integer.</returns>
   public static bool BNEndsWith(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return false;

      return string.IsNullOrEmpty(toCheck) || str.EndsWith(toCheck, comp);
   }

   /// <summary>
   /// Returns the index of the last occurence of a given string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String for the index.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>The index of the last occurence of the given string if the string is integer.</returns>
   public static int BNLastIndexOf(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return 0;

      return string.IsNullOrEmpty(toCheck) ? 0 : str.LastIndexOf(toCheck, comp);
   }

   /// <summary>
   /// Returns the index of the first occurence of a given string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String for the index.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>The index of the first occurence of the given string if the string is integer.</returns>
   public static int BNIndexOf(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return 0;

      return string.IsNullOrEmpty(toCheck) ? 0 : str.IndexOf(toCheck, comp);
   }

   /// <summary>
   /// Returns the index of the first occurence of a given string.
   /// </summary>
   /// <param name="str">String-instance.</param>
   /// <param name="toCheck">String for the index.</param>
   /// <param name="startIndex">Start index for the check.</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>The index of the first occurence of the given string if the string is integer.</returns>
   public static int BNIndexOf(this string? str, string? toCheck, int startIndex, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return 0;

      return string.IsNullOrEmpty(toCheck) ? 0 : str.IndexOf(toCheck, startIndex, comp);
   }

   /// <summary>
   /// Converts the value of a string to a Base64-string.
   /// </summary>
   /// <param name="str">Input string.</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8).</param>
   /// <returns>String value as converted Base64-string.</returns>
   public static string? BNToBase64(this string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return _encoding.GetBytes(str).BNToBase64();
   }

   /// <summary>
   /// Converts the value of a Base64-string to a string.
   /// </summary>
   /// <param name="str">Input Base64-string.</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8).</param>
   /// <returns>Base64-string value as converted string.</returns>
   public static string? BNFromBase64(this string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      byte[]? base64 = str.BNFromBase64ToByteArray();
      return base64 == null ? null : _encoding.GetString(base64);
   }

   /// <summary>
   /// Converts the value of a Base64-string to a byte-array.
   /// </summary>
   /// <param name="str">Input Base64-string.</param>
   /// <returns>Base64-Byte-array from the Base64-string.</returns>
   public static byte[]? BNFromBase64ToByteArray(this string? str)
   {
      return str == null ? null : Convert.FromBase64String(str);
   }

   /// <summary>
   /// Converts the value of a string to a Hex-string (with Unicode support).
   /// </summary>
   /// <param name="str">Input string.</param>
   /// <param name="addPrefix">Add "0x"-as prefix (optional, default: false).</param>
   /// <returns>String value as converted Hex-string.</returns>
   public static string? BNToHex(this string? str, bool addPrefix = false)
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
   /// Converts the Hex-value of a string to a string (with Unicode support).
   /// </summary>
   /// <param name="hex">Input as Hex-string.</param>
   /// <returns>Hex-string value as converted string.</returns>
   public static string? BNHexToString(this string? hex)
   {
      if (hex == null)
         return null;

      string _hex = hex;

      if (_hex.StartsWith("0x"))
         _hex = _hex.Substring(2);

      if (hex.Length % 2 != 0)
         throw new FormatException($"String seems to be an invalid hex-code: {hex}");

      byte[] bytes = new byte[_hex.Length / 2];
      for (int ii = 0; ii < bytes.Length; ii++)
      {
         bytes[ii] = Convert.ToByte(hex.Substring(ii * 2, 2), 16);
      }

      //return Encoding.ASCII.GetString(bytes);
      return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
   }

   /// <summary>
   /// Converts the Hex-value of a string to number.
   /// </summary>
   /// <param name="hex">Input as Hex-string.</param>
   /// <returns>Hex-string value as converted number.</returns>
   public static T? BNHexToNumber<T>(this string? hex) where T : INumber<T>
   {
      if (hex == null)
         return default;

      if (hex.StartsWith("0x"))
         hex = hex.Substring(2);

      return T.Parse(hex, System.Globalization.NumberStyles.HexNumber, null);
   }

   /// <summary>
   /// Converts the value of a string to a byte-array.
   /// </summary>
   /// <param name="str">Input string.</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8).</param>
   /// <returns>Byte-array with the string.</returns>
   public static byte[]? BNToByteArray(this string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return _encoding.GetBytes(str);
   }

   /// <summary>
   /// Cleans a given text from tags.
   /// </summary>
   /// <param name="str">Input to clean.</param>
   /// <returns>Clean text without tags.</returns>
   public static string? BNClearTags(this string? str)
   {
      return str != null ? Constants.REGEX_CLEAN_TAGS.Replace(str, string.Empty).Trim() : null;
   }

   /// <summary>
   /// Cleans a given text from multiple spaces.
   /// </summary>
   /// <param name="str">Input to clean.</param>
   /// <returns>Clean text without multiple spaces.</returns>
   public static string? BNClearSpaces(this string? str)
   {
      return str != null ? Constants.REGEX_CLEAN_SPACES.Replace(str, " ").Trim() : null;
   }

   /// <summary>
   /// Cleans a given text from line endings.
   /// </summary>
   /// <param name="str">Input to clean.</param>
   /// <returns>Clean text without line endings.</returns>
   public static string? BNClearLineEndings(this string? str)
   {
      return str != null ? Constants.REGEX_LINEENDINGS.Replace(str, string.Empty).Trim() : null;
   }

   /// <summary>
   /// Creates a fixed length string.
   /// </summary>
   /// <param name="str">Input to fix.</param>
   /// <param name="length">Length of the string</param>
   /// <param name="filler">Filler charachter for the string (optional, default ' ')</param>
   /// <param name="padRight">Right padding - otherwise left padding (optional, default: true)</param>
   /// <returns>Fix length string</returns>
   public static string BNFixedLength(this string? str, int length, char filler = ' ', bool padRight = true)
   {
      if (str == null)
         return new string(filler, length);

      int diff = length - str.Length;

      if (diff > 0)
      {
         string fill = new string(filler, diff);

         return padRight ? $"{str}{fill}" : $"{fill}{str}";
      }

      return str.Substring(0, length);
   }
}