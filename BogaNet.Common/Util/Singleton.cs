using System.Reflection;
using System;

namespace BogaNet.Util;

/// <summary>
/// Generic singleton.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : class
{
   #region Variables

   private static object _mutex = new();
   private static T? _instance;

   #endregion

   #region Public methods

   public static T Instance
   {
      get
      {
         if (_instance == null)
         {
            lock (_mutex)
            {
               if (_instance == null)
               {
                  //_instance = new T();

                  ConstructorInfo? ci = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                  if (ci == null)
                  {
                     throw new InvalidOperationException("Class must contain a private constructor");
                  }

                  _instance = (T)ci.Invoke(null);
               }
            }
         }

         return _instance;
      }
   }

   #endregion
}