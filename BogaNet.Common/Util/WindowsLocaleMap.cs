using System.Collections.Generic;
using System.Linq;
using System;
using BogaNet.Extension;

namespace BogaNet.Util;

/// <summary>
/// Windows Locale-to-ISO mapper.
/// </summary>
public abstract class WindowsLocaleMap
{
   #region Variables

   protected static readonly Lazy<IDictionary<int, string>> _mappings = new(buildMappings);

   #endregion

   #region Public methods

   /// <summary>
   /// Tries to get the type of the ISO-culture (639/3166) from the provided locale.
   /// </summary>
   /// <param name="locale">Windows locale</param>
   /// <param name="isoCulture">The variable to store the ISO type</param>
   /// <returns>True if the operation was successful</returns>
   public static bool TryGetISO(int locale, out string isoCulture)
   {
      return _mappings.Value.TryGetValue(locale, out isoCulture!);
   }

   /// <summary>
   /// Gets the type of the ISO-culture (639/3166) from the provided locale.
   /// </summary>
   /// <param name="locale">Windows locale</param>
   /// <returns>The ISO-culture</returns>
   public static string GetISO(int locale)
   {
      return TryGetISO(locale, out string? result) ? result : "en-us";
   }

   /// <summary>
   /// Tries to get the type of the Windows Locale from the provided ISO-culture (639/3166).
   /// </summary>
   /// <param name="isoCulture">ISO-culture</param>
   /// <param name="windowsLocale">The variable to store the Windows Locale</param>
   /// <returns>True if the operation was successful</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static bool TryGetLocale(string isoCulture, out int windowsLocale)
   {
      ArgumentException.ThrowIfNullOrEmpty(isoCulture);

      foreach (KeyValuePair<int, string> kvp in _mappings.Value.Where(kvp => kvp.Value.Substring(0, kvp.Value.Length > isoCulture.Length ? isoCulture.Length : kvp.Value.Length).BNEquals(isoCulture)))
      {
         windowsLocale = kvp.Key;
         return true;
      }

      windowsLocale = 0;
      return false;
   }

   /// <summary>
   /// Gets the type of the Windows Locale from the provided ISO-culture (639/3166).
   /// </summary>
   /// <param name="isoCulture">ISO-culture</param>
   /// <returns>The Windows Locale</returns>
   /// <exception cref="ArgumentNullException"></exception>
   public static int GetLocale(string isoCulture)
   {
      return TryGetLocale(isoCulture, out int result) ? result : 1033;
   }

   #endregion

   #region Private methods

