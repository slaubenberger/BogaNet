namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated char implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class CharObf : ObfuscatedValueType<ByteObf, char> //NUnit
{
   private CharObf(char value) : base(value)
   {
   }

   public static implicit operator CharObf(char value)
   {
      return new CharObf(value);
   }

   public static implicit operator char(CharObf custom)
   {
      return custom._value;
   }
}