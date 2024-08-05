using System;

namespace BogaNet.BWF;

/// <summary>
/// Constants for BWF in Avalonia.
/// </summary>
public static class BWFAvaloniaConstants
{
   #region Changable variables

   #region LTR sources

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Czech.
   /// </summary>
   public static Tuple<string, string> BWF_AV_CS = new("cs", "/Assets/Filters/ltr/cs.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Danish.
   /// </summary>
   public static Tuple<string, string> BWF_AV_DA = new("da", "/Assets/Filters/ltr/da.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: German.
   /// </summary>
   public static Tuple<string, string> BWF_AV_DE = new("de", "/Assets/Filters/ltr/de.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Swiss-German.
   /// </summary>
   public static Tuple<string, string> BWF_AV_DE_CH = new("de-ch", "/Assets/Filters/ltr/de_ch.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Greek.
   /// </summary>
   public static Tuple<string, string> BWF_AV_EL = new("el", "/Assets/Filters/ltr/el.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: English.
   /// </summary>
   public static Tuple<string, string> BWF_AV_EN = new("en", "/Assets/Filters/ltr/en.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Spanish.
   /// </summary>
   public static Tuple<string, string> BWF_AV_ES = new("es", "/Assets/Filters/ltr/es.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Finnish.
   /// </summary>
   public static Tuple<string, string> BWF_AV_FI = new("fi", "/Assets/Filters/ltr/fi.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: French.
   /// </summary>
   public static Tuple<string, string> BWF_AV_FR = new("fr", "/Assets/Filters/ltr/fr.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Hindi.
   /// </summary>
   public static Tuple<string, string> BWF_AV_HI = new("hi", "/Assets/Filters/ltr/hi.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Hungarian.
   /// </summary>
   public static Tuple<string, string> BWF_AV_HU = new("hu", "/Assets/Filters/ltr/hu.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Italian.
   /// </summary>
   public static Tuple<string, string> BWF_AV_IT = new("it", "/Assets/Filters/ltr/it.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Japanese.
   /// </summary>
   public static Tuple<string, string> BWF_AV_JA = new("ja", "/Assets/Filters/ltr/ja.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Korean.
   /// </summary>
   public static Tuple<string, string> BWF_AV_KO = new("ko", "/Assets/Filters/ltr/ko.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Dutch.
   /// </summary>
   public static Tuple<string, string> BWF_AV_NL = new("nl", "/Assets/Filters/ltr/nl.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Norwegian.
   /// </summary>
   public static Tuple<string, string> BWF_AV_NO = new("no", "/Assets/Filters/ltr/no.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Polish.
   /// </summary>
   public static Tuple<string, string> BWF_AV_PL = new("pl", "/Assets/Filters/ltr/pl.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Portuguese.
   /// </summary>
   public static Tuple<string, string> BWF_AV_PT = new("pt", "/Assets/Filters/ltr/pt.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Russian.
   /// </summary>
   public static Tuple<string, string> BWF_AV_RU = new("ru", "/Assets/Filters/ltr/ru.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Swedish.
   /// </summary>
   public static Tuple<string, string> BWF_AV_SV = new("sv", "/Assets/Filters/ltr/sv.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Thai.
   /// </summary>
   public static Tuple<string, string> BWF_AV_TH = new("th", "/Assets/Filters/ltr/th.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Turkish.
   /// </summary>
   public static Tuple<string, string> BWF_AV_TR = new("tr", "/Assets/Filters/ltr/tr.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Vietnamese.
   /// </summary>
   public static Tuple<string, string> BWF_AV_VI = new("vi", "/Assets/Filters/ltr/vi.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Chinese.
   /// </summary>
   public static Tuple<string, string> BWF_AV_ZH = new("zh", "/Assets/Filters/ltr/zh.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Emojis.
   /// </summary>
   public static Tuple<string, string> BWF_AV_EMOJI = new("emoji", "/Assets/Filters/special/emoji.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: global bad words.
   /// </summary>
   public static Tuple<string, string> BWF_AV_GLOBAL = new("global", "/Assets/Filters/special/global.txt");

   /// <summary>
   /// All left-to-right (LTR) sources in Avalonia for BadWordFilter.
   /// </summary>
   public static Tuple<string, string>[] BWF_AV_LTR =
   [
      BWF_AV_CS, BWF_AV_DA, BWF_AV_DE, BWF_AV_DE_CH, BWF_AV_EL, BWF_AV_EN, BWF_AV_ES, BWF_AV_FI, BWF_AV_FR, BWF_AV_HI, BWF_AV_HU, BWF_AV_IT, BWF_AV_JA,
      BWF_AV_KO, BWF_AV_NL, BWF_AV_NO, BWF_AV_PL, BWF_AV_PT, BWF_AV_RU, BWF_AV_SV, BWF_AV_TH, BWF_AV_TR, BWF_AV_VI, BWF_AV_ZH, BWF_AV_EMOJI, BWF_AV_GLOBAL
   ];

   #endregion

   #region RTL sources

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Arabic.
   /// </summary>
   public static Tuple<string, string> BWF_AV_AR = new("ar", "/Assets/Filters/rtl/ar.txt");

   /// <summary>
   /// Source in Avalonia for BadWordFilter: Persian.
   /// </summary>
   public static Tuple<string, string> BWF_AV_FA = new("fa", "/Assets/Filters/rtl/fa.txt");

   /// <summary>
   /// All right-to-left (RTL) sources in Avalonia for BadWordFilter.
   /// </summary>
   public static Tuple<string, string>[] BWF_AV_RTL = [BWF_AV_AR, BWF_AV_FA];

   #endregion

   /// <summary>
   /// Source in Avalonia for DomainFilter: all domains.
   /// </summary>
   public static Tuple<string, string> DOMAINS_AV = new("domains", "/Assets/Filters/domains.txt");

   #endregion
}