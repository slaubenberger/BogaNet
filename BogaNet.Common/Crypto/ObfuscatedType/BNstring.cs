namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated string implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNstring : CustomString<BNbyte>
{
   private BNstring(string value) : base(value)
   {
   }

   public static implicit operator BNstring(string value)
   {
      return new BNstring(value);
   }

   public static implicit operator string(BNstring custom)
   {
      return custom._value;
   }
}
