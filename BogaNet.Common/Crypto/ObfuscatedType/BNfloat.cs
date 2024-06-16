namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated float implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class BNfloat : CustomValueType<BNfloat, float>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private BNfloat(float value) : base(value)
   {
   }

   public static implicit operator BNfloat(float value)
   {
      return new BNfloat(value);
   }

   public static implicit operator float(BNfloat custom)
   {
      return custom._value;
   }
}