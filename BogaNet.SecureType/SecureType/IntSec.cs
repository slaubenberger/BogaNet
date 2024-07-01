namespace BogaNet.SecureType;

/// <summary>
/// Secure int implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class IntSec : SecureValueType<IntSec, int> //NUnit
{
   private IntSec(int value) : base(value)
   {
   }

   public static implicit operator IntSec(int value)
   {
      return new IntSec(value);
   }

   public static implicit operator int(IntSec custom)
   {
      return custom._value;
   }
}