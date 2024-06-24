using System.Collections.Generic;
using System.Numerics;
using System;
using Microsoft.Extensions.Logging;
using BogaNet.Helper;
using BogaNet.ObfuscatedType;
using System.Text;

namespace BogaNet.SecureType;

/// <summary>
/// Secure implementation for value types.
/// </summary>
/// <typeparam name="TCustom"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class SecureValueType<TCustom, TValue> where TValue : INumber<TValue>
{
   #region Variables

   private static readonly ILogger<SecureValueType<TCustom, TValue>> _logger = GlobalLogging.CreateLogger<SecureValueType<TCustom, TValue>>();

   protected abstract ByteObf[] key { get; }
   protected abstract ByteObf[] iv { get; }
   private byte[] secretValue;

   #endregion

   #region Properties

   protected TValue _value
   {
      get
      {
         return AESHelper.Decrypt(secretValue, key.ToByteArray(), iv.ToByteArray()).BNToNumber<TValue>();
         /*
         Type type = typeof(TValue);

         string? plainValue = AESHelper.DecryptToString(secretValue, key.ToByteArray(), iv.ToByteArray(), Encoding.ASCII);

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
            case Type t when t == typeof(decimal):
               decimal decVal = decimal.Parse(plainValue);
               return TValue.CreateTruncating(decVal);
            default:
               _logger.LogWarning("Number type is not supported!");
               break;
         }

         return TValue.CreateTruncating(0);
         */
      }

      private set
      {
         Type type = typeof(TValue);

         if (type == typeof(char))
         {
           
            var b = value.BNToByteArray();
            int a = 5;
         }

         secretValue = AESHelper.Encrypt(value.BNToByteArray(), key.ToByteArray(), iv.ToByteArray());
         //secretValue = AESHelper.Encrypt(value.ToString(), key.ToByteArray(), iv.ToByteArray(), Encoding.ASCII);
      }
   }

   #endregion

   #region Constructors

   public SecureValueType(TValue value)
   {
      _value = value;
   }

   #endregion

   #region Operators

   public static bool operator <(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return Comparer<TValue>.Default.Compare(a._value, b._value) < 0;
   }

   public static bool operator >(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return !(a < b);
   }

   public static bool operator <=(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return (a < b) || (a == b);
   }

   public static bool operator >=(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return (a > b) || (a == b);
   }

   public static bool operator ==(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return a.Equals((object)b);
   }

   public static bool operator !=(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return !(a == b);
   }

   public static TCustom operator +(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
   {
      return (dynamic)a._value + b._value;
   }

   public static TCustom operator -(SecureValueType<TCustom, TValue> a, SecureValueType<TCustom, TValue> b)
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
      return equals((SecureValueType<TCustom, TValue>)obj);
   }

   public override int GetHashCode()
   {
      return EqualityComparer<TValue>.Default.GetHashCode(_value);
   }

   #endregion

   #region Private methods

   protected bool equals(SecureValueType<TCustom, TValue> other)
   {
      return EqualityComparer<TValue>.Default.Equals(_value, other._value);
   }

   #endregion
}