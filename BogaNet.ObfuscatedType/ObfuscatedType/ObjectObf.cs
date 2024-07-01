using System.Collections.Generic;
using BogaNet.Extension;
using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated object implementation. This prevents the object from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class ObjectObf<T> where T : class //NUnit
{
   //TODO the usage should be improved if possible, currently it's more like a storage container for objects...

   #region Variables

   private static readonly byte _shift = Obfuscator.GenerateIV();
   private readonly byte _offset;
   private readonly byte _iv;
   private byte[]? _obfValue;

   #endregion

   #region Properties

   private byte _obfOffset => (byte)(_offset - _shift);
   private byte _obf => (byte)(_iv - _obfOffset);

   private T _value
   {
      get => (Obfuscator.Deobfuscate(_obfValue, _obf).BNToObject<T>() ?? default)!;
      set => _obfValue = Obfuscator.Obfuscate(value.BNToByteArray(), _obf);
   }

   #endregion

   #region Constructors

   private ObjectObf(T value)
   {
      _offset = (byte)(Obfuscator.GenerateIV() + _shift);
      _iv = (byte)(Obfuscator.GenerateIV() + _obfOffset);
      _value = value;
   }

   #endregion

   #region Operators

   public static implicit operator ObjectObf<T>(T value)
   {
      return new ObjectObf<T>(value);
   }

   public static implicit operator T(ObjectObf<T> custom)
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
      return _value.ToString()!;
   }

   public override bool Equals(object? obj)
   {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;

      if (obj.GetType() == typeof(T))
         return _value.Equals(obj);

      return obj.GetType() == GetType() && equals((ObjectObf<T>)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<T>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   private bool equals(ObjectObf<T> other)
   {
      return EqualityComparer<T>.Default.Equals(_value, other._value);
   }

   #endregion
}