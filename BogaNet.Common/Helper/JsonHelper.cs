﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.IO;

namespace BogaNet.Helper;

/// <summary>
/// Helper-class for JSON. Extends https://www.newtonsoft.com/json
/// </summary>
public abstract class JsonHelper
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(JsonHelper));

   /// <summary>
   /// Format for JSON as single line.
   /// </summary>
   public static JsonSerializerSettings FORMAT_NONE =>
      new()
      {
         Formatting = Formatting.None,
         DateParseHandling = DateParseHandling.DateTime,
         DateTimeZoneHandling = DateTimeZoneHandling.Utc,
         DateFormatHandling = DateFormatHandling.IsoDateFormat,
         FloatFormatHandling = FloatFormatHandling.String,
         NullValueHandling = NullValueHandling.Ignore,
         FloatParseHandling = FloatParseHandling.Double,
         MissingMemberHandling = MissingMemberHandling.Ignore,
         Converters = [new Newtonsoft.Json.Converters.StringEnumConverter()]
         //StringEscapeHandling = StringEscapeHandling.Default,
         //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
         //ConstructorHandling = ConstructorHandling.Default,
         //DefaultValueHandling = DefaultValueHandling.Ignore,
         //ObjectCreationHandling = ObjectCreationHandling.Auto,
         //MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
         //PreserveReferencesHandling = PreserveReferencesHandling.All,
         //TypeNameHandling = TypeNameHandling.All
      };

   /// <summary>
   /// Format for indented JSON.
   /// </summary>
   public static JsonSerializerSettings FORMAT_INDENTED =>
      new()
      {
         Formatting = Formatting.Indented,
         DateParseHandling = DateParseHandling.DateTime,
         DateTimeZoneHandling = DateTimeZoneHandling.Utc,
         DateFormatHandling = DateFormatHandling.IsoDateFormat,
         FloatFormatHandling = FloatFormatHandling.String,
         NullValueHandling = NullValueHandling.Ignore,
         FloatParseHandling = FloatParseHandling.Double,
         MissingMemberHandling = MissingMemberHandling.Ignore,
         Converters = [new Newtonsoft.Json.Converters.StringEnumConverter()]
      };

   /// <summary>
   /// Serialize an object to an JSON-file.
   /// </summary>
   /// <param name="obj">Object to serialize</param>
   /// <param name="path">File name of the JSON</param>
   /// <param name="settings">Serializer settings (optional)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool SerializeToFile(object? obj, string? path, JsonSerializerSettings? settings = null)
   {
      return Task.Run(() => SerializeToFileAsync(obj, path, settings)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Serialize an object to an JSON-file asynchronously.
   /// </summary>
   /// <param name="obj">Object to serialize</param>
   /// <param name="path">File name of the JSON</param>
   /// <param name="settings">Serializer settings (optional)</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> SerializeToFileAsync(object? obj, string? path, JsonSerializerSettings? settings = null)
   {
      if (obj == null)
         throw new ArgumentNullException(nameof(obj));
      if (path == null)
         throw new ArgumentNullException(nameof(path));

      try
      {
         return await FileHelper.WriteAllTextAsync(path, SerializeToString(obj, settings));
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not serialize the object to a file");
         throw;
      }
   }

   /// <summary>
   /// Serialize an object to an JSON-string.
   /// </summary>
   /// <param name="obj">Object to serialize</param>
   /// <param name="settings">Serializer settings (optional)</param>
   /// <returns>Object as JSON-string</returns>
   /// <exception cref="Exception"></exception>
   public static string SerializeToString(object? obj, JsonSerializerSettings? settings = null)
   {
      if (obj == null)
         throw new ArgumentNullException(nameof(obj));

      try
      {
         return JsonConvert.SerializeObject(obj, settings ?? FORMAT_INDENTED);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not convert JSON: {obj}");
         throw;
      }
   }

   /// <summary>
   /// Deserialize a JSON-file to an object.
   /// </summary>
   /// <param name="path">JSON-file of the object</param>
   /// <param name="settings">Serializer settings (optional)</param>
   /// <returns>Object</returns>
   /// <exception cref="Exception"></exception>
   public static T? DeserializeFromFile<T>(string? path, JsonSerializerSettings? settings = null)
   {
      return Task.Run(() => DeserializeFromFileAsync<T>(path, settings)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Deserialize a JSON-file to an object asynchronously.
   /// </summary>
   /// <param name="path">JSON-file of the object</param>
   /// <param name="settings">Serializer settings (optional)</param>
   /// <returns>Object</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<T?> DeserializeFromFileAsync<T>(string? path, JsonSerializerSettings? settings = null)
   {
      if (path == null)
         throw new ArgumentNullException(nameof(path));

      try
      {
         return DeserializeFromString<T>(await File.ReadAllTextAsync(path), settings);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not deserialize the object from a file");
         throw;
      }
   }

   /// <summary>
   /// Deserialize a JSON-string to an object.
   /// </summary>
   /// <param name="jsonAsString">JSON of the object</param>
   /// <param name="settings">Serializer settings (optional)</param>
   /// <returns>Object</returns>
   /// <exception cref="Exception"></exception>
   public static T? DeserializeFromString<T>(string? jsonAsString, JsonSerializerSettings? settings = null)
   {
      if (string.IsNullOrEmpty(jsonAsString))
         throw new ArgumentNullException(nameof(jsonAsString));

      try
      {
         return JsonConvert.DeserializeObject<T>(jsonAsString, settings ?? FORMAT_INDENTED);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not convert JSON: {jsonAsString}");
         throw;
      }
   }
}