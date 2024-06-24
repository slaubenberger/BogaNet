using System.Reflection;
using System;

namespace BogaNet.Util;

/// <summary>
/// Generic singleton.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : class
{
   private static object mutex = new();
   private static T? instance;

   public static T? Instance
   {
      get
      {
         if (instance == null)
         {
            lock (mutex)
            {
               if (instance == null)
               {
                  ConstructorInfo? ci = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                  if (ci == null)
                  {
                     throw new InvalidOperationException("Class must contain a private constructor");
                  }

                  instance = (T)ci.Invoke(null);
               }
            }
         }

         return instance;
      }
   }
}