using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated sbyte implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class SbyteObf : ObfuscatedValueType<SbyteObf, sbyte> //NUnit
{
   private static readonly byte _obf = (byte)(Obfuscator.GenerateIV() + 137);
   protected override byte obf => (byte)(_obf - 137);

   private SbyteObf(sbyte value) : base(value)
   {
   }

   public static implicit operator SbyteObf(sbyte value)
   {
      return new SbyteObf(value);
   }

   public static implicit operator sbyte(SbyteObf custom)
   {
      return custom._value;
   }
}