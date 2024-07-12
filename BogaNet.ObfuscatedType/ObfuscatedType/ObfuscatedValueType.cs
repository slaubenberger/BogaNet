using System.Collections.Generic;
using System.Numerics;
using BogaNet.Extension;
using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated implementation for value types.
/// </summary>
/// <typeparam name="TCustom"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class ObfuscatedValueType<TCustom, TValue> where TValue : INumber<TValue>
{
   #region Variables

   private static readonly byte _shift = Obfuscator.GenerateIV();
   private readonly byte _offset;
   private readonly byte _iv;
   private byte[] _obfValue = [];

   #endregion

   #region Properties

   private byte _obfOffset => (byte)(_offset - _shift);
   protected byte _obf => (byte)(_iv - _obfOffset);

   protected TValue _value
   {
      get => Obfuscator.Deobfuscate(_obfValue, _obf).BNToNumber<TValue>();

      private set => _obfValue = Obfuscator.Obfuscate(value.BNToByteArray(), _obf);
   }

   #endregion

   #region Constructors

   protected ObfuscatedValueType(TValue value)
   {
      _offset = (byte)(Obfuscator.GenerateIV() + _shift);
      _iv = (byte)(Obfuscator.GenerateIV() + _obfOffset);
      _value = value;
   }

   #endregion

   #region Operators

   public static bool operator <(ObfuscatedValueType<TCustom, TValue> a, ObfuscatedValueType<TCustom, TValue> b)
   {
      return Comparer<TValue>.Default.Compare(a._value, b._value) < 0;
   }

   public static bool operator >(ObfuscatedValueType<TCustom, TValue> a, ObfuscatedValueType<TCustom, TValue> b)
   {
      return !(a < b);
   }

   public static bool operator <=(ObfuscatedValueType<TCustom, TValue> a, ObfuscatedValueType<TCustom, TValue> b)
   {
      return a < b || a == b;
   }

   public static bool operator >=(ObfuscatedValueType<TCustom, TValue> a, ObfuscatedValueType<TCustom, TValue> b)
   {
      return a > b || a == b;
   }

   public static bool operator ==(ObfuscatedValueType<TCustom, TValue> a, ObfuscatedValueType<TCustom, TValue> b)
   {
      return a.Equals(b);
   }

   public static bool operator !=(ObfuscatedValueType<TCustom, TValue> a, ObfuscatedValueType<TCustom, TValue> b)
   {
      return !(a == b);
   }

   public static TCustom operator +(ObfuscatedValueType<TCustom, TValue> a, ObfuscatedValueType<TCustom, TValue> b)
   {
      return (dynamic)a._value + b._value;
   }

   public static TCustom operator -(ObfuscatedValueType<TCustom, TValue> a, ObfuscatedValueType<TCustom, TValue> b)
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

      return obj.GetType() == GetType() && equals((ObfuscatedValueType<TCustom, TValue>)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<TValue>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   protected bool equals(ObfuscatedValueType<TCustom, TValue> other)
   {
      return EqualityComparer<TValue>.Default.Equals(_value, other._value);
   }

   #endregion
}