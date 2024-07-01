namespace BogaNet.SecureType;

/// <summary>
/// Secure byte implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class ByteSec : SecureValueType<ByteSec, byte> //NUnit
{
   private ByteSec(byte value) : base(value)
   {
   }

   public static implicit operator ByteSec(byte value)
   {
      return new ByteSec(value);
   }

   public static implicit operator byte(ByteSec custom)
   {
      return custom._value;
   }
}