# BogaNet.BadWordFilter
The “Bad Word Filter” (aka profanity or obscenity filter) is a fresh implementation for .NET8 of the well-known Unity package [BadWordFilter PRO](https://assetstore.unity.com/packages/slug/26255?aid=1011lNGT) and is exactly what the title suggests: a tool to filter swearwords and other “bad sentences”.
The library already includes support for over 5’000 of regular expressions (equivalent to tens of thousands of word variations) in 25 languages:
Arabic, Chinese, Czech, Danish, Dutch, English, Finnish, French, German, Greek, Hindi, Hungarian, Italian, Japanese, Korean, Norwegian, Persian, Polish, Portuguese, Russian, Spanish, Swedish, Thai, Turkish and Vietnamese.

Fell free to add any words and languages that are missing!

BWF also includes those additional filters: 
* Domains (URLs/emails)
* Global bad words
* Emojis (miscellaneous symbols)
* Excessive capitalization
* Excessive punctuation.

It supports any language and any writing system.

## Main classes and example code
* [Pacifier](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1i18n_1_1_localizer.html): Combines all filters into one.
* [BadWordFilter](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1i18n_1_1_localizer.html): Filter to remove bad words aka profanity.
* [DomainFilter](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1i18n_1_1_localizer.html): Filter to remove domains (urls/emails etc.).
* [CapitalizationFilter](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1i18n_1_1_localizer.html): Filter to remove excessive capitalization.
* [PunctuationFilter](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1i18n_1_1_localizer.html): Filter to remove excessive punctuation.

```csharp
Localizer.Instance.LoadFiles("./Resources/Translation.csv", "./Resources/Translation_de.csv"); //load the translation files
Localizer.Instance.Culture = new CultureInfo("en"); //set the culture to English
Console.WriteLine(Localizer.Instance.GetText("Greeting"));
```

## Nuget:
[BogaNet.BadWordFilter](https://www.nuget.org/packages/BogaNet.BadWordFilter/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)
