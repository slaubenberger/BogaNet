using System.Collections.Generic;

namespace BogaNet.Util;

/// <summary>
/// Simple dependency injection (DI) container.
/// </summary>
public static class DIContainer
{
   private static readonly Dictionary<System.Type, object?> _references = new();

   /// <summary>
   /// Bind an instance to a given interface.
   /// </summary>
   /// <param name="theInstance">Instance of the object</param>
   /// <typeparam name="TInterface">Interface of the instance</typeparam>
   /// <typeparam name="TValue">Class of the object</typeparam>
   public static void Bind<TInterface, TValue>(TValue? theInstance) where TValue : TInterface where TInterface : class
   {
      _references[typeof(TInterface)] = theInstance;
   }

   /// <summary>
   /// Resolves an interface to a bound object.
   /// </summary>
   /// <typeparam name="TInterface">Interface of the instance</typeparam>
   /// <returns>Object instance</returns>
   public static TInterface? Resolve<TInterface>()
   {
      if (_references.TryGetValue(typeof(TInterface), out object? value))
         return value == null ? default : (TInterface)value;

      return default;
   }

   /// <summary>
   /// Removes a bound object.
   /// </summary>
   /// <typeparam name="TInterface">Interface of the instance</typeparam>
   /// <returns>Object instance</returns>
   public static void Unbind<TInterface>()
   {
      if (_references.ContainsKey(typeof(TInterface)))
         _references.Remove(typeof(TInterface));
   }

   /// <summary>
   /// True if an interface is bound to an object.
   /// </summary>
   /// <typeparam name="TInterface">Interface of the instance</typeparam>
   /// <returns>True if an interface is bound to an object</returns>
   public static bool isBound<TInterface>()
   {
      return Resolve<TInterface>() != null;
   }
}