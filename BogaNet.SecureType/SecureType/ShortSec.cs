namespace BogaNet.SecureType;

/// <summary>
/// Secure short implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class ShortSec : SecureValueType<ShortSec, short> //NUnit
{
   private ShortSec(short value) : base(value)
   {
   }

   public static implicit operator ShortSec(short value)
   {
      return new ShortSec(value);
   }

   public static implicit operator short(ShortSec custom)
   {
      return custom._value;
   }
}