   private static Dictionary<int, string> buildMappings()
   {
      Dictionary<int, string> mappings = new()
      {
         { 1025, "ar-SA" },
         { 1026, "bg-BG" },
         { 1027, "ca-ES" },
         { 1028, "zh-TW" },
         { 1029, "cs-CZ" },
         { 1030, "da-DK" },
         { 1031, "de-DE" },
         { 1032, "el-GR" },
         { 1033, "en-US" },
         { 1035, "fi-FI" },
         { 1036, "fr-FR" },
         { 1037, "he-IL" },
         { 1038, "hu-HU" },
         { 1039, "is-IS" },
         { 1040, "it-IT" },
         { 1041, "ja-JP" },
         { 1042, "ko-KR" },
         { 1043, "nl-NL" },
         { 1044, "nb-NO" },
         { 1045, "pl-PL" },
         { 1046, "pt-BR" },
         { 1047, "rm-CH" },
         { 1048, "ro-RO" },
         { 1049, "ru-RU" },
         { 1050, "hr-HR" },
         { 1051, "sk-SK" },
         { 1052, "sq-AL" },
         { 1053, "sv-SE" },
         { 1054, "th-TH" },
         { 1055, "tr-TR" },
         { 1056, "ur-PK" },
         { 1057, "id-ID" },
         { 1058, "uk-UA" },
         { 1059, "be-BY" },
         { 1060, "sl-SI" },
         { 1061, "et-EE" },
         { 1062, "lv-LV" },
         { 1063, "lt-LT" },
         { 1064, "tg-Cyrl-TJ" },
         { 1065, "fa-IR" },
         { 1066, "vi-VN" },
         { 1067, "hy-AM" },
         { 1068, "az-Latn-AZ" },
         { 1069, "eu-ES" },
         { 1070, "hsb-DE" },
         { 1071, "mk-MK" },
         { 1072, "st-ZA" },
         { 1073, "ts-ZA" },
         { 1074, "tn-ZA" },
         { 1075, "ve-ZA" },
         { 1076, "xh-ZA" },
         { 1077, "zu-ZA" },
         { 1078, "af-ZA" },
         { 1079, "ka-GE" },
         { 1080, "fo-FO" },
         { 1081, "hi-IN" },
         { 1082, "mt-MT" },
         { 1083, "se-NO" },
         { 1085, "yi-001" },
         { 1086, "ms-MY" },
         { 1087, "kk-KZ" },
         { 1088, "ky-KG" },
         { 1089, "sw-KE" },
         { 1090, "tk-TM" },
         { 1091, "uz-Latn-UZ" },
         { 1092, "tt-RU" },
         { 1093, "bn-IN" },
         { 1094, "pa-IN" },
         { 1095, "gu-IN" },
         { 1096, "or-IN" },
         { 1097, "ta-IN" },
         { 1098, "te-IN" },
         { 1099, "kn-IN" },
         { 1100, "ml-IN" },
         { 1101, "as-IN" },
         { 1102, "mr-IN" },
         { 1103, "sa-IN" },
         { 1104, "mn-MN" },
         { 1105, "bo-CN" },
         { 1106, "cy-GB" },
         { 1107, "km-KH" },
         { 1108, "lo-LA" },
         { 1109, "my-MM" },
         { 1110, "gl-ES" },
         { 1111, "kok-IN" },
         { 1112, "mni-IN" },
         { 1113, "sd-Deva-IN" },
         { 1114, "syr-SY" },
         { 1115, "si-LK" },
         { 1116, "chr-Cher-US" },
         { 1117, "iu-Cans-CA" },
         { 1118, "am-ET" },
         { 1119, "tzm-Arab-MA" },
         { 1120, "ks-Arab" },
         { 1121, "ne-NP" },
         { 1122, "fy-NL" },
         { 1123, "ps-AF" },
         { 1124, "fil-PH" },
         { 1125, "dv-MV" },
         { 1126, "bin-NG" },
         { 1127, "ff-NG" },
         { 1128, "ha-Latn-NG" },
         { 1129, "ibb-NG" },
         { 1130, "yo-NG" },
         { 1131, "quz-BO" },
         { 1132, "nso-ZA" },
         { 1133, "ba-RU" },
         { 1134, "lb-LU" },
         { 1135, "kl-GL" },
         { 1136, "ig-NG" },
         { 1137, "kr-NG" },
         { 1138, "om-ET" },
         { 1139, "ti-ET" },
         { 1140, "gn-PY" },
         { 1141, "haw-US" },
         { 1142, "la-001" },
         { 1143, "so-SO" },
         { 1144, "ii-CN" },
         { 1145, "pap-029" },
         { 1146, "arn-CL" },
         { 1148, "moh-CA" },
         { 1150, "br-FR" },
         { 1152, "ug-CN" },
         { 1153, "mi-NZ" },
         { 1154, "oc-FR" },
         { 1155, "co-FR" },
         { 1156, "gsw-FR" },
         { 1157, "sah-RU" },
         { 1158, "quc-Latn-GT" },
         { 1159, "rw-RW" },
         { 1160, "wo-SN" },
         { 1164, "prs-AF" },
         { 1169, "gd-GB" },
         { 1170, "ku-Arab-IQ" },
         { 2049, "ar-IQ" },
         { 2051, "ca-ES-valencia" },
         { 2052, "zh-CN" },
         { 2055, "de-CH" },
         { 2057, "en-GB" },
         { 2058, "es-MX" },
         { 2060, "fr-BE" },
         { 2064, "it-CH" },
         { 2067, "nl-BE" },
         { 2068, "nn-NO" },
         { 2070, "pt-PT" },
         { 2072, "ro-MD" },
         { 2073, "ru-MD" },
         { 2077, "sv-FI" },
         { 2080, "ur-IN" },
         { 2092, "az-Cyrl-AZ" },
         { 2094, "dsb-DE" },
         { 2098, "tn-BW" },
         { 2107, "se-SE" },
         { 2108, "ga-IE" },
         { 2110, "ms-BN" },
         { 2115, "uz-Cyrl-UZ" },
         { 2117, "bn-BD" },
         { 2118, "pa-Arab-PK" },
         { 2121, "ta-LK" },
         { 2128, "mn-Mong-CN" },
         { 2137, "sd-Arab-PK" },
         { 2141, "iu-Latn-CA" },
         { 2143, "tzm-Latn-DZ" },
         { 2144, "ks-Deva-IN" },
         { 2145, "ne-IN" },
         { 2151, "ff-Latn-SN" },
         { 2155, "quz-EC" },
         { 2163, "ti-ER" },
         { 3073, "ar-EG" },
         { 3076, "zh-HK" },
         { 3079, "de-AT" },
         { 3081, "en-AU" },
         { 3082, "es-ES" },
         { 3084, "fr-CA" },
         { 3131, "se-FI" },
         { 3152, "mn-Mong-MN" },
         { 3153, "dz-BT" },
         { 3179, "quz-PE" },
         { 4097, "ar-LY" },
         { 4100, "zh-SG" },
         { 4103, "de-LU" },
         { 4105, "en-CA" },
         { 4106, "es-GT" },
         { 4108, "fr-CH" },
         { 4122, "hr-BA" },
         { 4155, "smj-NO" },
         { 4191, "tzm-Tfng-MA" },
         { 5121, "ar-DZ" },
         { 5124, "zh-MO" },
         { 5127, "de-LI" },
         { 5129, "en-NZ" },
         { 5130, "es-CR" },
         { 5132, "fr-LU" },
         { 5146, "bs-Latn-BA" },
         { 5179, "smj-SE" },
         { 6145, "ar-MA" },
         { 6153, "en-IE" },
         { 6154, "es-PA" },
         { 6156, "fr-MC" },
         { 6170, "sr-Latn-BA" },
         { 6203, "sma-NO" },
         { 7169, "ar-TN" },
         { 7177, "en-ZA" },
         { 7178, "es-DO" },
         { 7180, "fr-029" },
         { 7194, "sr-Cyrl-BA" },
         { 7227, "sma-SE" },
         { 8193, "ar-OM" },
         { 8201, "en-JM" },
         { 8202, "es-VE" },
         { 8204, "fr-RE" },
         { 8218, "bs-Cyrl-BA" },
         { 8251, "sms-FI" },
         { 9217, "ar-YE" },
         { 9225, "en-029" },
         { 9226, "es-CO" },
         { 9228, "fr-CD" },
         { 9242, "sr-Latn-RS" },
         { 9275, "smn-FI" },
         { 10241, "ar-SY" },
         { 10249, "en-BZ" },
         { 10250, "es-PE" },
         { 10252, "fr-SN" },
         { 10266, "sr-Cyrl-RS" },
         { 11265, "ar-JO" },
         { 11273, "en-TT" },
         { 11274, "es-AR" },
         { 11276, "fr-CM" },
         { 11290, "sr-Latn-ME" },
         { 12289, "ar-LB" },
         { 12297, "en-ZW" },
         { 12298, "es-EC" },
         { 12300, "fr-CI" },
         { 12314, "sr-Cyrl-ME" },
         { 13313, "ar-KW" },
         { 13321, "en-PH" },
         { 13322, "es-CL" },
         { 13324, "fr-ML" },
         { 14337, "ar-AE" },
         { 14345, "en-ID" },
         { 14346, "es-UY" },
         { 14348, "fr-MA" },
         { 15361, "ar-BH" },
         { 15369, "en-HK" },
         { 15370, "es-PY" },
         { 15372, "fr-HT" },
         { 16385, "ar-QA" },
         { 16393, "en-IN" },
         { 16394, "es-BO" },
         { 17417, "en-MY" },
         { 17418, "es-SV" },
         { 18441, "en-SG" },
         { 18442, "es-HN" },
         { 19466, "es-NI" },
         { 20490, "es-PR" },
         { 21514, "es-US" },
         { 22538, "es-419" },
         { 23562, "es-CU" },
         { 25626, "bs-Cyrl" },
         { 26650, "bs-Latn" },
         { 27674, "sr-Cyrl" },
         { 28698, "sr-Latn" },
         { 28731, "smn" },
         { 29740, "az-Cyrl" },
         { 29755, "sms" },
         { 30724, "zh" },
         { 30740, "nn" },
         { 30746, "bs" },
         { 30764, "az-Latn" },
         { 30779, "sma" },
         { 30787, "uz-Cyrl" },
         { 30800, "mn-Cyrl" },
         { 30813, "iu-Cans" },
         { 30815, "tzm-Tfng" },
         { 31748, "zh-Hant" },
         { 31764, "nb" },
         { 31770, "sr" },
         { 31784, "tg-Cyrl" },
         { 31790, "dsb" },
         { 31803, "smj" },
         { 31811, "uz-Latn" },
         { 31814, "pa-Arab" },
         { 31824, "mn-Mong" },
         { 31833, "sd-Arab" },
         { 31836, "chr-Cher" },
         { 31837, "iu-Latn" },
         { 31839, "tzm-Latn" },
         { 31847, "ff-Latn" },
         { 31848, "ha-Latn" },
         { 31878, "quc-Latn" },
         { 31890, "ku-Arab" }
      };

      return mappings;
   }

   #endregion
}