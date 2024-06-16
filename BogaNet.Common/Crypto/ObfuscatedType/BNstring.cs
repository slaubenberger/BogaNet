using System.Collections.Generic;

namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated string implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNstring
{
   private static readonly byte obf = Obfuscator.GenerateIV();
   private string obfValue;

   public BNstring(string value)
   {
      _value = value;
   }

   public static implicit operator BNstring(string value)
   {
      return new BNstring(value);
   }

   public static implicit operator string(BNstring custom)
   {
      return custom._value;
   }

   private string _value
   {
      get { return Obfuscator.Deobfuscate(obfValue, obf); }
      set { obfValue = Obfuscator.Obfuscate(value, obf); }
   }

   public override string ToString()
   {
      return _value;
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
   protected bool Equals(BNstring other)
   {
      return EqualityComparer<string>.Default.Equals(_value, other._value);
   }

   public override bool Equals(object obj)
   {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((BNstring)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<string>.Default.GetHashCode(_value);
   }
}