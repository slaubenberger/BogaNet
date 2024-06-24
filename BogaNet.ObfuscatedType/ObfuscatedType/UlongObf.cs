using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated ulong implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class UlongObf : ObfuscatedValueType<UlongObf, ulong> //NUnit
{
   private static readonly byte _obf = (byte)(Obfuscator.GenerateIV() + 197);
   protected override byte obf => (byte)(_obf - 197);

   private UlongObf(ulong value) : base(value)
   {
   }

   public static implicit operator UlongObf(ulong value)
   {
      return new UlongObf(value);
   }

   public static implicit operator ulong(UlongObf custom)
   {
      return custom._value;
   }
}