using System.Collections.Generic;
using BogaNet.Extension;
using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure object implementation. This prevents the object from being readable in the memory of the application.
/// </summary>
public class ObjectSec<T> //NUnit
{
   //TODO the usage should be improved if possible, currently it's more like a storage container for objects...

   #region Variables

   private readonly ByteObf[] key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] iv = AESHelper.GenerateIV().BNToByteObfArray();
   private byte[]? secretValue;

   #endregion

   #region Properties

   private T _value
   {
      get => AESHelper.Decrypt(secretValue, key.ToByteArray(), iv.ToByteArray()).BNToObject<T>() ?? default;
      set => secretValue = AESHelper.Encrypt(value.BNToByteArray(), key.ToByteArray(), iv.ToByteArray());
   }

   #endregion

   #region Constructors

   private ObjectSec(T value)
   {
      _value = value;
   }

   #endregion

   #region Operators

   public static implicit operator ObjectSec<T>(T value)
   {
      return new ObjectSec<T>(value);
   }

   public static implicit operator T(ObjectSec<T> custom)
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

      if (obj.GetType() == typeof(T))
         return _value.Equals(obj);

      return obj.GetType() == GetType() && equals((ObjectSec<T>)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<T>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   private bool equals(ObjectSec<T> other)
   {
      return EqualityComparer<T>.Default.Equals(_value, other._value);
   }

   #endregion
}