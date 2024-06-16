namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated ulong implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNulong : CustomValueType<BNulong, ulong>
{
   private BNulong(ulong value) : base(value)
   {
   }

   public static implicit operator BNulong(ulong value)
   {
      return new BNulong(value);
   }

   public static implicit operator ulong(BNulong custom)
   {
      return custom._value;
   }
}
