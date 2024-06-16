namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated short implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class BNushort : CustomValueType<BNushort, ushort>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private BNushort(ushort value) : base(value)
   {
   }

   public static implicit operator BNushort(ushort value)
   {
      return new BNushort(value);
   }

   public static implicit operator ushort(BNushort custom)
   {
      return custom._value;
   }
}