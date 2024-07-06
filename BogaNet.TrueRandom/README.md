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

## Quota
"TrueRandom" uses the API of [random.org](https://www.random.org/), which provides a free tier with a quota limitation of 1'000'000 random bits per IP-address in 24 hours.
This allows to generate at least:
* 120'000 bytes
* 30'000 integers/floats (depends on the size)
* 12'000 strings (length of 10 chars, depends on the settings)
* 3'000 sequences (interval of 10 elements)

If the quota expires, C# pseudo-random will be used automatically.
It is recommended to use "TrueRandom" only to set seeds in the PRNG and refresh them as desired to reduce the delay and quota usage.

## Main classes
* [CheckQuota](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_check_quota.html)
* [TRNGBytes](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_t_r_n_g_bytes.html)
* [TRNGFloat](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_t_r_n_g_float.html)
* [TRNGInteger](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_t_r_n_g_integer.html)
* [TRNGSequence](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_t_r_n_g_sequence.html)
* [TRNGString](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_t_r_n_g_string.html)

## Nuget:
[BogaNet.TrueRandom](https://www.nuget.org/packages/BogaNet.TrueRandom/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)