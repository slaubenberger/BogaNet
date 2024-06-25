using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure uint implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class UintSec : SecureValueType<UintSec, uint> //NUnit
{
   private UintSec(uint value) : base(value)
   {
   }

   public static implicit operator UintSec(uint value)
   {
      return new UintSec(value);
   }

   public static implicit operator uint(UintSec custom)
   {
      return custom._value;
   }
}