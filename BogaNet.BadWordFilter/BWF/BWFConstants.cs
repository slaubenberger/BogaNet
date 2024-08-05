using System;

namespace BogaNet.BWF;

public static class BWFConstants
{
   #region Changable variables

   #region LTR sources

   /// <summary>
   /// Source for BadWordFilter: Czech.
   /// </summary>
   public static Tuple<string, string> BWF_CS = new("cs", "./Resources/Filters/ltr/cs.txt");

   /// <summary>
   /// Source for BadWordFilter: Danish.
   /// </summary>
   public static Tuple<string, string> BWF_DA = new("da", "./Resources/Filters/ltr/da.txt");

   /// <summary>
   /// Source for BadWordFilter: German.
   /// </summary>
   public static Tuple<string, string> BWF_DE = new("de", "./Resources/Filters/ltr/de.txt");

   /// <summary>
   /// Source for BadWordFilter: Swiss-German.
   /// </summary>
   public static Tuple<string, string> BWF_DE_CH = new("de-ch", "./Resources/Filters/ltr/de_ch.txt");

   /// <summary>
   /// Source for BadWordFilter: Greek.
   /// </summary>
   public static Tuple<string, string> BWF_EL = new("el", "./Resources/Filters/ltr/el.txt");

   /// <summary>
   /// Source for BadWordFilter: English.
   /// </summary>
   public static Tuple<string, string> BWF_EN = new("en", "./Resources/Filters/ltr/en.txt");

   /// <summary>
   /// Source for BadWordFilter: Spanish.
   /// </summary>
   public static Tuple<string, string> BWF_ES = new("es", "./Resources/Filters/ltr/es.txt");

   /// <summary>
   /// Source for BadWordFilter: Finnish.
   /// </summary>
   public static Tuple<string, string> BWF_FI = new("fi", "./Resources/Filters/ltr/fi.txt");

   /// <summary>
   /// Source for BadWordFilter: French.
   /// </summary>
   public static Tuple<string, string> BWF_FR = new("fr", "./Resources/Filters/ltr/fr.txt");

   /// <summary>
   /// Source for BadWordFilter: Hindi.
   /// </summary>
   public static Tuple<string, string> BWF_HI = new("hi", "./Resources/Filters/ltr/hi.txt");

   /// <summary>
   /// Source for BadWordFilter: Hungarian.
   /// </summary>
   public static Tuple<string, string> BWF_HU = new("hu", "./Resources/Filters/ltr/hu.txt");

   /// <summary>
   /// Source for BadWordFilter: Italian.
   /// </summary>
   public static Tuple<string, string> BWF_IT = new("it", "./Resources/Filters/ltr/it.txt");

   /// <summary>
   /// Source for BadWordFilter: Japanese.
   /// </summary>
   public static Tuple<string, string> BWF_JA = new("ja", "./Resources/Filters/ltr/ja.txt");

   /// <summary>
   /// Source for BadWordFilter: Korean.
   /// </summary>
   public static Tuple<string, string> BWF_KO = new("ko", "./Resources/Filters/ltr/ko.txt");

   /// <summary>
   /// Source for BadWordFilter: Dutch.
   /// </summary>
   public static Tuple<string, string> BWF_NL = new("nl", "./Resources/Filters/ltr/nl.txt");

   /// <summary>
   /// Source for BadWordFilter: Norwegian.
   /// </summary>
   public static Tuple<string, string> BWF_NO = new("no", "./Resources/Filters/ltr/no.txt");

   /// <summary>
   /// Source for BadWordFilter: Polish.
   /// </summary>
   public static Tuple<string, string> BWF_PL = new("pl", "./Resources/Filters/ltr/pl.txt");

   /// <summary>
   /// Source for BadWordFilter: Portuguese.
   /// </summary>
   public static Tuple<string, string> BWF_PT = new("pt", "./Resources/Filters/ltr/pt.txt");

   /// <summary>
   /// Source for BadWordFilter: Russian.
   /// </summary>
   public static Tuple<string, string> BWF_RU = new("ru", "./Resources/Filters/ltr/ru.txt");

   /// <summary>
   /// Source for BadWordFilter: Swedish.
   /// </summary>
   public static Tuple<string, string> BWF_SV = new("sv", "./Resources/Filters/ltr/sv.txt");

   /// <summary>
   /// Source for BadWordFilter: Thai.
   /// </summary>
   public static Tuple<string, string> BWF_TH = new("th", "./Resources/Filters/ltr/th.txt");

   /// <summary>
   /// Source for BadWordFilter: Turkish.
   /// </summary>
   public static Tuple<string, string> BWF_TR = new("tr", "./Resources/Filters/ltr/tr.txt");

   /// <summary>
   /// Source for BadWordFilter: Vietnamese.
   /// </summary>
   public static Tuple<string, string> BWF_VI = new("vi", "./Resources/Filters/ltr/vi.txt");

   /// <summary>
   /// Source for BadWordFilter: Chinese.
   /// </summary>
   public static Tuple<string, string> BWF_ZH = new("zh", "./Resources/Filters/ltr/zh.txt");

   /// <summary>
   /// Source for BadWordFilter: Emojis.
   /// </summary>
   public static Tuple<string, string> BWF_EMOJI = new("emoji", "./Resources/Filters/special/emoji.txt");

   /// <summary>
   /// Source for BadWordFilter: global bad words.
   /// </summary>
   public static Tuple<string, string> BWF_GLOBAL = new("global", "./Resources/Filters/special/global.txt");

   /// <summary>
   /// All left-to-right (LTR) sources for BadWordFilter.
   /// </summary>
   public static Tuple<string, string>[] BWF_LTR =
   [
      BWF_CS, BWF_DA, BWF_DE, BWF_DE_CH, BWF_EL, BWF_EN, BWF_ES, BWF_FI, BWF_FR, BWF_HI, BWF_HU, BWF_IT, BWF_JA,
      BWF_KO, BWF_NL, BWF_NO, BWF_PL, BWF_PT, BWF_RU, BWF_SV, BWF_TH, BWF_TR, BWF_VI, BWF_ZH, BWF_EMOJI, BWF_GLOBAL
   ];

   #endregion

   #region RTL sources

   /// <summary>
   /// Source for BadWordFilter: Arabic.
   /// </summary>
   public static Tuple<string, string> BWF_AR = new("ar", "./Resources/Filters/rtl/ar.txt");

   /// <summary>
   /// Source for BadWordFilter: Persian.
   /// </summary>
   public static Tuple<string, string> BWF_FA = new("fa", "./Resources/Filters/rtl/fa.txt");

   /// <summary>
   /// All right-to-left (RTL) sources for BadWordFilter.
   /// </summary>
   public static Tuple<string, string>[] BWF_RTL = [BWF_AR, BWF_FA];

   #endregion

   /// <summary>
   /// Source for DomainFilter: all domains.
   /// </summary>
   public static Tuple<string, string> DOMAINS = new("en", "./Resources/Filters/ltr/en.txt");

   /// <summary>
   /// Debug the BadWordFilter. Enable this to identify the exact word that was detected.
   /// </summary>
   public static bool DEBUG_BADWORDS = false;

   /// <summary>
   /// Debug the DomainFilter. Enable this to identify the exact domain that was detected.
   /// </summary>
   public static bool DEBUG_DOMAINS = false;

   #endregion
}