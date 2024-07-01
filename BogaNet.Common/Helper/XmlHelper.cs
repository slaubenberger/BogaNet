using System.Text;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.IO;

namespace BogaNet.Helper;

/// <summary>
/// Helper for XML operations.
/// </summary>
public abstract class XmlHelper
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(XmlHelper));

   #endregion

   #region Public methods

   /// <summary>
   /// Serialize an object to a XML-file.
   /// </summary>
   /// <param name="obj">Object to serialize</param>
   /// <param name="filename">File name of the XML</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static bool SerializeToFile<T>(T? obj, string? filename)
   {
      return Task.Run(() => SerializeToFileAsync(obj, filename)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Serialize an object to a XML-file asynchronously.
   /// </summary>
   /// <param name="obj">Object to serialize</param>
   /// <param name="filename">File name of the XML</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<bool> SerializeToFileAsync<T>(T? obj, string? filename)
   {
      ArgumentNullException.ThrowIfNull(obj);
      ArgumentNullException.ThrowIfNull(filename);

      try
      {
         return await FileHelper.WriteAllTextAsync(filename, SerializeToString(obj));
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not serialize the object to a file");
         throw;
      }
   }

   /// <summary>
   /// Serialize an object to a XML-string.
   /// </summary>
   /// <param name="obj">Object to serialize</param>
   /// <returns>Object as XML-string</returns>
   /// <exception cref="Exception"></exception>
   public static string SerializeToString<T>(T? obj)
   {
      ArgumentNullException.ThrowIfNull(obj);

      byte[] result = SerializeToByteArray(obj);

      return Encoding.UTF8.GetString(result).Trim('\uFEFF', '\u200B'); //remove invalid BOM
   }

   /// <summary>
   /// Serialize an object to a XML byte-array.
   /// </summary>
   /// <param name="obj">Object to serialize</param>
   /// <returns>Object as XML byte-array</returns>
   /// <exception cref="Exception"></exception>
   public static byte[] SerializeToByteArray<T>(T? obj)
   {
      ArgumentNullException.ThrowIfNull(obj);

      try
      {
         MemoryStream ms = new();

         XmlSerializer xs = new(obj.GetType());
         XmlTextWriter xmlTextWriter = new(ms, Encoding.UTF8);
         xmlTextWriter.Formatting = Formatting.Indented;
         xmlTextWriter.Indentation = 3;
         xs.Serialize(xmlTextWriter, obj);

         Stream? stream = xmlTextWriter.BaseStream;

         if (stream != null)
            ms = (MemoryStream)stream;

         return ms.ToArray();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not serialize the object to a byte-array");
         throw;
      }

      //return null;
   }

   /// <summary>
   /// Deserialize a XML-file to an object.
   /// </summary>
   /// <param name="filename">XML-file of the object</param>
   /// <param name="skipBOM">Skip BOM (optional, default: false)</param>
   /// <returns>Object</returns>
   /// <exception cref="Exception"></exception>
   public static T? DeserializeFromFile<T>(string? filename, bool skipBOM = false)
   {
      return Task.Run(() => DeserializeFromFileAsync<T>(filename, skipBOM)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Deserialize a XML-file to an object asynchronously.
   /// </summary>
   /// <param name="filename">XML-file of the object</param>
   /// <param name="skipBOM">Skip BOM (optional, default: false)</param>
   /// <returns>Object</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<T?> DeserializeFromFileAsync<T>(string? filename, bool skipBOM = false)
   {
      ArgumentNullException.ThrowIfNull(filename);

      try
      {
         if (FileHelper.ExistsFile(filename))
         {
            string? data = await FileHelper.ReadAllTextAsync(filename);

            if (string.IsNullOrEmpty(data))
            {
               _logger.LogWarning($"Data was null: {filename}");
            }
            else
            {
               return DeserializeFromString<T>(data, skipBOM);
            }
         }
         else
         {
            _logger.LogError($"File does not exist: {filename}");
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not deserialize the object from a file");
         throw;
      }

      return default;
   }

   /// <summary>
   /// Deserialize a XML-string to an object.
   /// </summary>
   /// <param name="xmlAsString">XML of the object</param>
   /// <param name="skipBOM">Skip BOM (optional, default: true)</param>
   /// <returns>Object</returns>
   /// <exception cref="Exception"></exception>
   public static T? DeserializeFromString<T>(string? xmlAsString, bool skipBOM = true)
   {
      ArgumentNullException.ThrowIfNull(xmlAsString);

      try
      {
         XmlSerializer xs = new(typeof(T));

         using StringReader sr = new(xmlAsString.Trim());

         if (skipBOM)
            sr.Read(); //skip BOM

         object? obj = xs.Deserialize(sr);

         if (obj != null)
            return (T)obj;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not deserialize the object from a string");
         throw;
      }

      return default;
   }

   /// <summary>
   /// Deserialize a XML byte-array to an object.
   /// </summary>
   /// <param name="data">XML of the object</param>
   /// <returns>Object</returns>
   /// <exception cref="Exception"></exception>
   public static T? DeserializeFromByteArray<T>(byte[]? data)
   {
      ArgumentNullException.ThrowIfNull(data);

      try
      {
         XmlSerializer xs = new(typeof(T));
         MemoryStream ms = new(data);

         object? obj = xs.Deserialize(ms);

         if (obj != null)
            return (T)obj;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not deserialize the object from a byte-array");
         throw;
      }

      return default;
   }

   #endregion
}