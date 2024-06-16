namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated char implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNchar : CustomValueType<BNbyte, char>
{
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
