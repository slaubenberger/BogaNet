# BogaNet.Avalonia.Browser
Browser-specific helpers for Avalonia development.
It also contains various JavaScript-helper files.
This package most likely works with other C# web-technologies like Blazor and Razor, but its untested outside of Avalonia.

## Main classes and usage
* [BrowserPreferencesContainer](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_prefs_1_1_browser_preferences_container.html): Preferences-container for browser, for more see: [BogaNet.Prefs](https://www.nuget.org/packages/BogaNet.Prefs/).
* [BrowserVoiceProvider](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_t_t_s_1_1_provider_1_1_browser_voice_provider.html): TTS for browser, for more see: [BogaNet.TTS](https://www.nuget.org/packages/BogaNet.TTS/).
* [UrlHelper](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_helper_1_1_url_helper.html): Get and set the URL of the application.

JavaScript-files:
* [boganet_exit.js](https://raw.githubusercontent.com/slaubenberger/BogaNet/develop/BogaNet.Avalonia.Browser/wwwroot/boganet_exit.js): Prevents the app from closing/reload and offers a callback to handle the case.
* [boganet_prefs.js](https://raw.githubusercontent.com/slaubenberger/BogaNet/develop/BogaNet.Avalonia.Browser/wwwroot/boganet_prefs.js): Bridge for [BrowserPreferencesContainer](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_prefs_1_1_browser_preferences_container.html).
* [boganet_tts.js](https://raw.githubusercontent.com/slaubenberger/BogaNet/develop/BogaNet.Avalonia.Browser/wwwroot/boganet_tts.js): Bridge for [BrowserVoiceProvider](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_t_t_s_1_1_provider_1_1_browser_voice_provider.html).
* [boganet_url.js](https://raw.githubusercontent.com/slaubenberger/BogaNet/develop/BogaNet.Avalonia.Browser/wwwroot/boganet_url.js): Get and set the URL of the application.

Unfortunately, you have to manually copy the desired files to "wwwroot" since I don't know how to include it in the Nuget-package correctly... Tips are welcome! :-)

### Exit
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

### Preferences
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

### TTS
To use the [BogaNet.TTS](https://www.nuget.org/packages/BogaNet.TTS/), add/modify "Program.cs" from the Avalonia Browser-project:
```csharp
private static async Task Main(string[] args)
{
    await JSHost.ImportAsync("boganet_tts", "../boganet_tts.js");
    Speaker.Instance.CustomVoiceProvider = new BrowserVoiceProvider();
    //...rest of the Main-code
}
```

### URL
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

## Nuget:
[BogaNet.Avalonia.Browser](https://www.nuget.org/packages/BogaNet.Avalonia.Browser/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)
