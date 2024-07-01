namespace BogaNet.SecureType;

/// <summary>
/// Secure ulong implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class UlongSec : SecureValueType<UlongSec, ulong> //NUnit
{
   private UlongSec(ulong value) : base(value)
   {
   }

   public static implicit operator UlongSec(ulong value)
   {
      return new UlongSec(value);
   }

   public static implicit operator ulong(UlongSec custom)
   {
      return custom._value;
   }
}