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
* Excessive punctuation

It supports any language and any writing system.

## Main classes and example code
* [Pacifier](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_b_w_f_1_1_pacifier.html): Combines all filters into one.
* [BadWordFilter](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_b_w_f_1_1_filter_1_1_bad_word_filter.html): Filter to remove bad words aka profanity.
* [DomainFilter](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_b_w_f_1_1_filter_1_1_domain_filter.html): Filter to remove domains (urls/emails etc.).
* [CapitalizationFilter](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_b_w_f_1_1_filter_1_1_capitalization_filter.html): Filter to remove excessive capitalization.
* [PunctuationFilter](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_b_w_f_1_1_filter_1_1_punctuation_filter.html): Filter to remove excessive punctuation.

```csharp
// load bad words for all left-to-right written languages
BadWordFilter.Instance.LoadFiles(true, BWFConstants.BWF_LTR);

// load bad words for all right-to-left written languages
BadWordFilter.Instance.LoadFiles(false, BWFConstants.BWF_RTL);

// load all domains to detect urls, emails etc.
DomainFilter.Instance.LoadFiles(BWFConstants.DOMAINS);

string foulText = "MARTIANS are assholes/arschlöcher!!!!!!!!!!  => WATCH: https//mytruthpage.com/weirdowatch/martians123.divx or WRITE an EMAIL: weirdo@gmail.com";

// does the text contain any bad words, domains, excessive capitalizations/punctuations?
bool contains = Pacifier.Instance.Contains(foulText);
Console.WriteLine("Contains: " + contains);

// get all bad words, domains and excessive capitalizations/punctuations
var allBaddies = Pacifier.Instance.GetAll(foulText);
Console.WriteLine(allBaddies.BNDump());

// replace all bad words, domains or excessive capitalizations/punctuations
string removedProfanity = Pacifier.Instance.ReplaceAll(foulText);
Console.WriteLine(removedProfanity);
```

## Nuget:
[BogaNet.BadWordFilter](https://www.nuget.org/packages/BogaNet.BadWordFilter/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)
