using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure nint implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class NintSec : SecureValueType<NintSec, nint> //NUnit
{
   private NintSec(nint value) : base(value)
   {
   }

   public static implicit operator NintSec(nint value)
   {
      return new NintSec(value);
   }

   public static implicit operator nint(NintSec custom)
   {
      return custom._value;
   }
}