using System.Collections.Generic;
using BogaNet.Util;
using System;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated boolean implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class BoolObf //NUnit
{
   #region Variables

   private static readonly byte _shift = Obfuscator.GenerateIV();
   private readonly byte _offset;
   private readonly byte _iv;
   private byte[] _obfValue = [];

   #endregion

   #region Properties

   private byte _obfOffset => (byte)(_offset - _shift);
   private byte _obf => (byte)(_iv - _obfOffset);

   private bool _value
   {
      get => BitConverter.ToBoolean(Obfuscator.Deobfuscate(_obfValue, _obf));
      set => _obfValue = Obfuscator.Obfuscate(BitConverter.GetBytes(value), _obf);
   }

   #endregion

   #region Constructors

   private BoolObf(bool value)
   {
      _offset = (byte)(Obfuscator.GenerateIV() + _shift);
      _iv = (byte)(Obfuscator.GenerateIV() + _obfOffset);
      _value = value;
   }

   #endregion

   #region Operators

   public static implicit operator BoolObf(bool value)
   {
      return new BoolObf(value);
   }

   public static implicit operator bool(BoolObf custom)
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

      return obj.GetType() == GetType() && equals((BoolObf)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<bool>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   private bool equals(BoolObf other)
   {
      return EqualityComparer<bool>.Default.Equals(_value, other._value);
   }

   #endregion
}