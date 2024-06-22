using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Numerics;
using System;
using System.Linq;

namespace BogaNet.Helper;

/// <summary>
/// Helper methods for objects.
/// </summary>
public abstract class ObjectHelper
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(ObjectHelper));

   #endregion

   #region Public methods

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