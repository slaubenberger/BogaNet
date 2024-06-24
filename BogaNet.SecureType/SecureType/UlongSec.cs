using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure ulong implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class UlongSec : SecureValueType<UlongSec, ulong> //NUnit
{
   private readonly ByteObf[] _key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] _iv = AESHelper.GenerateIV().BNToByteObfArray();

   protected override ByteObf[] key => _key;
   protected override ByteObf[] iv => _iv;

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