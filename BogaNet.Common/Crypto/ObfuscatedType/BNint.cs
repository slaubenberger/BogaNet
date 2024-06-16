namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated int implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class BNint : CustomValueType<BNint, int>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private BNint(int value) : base(value)
   {
   }

   public static implicit operator BNint(int value)
   {
      return new BNint(value);
   }

   public static implicit operator int(BNint custom)
   {
      return custom._value;
   }
}