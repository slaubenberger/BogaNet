using BogaNet.Helper;
using BogaNet.ObfuscatedType;

namespace BogaNet.SecureType;

/// <summary>
/// Secure char implementation. This prevents the value from being readable in the memory of the application.
/// </summary>
public class CharSec : SecureValueType<ByteSec, char> //NUnit
{
   private readonly ByteObf[] _key = AESHelper.GenerateKey().BNToByteObfArray();
   private readonly ByteObf[] _iv = AESHelper.GenerateIV().BNToByteObfArray();

   protected override ByteObf[] key => _key;
   protected override ByteObf[] iv => _iv;

   private CharSec(char value) : base(value)
   {
   }

   public static implicit operator CharSec(char value)
   {
      return new CharSec(value);
   }

   public static implicit operator char(CharSec custom)
   {
      return custom._value;
   }
}