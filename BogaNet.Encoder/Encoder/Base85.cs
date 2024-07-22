using System;
using System.Text;
using System.IO;
using BogaNet.Extension;
using System.Threading.Tasks;
using BogaNet.Helper;

namespace BogaNet.Encoder;

/// <summary>
/// Base85 encoder class.
/// Partially based on: https://github.com/coding-horror/ascii85/tree/master
/// </summary>
public static class Base85
{
   #region Variables

   /// <summary>
   /// Prefix mark that identifies an encoded Base85-string (default: <~).
   /// </summary>
   public static string PrefixMark = "<~";

   /// <summary>
   /// Suffix mark that identifies an encoded Base85-string (default: ~>).
   /// </summary>
   public static string SuffixMark = "~>";

   #endregion

   #region Public methods

   /// <summary>
   /// Converts a Base85-string to a byte-array.
   /// </summary>
   /// <param name="base85string">Data as Base85-string</param>
   /// <param name="useMarks">Add the Prefix and Suffix marks when encoding, and enforce their presence for decoding (default: false).</param>
   /// <returns>Data as byte-array</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] FromBase85String(string base85string, bool useMarks = false)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(base85string);

      Ascii85 ascii85 = new()
      {
         PrefixMark = PrefixMark,
         SuffixMark = SuffixMark,
         LineLength = 0,
         EnforceMarks = useMarks
      };

