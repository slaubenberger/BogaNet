namespace BogaNet;

/// <summary>
/// Extension methods for Stream.
/// </summary>
public static class ExtensionStream
{
   /// <summary>
   /// Reads the full content of a Stream.
   /// </summary>
   /// <param name="input">Stream-instance to read</param>
   /// <returns>Byte-array of the Stream content</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] BNReadFully(this Stream? input)
   {
      return Task.Run(() => BNReadFullyAsync(input)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Reads the full content of a Stream asynchronously.
   /// </summary>
   /// <param name="input">Stream-instance to read</param>
   /// <returns>Byte-array of the Stream content</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static async Task<byte[]> BNReadFullyAsync(this Stream? input)
   {
      if (input == null)
         throw new ArgumentNullException(nameof(input));

      using MemoryStream ms = new();
      await input.CopyToAsync(ms);
      return ms.ToArray();
   }
}