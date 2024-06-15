using Microsoft.Extensions.Logging;
using System.Text;
using System.Collections.Generic;
using System;

namespace BogaNet;

/// <summary>
/// Extension methods for IDictionary.
/// </summary>
public static class ExtensionDictionary
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ExtensionDictionary));

   /// <summary>
   /// Dumps a dictionary to a string.
   /// </summary>
   /// <param name="dict">IDictionary-instance to dump</param>
   /// <param name="appendNewLine">Append new line, otherwise use the given delimiter (optional, default: true)</param>
   /// <param name="prefix">Prefix for every element (optional, default: empty)</param>
   /// <param name="postfix">Postfix for every element (optional, default: empty)</param>
   /// <param name="delimiter">Delimiter if appendNewLine is false (optional, default: "; ")</param>
   /// <returns>String with lines for all dictionary entries</returns>
   public static string? BNDump<K, V>(this IDictionary<K, V>? dict, bool appendNewLine = true, string? prefix = "", string? postfix = "", string delimiter = "; ")
   {
      if (dict == null)
         return null;

      StringBuilder sb = new();

      foreach (KeyValuePair<K, V> kvp in dict)
      {
         if (0 < sb.Length)
         {
            sb.Append(appendNewLine ? Environment.NewLine : delimiter);
         }

         sb.Append(prefix);
         sb.Append("Key = ");
         sb.Append(kvp.Key);
         sb.Append(", Value = ");
         //sb.Append(kvp.Value.BNToString());
         sb.Append(kvp.Value);
         sb.Append(postfix);
      }

      return sb.ToString();
   }

   /// <summary>
   /// Adds a dictionary to an existing one.
   /// </summary>
   /// <param name="dict">IDictionary-instance</param>
   /// <param name="collection">Dictionary to add</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void BNAddRange<K, V>(this IDictionary<K, V>? dict, IDictionary<K, V>? collection) where K : notnull
   {
      if (dict == null)
         throw new ArgumentNullException(nameof(dict));

      if (collection == null)
         throw new ArgumentNullException(nameof(collection));

      foreach (KeyValuePair<K, V> item in collection)
      {
         if (!dict.ContainsKey(item.Key))
         {
            dict.Add(item.Key, item.Value);
         }
         else
         {
            // handle duplicate key issue here
            _logger.LogWarning($"Duplicate key found: {item.Key} - {item.Value}");
         }
      }
   }

   /// <summary>
   /// Converts a dictionary to a XML serializable dictionary.
   /// </summary>
   /// <param name="dict">Standard dictionary</param>
   /// <returns>XML serializable dictionary</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static DictionaryXML<K, V> BNToDictionaryXML<K, V>(this IDictionary<K, V>? dict) where K : notnull
   {
      DictionaryXML<K, V> xmlDict = new();
      xmlDict.BNAddRange(dict);

      return xmlDict;
   }
}