      return ascii85.Decode(base85string);
   }

   /// <summary>
   /// Converts a byte-array to a Base85-string.
   /// </summary>
   /// <param name="bytes">Data as byte-array</param>
   /// <param name="useMarks">Add the Prefix and Suffix marks when encoding, and enforce their presence for decoding (default: false).</param>
   /// <returns>Data as encoded Base85-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase85String(byte[] bytes, bool useMarks = false)
   {
      ArgumentNullException.ThrowIfNull(bytes);

      Ascii85 ascii85 = new()
      {
         PrefixMark = PrefixMark,
         SuffixMark = SuffixMark,
         LineLength = 0,
         EnforceMarks = useMarks
      };

      return ascii85.Encode(bytes);
   }

   /// <summary>
   /// Converts the value of a string to a Base85-string.
   /// </summary>
   /// <param name="str">Input string</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <param name="useMarks">Add the Prefix and Suffix marks when encoding, and enforce their presence for decoding (default: false).</param>
   /// <returns>String value as converted Base85-string</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string ToBase85String(string str, Encoding? encoding = null, bool useMarks = false)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(str);

      byte[] bytes = str.BNToByteArray(encoding);
      //bytes.BNReverse();
      return ToBase85String(bytes, useMarks);
   }

   /// <summary>
   /// Converts a file to a Base85-string.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <param name="useMarks">Add the Prefix and Suffix marks when encoding, and enforce their presence for decoding (default: false).</param>
   /// <returns>File content as converted Base85-string</returns>
   /// <exception cref="Exception"></exception>
   public static string Base85FromFile(string file, bool useMarks = false)
   {
      return Task.Run(() => Base85FromFileAsync(file, useMarks)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Converts a file to a Base85-string asynchronously.
   /// </summary>
   /// <param name="file">File to convert</param>
   /// <param name="useMarks">Add the Prefix and Suffix marks when encoding, and enforce their presence for decoding (default: false).</param>
   /// <returns>File content as converted Base85-string</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<string> Base85FromFileAsync(string file, bool useMarks = false)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return ToBase85String(await FileHelper.ReadAllBytesAsync(file), useMarks);
   }

   /// <summary>
   /// Converts a Base85-string to a file.
   /// </summary>
   /// <param name="file">File to write the content of the Base85-string</param>
   /// <param name="base85string">Data as Base85-string</param>
   /// <param name="useMarks">Add the Prefix and Suffix marks when encoding, and enforce their presence for decoding (default: false).</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool FileFromBase85(string file, string base85string, bool useMarks = false)
   {
      return Task.Run(() => FileFromBase85Async(file, base85string, useMarks)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Converts a Base85-string to a file asynchronously.
   /// </summary>
   /// <param name="file">File to write the content of the Base85-string</param>
   /// <param name="base85string">Data as Base85-string</param>
   /// <param name="useMarks">Add the Prefix and Suffix marks when encoding, and enforce their presence for decoding (default: false).</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> FileFromBase85Async(string file, string base85string, bool useMarks = false)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(file);

      return await FileHelper.WriteAllBytesAsync(file, FromBase85String(base85string, useMarks));
   }

   #endregion


   /// <summary>
   /// C# implementation of ASCII85 encoding.
   /// Based on C code from http://www.stillhq.com/cgi-bin/cvsweb/ascii85/
   /// </summary>
   /// <remarks>
   /// Jeff Atwood
   /// http://www.codinghorror.com/blog/archives/000410.html
   /// </remarks>
   private class Ascii85
   {
      /// <summary>
      /// Prefix mark that identifies an encoded ASCII85 string, traditionally '<~'
      /// </summary>
      public string PrefixMark = "<~";

      /// <summary>
      /// Suffix mark that identifies an encoded ASCII85 string, traditionally '~>'
      /// </summary>
      public string SuffixMark = "~>";

      /// <summary>
      /// Maximum line length for encoded ASCII85 string;
      /// set to zero for one unbroken line.
      /// </summary>
      public int LineLength = 75;

      /// <summary>
      /// Add the Prefix and Suffix marks when encoding, and enforce their presence for decoding
      /// </summary>
      public bool EnforceMarks = true;

      private const int _asciiOffset = 33;
      private static readonly uint[] pow85 = [85 * 85 * 85 * 85, 85 * 85 * 85, 85 * 85, 85, 1];

      private readonly byte[] _encodedBlock = new byte[5];
      private readonly byte[] _decodedBlock = new byte[4];
      private uint _tuple = 0;
      private int _linePos = 0;

      /// <summary>
      /// Decodes an ASCII85 encoded string into the original binary data
      /// </summary>
      /// <param name="s">ASCII85 encoded string</param>
      /// <returns>byte array of decoded binary data</returns>
      public byte[] Decode(string s)
      {
         if (EnforceMarks)
         {
            if (!s.BNStartsWith(PrefixMark) | !s.BNEndsWith(SuffixMark))
               throw new Exception("ASCII85 encoded data should begin with '" + PrefixMark + "' and end with '" + SuffixMark + "'");
         }

         // strip prefix and suffix if present
         if (s.BNStartsWith(PrefixMark))
            s = s.Substring(PrefixMark.Length);

         if (s.BNEndsWith(SuffixMark))
            s = s.Substring(0, s.Length - SuffixMark.Length);

         MemoryStream ms = new();
         int count = 0;

         foreach (char c in s)
         {
            bool processChar;

            switch (c)
            {
               case 'z':
                  if (count != 0)
                  {
                     throw new Exception("The character 'z' is invalid inside an ASCII85 block.");
                  }

                  _decodedBlock[0] = 0;
                  _decodedBlock[1] = 0;
                  _decodedBlock[2] = 0;
                  _decodedBlock[3] = 0;
                  ms.Write(_decodedBlock, 0, _decodedBlock.Length);
                  processChar = false;
                  break;
               case '\n':
               case '\r':
               case '\t':
               case '\0':
               case '\f':
               case '\b':
                  processChar = false;
                  break;
               default:
                  if (c is < '!' or > 'u')
                     throw new Exception("Bad character '" + c + "' found. ASCII85 only allows characters '!' to 'u'.");

                  processChar = true;
                  break;
            }

            if (processChar)
            {
               _tuple += (uint)(c - _asciiOffset) * pow85[count];
               count++;

               if (count == _encodedBlock.Length)
               {
                  decodeBlock();
                  ms.Write(_decodedBlock, 0, _decodedBlock.Length);
                  _tuple = 0;
                  count = 0;
               }
            }
         }

         // if we have some bytes left over at the end...
         if (count != 0)
         {
            if (count == 1)
               throw new Exception("The last block of ASCII85 data cannot be a single byte.");

            count--;
            _tuple += pow85[count];
            decodeBlock(count);

            for (int i = 0; i < count; i++)
            {
               ms.WriteByte(_decodedBlock[i]);
            }
         }

         return ms.ToArray();
      }

      /// <summary>
      /// Encodes binary data into a plaintext ASCII85 format string
      /// </summary>
      /// <param name="ba">binary data to encode</param>
      /// <returns>ASCII85 encoded string</returns>
      public string Encode(byte[] ba)
      {
         StringBuilder sb = new(ba.Length * (_encodedBlock.Length / _decodedBlock.Length));
         _linePos = 0;

         if (EnforceMarks)
            appendString(sb, PrefixMark);

         int count = 0;
         _tuple = 0;

         foreach (byte b in ba)
         {
            if (count >= _decodedBlock.Length - 1)
            {
               _tuple |= b;

               if (_tuple == 0)
               {
                  appendChar(sb, 'z');
               }
               else
               {
                  encodeBlock(sb);
               }

               _tuple = 0;
               count = 0;
            }
            else
            {
               _tuple |= (uint)(b << (24 - count * 8));
               count++;
            }
         }

         // if we have some bytes left over at the end...
         if (count > 0)
            encodeBlock(count + 1, sb);

         if (EnforceMarks)
            appendString(sb, SuffixMark);

         return sb.ToString();
      }

      private void encodeBlock(StringBuilder sb)
      {
         encodeBlock(_encodedBlock.Length, sb);
      }

      private void encodeBlock(int count, StringBuilder sb)
      {
         for (int i = _encodedBlock.Length - 1; i >= 0; i--)
         {
            _encodedBlock[i] = (byte)(_tuple % 85 + _asciiOffset);
            _tuple /= 85;
         }

         for (int i = 0; i < count; i++)
         {
            char c = (char)_encodedBlock[i];
            appendChar(sb, c);
         }
      }

      private void decodeBlock()
      {
         decodeBlock(_decodedBlock.Length);
      }

      private void decodeBlock(int bytes)
      {
         for (int ii = 0; ii < bytes; ii++)
         {
            _decodedBlock[ii] = (byte)(_tuple >> (24 - ii * 8));
         }
      }

      private void appendString(StringBuilder sb, string s)
      {
         if (LineLength > 0 && _linePos + s.Length > LineLength)
         {
            _linePos = 0;
            sb.Append('\n');
         }
         else
         {
            _linePos += s.Length;
         }

         sb.Append(s);
      }

      private void appendChar(StringBuilder sb, char c)
      {
         sb.Append(c);
         _linePos++;

         if (LineLength > 0 && _linePos >= LineLength)
         {
            _linePos = 0;
            sb.Append('\n');
         }
      }
   }
}