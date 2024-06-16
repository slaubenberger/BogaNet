namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated char implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure!
/// </summary>
public class BNchar : CustomValueType<BNbyte, char>
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf { get; } = _obf;

   private BNchar(char value) : base(value)
   {
   }

   public static implicit operator BNchar(char value)
   {
      return new BNchar(value);
   }

   public static implicit operator char(BNchar custom)
   {
      return custom._value;
   }
}