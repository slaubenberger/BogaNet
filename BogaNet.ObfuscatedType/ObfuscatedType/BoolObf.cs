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

   private static readonly byte _obf = (byte)(Obfuscator.GenerateIV() + 223);
   private byte obf => (byte)(_obf - 223);
   private byte[]? obfValue;

   #endregion

   #region Properties

   private bool _value
   {
      get => BitConverter.ToBoolean(Obfuscator.Deobfuscate(obfValue, obf));
      set => obfValue = Obfuscator.Obfuscate(BitConverter.GetBytes(value), obf);
   }

   #endregion

   #region Constructors

   private BoolObf(bool value)
   {
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

      if (obj.GetType() == typeof(bool))
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