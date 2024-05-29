using System.Text;

namespace BogaNet.Crypto;

/// <summary>
/// Obfuscator for strings and byte-arrays.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public abstract class Obfuscator
{
   private const byte DEFAULT_IV = 76; //TODO change the value in every project!

   /// <summary>
   /// Obfuscate a byte-array.
   /// </summary>
   /// <param name="data">byte-array to obfuscate</param>
   /// <param name="IV">Initial-Vector byte (optional)</param>
   /// <returns>Obfuscated byte-array</returns>
   public static byte[]? Obfuscate(byte[]? data, byte IV = DEFAULT_IV)
   {
      if (data == null)
         return null;

      Array.Reverse(data);

      byte[] result = new byte[data.Length];
      byte lastByte = 0;

      for (int ii = 0; ii < data.Length; ii++)
      {
         byte currentByte = data[ii];
         lastByte = ii == 0 ? (byte)(currentByte + IV) : (byte)(currentByte + lastByte);

         result[ii] = lastByte;
      }

      return result;
   }

   /// <summary>
   /// Obfuscate a string.
   /// </summary>
   /// <param name="plainText">String to obfuscate</param>
   /// <param name="IV">Initial-Vector byte (optional)</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>Obfuscated string</returns>
   public static string? Obfuscate(string? plainText, byte IV = DEFAULT_IV, Encoding? encoding = null)
   {
      if (string.IsNullOrEmpty(plainText))
         return plainText;

      Encoding _encoding = encoding ?? Encoding.UTF8;

      byte[]? result = Obfuscate(_encoding.GetBytes(plainText), IV);

      return result?.BNToBase64()?.Replace('=', '!').Replace('+', '-').Replace('/', '_');
   }

   /// <summary>
   /// De-obfuscate a byte-array.
   /// </summary>
   /// <param name="obfuscatedData">byte-array to de-obfuscate</param>
   /// <param name="IV">Initial-Vector byte (optional)</param>
   /// <returns>De-obfuscated byte-array</returns>
   public static byte[]? Deobfuscate(byte[]? obfuscatedData, byte IV = DEFAULT_IV)
   {
      if (obfuscatedData == null)
         return null;

      byte[] result = new byte[obfuscatedData.Length];

      for (int ii = obfuscatedData.Length - 1; ii >= 0; ii--)
      {
         byte currentByte = obfuscatedData[ii];
         byte lastByte = ii == 0 ? (byte)(currentByte - IV) : (byte)(currentByte - obfuscatedData[ii - 1]);

         result[ii] = lastByte;
      }

      Array.Reverse(result);

      return result;
   }

   /// <summary>
   /// De-obfuscate a string.
   /// </summary>
   /// <param name="obfuscatedText">String to de-obfuscate</param>
   /// <param name="IV">Initial-Vector byte (optional)</param>
   /// <param name="encoding">Encoding of the string (optional, default: UTF8)</param>
   /// <returns>De-obfuscated string</returns>
   public static string? Deobfuscate(string? obfuscatedText, byte IV = DEFAULT_IV, Encoding? encoding = null)
   {
      if (string.IsNullOrEmpty(obfuscatedText))
         return obfuscatedText;

      byte[]? result = Deobfuscate(obfuscatedText.Replace('!', '=').Replace('-', '+').Replace('_', '/').BNFromBase64ToByteArray(), IV);

      Encoding _encoding = encoding ?? Encoding.UTF8;

      return result != null ? _encoding.GetString(result) : null;
   }
}