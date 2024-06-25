using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure long implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class LongSec : SecureValueType<LongSec, long> //NUnit
{
   private LongSec(long value) : base(value)
   {
   }

   public static implicit operator LongSec(long value)
   {
      return new LongSec(value);
   }

   public static implicit operator long(LongSec custom)
   {
      return custom._value;
   }
}