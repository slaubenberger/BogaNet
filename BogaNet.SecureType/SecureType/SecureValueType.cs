using System.Collections.Generic;
using System.Numerics;
using BogaNet.Extension;
using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure implementation for value types.
/// </summary>
/// <typeparam name="TCustom"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class SecureValueType<TCustom, TValue> where TValue : INumber<TValue>
{
   #region Variables

   private readonly ByteObf[] _key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] _iv = AESHelper.GenerateIV().BNToByteObfArray();
   private byte[]? secretValue;

   #endregion

   #region Properties

   protected ByteObf[] key => _key;

   protected ByteObf[] iv => _iv;

   protected TValue _value
   {
      get => AESHelper.Decrypt(secretValue, key.ToByteArray(), iv.ToByteArray()).BNToNumber<TValue>()!;

      private set => secretValue = AESHelper.Encrypt(value.BNToByteArray(), key.ToByteArray(), iv.ToByteArray());
   }

   #endregion

   #region Constructors

   protected SecureValueType(TValue value)
   {
      _value = value;
   }

   #endregion

   #region Operators

   public static bool operator <(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return Comparer<TValue>.Default.Compare(a._value, b._value) < 0;
   }

   public static bool operator >(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return !(a < b);
   }

   public static bool operator <=(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return a < b || a == b;
   }

   public static bool operator >=(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return a > b || a == b;
   }

   public static bool operator ==(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return a.Equals(b);
   }

   public static bool operator !=(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return !(a == b);
   }

   public static TCustom operator +(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return (dynamic)a._value + b._value;
   }

   public static TCustom operator -(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return (dynamic)a._value - b._value;
   }

   #endregion

   #region Overridden methods

   public override string ToString()
   {
      return _value.ToString() ?? string.Empty;
   }

   public override bool Equals(object? obj)
   {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;

      if (obj.GetType() == typeof(TValue))
         return _value.Equals(obj);

      return obj.GetType() == GetType() && equals((SecureValueType<TCustom, TValue>)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<TValue>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   protected bool equals(SecureValueType<TCustom, TValue> other)
   {
      return EqualityComparer<TValue>.Default.Equals(_value, other._value);
   }

   #endregion
}