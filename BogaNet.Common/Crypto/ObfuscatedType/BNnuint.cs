namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated nuint implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNnuint : CustomValueType<BNnuint, nuint>
{
   private BNnuint(nuint value) : base(value)
   {
   }

   public static implicit operator BNnuint(nuint value)
   {
      return new BNnuint(value);
   }

   public static implicit operator nuint(BNnuint custom)
   {
      return custom._value;
   }
}
