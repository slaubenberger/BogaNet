# BogaNet.i18n
Localizer/translator for C# applications.

## Main class and example code
* [Localizer](https://www.crosstales.com/media/data/BogaNet/api/class_localizer.html): i18n localizer.

```csharp
Localizer.Instance.LoadFiles("./Resources/Translation.csv", "./Resources/Translation_de.csv"); //load the translation files
Localizer.Instance.Culture = new CultureInfo("en"); //set the culture to English
Console.WriteLine(Localizer.Instance.GetText("GreetingText"));
```

## Nuget:
[BogaNet.i18n](https://www.nuget.org/packages/BogaNet.i18n/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)