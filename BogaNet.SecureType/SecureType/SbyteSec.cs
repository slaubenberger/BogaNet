using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure sbyte implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class SbyteSec : SecureValueType<SbyteSec, sbyte> //NUnit
{
   private readonly ByteObf[] _key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] _iv = AESHelper.GenerateIV().BNToByteObfArray();

   protected override ByteObf[] key => _key;
   protected override ByteObf[] iv => _iv;

   private SbyteSec(sbyte value) : base(value)
   {
   }

   public static implicit operator SbyteSec(sbyte value)
   {
      return new SbyteSec(value);
   }

   public static implicit operator sbyte(SbyteSec custom)
   {
      return custom._value;
   }
}