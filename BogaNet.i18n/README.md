# BogaNet.i18n
Localizer/translator for C# applications.

## Main class and example code
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

## Nuget:
[BogaNet.i18n](https://www.nuget.org/packages/BogaNet.i18n/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)