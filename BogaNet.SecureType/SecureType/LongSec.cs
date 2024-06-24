using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure long implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class LongSec : SecureValueType<LongSec, long> //NUnit
{
   private readonly ByteObf[] _key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] _iv = AESHelper.GenerateIV().BNToByteObfArray();

   protected override ByteObf[] key => _key;
   protected override ByteObf[] iv => _iv;

   private LongSec(long value) : base(value)
   {
   }

   public static implicit operator LongSec(long value)
   {
      return new LongSec(value);
   }

   public static implicit operator long(LongSec custom)
   {
      return custom._value;
   }
}