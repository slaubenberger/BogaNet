﻿using System.Text;
using System.Numerics;
using System;
using System.Linq;

namespace BogaNet;

/// <summary>
/// Extension methods for strings.
/// </summary>
public static class StringExtension
{
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
      Array.Reverse(charArray);

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
   /// <param name="splitChar">Split-character (optional, default: ' ')</param>
   /// <returns>True if the string contains any parts of the given string</returns>
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
   /// <param name="str">String-instance</param>
   /// <param name="searchTerms">Search terms separated by the given split-character</param>
   /// <param name="splitChar">Split-character (optional, default: ' ')</param>
   /// <returns>True if the string contains all parts of the given string</returns>
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

   /// <summary>
   /// Converts the value of a string to a Hex-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="addPrefix">Add "0x"-as prefix (optional, default: false)</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Hex-string</returns>
   public static string? BNToHex(this string? str, bool addPrefix = false, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      StringBuilder sb = new();

      if (addPrefix)
         sb.Append("0x");

      Encoding _encoding = encoding ?? Encoding.UTF8;

      byte[] bytes = _encoding.GetBytes(str);
      foreach (byte t in bytes)
      {
         sb.Append(t.ToString("X2"));
      }

      return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
   }

   /// <summary>
   /// Converts the Hex-value of a string to a string.
   /// </summary>
   /// <param name="hex">Input as Hex-string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Hex-string value as converted string</returns>
   public static string? BNHexToString(this string? hex, Encoding? encoding = null)
   {
      byte[]? bytes = hex?.BNHexToByteArray();

      Encoding _encoding = encoding ?? Encoding.UTF8;
      return bytes == null ? null : _encoding.GetString(bytes);
   }

   /// <summary>
   /// Converts the Hex-value of a string to number.
   /// </summary>
   /// <param name="hex">Input as Hex-string</param>
   /// <returns>Hex-string value as converted number</returns>
   public static T? BNHexToNumber<T>(this string? hex) where T : INumber<T>
   {
      if (hex == null)
         return default;

      if (hex.StartsWith("0x"))
         hex = hex.Substring(2);

      return T.Parse(hex, System.Globalization.NumberStyles.HexNumber, null);
   }

   /// <summary>
   /// Converts the Hex-value of a string to a byte-array.
   /// </summary>
   /// <param name="hex">Input as Hex-string</param>
   /// <returns>Hex-string value as converted byte-array</returns>
   public static byte[]? BNHexToByteArray(this string? hex)
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

      return bytes;
   }

   /// <summary>
   /// Converts the value of a string to a byte-array.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Byte-array with the string</returns>
   public static byte[]? BNToByteArray(this string? str, Encoding? encoding = null)
   {
      if (str == null)
         return null;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return _encoding.GetBytes(str);
   }
}