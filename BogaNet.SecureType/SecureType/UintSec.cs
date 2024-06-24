using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure uint implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class UintSec : SecureValueType<UintSec, uint> //NUnit
{
   private readonly ByteObf[] _key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] _iv = AESHelper.GenerateIV().BNToByteObfArray();

   protected override ByteObf[] key => _key;
   protected override ByteObf[] iv => _iv;

   private UintSec(uint value) : base(value)
   {
   }

   public static implicit operator UintSec(uint value)
   {
      return new UintSec(value);
   }

   public static implicit operator uint(UintSec custom)
   {
      return custom._value;
   }
}