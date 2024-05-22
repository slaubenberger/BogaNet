﻿using System.Text;

namespace BogaNet;

/// <summary>
/// Extension methods for IList.
/// </summary>
public static class ExtensionList
{
   /// <summary>
   /// Generates a string list with all entries (via CTToString).
   /// </summary>
   /// <param name="list">IList-instance to ToString.</param>
   /// <returns>String list with all entries (via CTToString).</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static string BNToString<T>(this IList<T>? list)
   {
      if (list == null)
         throw new ArgumentNullException(nameof(list));

      StringBuilder sb = new();

      sb.Append(list.GetType().Name);
      sb.Append(":[");

      for (int ii = 0; ii < list.Count; ii++)
      {
         sb.Append(list[ii].BNToString());

         if (ii < list.Count - 1)
            sb.Append(',');
      }

      sb.Append(']');

      return sb.ToString();
   }

   /// <summary>
   /// Shuffles a List.
   /// </summary>
   /// <param name="list">IList-instance to shuffle.</param>
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
   /// <param name="list">IList-instance to dump.</param>
   /// <param name="prefix">Prefix for every element (optional, default: empty).</param>
   /// <param name="postfix">Postfix for every element (optional, default: empty).</param>
   /// <param name="appendNewLine">Append new line, otherwise use the given delimiter (optional, default: false).</param>
   /// <param name="delimiter">Delimiter if appendNewLine is false (optional, default: "; ").</param>
   /// <returns>String with lines for all list entries.</returns>
   public static string? BNDump<T>(this IList<T>? list, string? prefix = "", string? postfix = "", bool appendNewLine = true, string delimiter = "; ")
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
         sb.Append(element.BNToString());
         sb.Append(postfix);
      }

      return sb.ToString();
   }

   /// <summary>
   /// Generates a string list with all entries (via BNToString).
   /// </summary>
   /// <param name="list">IList-instance to ToString.</param>
   /// <returns>String list with all entries (via CTToString).</returns>
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
   /// <param name="str">String list-instance.</param>
   /// <param name="toCheck">String to check.</param>
   /// <param name="comp">StringComparer (optional, default: StringComparer.OrdinalIgnoreCase)</param>
   /// <returns>True if the string list contains the given string.</returns>
   public static bool BNContains(this IList<string>? str, string? toCheck, StringComparer? comp = null)
   {
      if (str == null)
         return false;

      comp ??= StringComparer.OrdinalIgnoreCase;

      return str.Contains(toCheck, comp);
   }
}