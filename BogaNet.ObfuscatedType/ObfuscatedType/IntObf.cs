namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated int implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class IntObf : ObfuscatedValueType<IntObf, int> //NUnit
{
   private IntObf(int value) : base(value)
   {
   }

   public static implicit operator IntObf(int value)
   {
      return new IntObf(value);
   }

   public static implicit operator int(IntObf custom)
   {
      return custom._value;
   }
}