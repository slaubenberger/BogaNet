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

   private readonly ByteObf[] _key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] _iv = AESHelper.GenerateIV().BNToByteObfArray();
   private byte[] _secretValue = [];

   #endregion

   #region Properties

   private bool _value
   {
      get => BitConverter.ToBoolean(AESHelper.Decrypt(_secretValue, _key.ToByteArray(), _iv.ToByteArray()));
      set => _secretValue = AESHelper.Encrypt(BitConverter.GetBytes(value), _key.ToByteArray(), _iv.ToByteArray());
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

      if (obj is bool)
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