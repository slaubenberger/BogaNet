using System.Collections.Generic;
using BogaNet.Extension;
using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated string implementation. This prevents the string from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class StringObf //NUnit
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

   private string _value
   {
      get => Obfuscator.Deobfuscate(_obfValue, _obf).BNToString();
      set => _obfValue = Obfuscator.Obfuscate(value, _obf);
   }

   #endregion

   #region Constructors

   private StringObf(string value)
   {
      _offset = (byte)(Obfuscator.GenerateIV() + _shift);
      _iv = (byte)(Obfuscator.GenerateIV() + _obfOffset);
      _value = value;
   }

   #endregion

   #region Operators

   public static implicit operator StringObf(string value)
   {
      return new StringObf(value);
   }

   public static implicit operator string(StringObf custom)
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

      if (obj is string)
         return _value.Equals(obj);

      return obj.GetType() == GetType() && equals((StringObf)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<string>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   private bool equals(StringObf other)
   {
      return EqualityComparer<string>.Default.Equals(_value, other._value);
   }

   #endregion
}