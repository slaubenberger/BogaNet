using BogaNet.Extension;

namespace BogaNet.Test;

public static class TestConstants
{
   public const string LatinText = Constants.SIGNS_EXT;
   public static readonly string LatinTextCorrupted = LatinText.BNReplace("0", "1");
   public const string NonLatinText = "ハローワールド! 世界您好！หวัดดีชาวโลก!";
}