namespace BogaNet.Crypto.ObfuscatedType;

/// <summary>
/// Obfuscated double implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: This class is not cryptographically secure and don't use it for intense computations!
/// </summary>
public class BNdouble : CustomValueType<BNdouble, double>
{
   private BNdouble(double value) : base(value)
   {
   }

   public static implicit operator BNdouble(double value)
   {
      return new BNdouble(value);
   }

   public static implicit operator double(BNdouble custom)
   {
      return custom._value;
   }
}
