namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated short implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNshort : CustomValueType<BNshort, short>
{
   private BNshort(short value) : base(value)
   {
   }

   public static implicit operator BNshort(short value)
   {
      return new BNshort(value);
   }

   public static implicit operator short(BNshort custom)
   {
      return custom._value;
   }
}
