using System.Collections.Generic;
using System.Numerics;
using System;
using Microsoft.Extensions.Logging;

namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Custom implementation for value types.
/// </summary>
/// <typeparam name="TCustom"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class CustomValueType<TCustom, TValue> where TValue : INumber<TValue>
{
   private static readonly ILogger<CustomValueType<TCustom, TValue>> _logger = GlobalLogging.CreateLogger<CustomValueType<TCustom, TValue>>();

   private static readonly byte obf = Obfuscator.GenerateIV();
   private string obfValue;

   protected TValue _value
   {
      get
      {
         Type type = typeof(TValue);

         string plainValue = Obfuscator.Deobfuscate(obfValue, obf);

         switch (type)
         {
            case Type t when t == typeof(double):
               double doubleVal = double.Parse(plainValue);
               return TValue.CreateTruncating(doubleVal);
            case Type t when t == typeof(float):
               float floatVal = float.Parse(plainValue);
               return TValue.CreateTruncating(floatVal);
            case Type t when t == typeof(long):
               long longVal = long.Parse(plainValue);
               return TValue.CreateTruncating(longVal);
            case Type t when t == typeof(ulong):
               ulong ulongVal = ulong.Parse(plainValue);
               return TValue.CreateTruncating(ulongVal);
            case Type t when t == typeof(int):
               int intVal = int.Parse(plainValue);
               return TValue.CreateTruncating(intVal);
            case Type t when t == typeof(uint):
               uint uintVal = uint.Parse(plainValue);
               return TValue.CreateTruncating(uintVal);
            case Type t when t == typeof(short):
               short shortVal = short.Parse(plainValue);
               return TValue.CreateTruncating(shortVal);
            case Type t when t == typeof(ushort):
               ushort ushortVal = ushort.Parse(plainValue);
               return TValue.CreateTruncating(ushortVal);
            case Type t when t == typeof(nint):
               nint nintVal = nint.Parse(plainValue);
               return TValue.CreateTruncating(nintVal);
            case Type t when t == typeof(nuint):
               nint nuintVal = nint.Parse(plainValue);
               return TValue.CreateTruncating(nuintVal);
            case Type t when t == typeof(byte):
               byte byteVal = byte.Parse(plainValue);
               return TValue.CreateTruncating(byteVal);
            case Type t when t == typeof(sbyte):
               sbyte sbyteVal = sbyte.Parse(plainValue);
               return TValue.CreateTruncating(sbyteVal);
            case Type t when t == typeof(char):
               char charVal = char.Parse(plainValue);
               return TValue.CreateTruncating(charVal);
            default:
               _logger.LogWarning("Number type is not supported!");
               break;
         }

         return default;
      }
      private set { obfValue = Obfuscator.Obfuscate(value.ToString(), obf); }
   }

   public CustomValueType(TValue value)
   {
      _value = value;
   }

   public override string ToString()
   {
      return _value.ToString();
   }

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

   protected bool Equals(CustomValueType<TCustom, TValue> other)
   {
      return EqualityComparer<TValue>.Default.Equals(_value, other._value);
   }

   public override bool Equals(object obj)
   {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((CustomValueType<TCustom, TValue>)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<TValue>.Default.GetHashCode(_value);
   }
}