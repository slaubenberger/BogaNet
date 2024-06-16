namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated ulong implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class BNulong : CustomValueType<BNulong, ulong>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private BNulong(ulong value) : base(value)
   {
   }

   public static implicit operator BNulong(ulong value)
   {
      return new BNulong(value);
   }

   public static implicit operator ulong(BNulong custom)
   {
      return custom._value;
   }
}