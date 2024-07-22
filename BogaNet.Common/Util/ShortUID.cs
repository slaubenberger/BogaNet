using System.Text;
using System;
using BogaNet.Encoder;
using BogaNet.Extension;

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
      Code = data.BNToString(Encoding.ASCII);
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
      return Code.BNToByteArray(Encoding.ASCII);
   }

   /// <summary>
   /// Converts the ShortUID to a Guid.
   /// </summary>
   /// <returns>Guid-instance</returns>
   public Guid ToGuid()
   {
      //Guid guid = new(Base64.FromBase64String(Code + "=="));
      //Guid guid = new(Base85.FromBase85String(Code));
      Guid guid = new(Base91.FromBase91String(Code));

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

      return Code.Equals(su.Code);
   }

   public override int GetHashCode()
   {
      return Code.GetHashCode();
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

      //string guid = Base64.ToBase64String(uid.ToByteArray());
      //string guid = Base85.ToBase85String(uid.ToByteArray());
      string guid = Base91.ToBase91String(uid.ToByteArray());

      return new ShortUID(guid);
   }
}