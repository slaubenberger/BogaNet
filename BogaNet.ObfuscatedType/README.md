# BogaNet.ObfuscatedType
Various obfuscated types for all value types and strings. This types prevent the values from being "plain" in memory and offers some protection against bad actors (like memory scanners and searchers).

## Main classes and example code
Obfuscated types for:
* [Integral numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types)
* [Floating-point numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)
* [char](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char)
* [string](https://learn.microsoft.com/en-us/dotnet/api/system.string?view=net-8.0)

```cs
var loc = Localizer.Instance;
loc.LoadFiles("./Resources/Translation.csv", "./Resources/Translation_de.csv"); //load the translation files
loc.Culture = new CultureInfo("en"); //set the culture to English
Console.WriteLine(loc.GetText("GreetingText"));
```

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)

## Nuget:
[BogaNet.ObfuscatedType](https://www.nuget.org/packages/BogaNet.ObfuscatedType/)