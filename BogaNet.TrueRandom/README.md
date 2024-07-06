# BogaNet.TrueRandom

## Why use TrueRandom?
“TrueRandom” can generate random numbers and they are “truly random”, because they are generated with atmospheric noise, which supersedes the pseudo-random number algorithms typically use in computer programs.
TrueRandom can be used for holding drawings, lotteries and sweepstakes, to drive online games, for scientific applications and for art and music.

Here some more information regarding “true” vs. “pseudo-” random:
There are two principal methods used to generate random numbers. The first method measures some physical phenomenon that is expected to be random and then compensates for possible biases in the measurement process. Example sources include measuring atmospheric noise, thermal noise, and other external electromagnetic and quantum phenomena. For example, cosmic background radiation or radioactive decay as measured over short timescales represent sources of natural entropy.
The second method uses computational algorithms that can produce long sequences of apparently random results, which are in fact completely determined by a shorter initial value, known as a seed value or key. As a result, the entire seemingly random sequence can be reproduced if the seed value is known. This type of random number generator is often called a pseudorandom number generator. This type of generator typically does not rely on sources of naturally occurring entropy, though it may be periodically seeded by natural sources. This generator type is non-blocking, so they are not rate-limited by an external event, making large bulk reads a possibility.
![Comparison TrueRandom vs. C# Random](https://github.com/slaubenberger/BogaNet/blob/develop/Resources/images/TrueRandom.jpg?raw=true)

For more, please read this:
https://en.wikipedia.org/wiki/Random_number_generation

## How does this differ from C# Random?
Perhaps you have wondered how C# generates randomness. In reality, random numbers used in C# are pseudo-random, which means they are generated in a predictable fashion using a mathematical formula.

This is fine for many purposes, but it may not be random in the way you expect it to be when you think of dice rolls and lottery drawings.

## How does it work
"TrueRandom" uses the API of [random.org](https://www.random.org/), which provides a free tier with a quota limitation of 1'000'000 random bits per IP-address in 24 hours.
This allows to generate at least:
* 120'000 bytes
* 30'000 integers/floats (depends on the size)
* 12'000 strings (length of 10 chars, depends on the settings)
* 3'000 sequences (interval of 10 elements)

If the quota expires, C# pseudo-random will be used automatically.
It is recommended to use "TrueRandom" only to set seeds in the PRNG and refresh them as desired to reduce the delay and quota usage.

## Main classes and example code
Secure types for:
* [Integral numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types)
* [Floating-point numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)
* [bool](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool)
* [char](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char)
* [string](https://learn.microsoft.com/en-us/dotnet/api/system.string?view=net-8.0)
* all **objects** (acts currently only as storage container)

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
[BogaNet.TrueRandom](https://www.nuget.org/packages/BogaNet.TrueRandom/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)