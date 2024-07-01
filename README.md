# BogaNet
.NET 8 version of Boga, a collection of tools to make C# development a joyride.

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## BogaNet.Avalonia
Little helpers to speed up Avalonia development.

### Main classes
* [ImageHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_avalonia_1_1_helper_1_1_image_helper.html): Helper for images in Avalonia.
* [ResourceHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_avalonia_1_1_helper_1_1_resource_helper.html): Helper for resources in Avalonia.

### Nuget:
[BogaNet.Avalonia](https://www.nuget.org/packages/BogaNet.Avalonia/)

## BogaNet.Common
Main library for all BogaNet-packages filled with useful helpers to speed up C# development.

### Main classes
* [Base16](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_encoder_1_1_base16.html): Base16 (aka Hex) encoder.
* [Base32](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_encoder_1_1_base32.html): Base32 encoder.
* [Base64](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_encoder_1_1_base64.html): Base64 encoder.
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

## BogaNet.Crypto
Various helpers for cryptographic functions, like hashing (SHA), asymmetric (AES) and symmetric (RSA) encryption/decryption, and HMAC.

### Main classes
* [AESHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_a_e_s_helper.html): Helper for AES cryptography.
* [HashHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_hash_helper.html): Helper for hash computations. It contains ready-to-use Implementations of SHA256, SHA384 and SHA512.
* [HMACHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_h_m_a_c_helper.html): Helper for HMAC cryptography. It contains ready-to-use Implementations of HMAC256, HMAC384 and HMAC512.
* [RSAHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_r_s_a_helper.html): Helper for RSA cryptography and X509 certificates.

### Nuget:
[BogaNet.Crypto](https://www.nuget.org/packages/BogaNet.Crypto/)

## BogaNet.i18n
Localizer for C# applications.

### Main class and example code
* [Localizer](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1i18n_1_1_localizer.html): i18n localizer.

```csharp
Localizer.Instance.LoadFiles("./Resources/Translation.csv", "./Resources/Translation_de.csv"); //load the translation files
Localizer.Instance.Culture = new CultureInfo("en"); //set the culture to English
Console.WriteLine(Localizer.Instance.GetText("GreetingText"));
```

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
* all objects (currently only as storage container)

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
* all objects (currently only as storage container)

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
