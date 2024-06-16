namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated nint implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNnint : CustomValueType<BNnint, nint>
{
   private BNnint(nint value) : base(value)
   {
   }

   public static implicit operator BNnint(nint value)
   {
      return new BNnint(value);
   }

   public static implicit operator nint(BNnint custom)
   {
      return custom._value;
   }
}
