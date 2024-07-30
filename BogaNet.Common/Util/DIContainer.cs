using System.Collections.Generic;
using System;

namespace BogaNet.Util;

/// <summary>
/// Simple dependency injection (DI) container.
/// </summary>
public static class DIContainer
{
   private static readonly Dictionary<Type, object?> _container = new();

   #region Public methods

   /// <summary>
   /// Bind an instance to a given Type.
   /// </summary>
   /// <param name="instance">Instance of the Type</param>
   /// <typeparam name="TType">Type (interface/class) of the instance</typeparam>
   /// <typeparam name="TValue">Class of the instance</typeparam>
   /// <exception cref="ArgumentNullException"></exception>
   public static void Bind<TType, TValue>(TValue instance) where TValue : TType where TType : class
   {
      ArgumentNullException.ThrowIfNull(instance);

      //TODO check for existing instances?
      _container[typeof(TType)] = instance;
   }

   /// <summary>
   /// Resolves a Type to a bound instance.
   /// </summary>
   /// <typeparam name="TType">Type (interface/class) of the instance</typeparam>
   /// <returns>Type-instance</returns>
   /// <exception cref="Exception"></exception>
   public static TType Resolve<TType>()
   {
      if (!_container.ContainsKey(typeof(TType)))
         throw new KeyNotFoundException();

      object? value = _container[typeof(TType)];
      return (TType)value!;
   }

   /// <summary>
   /// Resolves a Type to a bound instance.
   /// </summary>
   /// <typeparam name="TType">Type (interface/class) of the instance</typeparam>
   /// <returns>Type-instance</returns>
   public static bool TryResolve<TType>(out TType result)
   {
      if (!_container.ContainsKey(typeof(TType)))
      {
         result = default!;
         return false;
      }

      result = Resolve<TType>();
      return true;
   }

   /// <summary>
   /// Removes a bound instance.
   /// </summary>
   /// <typeparam name="TType">Type (interface/class) of the instance</typeparam>
   public static void Unbind<TType>()
   {
      if (_container.ContainsKey(typeof(TType)))
         _container.Remove(typeof(TType));
   }

   /// <summary>
   /// True if a Type is bound to an instance.
   /// </summary>
   /// <typeparam name="TType">Type (interface/class) of the instance</typeparam>
   /// <returns>True if a Type is bound to an instance</returns>
   public static bool IsBound<TType>()
   {
      return _container.ContainsKey(typeof(TType));
   }

   #endregion
}