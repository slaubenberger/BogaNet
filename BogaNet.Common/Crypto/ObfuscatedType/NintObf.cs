namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated nint implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class NintObf : ObfuscatedValueType<NintObf, nint>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

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