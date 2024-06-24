using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure short implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class ShortSec : SecureValueType<ShortSec, short> //NUnit
{
   private readonly ByteObf[] _key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] _iv = AESHelper.GenerateIV().BNToByteObfArray();

   protected override ByteObf[] key => _key;
   protected override ByteObf[] iv => _iv;

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