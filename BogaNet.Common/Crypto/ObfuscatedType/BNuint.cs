namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated uint implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNuint : CustomValueType<BNuint, uint>
{
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
