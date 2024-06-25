# BogaNet.ObfuscatedType
Various obfuscated types for all value types and strings. This types prevent the values from being "plain" in memory and offers some protection against bad actors (like memory scanners and searchers).

## Main classes and example code
Obfuscated types for:
* [Integral numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types)
* [Floating-point numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)
* [bool](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool)
* [char](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char)
* [string](https://learn.microsoft.com/en-us/dotnet/api/system.string?view=net-8.0)

```cs
DoubleObf age = 35.8;
double years = 7;
age += years;

Console.WriteLine(age.ToString());

StringObf text = "Hello Wörld!";
string frag = " byebye!";
text += frag;

Console.WriteLine(text);
```

## Nuget:
[BogaNet.ObfuscatedType](https://www.nuget.org/packages/BogaNet.ObfuscatedType/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)