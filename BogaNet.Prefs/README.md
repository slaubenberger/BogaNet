# BogaNet.Prefs
Handles preferences/settings for C# applications. It supports all values types, strings, DateTime and object.
Furthermore, it allows to store the data in obfuscated form to prevent it from being easily read/modified.
The data is automatically stored at application exit.

## Main class and example code
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

## Nuget:
[BogaNet.Prefs](https://www.nuget.org/packages/BogaNet.Prefs/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)