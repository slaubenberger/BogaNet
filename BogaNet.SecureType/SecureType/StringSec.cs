using System.Collections.Generic;
using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure string implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class StringSec //NUnit
{
   #region Variables

   private static readonly ByteObf[] key = AESHelper.GenerateKey().BNToByteObfArray();
   private static readonly ByteObf[] iv = AESHelper.GenerateIV().BNToByteObfArray();
   private byte[]? secretValue;

   #endregion

   #region Properties

   private string _value
   {
      get => AESHelper.Decrypt(secretValue, key.ToByteArray(), iv.ToByteArray()).BNToString() ?? string.Empty;
      set => secretValue = AESHelper.Encrypt(value.BNToByteArray(), key.ToByteArray(), iv.ToByteArray());
   }

   #endregion

   #region Constructors

   private StringSec(string value)
   {
      _value = value;
   }

   #endregion

   #region Operators

   public static implicit operator StringSec(string value)
   {
      return new StringSec(value);
   }

   public static implicit operator string(StringSec custom)
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

      return obj.GetType() == GetType() && equals((StringSec)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<string>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   private bool equals(StringSec other)
   {
      return EqualityComparer<string>.Default.Equals(_value, other._value);
   }

   #endregion
}