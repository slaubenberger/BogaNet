using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure byte implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class ByteSec : SecureValueType<ByteSec, byte> //NUnit
{
   private readonly ByteObf[] _key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] _iv = AESHelper.GenerateIV().BNToByteObfArray();

   protected override ByteObf[] key => _key;
   protected override ByteObf[] iv => _iv;

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