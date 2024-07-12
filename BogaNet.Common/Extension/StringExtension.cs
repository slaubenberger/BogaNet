using System.Text;
using System;
using System.Linq;

namespace BogaNet.Extension;

/// <summary>
/// Extension methods for strings.
/// </summary>
public static class StringExtension
{
   #region Public methods

   /// <summary>
   /// Reverses a string.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <returns>Reversed string</returns>
   public static string? BNReverse(this string? str)
   {
      if (str == null)
         return str;

      char[] charArray = str.ToCharArray();
      charArray.BNReverse();

      return new string(charArray);
   }

   /// <summary>
   /// Case insensitive 'Replace' per default.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="oldString">String to replace</param>
   /// <param name="newString">New replacement string</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>Replaced string</returns>
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
   /// Removes characters from a string.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="removeChars">Characters to remove</param>
   /// <returns>String without the given characters</returns>
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
   /// <param name="str">String-instance</param>
   /// <param name="toCheck">String to check</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string contains the given string</returns>
   public static bool BNEquals(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      return str?.Equals(toCheck, comp) == true;
   }

   /// <summary>
   /// Case insensitive 'Contains' per default.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="toCheck">String to check</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string contains the given string</returns>
   public static bool BNContains(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      return toCheck != null && str?.IndexOf(toCheck, comp) >= 0;
   }

   /// <summary>
   /// Contains any given string.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="searchTerms">Search terms separated by the given split-character</param>
   /// <returns>True if the string contains any parts of the given string</returns>
   public static bool BNContainsAny(this string? str, params string[]? searchTerms)
   {
      if (str == null)
         return false;

      if (searchTerms == null)
         return false;

      return searchTerms.Any(searchTerm => str.BNContains(searchTerm));
   }

   /// <summary>
   /// Contains all given strings.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="searchTerms">Search terms separated by the given split-character</param>
   /// <returns>True if the string contains all parts of the given string</returns>
   public static bool BNContainsAll(this string? str, params string[]? searchTerms)
   {
      if (str == null)
         return false;

      if (searchTerms == null)
         return false;

      return searchTerms.All(searchTerm => str.BNContains(searchTerm));
   }

   /// <summary>
   /// Checks if the string starts with another string.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="toCheck">String to check</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string is integer</returns>
   public static bool BNStartsWith(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return false;

      return string.IsNullOrEmpty(toCheck) || str.StartsWith(toCheck, comp);
   }

   /// <summary>
   /// Checks if the string ends with another string.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="toCheck">String to check</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>True if the string is integer</returns>
   public static bool BNEndsWith(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return false;

      return string.IsNullOrEmpty(toCheck) || str.EndsWith(toCheck, comp);
   }

   /// <summary>
   /// Returns the index of the last occurence of a given string.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="toCheck">String for the index</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>The index of the last occurence of the given string if the string is integer</returns>
   public static int BNLastIndexOf(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return 0;

      return string.IsNullOrEmpty(toCheck) ? 0 : str.LastIndexOf(toCheck, comp);
   }

   /// <summary>
   /// Returns the index of the first occurence of a given string.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="toCheck">String for the index</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>The index of the first occurence of the given string if the string is integer</returns>
   public static int BNIndexOf(this string? str, string? toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return 0;

      return string.IsNullOrEmpty(toCheck) ? 0 : str.IndexOf(toCheck, comp);
   }

/*
   /// <summary>
   /// Returns the index of the first occurence of a given string.
   /// </summary>
   /// <param name="str">String-instance</param>
   /// <param name="toCheck">String for the index</param>
   /// <param name="startIndex">Start index for the check</param>
   /// <param name="comp">StringComparison-method (optional, default: StringComparison.OrdinalIgnoreCase)</param>
   /// <returns>The index of the first occurence of the given string if the string is integer</returns>
   public static int BNIndexOf(this string? str, string? toCheck, int startIndex, StringComparison comp = StringComparison.OrdinalIgnoreCase)
   {
      if (str == null)
         return 0;

      return string.IsNullOrEmpty(toCheck) ? 0 : str.IndexOf(toCheck, startIndex, comp);
   }
*/
   /// <summary>
   /// Converts the value of a string to a byte-array.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Byte-array with the string</returns>
   public static byte[] BNToByteArray(this string str, Encoding? encoding = null)
   {
      ArgumentNullException.ThrowIfNull(str);

      Encoding enc = encoding ?? Encoding.UTF8;

      return enc.GetBytes(str);
   }

   /// <summary>
   /// Converts a byte-array to a string.
   /// </summary>
   /// <param name="bytes">Input string as byte-array</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <param name="offset">Offset inside the byte-array (optional, default: 0)</param>
   /// <param name="length">Number of bytes (optional, default: 0 = all)</param>
   /// <returns>String from the byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string BNToString(this byte[] bytes, Encoding? encoding = null, int offset = 0, int length = 0)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      int off = Math.Abs(offset);

      Encoding _encoding = encoding ?? Encoding.UTF8;

      if (off > 0)
      {
         int len = length > 0 ? length : bytes.Length;
         byte[] content = new byte[len];
         Buffer.BlockCopy(bytes, off, content, 0, len);
         string res = content.BNToString(encoding);
         return res.Trim('\0');
      }

      return _encoding.GetString(bytes);
   }

   #endregion
}