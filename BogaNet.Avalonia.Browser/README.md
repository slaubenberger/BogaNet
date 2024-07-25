# BogaNet.Avalonia.Browser
Browser-specific helpers for Avalonia development.
__This package is in experimental stage!__

## Main classes and usage
* [WebPreferencesContainer](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_prefs_1_1_web_preferences_container.html): Preferences-container for web, for more see: [BogaNet.Prefs](https://www.nuget.org/packages/BogaNet.Prefs/).

Add the following line to "Program.cs" in the Browser-project:
```csharp
public static AppBuilder BuildAvaloniaApp()
{
    Preferences.Instance.Container = new WebPreferencesContainer();
    
    return AppBuilder.Configure<App>();
}
```

To make it work in web-builds, please add the following line at the end of "main.js" under "wwwroot":
```
export const exports = await dotnetRuntime.getAssemblyExports(config.mainAssemblyName);
```

Unfortunately, you have to manually copy the "boganet_bridge.js" to "wwwroot" since I don't know how to include it in the Nuget-package correctly... Tips are welcome! :-)
[boganet_bridge.js](https://raw.githubusercontent.com/slaubenberger/BogaNet/develop/BogaNet.Avalonia.Browser/wwwroot/boganet_bridge.js)

## Nuget:
[BogaNet.Avalonia.Browser](https://www.nuget.org/packages/BogaNet.Avalonia.Browser/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)