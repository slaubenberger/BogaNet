# BogaNet
.NET 8 version of Boga, a collection of tools to make C# development a joyride.

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## BogaNet.Avalonia
Little helpers to speed up Avalonia development.

### Main classes
* [ImageHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_image_helper.html): Helper for images in Avalonia.
* [ResourceHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_resource_helper.html): Helper for resources in Avalonia.

### Nuget:
[BogaNet.Avalonia](https://www.nuget.org/packages/BogaNet.Avalonia/)

## BogaNet.Avalonia.Browser
Browser-specific helpers for Avalonia development.
It also contains various JavaScript-helper files.

### Main classes and usage
* [BrowserPreferencesContainer](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_prefs_1_1_browser_preferences_container.html): Preferences-container for browser, for more see: [BogaNet.Prefs](https://www.nuget.org/packages/BogaNet.Prefs/).
* [BrowserVoiceProvider](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_t_t_s_1_1_provider_1_1_browser_voice_provider.html): TTS for browser, for more see: [BogaNet.TTS](https://www.nuget.org/packages/BogaNet.TTS/).
* [UrlHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_url_helper.html): Get and set the URL of the application.

JavaScript-files:
* [boganet_exit.js](https://raw.githubusercontent.com/slaubenberger/BogaNet/develop/BogaNet.Avalonia.Browser/wwwroot/boganet_exit.js): Prevents the app from closing/reload and offers a callback to react
* [boganet_prefs.js](https://raw.githubusercontent.com/slaubenberger/BogaNet/develop/BogaNet.Avalonia.Browser/wwwroot/boganet_prefs.js): Bridge for BrowserPreferencesContainer
* [boganet_tts.js](https://raw.githubusercontent.com/slaubenberger/BogaNet/develop/BogaNet.Avalonia.Browser/wwwroot/boganet_tts.js): Bridge for BrowserVoiceProvider
* [boganet_url.js](https://raw.githubusercontent.com/slaubenberger/BogaNet/develop/BogaNet.Avalonia.Browser/wwwroot/boganet_url.js): Get and set the URL of the application

Unfortunately, you have to manually copy the desired files to "wwwroot" since I don't know how to include it in the Nuget-package correctly... Tips are welcome! :-)

#### Exit
This callback prevents close and reload-operations in the browser.
Add/modify the following code to in "Program.cs" from the Avalonia Browser-project and handle the case (e.g saving data etc.):
```csharp
private static async Task Main(string[] args)
{
    await JSHost.ImportAsync("boganet_exit", "../boganet_exit.js");
    //...rest of the Main-code
}

[JSExport]
internal static void Exit()
{
    Preferences.Instance.Save();
    //... your code (e.g. saving data etc.)
}
```

#### Preferences
To use the [BogaNet.Prefs](https://www.nuget.org/packages/BogaNet.Prefs/), add/modify "Program.cs" from the Avalonia Browser-project:
```csharp
private static async Task Main(string[] args)
{
    await JSHost.ImportAsync("boganet_prefs", "../boganet_prefs.js");
    Preferences.Instance.Container = new BrowserPreferencesContainer();
    await Preferences.Instance.LoadAsync();
    //...rest of the Main-code
}
```

#### TTS
To use the [BogaNet.TTS](https://www.nuget.org/packages/BogaNet.TTS/), add/modify "Program.cs" from the Avalonia Browser-project:
```csharp
private static async Task Main(string[] args)
{
    await JSHost.ImportAsync("boganet_tts", "../boganet_tts.js");
    Speaker.Instance.CustomVoiceProvider = BrowserVoiceProvider.Instance;
    //...rest of the Main-code
}
```

#### URL
To set/get the application URL, add/modify "Program.cs" from the Avalonia Browser-project:
```csharp
private static async Task Main(string[] args)
{
    await JSHost.ImportAsync("boganet_url", "../boganet_url.js");
    //...rest of the Main-code
}
```

The URL can then be accessed via UrlHelper:
```csharp
Console.WriteLine("Browser-URL: " + UrlHelper.URL);
```

### Nuget:
[BogaNet.Avalonia.Browser](https://www.nuget.org/packages/BogaNet.Avalonia.Browser/)


## BogaNet.Common
Main library for all BogaNet-packages filled with useful helpers to speed up C# development.

### Main classes
* [FileHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_file_helper.html): Various helper functions for filesystem operations.
* [JsonHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_json_helper.html): Helper for JSON operations.
* [NetworkHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_network_helper.html): Various helper functions for networking.
* [Obfuscator](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_util_1_1_obfuscator.html): Obfuscator for strings and byte-arrays.
* [ProcessRunner](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_util_1_1_process_runner.html): Executes applications and commands.
* [ShortUID](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_util_1_1_short_u_i_d.html): Short Guid implementation with a length of 22 characters (instead 36 of the normal Guid).
* [StringHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_string_helper.html): Helper methods for strings.
* [XmlHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_xml_helper.html): Helper for XML operations.

There are also many powerful extensions for arrays, bytes, dictionaries, lists, numbers and strings.

### Nuget:
[BogaNet.Common](https://www.nuget.org/packages/BogaNet.Common/)

## BogaNet.CRC
Various helpers for cyclic redundancy checks (CRC), namely CRC8, CRC16, CRC32 and CRC64.

### Main classes
* [CRC8](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_c_r_c_1_1_c_r_c8.html): Implementation of CRC with 8bit (byte).
* [CRC16](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_c_r_c_1_1_c_r_c16.html): Implementation of CRC with 16bit (ushort).
* [CRC32](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_c_r_c_1_1_c_r_c32.html): Implementation of CRC with 32bit (uint).
* [CRC64](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_c_r_c_1_1_c_r_c64.html): Implementation of CRC with 64bit (ulong).

### Nuget:
[BogaNet.CRC](https://www.nuget.org/packages/BogaNet.CRC/)

## BogaNet.Crypto
Various helpers for cryptographic functions, like hashing (SHA), asymmetric (AES) and symmetric (RSA) encryption/decryption, and HMAC.

### Main classes
* [AESHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_a_e_s_helper.html): Helper for AES cryptography.
* [HashHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_hash_helper.html): Helper for hash computations. It contains ready-to-use implementations of SHA256, SHA384, SHA512, SHA3-256, SHA3-384 and SHA3-512.
* [HMACHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_h_m_a_c_helper.html): Helper for HMAC cryptography. It contains ready-to-use implementations of HMAC-SHA256, HMAC-SHA384, HMAC-SHA512, HMAC-SHA3-256, HMAC-SHA3-384 and HMAC-SHA3-512.
* [RSAHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_r_s_a_helper.html): Helper for RSA cryptography and X509 certificates.

### Nuget:
[BogaNet.Crypto](https://www.nuget.org/packages/BogaNet.Crypto/)

## BogaNet.Encoder
A collection of various binary encoders.

### Main classes
* [Base2](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_encoder_1_1_base2.html): Base2 (aka binary) encoder.
* [Base16](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_encoder_1_1_base16.html): Base16 (aka Hex) encoder.
* [Base32](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_encoder_1_1_base32.html): Base32 encoder.
* [Base64](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_encoder_1_1_base64.html): Base64 encoder.
* [Base85](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_encoder_1_1_base85.html): Base85 encoder.
* [Base91](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_encoder_1_1_base91.html): Base91 encoder.

### Nuget:
[BogaNet.Encoder](https://www.nuget.org/packages/BogaNet.Encoder/)

## BogaNet.i18n
Localizer for C# applications.

### Main class and example code
* [Localizer](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1i18n_1_1_localizer.html): i18n localizer.

```csharp
Localizer.Instance.LoadFiles("./Resources/Translation.csv", "./Resources/Translation_de.csv"); //load the translation files
Localizer.Instance.Culture = new CultureInfo("en"); //set the culture to English
Console.WriteLine(Localizer.Instance.GetText("Greeting"));
```
The files for the translations are CSV-based with the following structure for the columns:
1. Key
2. Language code (ISO 639) and translation for the key
3. Additional column per language code and translation
```
key,en,de
Greeting,"Hi there!","Hallöchen zusammen!"
Name,"Name:","Name:"
Name_Tooltip,"Enter your name to access the application.","Gib deinen Namen ein um Zugang zur Applikation zu erhalten."
Name_Placeholder,"Enter your name here...","Gib deinen Namen hier ein..."
```
It's recommended to use one file per language code.

### Nuget:
[BogaNet.i18n](https://www.nuget.org/packages/BogaNet.i18n/)

## BogaNet.ObfuscatedType
Various obfuscated types for all value types, strings and objects. This types prevent the values from being "plain" in memory and offers some protection against bad actors (like memory scanners and searchers).

### Important note
This types are fast and lightweight, but not cryptographically secure! Use it for less sensitive data, like:
* Username
* First and last names
* Email addresses
* Mailing addresses
* Phone numbers
* Social media profile names
* Highscores

For sensitive data, like passwords etc., it is **strongly recommended** to use [BogaNet.SecureType](https://www.nuget.org/packages/BogaNet.SecureType/) instead.

### Main classes and example code
Obfuscated types for:
* [Integral numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types)
* [Floating-point numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)
* [bool](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool)
* [char](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char)
* [string](https://learn.microsoft.com/en-us/dotnet/api/system.string?view=net-8.0)
* all **objects** (acts currently only as storage container)

```csharp
DoubleObf age = 35.8;
double years = 7;
age += years;

Console.WriteLine(age.ToString());

StringObf text = "Hello Wörld!";
string frag = " byebye!";
text += frag;

Console.WriteLine(text);
```

### Nuget:
[BogaNet.ObfuscatedType](https://www.nuget.org/packages/BogaNet.ObfuscatedType/)

## BogaNet.Prefs
Handles preferences/settings for C# applications. It supports all values types, strings, DateTime and object.
Furthermore, it allows to store the data in obfuscated form to prevent it from being easily read.
The data is automatically stored at application exit.

### Main class and example code
* [Preferences](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_prefs_1_1_preferences.html): Preferences for the application.

```csharp
string textObf = "Hello obfuscated wörld!";
string keyObf = "textObf";

Preferences.Instance.Set(keyObf, textObf, true); //save text in obfuscated form
Console.WriteLine(Preferences.Instance.GetString(keyObf, true));

double number = 12.345;
string keyNumber = "number";

Preferences.Instance.Set(keyNumber, number);
Console.WriteLine(Preferences.Instance.GetNumber<double>(keyNumber).ToString());
```

### Nuget:
[BogaNet.Prefs](https://www.nuget.org/packages/BogaNet.Prefs/)

## BogaNet.SecureType
Various encrypted types for all value types, strings and objects. This types prevent the values from being "plain" in memory and offers high protection against bad actors (like memory scanners and searchers).

### Important note
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

### Main classes and example code
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
### Nuget:
[BogaNet.SecureType](https://www.nuget.org/packages/BogaNet.SecureType/)

## BogaNet.TrueRandom

### Why use TrueRandom?
“TrueRandom” can generate random numbers and they are “truly random”, because they are generated with atmospheric noise, which supersedes the pseudo-random number algorithms typically use in computer programs.
TrueRandom can be used for holding drawings, lotteries and sweepstakes, to drive online games, for scientific applications and for art and music.

Here some more information regarding “true” vs. “pseudo-” random:
There are two principal methods used to generate random numbers. The first method measures some physical phenomenon that is expected to be random and then compensates for possible biases in the measurement process. Example sources include measuring atmospheric noise, thermal noise, and other external electromagnetic and quantum phenomena. For example, cosmic background radiation or radioactive decay as measured over short timescales represent sources of natural entropy.
The second method uses computational algorithms that can produce long sequences of apparently random results, which are in fact completely determined by a shorter initial value, known as a seed value or key. As a result, the entire seemingly random sequence can be reproduced if the seed value is known. This type of random number generator is often called a pseudorandom number generator. This type of generator typically does not rely on sources of naturally occurring entropy, though it may be periodically seeded by natural sources. This generator type is non-blocking, so they are not rate-limited by an external event, making large bulk reads a possibility.
![Comparison TrueRandom vs. C# Random](https://raw.githubusercontent.com/slaubenberger/BogaNet/develop/Resources/images/TrueRandom.jpg)

For more, please read this:
https://en.wikipedia.org/wiki/Random_number_generation

### Quota
"TrueRandom" uses the API of [random.org](https://www.random.org/), which provides a free tier with a quota limitation of 1'000'000 random bits per IP-address in 24 hours.
This allows to generate at least:
* 120'000 bytes
* 30'000 integers/floats (depends on the size)
* 12'000 strings (length of 10 chars, depends on the settings)
* 3'000 sequences (interval of 10 elements)

If the quota expires, C# pseudo-random will be used automatically.
It is recommended to use "TrueRandom" only to set seeds in the PRNG and refresh them as desired to reduce the delay and quota usage.

### Main classes
* [CheckQuota](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_check_quota.html): Gets the remaining quota from www.random.org.
* [BytesTRNG](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_bytes_t_r_n_g.html): Generates true random byte-arrays in configurable intervals.
* [FloatTRNG](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_float_t_r_n_g.html): Generates true random floats in configurable intervals.
* [IntegerTRNG](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_integer_t_r_n_g.html): Generates true random integers in configurable intervals.
* [SequenceTRNG](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_sequence_t_r_n_g.html): Randomizes a given interval of integers, i.e. arrange them in random order.
* [StringTRNG](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_true_random_1_1_string_t_r_n_g.html): Generates true random strings of various length and character compositions.

### Nuget:
[BogaNet.TrueRandom](https://www.nuget.org/packages/BogaNet.TrueRandom/)

## BogaNet.TTS
Use the built-in Text-To-Speech (TTS) engine under Windows, OSX and Linux.
It provides all installed voices and support for [SSML](https://www.w3.org/TR/speech-synthesis/).
For an implementation for the web browser, please check my other package [BogaNet.Avalonia.Browser](https://www.nuget.org/packages/BogaNet.Avalonia.Browser/).
It's also possible to implement your own custom voice provider (e.g. to use other engines like AWS Polly, Azure, Google, ElvenLabs etc.).

### Note
The LinuxVoiceProvider uses eSpeak/eSpeak-NG as engine, which is also available for Windows and OSX.
Therefore it's possible to take advantage of this engine by installing it and setting the property "UseESpeak" on the Speaker-class to true.

#### Windows
BogaNet.TTS uses the SAPI-voices, visible by running the following command: %windir%\sysWOW64\speech\SpeechUX\SAPI.cpl

### Main classes and example code
* [Speaker](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_t_t_s_1_1_speaker.html): Main component for TTS-operations.
* [Voice](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_t_t_s_1_1_model_1_1_voice.html): Model for a voice.

```csharp
Speaker.Instance.Speak("Hello dear user, how are you?"); //talk with the system default voice

var voice = Speaker.Instance.VoiceForCulture("de"); //talk in German
Speaker.Instance.SpeakAsync("Hallo lieber Benutzer, wie geht es dir?", voice);
```

### Nuget:
[BogaNet.TTS](https://www.nuget.org/packages/BogaNet.TTS/)

## BogaNet.Unit
Various units, like area, bit, byte, length, temperature, volume and weight with easy conversion between different types.

### Units and example code
```csharp
decimal yard2 = AreaUnit.M2.Convert(AreaUnit.YARD2, 12); //Meter² to yards²
decimal kbit = BitUnit.BIT.Convert(BitUnit.kbit, 1200); //Bit to kbit
decimal kB = ByteUnit.BYTE.Convert(ByteUnit.kB, 1976); //Byte to kB
decimal meter = LengthUnit.YARD.Convert(LengthUnit.M, 9); //Yard to meter
decimal kelvin = TemperatureUnit.FAHRENHEIT.Convert(TemperatureUnit.KELVIN, 7800); //Fahrenheit to Kelvin
decimal pint = VolumeUnit.LITER.Convert(VolumeUnit.PINT_US, 5); //Liter to pint
decimal pound = WeightUnit.GRAM.Convert(WeightUnit.POUND, 150); //Gram to pound
```

### Nuget:
[BogaNet.Unit](https://www.nuget.org/packages/BogaNet.Unit/)
