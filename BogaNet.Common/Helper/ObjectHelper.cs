using Microsoft.Extensions.Logging;
using System.Reflection;
using System;
using System.Linq;

namespace BogaNet.Helper;

/// <summary>
/// Helper methods for objects.
/// </summary>
public abstract class ObjectHelper
{
   #region Variables

   private static readonly ILogger<ObjectHelper> _logger = GlobalLogging.CreateLogger<ObjectHelper>();

   #endregion

   #region Public methods

   /// <summary>
   /// Searches for a field in an object and returns the value.
   /// </summary>
   /// <param name="obj">Object-instance</param>
   /// <param name="name">Name of the field</param>
   /// <param name="flags">Binding flags for the field (optional, default: NonPublic/Instance)</param>
   /// <returns>Value of the field</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static object? GetField(object? obj, string name, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      ArgumentNullException.ThrowIfNull(obj);
      ArgumentNullException.ThrowIfNull(name);

      FieldInfo? field = obj.GetType().GetField(name, flags);

      return field != null ? field.GetValue(obj)! : null;
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
   public static T? GetField<T>(object? obj, string name, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      object? field = GetField(obj, name, flags);

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
   public static void SetField(object? obj, string name, object value, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      ArgumentNullException.ThrowIfNull(obj);
      ArgumentNullException.ThrowIfNull(name);
      ArgumentNullException.ThrowIfNull(value);

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
   public static object? GetProperty(object? obj, string name, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      ArgumentNullException.ThrowIfNull(obj);
      ArgumentNullException.ThrowIfNull(name);

      PropertyInfo? property = obj.GetType().GetProperty(name, flags);

      return property != null ? property.GetValue(obj)! : null;
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
   public static T? GetProperty<T>(object? obj, string name, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      var property = GetProperty(obj, name, flags);

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
   public static void SetProperty(object? obj, string name, object value, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
   {
      ArgumentNullException.ThrowIfNull(obj);
      ArgumentNullException.ThrowIfNull(name);
      ArgumentNullException.ThrowIfNull(value);

      obj.GetType().GetProperty(name, flags)?.SetValue(obj, value);
   }

   /// <summary>
   /// Invokes a method on a full qualified class.
   /// </summary>
   /// <param name="className">Full qualified name of the class</param>
   /// <param name="methodName">Public static method of the class to execute</param>
   /// <param name="flags">Binding flags for the method (optional, default: static/public)</param>
   /// <param name="parameters">Parameters for the method (optional)</param>
   /// <exception cref="Exception"></exception>
   public static object? InvokeMethod(string? className, string? methodName, BindingFlags flags = BindingFlags.Static | BindingFlags.Public, params object[] parameters)
   {
      if (string.IsNullOrEmpty(className))
      {
         _logger.LogWarning("'className' is null or empty; can not execute.");
         return null;
      }

      if (string.IsNullOrEmpty(methodName))
      {
         _logger.LogWarning("'methodName' is null or empty; can not execute.");
         return null;
      }

      foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
      {
         try
         {
            if (type.FullName?.Equals(className) == true)
               if (type.IsClass)
               {
                  MethodInfo? method = type.GetMethod(methodName, flags);

                  if (method != null)
                     return method.Invoke(null, parameters);
               }
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not execute method call '{methodName}' for '{className}'");
            throw;
         }
      }

      _logger.LogWarning($"Could not find class ' {className}' or method '{methodName}'");

      return null;
   }

   #endregion
}