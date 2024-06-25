using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated nint implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class NintObf : ObfuscatedValueType<NintObf, nint> //NUnit
{
   private NintObf(nint value) : base(value)
   {
   }

   public static implicit operator NintObf(nint value)
   {
      return new NintObf(value);
   }

   public static implicit operator nint(NintObf custom)
   {
      return custom._value;
   }
}