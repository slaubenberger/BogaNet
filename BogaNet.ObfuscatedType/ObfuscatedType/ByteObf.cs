namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated byte implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class ByteObf : ObfuscatedValueType<ByteObf, byte> //NUnit
{
   private ByteObf(byte value) : base(value)
   {
   }

   public static implicit operator ByteObf(byte value)
   {
      return new ByteObf(value);
   }

   public static implicit operator byte(ByteObf custom)
   {
      return custom._value;
   }
}