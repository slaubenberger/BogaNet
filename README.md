# BogaNet
.NET 8 version of Boga, a collection of tools to make C# development a joyride.


## BogaNet.Avalonia
A library with many little helpers to speed up Avalonia development.

### Main classes
* [ImageHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_avalonia_1_1_helper_1_1_image_helper.html): Helper for images in Avalonia.
* [ResourceHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_avalonia_1_1_helper_1_1_resource_helper.html): Helper for resources in Avalonia.

### Nuget:
[BogaNet.Avalonia](https://www.nuget.org/packages/BogaNet.Avalonia/)

# BogaNet.Common
A library with many little helpers to speed up C# development.

## Main classes and functions
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

## Nuget:
[BogaNet.Common](https://www.nuget.org/packages/BogaNet.Common/)


## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)


## Nuget-packages:
* [BogaNet.Crypto](https://www.nuget.org/packages/BogaNet.Crypto/): various helpers for cryptographic functions, like hashing (SHA), asymmetric (RSA) and symmetric (AES) encryption/decryption, HMAC and CRC.
* [BogaNet.i18n](https://www.nuget.org/packages/BogaNet.i18n/): localizer for C# applications.
* [BogaNet.ObfuscatedType](https://www.nuget.org/packages/BogaNet.ObfuscatedType/): various obfuscated types for all value types and strings. This types prevent the values from being "plain" in memory and offers some protection against bad actors (like memory scanners and searchers).
* [BogaNet.SecureType](https://www.nuget.org/packages/BogaNet.SecureType/): various encrypted types for all value types and strings. This types prevent the values from being "plain" in memory and offers high protection against bad actors (like memory scanners and searchers).
* [BogaNet.Unit](https://www.nuget.org/packages/BogaNet.Unit/): Various units, like area, bit, byte, length, temperature, volume and weight with easy conversion between different types.