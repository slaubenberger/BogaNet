using System;
using Enumerable = System.Linq.Enumerable;
using System.Text;
using System.Threading.Tasks;
using BogaNet.Extension;
using BogaNet.Helper;

namespace BogaNet.Encoder;

/// <summary>
/// Base32 encoder class.
/// </summary>
public static class Base32 //NUnit
{
   #region Public methods

   /// <summary>
   /// Converts a Base32-string to a byte-array.
   /// </summary>
   /// <param name="base32string">Data as Base32-string</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase32String(string base32string)
   {
      ArgumentException.ThrowIfNullOrEmpty(base32string);

      base32string = base32string.TrimEnd('=');
      int byteCount = base32string.Length * 5 / 8;
      byte[] returnArray = new byte[byteCount];

      byte currentByte = 0;
      byte bitsRemaining = 8;
      int arrayIndex = 0;

      foreach (int cValue in Enumerable.Select(base32string, charToValue))
      {
         int mask;
         if (bitsRemaining > 5)
         {
            mask = cValue << (bitsRemaining - 5);
            currentByte = (byte)(currentByte | mask);
            bitsRemaining -= 5;
         }
         else
         {
            mask = cValue >> (5 - bitsRemaining);
            currentByte = (byte)(currentByte | mask);
            returnArray[arrayIndex++] = currentByte;
            currentByte = (byte)(cValue << (3 + bitsRemaining));
            bitsRemaining += 3;
         }
      }

      //if we didn't end with a full byte
      if (arrayIndex != byteCount)
         returnArray[arrayIndex] = currentByte;

      return returnArray;
   }

   /// <summary>
   /// Converts a byte-array to a Base32-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <returns>Data as encoded Base32-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase32String(params byte[] bytes)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      int charCount = (int)Math.Ceiling(bytes.Length / 5d) * 8;
      char[] returnArray = new char[charCount];

      byte nextChar = 0, bitsRemaining = 5;
      int arrayIndex = 0;

      foreach (byte b in bytes)
      {
         nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
         returnArray[arrayIndex++] = valueToChar(nextChar);

         if (bitsRemaining < 4)
         {
            nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
            returnArray[arrayIndex++] = valueToChar(nextChar);
            bitsRemaining += 5;
         }

         bitsRemaining -= 3;
         nextChar = (byte)((b << bitsRemaining) & 31);
      }

      //if we didn't end with a full char
      if (arrayIndex != charCount)
      {
         returnArray[arrayIndex++] = valueToChar(nextChar);
         while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding
      }

      return new string(returnArray);
   }

   /// <summary>
   /// Converts the value of a string to a Base32-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>String value as converted Base32-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase32String(string str, Encoding? encoding = null)
   {
      return ToBase32String(str.BNToByteArray(encoding));
   }

   /// <summary>
   /// Converts a file to a Base32-string.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base32-string</returns>
   /// <exception cref="Exception"></exception>
   public static string Base32FromFile(string file)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return ToBase32String(FileHelper.ReadAllBytes(file));
   }

   /// <summary>
   /// Converts a file to a Base32-string asynchronously.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <returns>File content as converted Base32-string</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string> Base32FromFileAsync(string file)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return ToBase32String(await FileHelper.ReadAllBytesAsync(file));
   }

   /// <summary>
   /// Converts a Base32-string to a file.
   /// </summary>
   /// <param name="file">File to write the content of the Base32-string</param>
   /// <param name="base32string">Data as Base32-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool FileFromBase32(string file, string base32string)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return FileHelper.WriteAllBytes(file, FromBase32String(base32string));
   }

   /// <summary>
   /// Converts a Base32-string to a file asynchronously.
   /// </summary>
   /// <param name="file">File to write the content of the Base32-string</param>
   /// <param name="base32string">Data as Base32-string</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> FileFromBase32Async(string file, string base32string)
   {
      ArgumentException.ThrowIfNullOrEmpty(file);

      return await FileHelper.WriteAllBytesAsync(file, FromBase32String(base32string));
   }

   #endregion

   #region Private methods

   private static int charToValue(char c)
   {
      int value = c;

      return value switch
      {
         //65-90 == uppercase letters
         < 91 and > 64 => value - 65,
         //50-55 == numbers 2-7
         < 56 and > 49 => value - 24,
         //97-122 == lowercase letters
         < 123 and > 96 => value - 97,
         _ => throw new ArgumentException("Character is not a Base32 character.", nameof(c))
      };
   }

   private static char valueToChar(byte b)
   {
      return b switch
      {
         < 26 => (char)(b + 65),
         < 32 => (char)(b + 24),
         _ => throw new ArgumentException("Byte is not a Base32 value.", nameof(b))
      };
   }

   #endregion
}