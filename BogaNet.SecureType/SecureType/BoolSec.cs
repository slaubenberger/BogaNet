using System.Collections.Generic;
using System;
using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure boolean implementation. This prevents the value from being "plain" in the memory of the application.
/// </summary>
public class BoolSec //NUnit
{
   #region Variables

   private static readonly ByteObf[] key = AESHelper.GenerateKey().BNToByteObfArray();
   private static readonly ByteObf[] iv = AESHelper.GenerateIV().BNToByteObfArray();
   private byte[]? secretValue;

   #endregion

   #region Properties

   private bool _value
   {
      get => BitConverter.ToBoolean(AESHelper.Decrypt(secretValue, key.ToByteArray(), iv.ToByteArray()));
      set => secretValue = AESHelper.Encrypt(BitConverter.GetBytes(value), key.ToByteArray(), iv.ToByteArray());
   }

   #endregion

   #region Constructors

   private BoolSec(bool value)
   {
      _value = value;
   }

   #endregion

   #region Operators

   public static implicit operator BoolSec(bool value)
   {
      return new BoolSec(value);
   }

   public static implicit operator bool(BoolSec custom)
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
      return _value.ToString();
   }

   public override bool Equals(object? obj)
   {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;

      if (obj.GetType() == typeof(bool))
         return _value.Equals(obj);

      return obj.GetType() == GetType() && equals((BoolSec)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<bool>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   private bool equals(BoolSec other)
   {
      return EqualityComparer<bool>.Default.Equals(_value, other._value);
   }

   #endregion
}