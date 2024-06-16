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
public abstract class CustomValueType<TCustom, TValue> where TValue : INumber<TValue>
{
   #region Variables

   private static readonly ILogger<CustomValueType<TCustom, TValue>> _logger = GlobalLogging.CreateLogger<CustomValueType<TCustom, TValue>>();

   protected abstract byte obf { get; } //= Obfuscator.GenerateIV();
   private string? obfValue;

   /*
   //secure, but slow implementation
   private static readonly byte[] key = AESHelper.GenerateKey();
   private static readonly byte[] iv = AESHelper.GenerateIV();
   private byte[] secretValue;
   */

   #endregion

   #region Properties

   protected TValue _value
   {
      get
      {
         Type type = typeof(TValue);

         //string plainValue = AESHelper.Decrypt(secretValue, key, iv).BNToString();
         string? plainValue = Obfuscator.Deobfuscate(obfValue, obf);

         if (plainValue == null)
            return TValue.CreateTruncating(0);

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

         return TValue.CreateTruncating(0);
      }
      private set
      {
         //secretValue = AESHelper.Encrypt(value.BNToByteArray(), key, iv);
         obfValue = Obfuscator.Obfuscate(value.ToString(), obf);
      }
   }

   #endregion

   #region Constructors

   public CustomValueType(TValue value)
   {
      _value = value;
   }

   #endregion

   #region Operators

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
      return equals((CustomValueType<TCustom, TValue>)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<TValue>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   protected bool equals(CustomValueType<TCustom, TValue> other)
   {
      return EqualityComparer<TValue>.Default.Equals(_value, other._value);
   }

   #endregion
}