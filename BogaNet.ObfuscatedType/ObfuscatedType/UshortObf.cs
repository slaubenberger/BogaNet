namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated short implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class UshortObf : ObfuscatedValueType<UshortObf, ushort> //NUnit
{
   private UshortObf(ushort value) : base(value)
   {
   }

   public static implicit operator UshortObf(ushort value)
   {
      return new UshortObf(value);
   }

   public static implicit operator ushort(UshortObf custom)
   {
      return custom._value;
   }
}