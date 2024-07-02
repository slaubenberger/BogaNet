using System.Text;
using BogaNet.Helper;

namespace BogaNet.Extension;

/// <summary>
/// Extension methods for objects.
/// </summary>
public static class ObjectExtension
{
   #region Public methods

   /// <summary>
   /// Converts an object to a byte-array (as JSON).
   /// </summary>
   /// <param name="obj">Given object</param>
   /// <returns>Byte-array with the object</returns>
   public static byte[]? BNToByteArray(this object? obj)
   {
      return JsonHelper.SerializeToString(obj, JsonHelper.FORMAT_NONE).BNToByteArray();
   }

   /// <summary>
   /// Converts a byte-array (as JSON) to an object.
   /// </summary>
   /// <param name="bytes">Byte-array</param>
   /// <returns>Object from the byte-array</returns>
   public static T? BNToObject<T>(this byte[]? bytes)
   {
      return JsonHelper.DeserializeFromString<T>(bytes.BNToString(), JsonHelper.FORMAT_NONE);
   }

   /// <summary>
   /// Adds a generic ToString-method to objects.
   /// </summary>
   /// <param name="obj">Object for the generic ToString</param>
   /// <returns>Generic ToString</returns>
   public static string BNToString(this object? obj)
   {
      if (obj == null)
         return string.Empty;

      StringBuilder sb = new();

      sb.Append(obj.GetType().Name);
      sb.Append(":[");
      sb.Append(JsonHelper.SerializeToString(obj, JsonHelper.FORMAT_NONE));
      sb.Append(']');

      return sb.ToString();
   }

   #endregion
}