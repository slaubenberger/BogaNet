using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated long implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class LongObf : ObfuscatedValueType<LongObf, long> //NUnit
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf => _obf;

   private LongObf(long value) : base(value)
   {
   }

   public static implicit operator LongObf(long value)
   {
      return new LongObf(value);
   }

   public static implicit operator long(LongObf custom)
   {
      return custom._value;
   }
}