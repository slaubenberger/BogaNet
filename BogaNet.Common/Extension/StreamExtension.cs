﻿using System.Threading.Tasks;
using System;
using System.IO;

namespace BogaNet.Extension;

/// <summary>
/// Extension methods for Stream.
/// </summary>
public static class StreamExtension
{
   #region Public methods

   /// <summary>
   /// Reads the full content of a Stream.
   /// </summary>
   /// <param name="input">Stream-instance to read</param>
   /// <returns>Byte-array of the Stream content</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static byte[] BNReadFully(this Stream input)
   {
      ArgumentNullException.ThrowIfNull(input);

      using MemoryStream ms = new();
      input.CopyTo(ms);
      return ms.ToArray();
   }

   /// <summary>
   /// Reads the full content of a Stream asynchronously.
   /// </summary>
   /// <param name="input">Stream-instance to read</param>
   /// <returns>Byte-array of the Stream content</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static async Task<byte[]> BNReadFullyAsync(this Stream input)
   {
      ArgumentNullException.ThrowIfNull(input);

      using MemoryStream ms = new();
      await input.CopyToAsync(ms);
      return ms.ToArray();
   }

   #endregion
}