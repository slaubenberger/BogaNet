using BogaNet.Util;

namespace BogaNet.ObfuscatedType;

/// <summary>
/// Obfuscated double implementation. This prevents the value from being "plain" in the memory of the application.
/// NOTE: this class is not cryptographically secure!
/// </summary>
public class DoubleObf : ObfuscatedValueType<DoubleObf, double> //NUnit
{
   private DoubleObf(double value) : base(value)
   {
   }

   public static implicit operator DoubleObf(double value)
   {
      return new DoubleObf(value);
   }

   public static implicit operator double(DoubleObf custom)
   {
      return custom._value;
   }
}