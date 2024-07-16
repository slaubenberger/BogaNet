using System.Linq;
using BogaNet.Extension;
using BogaNet.TTS.Model.Enum;
using BogaNet.Helper;

namespace BogaNet.TTS.Util
{
   /// <summary>Various helper functions.</summary>
   public abstract class Helper
   {
      #region Variables

      private static readonly string[] appleFemales =
      [
         "Alice",
         "Alva",
         "Amelie", //old
         "Amélie",
         "Anna",
         "Carmit",
         "Catherine", //iOS
         "Damayanti",
         "Ellen",
         "Flo",
         "Fiona", //old?
         "Grandma",
         "Helena", //iOS
         "Ioana",
         "Joana",
         "Kanya",
         "Karen",
         "Kathy",
         "Kyoko",
         "Lana",
         "Laura",
         "Lekha",
         "Lesya",
         "Li-mu", //iOS
         "Linh",
         "Luciana",
         "Marie", //iOS
         "Mariska", //old?
         "Martha", //iOS
         "Meijia",
         "Mei-Jia", //old?
         "Melina",
         "Milena",
         "Moira",
         "Monica", //old?
         "Mónica",
         "Montse",
         "Nicky", //iOS
         "Nora",
         "O-ren", //iOS
         "Paulina",
         "Samantha",
         "Sandy",
         "Sara",
         "Satu",
         "Shelley",
         "Sinji",
         "Sin-ji", //old?
         "Tessa",
         "Tingting",
         "Ting-Ting", //old?
         "Tünde",
         "Veena", //macOS
         "Victoria", //macOS
         "Yelda",
         "Yu-shu", //iOS
         "Yuna",
         "Zosia",
         "Zuzana"
      ];

      private static readonly string[] appleMales =
      [
         "Aaron", //iOS
         "Albert",
         "Alex", //old?
         "Arthur", //iOS
         "Daniel",
         "Diego", //old?
         "Eddy",
         "Fred",
         "Grandpa",
         "Gordon", //old?
         "Hattori", //iOS
         "Jacques",
         "Jester",
         "Jorge", //old?
         "Juan", //old?
         "Majed",
         "Luca", //macOS
         "Maged",
         "Martin", //iOS
         "Ralph",
         "Reed",
         "Rishi",
         "Rocko",
         "Thomas",
         "Xander",
         "Yuri" //old?
      ];

      private static readonly string[] wsaFemales =
      [
         "Ayumi",
         "Haruka",
         "Caroline",
         "Catherine",
         "Elsa",
         "Hazel",
         "Susan",
         "Heami",
         "Hedda",
         "Katja",
         "Heera",
         "Heidi",
         "Helena",
         "Laura",
         "Helia",
         "Helle",
         "Herena",
         "Hoda",
         "Hortence",
         "Julie",
         "Huihui",
         "Yaoyao",
         "Irina",
         "Kalpana",
         "Linda",
         "Maria",
         "Paulina",
         "Sabina",
         "Tracy",
         "Yating",
         "Hanhan",
         "Zira"
      ];

      private static readonly string[] wsaMales =
      [
         "Adam",
         "An",
         "Andika",
         "Andrei",
         "Asaf",
         "Bart",
         "Bengt",
         "Claude",
         "Cosimo",
         "Daniel",
         "Danny",
         "David",
         "Mark",
         "Filip",
         "Frank",
         "George",
         "Hemant",
         "Ichiro",
         "Ivan",
         "James",
         "Jon",
         "Kangkang",
         "Karsten",
         "Lado",
         "Matej",
         "Naayf",
         "Pablo",
         "Pattara",
         "Paul",
         "Pavel",
         "Raul",
         "Ravi",
         "Richard",
         "Rizwan",
         "Shaun",
         "Stefan",
         "Stefanos",
         "Szabolcs",
         "Tolga",
         "Valluvar",
         "Vit",
         "Zhiwei"
      ];

      #endregion


      #region Static properties

/*
      /// <summary>The current provider type.</summary>
      /// <returns>Current provider type.</returns>
      public static ProviderType CurrentProviderType
      {
         get
         {
            if (Speaker.Instance.CustomMode)
               return ProviderType.Custom;

            if (IsWindows && !Speaker.Instance.ESpeakMode)
               return ProviderType.Windows;

            if (isAndroidPlatform)
               return ProviderType.Android;

            if (isIOSBasedPlatform)
               return ProviderType.iOS;

            if (isWSABasedPlatform)
               return ProviderType.WSA;

            if (isMacOSPlatform && !Speaker.Instance.ESpeakMode)
               return ProviderType.macOS;

            return ProviderType.Linux;
         }
      }
*/

      #endregion


      #region Static methods

      /// <summary>Converts a string to a Gender.</summary>
      /// <param name="gender">Gender as text.</param>
      /// <returns>Gender from the given string.</returns>
      public static Gender StringToGender(string gender)
      {
         if ("male".BNEquals(gender) || "m".BNEquals(gender))
            return Gender.MALE;

         if ("female".BNEquals(gender) || "f".BNEquals(gender))
            return Gender.FEMALE;

         return Gender.UNKNOWN;
      }

      /// <summary>Converts an Apple voice name to a Gender.</summary>
      /// <param name="voiceName">Voice name.</param>
      /// <returns>Gender from the given Apple voice name.</returns>
      public static Gender AppleVoiceNameToGender(string voiceName)
      {
         if (!string.IsNullOrEmpty(voiceName))
         {
            if (appleFemales.Any(female => voiceName.BNContains(female)))
               return Gender.FEMALE;

            if (appleMales.Any(male => voiceName.BNContains(male)))
               return Gender.MALE;
         }

         return Gender.UNKNOWN;
      }

      /// <summary>Converts an WSA voice name to a Gender.</summary>
      /// <param name="voiceName">Voice name.</param>
      /// <returns>Gender from the given WSA voice name.</returns>
      public static Gender WSAVoiceNameToGender(string voiceName)
      {
         if (!string.IsNullOrEmpty(voiceName))
         {
            if (wsaFemales.Any(female => voiceName.BNContains(female)))
               return Gender.FEMALE;

            if (wsaMales.Any(male => voiceName.BNContains(male)))
               return Gender.MALE;
         }

         return Gender.UNKNOWN;
      }

      /// <summary>Cleans a given text to contain only letters or digits.</summary>
      /// <param name="text">Text to clean.</param>
      /// <param name="removeTags">Removes tags from text (default: true, optional).</param>
      /// <param name="clearSpaces">Clears multiple spaces from text (default: true, optional).</param>
      /// <param name="clearLineEndings">Clears line endings from text (default: true, optional).</param>
      /// <returns>Clean text with only letters and digits.</returns>
      public static string CleanText(string text, bool removeTags = true, bool clearSpaces = true, bool clearLineEndings = true)
      {
         string result = text;

         if (removeTags)
            result = StringHelper.RemoveTags(result);

         if (clearSpaces)
            result = StringHelper.RemoveSpaces(result);

         if (clearLineEndings)
            result = StringHelper.RemoveLineEndings(result);

         return result;
      }

      #endregion
   }
}