using System;
using System.Linq;
using System.Numerics;

namespace BogaNet.Util;

/// <summary>
/// Short Guid implementation with 22 characters instead of 36 (normal Guid).
/// </summary>
public class ShortUID
{
   #region Variables

   private readonly byte[] _guid;
   private string? _uid;

   /// <summary>
   /// Guid as 22 character representation, safe format suitable for URLs and files.
   /// </summary>
   public string UID => _uid ??= Convert.ToBase64String(_guid).Replace("/", "_").Replace("+", "-").Replace("=", "");

   #endregion

   #region Constructors

   /// <summary>
   /// Constructor for a ShortUID with a given byte-array.
   /// </summary>
   /// <param name="data">ShortUID as byte-array</param>
   public ShortUID(byte[] data)
   {
      _guid = data;
   }

   /// <summary>
   /// Constructor for a ShortUID with a given string.
   /// </summary>
   /// <param name="uid">ShortUID as string</param>
   public ShortUID(string uid)
   {
      _guid = Convert.FromBase64String(uid.Replace("_", "/").Replace("-", "+") + "==");
   }

   /// <summary>
   /// Empty constructor for a new ShortUID.
   /// </summary>
   public ShortUID()
   {
      _guid = NewShortUID()._guid;
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
      return _guid;
   }

   /// <summary>
   /// Converts the ShortUID to a Guid.
   /// </summary>
   /// <returns>Guid-instance</returns>
   public Guid ToGuid()
   {
      return new Guid(_guid);
   }

   #endregion

   #region Overridden methods

   public override string ToString()
   {
      return UID;
   }

   public override bool Equals(object? obj)
   {
      if (obj == null || GetType() != obj.GetType())
         return false;

      ShortUID su = (ShortUID)obj;

      return _guid.SequenceEqual(su._guid);
   }

   public override int GetHashCode()
   {
      return new BigInteger(_guid).GetHashCode();
   }

   #endregion
}

/// <summary>
/// Extension methods for Guid.
/// </summary>
public static class GuidExtension
{
   /// <summary>
   /// Converts a Guid to a ShortUID.
   /// </summary>
   /// <param name="uid">Guid-instance</param>
   /// <returns>ShortUID-instance</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static ShortUID BNToShortUID(this Guid uid)
   {
      ArgumentNullException.ThrowIfNull(uid);

      return new ShortUID(uid.ToByteArray());
   }
}