using System.Collections.Generic;
using System.Numerics;
using System;
using Microsoft.Extensions.Logging;

namespace BogaNet.Crypto.ObfuscatedType;

public class CustomString<TCustom>
{
   //private static readonly ILogger<CustomValueType<TCustom, TValue>> _logger = GlobalLogging.CreateLogger<CustomValueType<TCustom, TValue>>();

   private static byte obf;
   private string obfValue;

   static CustomString()
   {
      obf = Obfuscator.GenerateIV();
   }

   protected string _value
   {
      get
      {
         return Obfuscator.Deobfuscate(obfValue, obf);
      }
      private set { obfValue = Obfuscator.Obfuscate(value, obf); }
   }

   public CustomString(string value)
   {
      _value = value;
   }

   public override string ToString()
   {
      return _value;
   }
/*
   public static bool operator <(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
   {
      return Comparer<TValue>.Default.Compare(a._value, b._value) < 0;
   }

   public static bool operator >(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
   {
      return !(a < b);
   }

   public static bool operator <=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
   {
      return (a < b) || (a == b);
   }

   public static bool operator >=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
   {
      return (a > b) || (a == b);
   }

   public static bool operator ==(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
   {
      return a.Equals((object)b);
   }

   public static bool operator !=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
   {
      return !(a == b);
   }

   public static TCustom operator +(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
   {
      return (dynamic)a._value + b._value;
   }

   public static TCustom operator -(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
   {
      return ((dynamic)a._value - b._value);
   }
*/
   protected bool Equals(CustomString<TCustom> other)
   {
      return EqualityComparer<string>.Default.Equals(_value, other._value);
   }

   public override bool Equals(object obj)
   {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((CustomString<TCustom>)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<string>.Default.GetHashCode(_value);
   }
}