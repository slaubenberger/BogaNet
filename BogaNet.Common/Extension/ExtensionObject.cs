using System;
using System.Text;
using System.Reflection;
using BogaNet.Helper;

namespace BogaNet;

/// <summary>
/// Extension methods for objects.
/// </summary>
public static class ExtensionObject
{
   /// <summary>
   /// Searches for a field in an object and returns the value.
   /// </summary>
   /// <param name="obj">Object-instance</param>
   /// <param name="name">Name of the field</param>
   /// <param name="flags">Binding flags for the field (optional, default: NonPublic/Instance)</param>
   /// <returns>Value of the field</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static object? BNGetField(this object? obj, string name, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      if (obj == null)
         throw new ArgumentNullException(nameof(obj));

      if (string.IsNullOrEmpty(name))
         throw new ArgumentNullException(nameof(name));

      FieldInfo? field = obj.GetType().GetField(name, flags);

      if (field != null)
         return field.GetValue(obj)!;

      return null;
   }

   /// <summary>
   /// Searches for a field in an object and returns the value.
   /// </summary>
   /// <typeparam name="T">Type of the field</typeparam>
   /// <param name="obj">Object-instance</param>
   /// <param name="name">Name of the field</param>
   /// <param name="flags">Binding flags for the field (optional, default: NonPublic/Instance)</param>
   /// <returns>Value of the field</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static T? BNGetField<T>(this object? obj, string name, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      object? field = BNGetField(obj, name, flags);

      if (field != null)
         return (T)field;

      return default;
   }

   /// <summary>
   /// Sets the value of a field in an object.
   /// </summary>
   /// <param name="obj">Object-instance</param>
   /// <param name="name">Name of the field</param>
   /// <param name="value">Value as object</param>
   /// <param name="flags">Binding flags for the property (optional, default: NonPublic/Instance)</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void BNSetField(this object? obj, string name, object value, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      if (obj == null)
         throw new ArgumentNullException(nameof(obj));

      if (string.IsNullOrEmpty(name))
         throw new ArgumentNullException(nameof(name));

      if (value == null)
         throw new ArgumentNullException(nameof(value));

      obj.GetType().GetField(name, flags)?.SetValue(obj, value);
   }

   /// <summary>
   /// Searches for a property in an object and returns the value.
   /// </summary>
   /// <param name="obj">Object-instance</param>
   /// <param name="name">Name of the property</param>
   /// <param name="flags">Binding flags for the property (optional, default: NonPublic/Instance)</param>
   /// <returns>Value of the property</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static object? BNGetProperty(this object? obj, string name, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      if (obj == null)
         throw new ArgumentNullException(nameof(obj));

      if (string.IsNullOrEmpty(name))
         throw new ArgumentNullException(nameof(name));

      PropertyInfo? property = obj.GetType().GetProperty(name, flags);

      if (property != null)
         return property.GetValue(obj)!;

      return null;
   }

   /// <summary>
   /// Searches for a property in an object and returns the value.
   /// </summary>
   /// <typeparam name="T">Type of the property</typeparam>
   /// <param name="obj">Object-instance</param>
   /// <param name="name">Name of the property</param>
   /// <param name="flags">Binding flags for the property (optional, default: NonPublic/Instance)</param>
   /// <returns>Value of the property</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static T? BNGetProperty<T>(this object? obj, string name, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      var property = BNGetProperty(obj, name, flags);

      if (property != null)
         return (T)property;

      return default;
   }

   /// <summary>
   /// Sets the value of a property in an object.
   /// </summary>
   /// <param name="obj">Object-instance</param>
   /// <param name="name">Name of the property</param>
   /// <param name="value">Value as object</param>
   /// <param name="flags">Binding flags for the property (optional, default: NonPublic/Instance)</param>
   /// <exception cref="ArgumentNullException"></exception>
   public static void BNSetProperty(this object? obj, string name, object value, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      if (obj == null)
         throw new ArgumentNullException(nameof(obj));

      if (string.IsNullOrEmpty(name))
         throw new ArgumentNullException(nameof(name));

      if (value == null)
         throw new ArgumentNullException(nameof(value));

      obj.GetType().GetProperty(name, flags)?.SetValue(obj, value);
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
}