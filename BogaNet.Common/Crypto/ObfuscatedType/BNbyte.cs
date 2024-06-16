namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated byte implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class BNbyte : CustomValueType<BNbyte, byte>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private BNbyte(byte value) : base(value)
   {
   }

   public static implicit operator BNbyte(byte value)
   {
      return new BNbyte(value);
   }

   public static implicit operator byte(BNbyte custom)
   {
      return custom._value;
   }
}