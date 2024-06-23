namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated decimal implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class DecimalObf : ObfuscatedValueType<DecimalObf, decimal>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private DecimalObf(decimal value) : base(value)
   {
   }

   public static implicit operator DecimalObf(decimal value)
   {
      return new DecimalObf(value);
   }

   public static implicit operator decimal(DecimalObf custom)
   {
      return custom._value;
   }
}