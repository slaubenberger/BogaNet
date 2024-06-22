using System.Globalization;
using System.Text;
using System.Numerics;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Linq;
using BogaNet.Util;
using BogaNet.Helper;

namespace BogaNet.Helper;

/// <summary>
/// Helper methods for strings.
/// </summary>
public static class StringHelper
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(StringHelper));
   private static readonly Random _rnd = new();

   /// <summary>
   /// Converts a string to title case (first letter uppercase).
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <returns>Converted string in title case</returns>
   public static string? ToTitleCase(string? str)
   {
      return str == null ? str : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
   }

   /// <summary>
   /// Generates a "Lorem Ipsum" based on various parameters.
   /// </summary>
   /// <param name="length">Length of the text</param>
   /// <param name="minSentences">Minimum number of sentences for the text (optional, default: 1)</param>
   /// <param name="maxSentences">Maximal number of sentences for the text (optional, default: int.MaxValue)</param>
   /// <param name="minWords">Minimum number of words per sentence (optional, default: 1)</param>
   /// <param name="maxWords">Maximal number of words per sentence (optional, default: 15)</param>
   /// <returns>"Lorem Ipsum" based on the given parameters</returns>
   public static string GenerateLoremIpsum(int length, int minSentences = 1, int maxSentences = int.MaxValue, int minWords = 1, int maxWords = 15)
   {
      string[] words =
      [
         "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", "elit", "sed", "diam",
         "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
      ];

      int numSentences = _rnd.Next(maxSentences - minSentences) + minSentences + 1;

      StringBuilder result = new();

      for (int s = 0; s < numSentences && result.Length <= length; s++)
      {
         int numWords = _rnd.Next(maxWords - minWords) + minWords + 1;
         for (int w = 0; w < numWords && result.Length <= length; w++)
         {
            if (w > 0)
               result.Append(' ');

            result.Append(
               w == 0 ? StringHelper.ToTitleCase(words[_rnd.Next(words.Length)]) : words[_rnd.Next(words.Length)]);
         }

         result.Append(". ");
      }

      string text = result.ToString();

      if (length > 0 && text.Length > length)
         text = text.Substring(0, length - 1) + ".";

      return text;
   }

   /// <summary>
   /// Replaces new lines with a replacement string pattern.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="replacement">Replacement string pattern (optional, default: "#nl#")</param>
   /// <param name="newLine">New line string (optional, default: Environment.NewLine)</param>
   /// <returns>Replaced string without new lines</returns>
   public static string? RemoveNewLines(string? str, string? replacement = "#nl#", string? newLine = null)
   {
      return str?.Replace(string.IsNullOrEmpty(newLine) ? Environment.NewLine : newLine, replacement);
   }

   /// <summary>
   /// Replaces a given string pattern with new lines in a string.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="replacement">Replacement string pattern (optional, default: "#nl#")</param>
   /// <param name="newLine">New line string (optional, default: Environment.NewLine)</param>
   /// <returns>Replaced string with new lines</returns>
   public static string? AddNewLines(string? str, string? replacement = "#nl#", string? newLine = null)
   {
      return str?.BNReplace(replacement, string.IsNullOrEmpty(newLine) ? Environment.NewLine : newLine);
   }

   /// <summary>
   /// Checks if the string is numeric.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <returns>True if the string is numeric</returns>
   public static bool IsNumeric(string? str)
   {
      return str != null && double.TryParse(str, out double _);
   }

   /// <summary>
   /// Checks if the string is integer.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <returns>True if the string is integer</returns>
   public static bool IsInteger(string? str)
   {
      if (str == null)
         return false;

      return !str.Contains('.') && long.TryParse(str, out long _);
   }

   /// <summary>
   /// Checks if the string is an email address.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <returns>True if the string is an email address</returns>
   public static bool IsEmail(string? str)
   {
      return str != null && Constants.REGEX_EMAIL.IsMatch(str);
   }

   /// <summary>
   /// Checks if the string is a website address.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <returns>True if the string is a website address</returns>
   public static bool IsWebsite(string? str)
   {
      return str != null && Constants.REGEX_URL_WEB.IsMatch(str);
   }

   /// <summary>
   /// Checks if the string is a creditcard.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <returns>True if the string is a creditcard</returns>
   public static bool IsCreditcard(string? str)
   {
      return str != null && Constants.REGEX_CREDITCARD.IsMatch(str);
   }

   /// <summary>
   /// Checks if the string is alphanumeric.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <returns>True if the string is alphanumeric</returns>
   public static bool IsAlphanumeric(string? str)
   {
      return str != null && Constants.REGEX_ALPHANUMERIC.IsMatch(str);
   }

   /// <summary>
   /// Checks if the string has line endings.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <returns>True if the string has line endings</returns>
   public static bool HasLineEndings(string? str)
   {
      return str != null && Constants.REGEX_LINEENDINGS.IsMatch(str);
   }
