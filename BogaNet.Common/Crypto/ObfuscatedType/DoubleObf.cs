namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated double implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class DoubleObf : ObfuscatedValueType<DoubleObf, double>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private DoubleObf(double value) : base(value)
   {
   }

   public static implicit operator DoubleObf(double value)
   {
      return new DoubleObf(value);
   }

   public static implicit operator double(DoubleObf custom)
   {
      return custom._value;
   }
}