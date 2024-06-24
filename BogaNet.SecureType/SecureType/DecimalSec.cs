using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure decimal implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class DecimalSec : SecureValueType<DecimalSec, decimal> //NUnit
{
   private readonly ByteObf[] _key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] _iv = AESHelper.GenerateIV().BNToByteObfArray();

   protected override ByteObf[] key => _key;
   protected override ByteObf[] iv => _iv;

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