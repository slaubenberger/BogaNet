using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure double implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class DoubleSec : SecureValueType<DoubleSec, double> //NUnit
{ 
   private DoubleSec(double value) : base(value)
   {
   }

   public static implicit operator DoubleSec(double value)
   {
      return new DoubleSec(value);
   }

   public static implicit operator double(DoubleSec custom)
   {
      return custom._value;
   }
}