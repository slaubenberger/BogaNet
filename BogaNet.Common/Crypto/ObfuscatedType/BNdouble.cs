namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated double implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class BNdouble : CustomValueType<BNdouble, double>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private BNdouble(double value) : base(value)
   {
   }

   public static implicit operator BNdouble(double value)
   {
      return new BNdouble(value);
   }

   public static implicit operator double(BNdouble custom)
   {
      return custom._value;
   }
}