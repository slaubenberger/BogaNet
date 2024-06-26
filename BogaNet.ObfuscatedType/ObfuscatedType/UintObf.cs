namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated uint implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class UintObf : ObfuscatedValueType<UintObf, uint> //NUnit
{
   private UintObf(uint value) : base(value)
   {
   }

   public static implicit operator UintObf(uint value)
   {
      return new UintObf(value);
   }

   public static implicit operator uint(UintObf custom)
   {
      return custom._value;
   }
}