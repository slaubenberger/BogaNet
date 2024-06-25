using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure nuint implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class NuintSec : SecureValueType<NuintSec, nuint> //NUnit
{
   private NuintSec(nuint value) : base(value)
   {
   }

   public static implicit operator NuintSec(nuint value)
   {
      return new NuintSec(value);
   }

   public static implicit operator nuint(NuintSec custom)
   {
      return custom._value;
   }
}