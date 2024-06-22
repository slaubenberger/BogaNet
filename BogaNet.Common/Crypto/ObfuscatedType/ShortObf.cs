namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated short implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class ShortObf : ObfuscatedValueType<ShortObf, short>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private ShortObf(short value) : base(value)
   {
   }

   public static implicit operator ShortObf(short value)
   {
      return new ShortObf(value);
   }

   public static implicit operator short(ShortObf custom)
   {
      return custom._value;
   }
}