using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BogaNet;

/// <summary>
/// Helper-class for JSON. Extends https://www.newtonsoft.com/json
/// </summary>
public abstract class JsonHelper
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger("JsonHelper");

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
         //StringEscapeHandling = StringEscapeHandling.Default,
         //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
         //ConstructorHandling = ConstructorHandling.Default,
         //DefaultValueHandling = DefaultValueHandling.Ignore,
         //ObjectCreationHandling = ObjectCreationHandling.Auto,
         //MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
         //PreserveReferencesHandling = PreserveReferencesHandling.All,
         //TypeNameHandling = TypeNameHandling.All
      };

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
         //StringEscapeHandling = StringEscapeHandling.Default,
         //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
         //ConstructorHandling = ConstructorHandling.Default,
         //DefaultValueHandling = DefaultValueHandling.Ignore,
         //ObjectCreationHandling = ObjectCreationHandling.Auto,
         //MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
         //PreserveReferencesHandling = PreserveReferencesHandling.All,
         //TypeNameHandling = TypeNameHandling.All
      };

   /// <summary>Serialize an object to an JSON-file.</summary>
   /// <param name="obj">Object to serialize.</param>
   /// <param name="path">File name of the JSON.</param>
   /// <param name="settings">Serializer settings (optional).</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool SerializeToFile(object? obj, string? path, JsonSerializerSettings? settings = null)
   {
      if (obj == null)
         throw new ArgumentNullException(nameof(obj));
      if (path == null)
         throw new ArgumentNullException(nameof(path));

      try
      {
         return FileHelper.WriteAllText(path, SerializeToString(obj, settings));
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not serialize the object to a file");
         throw;
      }
   }

   /// <summary>Serialize an object to an JSON-string.</summary>
   /// <param name="obj">Object to serialize.</param>
   /// <param name="settings">Serializer settings (optional).</param>
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

   /// <summary>Deserialize a JSON-file to an object.</summary>
   /// <param name="path">JSON-file of the object</param>
   /// <param name="settings">Serializer settings (optional).</param>
   /// <returns>Object</returns>
   /// <exception cref="Exception"></exception>
   public static T? DeserializeFromFile<T>(string? path, JsonSerializerSettings? settings = null)
   {
      if (path == null)
         throw new ArgumentNullException(nameof(path));

      try
      {
         return DeserializeFromString<T>(File.ReadAllText(path), settings);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not deserialize the object from a file");
         throw;
      }
   }

   /// <summary>Deserialize a JSON-string to an object.</summary>
   /// <param name="jsonAsString">JSON of the object</param>
   /// <param name="settings">Serializer settings (optional).</param>
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