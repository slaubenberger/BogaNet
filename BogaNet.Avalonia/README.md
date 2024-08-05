# BogaNet.Avalonia
Little helpers for Avalonia development.

## Main classes
* [ImageHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_image_helper.html): Helper for images in Avalonia.
* [ResourceHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_resource_helper.html): Helper for resources in Avalonia.

### i18n
To load translations in Avalonia, move them to the "Assets"-folder. Then use them like this:
```csharp
ResourceHelper.ResourceAssembly = "YourApp"; //set your app ID here
 
Localizer.Instance.LoadResources("Assets/Translation.csv", "Assets/Translation_de.csv"); //load the translation files
Localizer.Instance.Culture = new CultureInfo("en"); //set the culture to English
Console.WriteLine(Localizer.Instance.GetText("Greeting"));
```

### BadWordFilter
To load BWF sources in Avalonia, move them to the "Assets/Filters"-folder. Then use them like this:
```csharp
ResourceHelper.ResourceAssembly = "YourApp"; //set your app ID here
 
BadWordFilter.Instance.LoadResources(true, BWFAvaloniaConstants.BWF_AV_EN, BWFAvaloniaConstants.BWF_AV_DE); //load English and German

// load all domains to detect urls, emails etc.
DomainFilter.Instance.LoadResources(BWFAvaloniaConstants.DOMAINS_AV);

string foulText = "MARTIANS are assholes/arschlöcher!!!!!!!!!!  => WATCH: https//mytruthpage.com/weirdowatch/martians123.divx or WRITE an EMAIL: weirdo@gmail.com";

// replace all bad words, domains or excessive capitalizations/punctuations
string removedProfanity = Pacifier.Instance.ReplaceAll(foulText);
Console.WriteLine(removedProfanity);
```

## Nuget:
[BogaNet.Avalonia](https://www.nuget.org/packages/BogaNet.Avalonia/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)
