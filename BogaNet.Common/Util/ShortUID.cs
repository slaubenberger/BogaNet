using System.Text;

namespace BogaNet.Util;

/// <summary>
/// Short Guid implementation with a length of 22 characters (instead 36 of the normal Guid).
/// </summary>
public class ShortUID
{
   #region Variables

   public string Code { get; set; }

   #endregion

   #region Constructors

   /// <summary>
   /// Constructor for a ShortUID with a given byte-array.
   /// </summary>
   /// <param name="data">ShortUID as byte-array</param>
   public ShortUID(byte[] data)
   {
      Code = data.BNToString(Encoding.ASCII) ?? string.Empty;
   }

   /// <summary>
   /// Constructor for a ShortUID with a given string.
   /// </summary>
   /// <param name="code">ShortUID as string</param>
   public ShortUID(string code)
   {
      Code = code; //.BNFixedLength(22, '-');
   }

   /// <summary>
   /// Empty constructor for a new ShortUID.
   /// </summary>
   public ShortUID()
   {
      Code = NewShortUID().Code;
   }

   #endregion

   #region Public methods

   /// <summary>
   /// Generates a new ShortUID.
   /// </summary>
   /// <returns>New ShortUID</returns>
   public static ShortUID NewShortUID()
   {
      return Guid.NewGuid().BNToShortUID();
   }

   /// <summary>
   /// Returns the ShortUID as byte-array.
   /// </summary>
   /// <returns>ShortUID as byte-array</returns>
   public byte[] ToByteArray()
   {
      return Code.BNToByteArray(Encoding.ASCII) ?? Array.Empty<byte>();
   }

   /// <summary>
   /// Converts the ShortUID to a Guid.
   /// </summary>
   /// <returns>Guid-instance</returns>
   public Guid ToGuid()
   {
      string guidText = Code.Replace("_", "/").Replace("-", "+") + "==";
      Guid guid = new(guidText.BNFromBase64ToByteArray() ?? Array.Empty<byte>());

      return guid;
   }

   #endregion

   #region Overridden methods

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

   #endregion
}

/// <summary>
/// Extension methods for Guid.
/// </summary>
public static class ExtensionGuid
{
   /// <summary>
   /// Converts a Guid to a ShortUID.
   /// </summary>
   /// <param name="uid">Guid-instance</param>
   /// <returns>ShortUID-instance</returns>
   public static ShortUID BNToShortUID(this Guid uid)
   {
      string? guid = uid.ToByteArray().BNToBase64();
      return new ShortUID(guid?.Substring(0, guid.Length - 2).Replace("/", "_").Replace("+", "-") ?? string.Empty);
   }
}