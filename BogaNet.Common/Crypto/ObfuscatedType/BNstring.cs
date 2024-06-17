using System.Collections.Generic;

namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated string implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class BNstring
{
   #region Variables

   private static readonly byte obf = Obfuscator.GenerateIV();
   private byte[]? obfValue;

   /*
   //secure, but slow implementation
   private static readonly byte[] key = AESHelper.GenerateKey();
   private static readonly byte[] iv = AESHelper.GenerateIV();
   private byte[] secretValue;
   */

   #endregion

   #region Properties

   private string _value
   {
      get
      {
         //return AESHelper.Decrypt(secretValue, key, iv).BNToString();
         return Obfuscator.Deobfuscate(obfValue, obf).BNToString() ?? string.Empty;
      }
      set
      {
         //secretValue = AESHelper.Encrypt(value.BNToByteArray(), key, iv);
         obfValue = Obfuscator.Obfuscate(value.BNToByteArray(), obf);
      }
   }

   #endregion

   #region Constructors

   public BNstring(string value)
   {
      _value = value;
   }

   #endregion

   #region Operators

   public static implicit operator BNstring(string value)
   {
      return new BNstring(value);
   }

   public static implicit operator string(BNstring custom)
   {
      return custom._value;
   }

/*
   public static bool operator ==(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
   {
      return a.Equals((object)b);
   }

   public static bool operator !=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
   {
      return !(a == b);
   }
*/

   #endregion

   #region Overridden methods

   public override string ToString()
   {
      return _value;
   }

   public override bool Equals(object? obj)
   {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;

      if (obj.GetType() == typeof(string))
         return _value.Equals(obj);

      if (obj.GetType() != GetType()) return false;
      return equals((BNstring)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<string>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   private bool equals(BNstring other)
   {
      return EqualityComparer<string>.Default.Equals(_value, other._value);
   }

   #endregion
}