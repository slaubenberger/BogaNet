using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BogaNet;

/// <summary>
/// Extension methods for IList.
/// </summary>
public static class ListExtension
{
   /// <summary>
   /// Shuffles a List.
   /// </summary>
   /// <param name="list">IList-instance to shuffle</param>
   /// <param name="seed">Seed for the PRNG (optional, default: 0 (=standard))</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void BNShuffle<T>(this IList<T>? list, int seed = 0)
   {
      if (list == null)
         throw new ArgumentNullException(nameof(list));

      Random rnd = seed == 0 ? new Random() : new Random(seed);
      int n = list.Count;

      while (n > 1)
      {
         int k = rnd.Next(n--);
         (list[n], list[k]) = (list[k], list[n]);
      }
   }

   /// <summary>
   /// Dumps a list to a string.
   /// </summary>
   /// <param name="list">IList-instance to dump</param>
   /// <param name="appendNewLine">Append new line, otherwise use the given delimiter (optional, default: true)</param>
   /// <param name="prefix">Prefix for every element (optional, default: empty)</param>
   /// <param name="postfix">Postfix for every element (optional, default: empty)</param>
   /// <param name="delimiter">Delimiter if appendNewLine is false (optional, default: "; ")</param>
   /// <returns>String with lines for all list entries</returns>
   public static string? BNDump<T>(this IList<T>? list, bool appendNewLine = true, string? prefix = "", string? postfix = "", string delimiter = "; ")
   {
      if (list == null)
         return null;

      StringBuilder sb = new();

      foreach (T element in list)
      {
         if (0 < sb.Length)
         {
            sb.Append(appendNewLine ? Environment.NewLine : delimiter);
         }

         sb.Append(prefix);
         //sb.Append(element.BNToString());
         sb.Append(element);
         sb.Append(postfix);
      }

      return sb.ToString();
   }

   /// <summary>
   /// Generates a string list with all entries (via BNToString).
   /// </summary>
   /// <param name="list">IList-instance to ToString</param>
   /// <returns>String list with all entries (via CTToString)</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static List<string> BNToStringList<T>(this IList<T>? list)
   {
      if (list == null)
         throw new ArgumentNullException(nameof(list));

      List<string> result = new(list.Count);
      result.AddRange(list.Select(element => null == element ? "null" : element.BNToString()));

      return result;
   }

   /// <summary>
   /// Default: case insensitive 'Contains'.
   /// </summary>
   /// <param name="str">String list-instance</param>
   /// <param name="toCheck">String to check</param>
   /// <param name="comp">StringComparer (optional, default: StringComparer.OrdinalIgnoreCase)</param>
   /// <returns>True if the string list contains the given string</returns>
   public static bool BNContains(this IList<string>? str, string? toCheck, StringComparer? comp = null)
   {
      if (str == null)
         return false;

      comp ??= StringComparer.OrdinalIgnoreCase;

      return str.Contains(toCheck, comp);
   }

   /// <summary>
   /// Returns a list with lists of a given chunk size
   /// </summary>
   /// <param name="source">Source list</param>
   /// <param name="chunkSize">Chunk size of the lists</param>
   /// <returns>List with lists of a given chunk size</returns>
   public static List<List<T>> BNChunkBy<T>(this IEnumerable<T> source, int chunkSize)
   {
      return source
         .Select((x, i) => new { Index = i, Value = x })
         .GroupBy(x => x.Index / chunkSize)
         .Select(x => x.Select(v => v.Value).ToList())
         .ToList();
   }
}