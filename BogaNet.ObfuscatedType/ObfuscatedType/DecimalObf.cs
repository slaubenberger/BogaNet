using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated decimal implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class DecimalObf : ObfuscatedValueType<DecimalObf, decimal> //NUnit
{
   private static readonly byte _obf = (byte)(Obfuscator.GenerateIV() + 37);
   protected override byte obf => (byte)(_obf - 37);

   private DecimalObf(decimal value) : base(value)
   {
   }

   public static implicit operator DecimalObf(decimal value)
   {
      return new DecimalObf(value);
   }

   public static implicit operator decimal(DecimalObf custom)
   {
      return custom._value;
   }
}