using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated float implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class FloatObf : ObfuscatedValueType<FloatObf, float> //NUnit
{
   private static readonly byte _obf = Obfuscator.GenerateIV();
   protected override byte obf => _obf;

   private FloatObf(float value) : base(value)
   {
   }

   public static implicit operator FloatObf(float value)
   {
      return new FloatObf(value);
   }

   public static implicit operator float(FloatObf custom)
   {
      return custom._value;
   }
}