/*
   /// <summary>
   /// Checks if the string has invalid characters.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <returns>True if the string has invalid characters</returns>
   public static bool HasInvalidChars(string? str) //TODO what kind of chars?
   {
      return str != null && Constants.REGEX_INVALID_CHARS.IsMatch(str);
   }
*/

   /// <summary>
   /// Converts the value of a string to a Base64-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Base64-string</returns>
   public static string? StringToBase64String(string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return ArrayHelper.ByteArrayToBase64String(_encoding.GetBytes(str));
   }

   /// <summary>
   /// Converts the value of a Base64-string to a string.
   /// </summary>
   /// <param name="str">Input Base64-string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Base64-string value as converted string</returns>
   public static string? StringFromBase64String(this string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      byte[]? base64 = ByteArrayFromBase64String(str);
      return base64 == null ? null : _encoding.GetString(base64);
   }

   /// <summary>
   /// Converts the value of a Base64-string to a byte-array.
   /// </summary>
   /// <param name="str">Input Base64-string</param>
   /// <returns>Base64-Byte-array from the Base64-string</returns>
   public static byte[]? ByteArrayFromBase64String(string? str)
   {
      return str == null ? null : Convert.FromBase64String(str);
   }


   /// <summary>
   /// Converts the value of a string to a Base32-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Base32-string</returns>
   public static string? StringToBase32String(string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return ArrayHelper.ByteArrayToBase32String(_encoding.GetBytes(str));
   }

   /// <summary>
   /// Converts the value of a Base32-string to a string.
   /// </summary>
   /// <param name="str">Input Base32-string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Base32-string value as converted string</returns>
   public static string? StringFromBase32String(this string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      byte[]? base32 = ByteArrayFromBase32String(str);
      return base32 == null ? null : _encoding.GetString(base32);
   }

   /// <summary>
   /// Converts the value of a Base32-string to a byte-array.
   /// </summary>
   /// <param name="str">Input Base32-string</param>
   /// <returns>Base32-Byte-array from the Base32-string</returns>
   public static byte[]? ByteArrayFromBase32String(string? str)
   {
      return str == null ? null : Base32.FromBase32String(str);
   }

   /// <summary>
   /// Cleans a given text from tags.
   /// </summary>
   /// <param name="str">Input to clean</param>
   /// <returns>Clean text without tags</returns>
   public static string? RemoveTags(string? str)
   {
      return str != null ? Constants.REGEX_CLEAN_TAGS.Replace(str, string.Empty).Trim() : null;
   }

   /// <summary>
   /// Cleans a given text from multiple spaces.
   /// </summary>
   /// <param name="str">Input to clean</param>
   /// <returns>Clean text without multiple spaces</returns>
   public static string? RemoveSpaces(string? str)
   {
      return str != null ? Constants.REGEX_CLEAN_SPACES.Replace(str, " ").Trim() : null;
   }

   /// <summary>
   /// Cleans a given text from line endings.
   /// </summary>
   /// <param name="str">Input to clean</param>
   /// <returns>Clean text without line endings</returns>
   public static string? RemoveLineEndings(this string? str)
   {
      return str != null ? Constants.REGEX_LINEENDINGS.Replace(str, string.Empty).Trim() : null;
   }

   /// <summary>
   /// Creates a fixed length string.
   /// </summary>
   /// <param name="str">Input to fix</param>
   /// <param name="length">Length of the string</param>
   /// <param name="filler">Filler charachter for the string (optional, default ' ')</param>
   /// <param name="padRight">Right padding - otherwise left padding (optional, default: true)</param>
   /// <returns>Fix length string</returns>
   public static string CreateFixedLength(string? str, int length, char filler = ' ', bool padRight = true)
   {
      if (str == null)
         return new string(filler, length);

      int diff = length - str.Length;

      if (diff > 0)
      {
         string fill = new(filler, diff);

         return padRight ? $"{str}{fill}" : $"{fill}{str}";
      }

      return str.Substring(0, length);
   }


   /// <summary>
   /// Split the given text to lines and return it as list.
   /// </summary>
   /// <param name="text">Complete text fragment</param>
   /// <param name="ignoreCommentedLines">Ignore commente lines (optional, default: true)</param>
   /// <param name="skipHeaderLines">Number of skipped header lines (optional, default: 0)</param>
   /// <param name="skipFooterLines">Number of skipped footer lines (optional, default: 0)</param>
   /// <returns>Splitted lines as list</returns>
   public static List<string> SplitToLines(string? text, bool ignoreCommentedLines = true, int skipHeaderLines = 0, int skipFooterLines = 0)
   {
      List<string> result = new(100);

      if (string.IsNullOrEmpty(text))
      {
         _logger.LogWarning("Parameter 'text' is null or empty => 'SplitStringToLines()' will return an empty string list.");
      }
      else
      {
         string[] lines = Constants.REGEX_LINEENDINGS.Split(text);

         for (int ii = 0; ii < lines.Length; ii++)
         {
            if (ii + 1 > skipHeaderLines && ii < lines.Length - skipFooterLines)
            {
               if (!string.IsNullOrEmpty(lines[ii]))
               {
                  if (ignoreCommentedLines)
                  {
                     if (!lines[ii].BNStartsWith("#")) //valid and not disabled line?
                        result.Add(lines[ii]);
                  }
                  else
                  {
                     result.Add(lines[ii]);
                  }
               }
            }
         }
      }

      return result;
   }

   /// <summary>
   /// Creates a string of characters with a given length.
   /// </summary>
   /// <param name="generateChars">Characters to generate the string (if more than one character is used, the generated string will be a randomized result of all characters)</param>
   /// <param name="stringLength">Length of the generated string</param>
   /// <returns>Generated string</returns>
   public static string CreateString(string? generateChars, int stringLength)
   {
      if (generateChars != null)
      {
         if (generateChars.Length > 1)
         {
            char[] chars = new char[stringLength];

            for (int ii = 0; ii < stringLength; ii++)
            {
               chars[ii] = generateChars[_rnd.Next(0, generateChars.Length)];
            }

            return new string(chars);
         }

         return generateChars.Length == 1 ? new string(generateChars[0], stringLength) : string.Empty;
      }

      return string.Empty;
   }

   /// <summary>
   /// Creates a string of characters with a given length.
   /// </summary>
   /// <param name="generateChar">Character to generate the string</param>
   /// <param name="stringLength">Length of the generated string</param>
   /// <returns>Generated string</returns>
   public static string CreateString(char? generateChar, int stringLength)
   {
      return CreateString(generateChar.ToString(), stringLength);
   }

   /// <summary>
   /// Decodes a HTML encoded string to a normal string.
   /// </summary>
   /// <param name="input">HTML encoded string</param>
   /// <returns>Normal string</returns>
   public static string DecodeHTML(string input)
   {
      return System.Web.HttpUtility.HtmlDecode(input);
   }

   /// <summary>
   /// Encodes a normal string to a HTML encoded string.
   /// </summary>
   /// <param name="input">Normal string</param>
   /// <returns>HTML encoded string</returns>
   public static string EncodeHTML(string input)
   {
      return System.Web.HttpUtility.HtmlEncode(input);
   }
   
   /// <summary>
   /// Escape the data in an URL (like spaces etc.).
   /// </summary>
   /// <param name="url"></param>
   /// <returns>Escaped URL</returns>
   public static string? EscapeURL(string? url) //TODO move to extensions?
   {
      return url == null ? null : Uri.EscapeDataString(url).Replace("%2F", "/").Replace("%3A", ":");
   }
}