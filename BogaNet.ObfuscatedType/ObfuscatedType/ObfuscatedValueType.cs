using System.Collections.Generic;
using System.Numerics;
using System;
using Microsoft.Extensions.Logging;
using System.Text;
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

   private static readonly ILogger<ObfuscatedValueType<TCustom, TValue>> _logger = GlobalLogging.CreateLogger<ObfuscatedValueType<TCustom, TValue>>();

   protected abstract byte obf { get; } //= Obfuscator.GenerateIV();
   private byte[]? obfValue;

   #endregion

   #region Properties

   protected TValue _value
   {
      get
      {
         //string? plainValue = Obfuscator.DeobfuscateToString(obfValue, obf, Encoding.ASCII);

         Type type = typeof(TValue);

         if (type == typeof(decimal))
         {
            string? plainValue = Obfuscator.DeobfuscateToString(obfValue, obf, Encoding.ASCII);
            decimal decVal = decimal.Parse(plainValue);
            return TValue.CreateTruncating(decVal);
         }
         else
         {
            return Obfuscator.Deobfuscate(obfValue).BNToNumber<TValue>();
         }
      }

      private set
      {
         Type type = typeof(TValue);

         if (type == typeof(decimal))
         {
            var tb = value.BNToByteArray();
            var dec = tb.BNToNumber<decimal>();
            var asStr = value.ToString();
            obfValue = Obfuscator.Obfuscate(value.ToString(), obf, Encoding.ASCII);
         }
         else
         {
            obfValue = Obfuscator.Obfuscate(value.BNToByteArray(), obf);
         }
      }
   }

   #endregion

   #region Constructors

   public ObfuscatedValueType(TValue value)
   {
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
      return (a < b) || (a == b);
   }

   public static bool operator >=(ObfuscatedValueType<TCustom, TValue> a, ObfuscatedValueType<TCustom, TValue> b)
   {
      return (a > b) || (a == b);
   }

   public static bool operator ==(ObfuscatedValueType<TCustom, TValue> a, ObfuscatedValueType<TCustom, TValue> b)
   {
      return a.Equals((object)b);
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
      return ((dynamic)a._value - b._value);
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

      if (obj.GetType() != GetType()) return false;
      return equals((ObfuscatedValueType<TCustom, TValue>)obj);
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