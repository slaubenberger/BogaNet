using System.Collections.Generic;
using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated string implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class StringObf //NUnit
{
   #region Variables

   private static readonly byte _obf = (byte)(Obfuscator.GenerateIV() + 173);
   private byte obf => (byte)(_obf - 173);
   private byte[]? obfValue;

   #endregion

   #region Properties

   private string _value
   {
      get => Obfuscator.DeobfuscateToString(obfValue, obf) ?? string.Empty;
      set => obfValue = Obfuscator.Obfuscate(value, obf);
   }

   #endregion

   #region Constructors

   private StringObf(string value)
   {
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

      if (obj.GetType() == typeof(string))
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