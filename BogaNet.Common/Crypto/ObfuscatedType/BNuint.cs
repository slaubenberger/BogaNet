namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated uint implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class BNuint : CustomValueType<BNuint, uint>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private BNuint(uint value) : base(value)
   {
   }

   public static implicit operator BNuint(uint value)
   {
      return new BNuint(value);
   }

   public static implicit operator uint(BNuint custom)
   {
      return custom._value;
   }
}