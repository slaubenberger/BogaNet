namespace BogaNet.SecureType;

/// <summary>
/// Secure float implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class FloatSec : SecureValueType<FloatSec, float> //NUnit
{
   private FloatSec(float value) : base(value)
   {
   }

   public static implicit operator FloatSec(float value)
   {
      return new FloatSec(value);
   }

   public static implicit operator float(FloatSec custom)
   {
      return custom._value;
   }
}