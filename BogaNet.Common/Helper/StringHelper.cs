using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Web;
using BogaNet.Extension;

namespace BogaNet.Helper;

/// <summary>
/// Helper methods for strings.
/// </summary>
public static class StringHelper
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(StringHelper));
   private static readonly Random _rnd = new();

   #endregion

   #region Public methods

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

      length = Math.Abs(length);
      minSentences = Math.Abs(minSentences);
      maxSentences = Math.Abs(maxSentences);
      minWords = Math.Abs(minWords);
      maxWords = Math.Abs(maxWords);

      int numSentences = _rnd.Next(maxSentences - minSentences) + minSentences + 1;

      StringBuilder result = new();

      for (int s = 0; s < numSentences && result.Length <= length; s++)
      {
         int numWords = _rnd.Next(maxWords - minWords) + minWords + 1;
         for (int w = 0; w < numWords && result.Length <= length; w++)
         {
            if (w > 0)
               result.Append(' ');

            result.Append(w == 0 ? ToTitleCase(words[_rnd.Next(words.Length)]) : words[_rnd.Next(words.Length)]);
         }

         result.Append(". ");
      }

      string text = result.ToString();

      if (length > 0 && text.Length > length)
         text = text[..(length - 1)] + ".";

      return text;
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
   /// Creates a fixed length string from a given string.
   /// </summary>
   /// <param name="str">Input string to fix length</param>
   /// <param name="length">Length of the string</param>
   /// <param name="filler">Filler character for the string (optional, default ' ')</param>
   /// <param name="padRight">Right padding - otherwise left padding (optional, default: true)</param>
   /// <returns>Fix length string</returns>
   public static string CreateFixedLengthString(string? str, int length, char filler = ' ', bool padRight = true)
   {
      length = Math.Abs(length);

      if (str == null)
         return new string(filler, length);

      int diff = length - str.Length;

      if (diff > 0)
      {
         string fill = new(filler, diff);

         return padRight ? $"{str}{fill}" : $"{fill}{str}";
      }

      return str[..length];
   }

   /// <summary>
   /// Creates a string of characters with a given length.
   /// </summary>
   /// <param name="length">Length of the generated string</param>
   /// <param name="fillerCharacters">Characters to fill the string (if more than one character is used, the generated string will be a randomized result of all characters)</param>
   /// <returns>Generated string</returns>
   public static string CreateString(int length, params char[]? fillerCharacters)
   {
      length = Math.Abs(length);

      if (fillerCharacters == null)
         return string.Empty;

      if (fillerCharacters.Length > 1)
      {
         char[] chars = new char[length];

         for (int ii = 0; ii < length; ii++)
         {
            chars[ii] = fillerCharacters[_rnd.Next(0, fillerCharacters.Length)];
         }

         return new string(chars);
      }

      return fillerCharacters.Length == 1 ? new string(fillerCharacters[0], length) : string.Empty;
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

      skipHeaderLines = Math.Abs(skipHeaderLines);
      skipFooterLines = Math.Abs(skipFooterLines);

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
   /// Decodes a HTML encoded string to a normal string.
   /// </summary>
   /// <param name="input">HTML encoded string</param>
   /// <returns>Normal string</returns>
   public static string DecodeFromHTMLString(string input)
   {
      return HttpUtility.HtmlDecode(input);
   }

   /// <summary>
   /// Encodes a normal string to a HTML encoded string.
   /// </summary>
   /// <param name="input">Normal string</param>
   /// <returns>HTML encoded string</returns>
   public static string EncodeToHTMLString(string input)
   {
      return HttpUtility.HtmlEncode(input);
   }

   /// <summary>
   /// Escape the data string in an URL (like spaces etc.).
   /// </summary>
   /// <param name="url"></param>
   /// <returns>Escaped URL</returns>
   public static string? EscapeURL(string? url)
   {
      return url == null ? null : Uri.EscapeDataString(url).Replace("%2F", "/").Replace("%3A", ":");
   }

   /// <summary>
   /// Removes or replaces quotations (") in a string (e.g. from a CSV)
   /// </summary>
   /// <param name="str"></param>
   /// <param name="replacement">Replacement string for the quotation (optional, default: "")</param>
   /// <param name="trim">Trim the string (optional, default: true)</param>
   /// <returns>String without quotations</returns>
   public static string? RemoveQuotation(this string? str, string replacement = "", bool trim = true)
   {
      if (str == null)
         return str;

      return trim ? str.Replace("\"", replacement).Trim() : str.Replace("\"", replacement);
   }

   /// <summary>
   /// Adds quotations (") to a string (e.g. for a CSV)
   /// </summary>
   /// <param name="str"></param>
   /// <returns>String with quotations</returns>
   public static string? AddQuotation(this string? str)
   {
      return str == null ? str : $"\"{str}\"";
   }

   /*
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
*/
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

   #endregion
}