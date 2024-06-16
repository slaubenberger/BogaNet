namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated long implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class BNlong : CustomValueType<BNlong, long>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private BNlong(long value) : base(value)
   {
   }

   public static implicit operator BNlong(long value)
   {
      return new BNlong(value);
   }

   public static implicit operator long(BNlong custom)
   {
      return custom._value;
   }
}