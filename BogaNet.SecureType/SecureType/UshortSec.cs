using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure ushort implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class UshortSec : SecureValueType<UshortSec, ushort> //NUnit
{
   private UshortSec(ushort value) : base(value)
   {
   }

   public static implicit operator UshortSec(ushort value)
   {
      return new UshortSec(value);
   }

   public static implicit operator ushort(UshortSec custom)
   {
      return custom._value;
   }
}