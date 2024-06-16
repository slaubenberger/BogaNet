namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated byte implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNbyte : CustomValueType<BNbyte, byte>
{
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
