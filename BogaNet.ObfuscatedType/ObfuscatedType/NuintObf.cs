using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated nuint implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class NuintObf : ObfuscatedValueType<NuintObf, nuint> //NUnit
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private NuintObf(nuint value) : base(value)
   {
   }

   public static implicit operator NuintObf(nuint value)
   {
      return new NuintObf(value);
   }

   public static implicit operator nuint(NuintObf custom)
   {
      return custom._value;
   }
}