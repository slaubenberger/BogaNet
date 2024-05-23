using Microsoft.Extensions.Logging;

namespace BogaNet.Util;

/// <summary>
/// Short GUID implementation with a length of 22 characters (instead 36 of the normal Guid)
/// </summary>
public class ShortUID
{
   public string Code { get; private set; }

   /// <summary>
   /// Constructor with a ShortUID-code
   /// </summary>
   /// <param name="code">Code of the ShortUID</param>
   public ShortUID(string code)
   {
      Code = code.BNFixedLength(22, '-');
   }

   /// <summary>
   /// Generates a new ShortUID
   /// </summary>
   /// <returns>New ShortUID</returns>
   public static ShortUID NewShortUID()
   {
      return Guid.NewGuid().BNToShortUID();
   }

   public override string ToString()
   {
      return Code;
   }

   public override bool Equals(object? obj)
   {
      if (obj == null || GetType() != obj.GetType())
         return false;

      ShortUID su = (ShortUID)obj;

      return Code == su.Code;
   }

   public override int GetHashCode()
   {
      return base.GetHashCode();
   }
}

/// <summary>
/// Extension methods for ShortUID
/// </summary>
public static class ExtensionShortUID
{
   private static readonly ILogger _logger = GlobalLogging.CreateLogger("ExtensionShortUID");

   /// <summary>
   /// Converts a ShortUID to a Guid
   /// </summary>
   /// <param name="uid">ShortUID-instance</param>
   /// <returns>Guid-instance</returns>
   public static Guid BNToGuid(this ShortUID? uid)
   {
      try
      {
         string guidText = uid?.Code.Replace("_", "/").Replace("-", "+") + "==";
         Guid guid = new(guidText.BNFromBase64ToByteArray() ?? Array.Empty<byte>());

         return guid;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not convert ShortUID to Guid!");
         throw;
      }
   }

   /// <summary>
   /// Converts a Guid to a ShortUID
   /// </summary>
   /// <param name="uid">Guid-instance</param>
   /// <returns>ShortUID-instance</returns>
   public static ShortUID BNToShortUID(this Guid uid)
   {
      string? guid = uid.ToByteArray().BNToBase64();
      return new ShortUID(guid?.Substring(0, guid.Length - 2).Replace("/", "_").Replace("+", "-") ?? string.Empty);
   }
}