namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated sbyte implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNsbyte : CustomValueType<BNsbyte, sbyte>
{
   private BNsbyte(sbyte value) : base(value)
   {
   }

   public static implicit operator BNsbyte(sbyte value)
   {
      return new BNsbyte(value);
   }

   public static implicit operator sbyte(BNsbyte custom)
   {
      return custom._value;
   }
}
