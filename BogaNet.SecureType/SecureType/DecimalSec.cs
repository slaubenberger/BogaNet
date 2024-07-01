namespace BogaNet.SecureType;

/// <summary>
/// Secure decimal implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class DecimalSec : SecureValueType<DecimalSec, decimal> //NUnit
{
   private DecimalSec(decimal value) : base(value)
   {
   }

   public static implicit operator DecimalSec(decimal value)
   {
      return new DecimalSec(value);
   }

   public static implicit operator decimal(DecimalSec custom)
   {
      return custom._value;
   }
}