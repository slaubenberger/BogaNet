using System.Text;

namespace BogaNet;

/// <summary>
/// Extension methods for objects.
/// </summary>
public static class ExtensionObject
{
   /// <summary>
   /// Extension method for objects.
   /// Adds a generic ToString-method to objects
   /// </summary>
   /// <param name="obj">Object for the generic ToString</param>
   /// /// <param name="bindingFlags">Used binding flags (optional, default: Instance|NonPublic|Public</param>
   /// <returns>Generic ToString</returns>
   public static string BNToString(this object? obj, System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.Instance |
                                                                                                   System.Reflection.BindingFlags.NonPublic |
                                                                                                   System.Reflection.BindingFlags.Public)
   {
      if (obj == null)
         return string.Empty;

      StringBuilder sb = new();

      sb.Append(obj.GetType().Name);
      sb.Append(":[");
      sb.Append(BogaNet.IO.JsonHelper.SerializeToString(obj, BogaNet.IO.JsonHelper.FORMAT_NONE));
      sb.Append(']');

      return sb.ToString();
   }
}