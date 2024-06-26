# BogaNet.SecureType
Various encrypted types for all value types and strings. This types prevent the values from being "plain" in memory and offers high protection against bad actors (like memory scanners and searchers).

## Important note
This types are performance and memory intense compared to the original C# types, but are cryptographically secure! Use it for sensitive data, like:
* Passwords
* Bank account/routing numbers
* Social security numbers (SSN)
* Drivers license numbers
* Passport ID
* Federal tax ID
* Employer identification numbers (EIN) 
* Health insurance policy/member numbers

For less sensitive data, like usernames etc., consider using [BogaNet.ObfuscatedType](https://www.nuget.org/packages/BogaNet.ObfuscatedType/).

## Main classes and example code
Secure types for:
* [Integral numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types)
* [Floating-point numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)
* [bool](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool)
* [char](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char)
* [string](https://learn.microsoft.com/en-us/dotnet/api/system.string?view=net-8.0)

```csharp
DoubleSec age = 35.8;
double years = 7;
age += years;

Console.WriteLine(age.ToString());

StringSec text = "Hello Wörld!";
string frag = " byebye!";
text += frag;

Console.WriteLine(text);
```
## Nuget:
[BogaNet.SecureType](https://www.nuget.org/packages/BogaNet.SecureType/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)