# BogaNet.TTS
This library is a new implementation for .NET8 of the highly-regarded Unity package [RT-Voice PRO](https://assetstore.unity.com/packages/slug/41068?aid=1011lNGT) and uses the built-in Text-To-Speech (TTS) engine under Windows, OSX and Linux.
It provides all installed voices and support for [SSML](https://www.w3.org/TR/speech-synthesis/).
For an implementation for the web browser, please check the package [BogaNet.Avalonia.Browser](https://www.nuget.org/packages/BogaNet.Avalonia.Browser/).
It's also possible to implement your own custom voice provider (e.g. to use other engines like AWS Polly, Azure, Google, ElvenLabs etc.).

## Note
The LinuxVoiceProvider uses eSpeak/eSpeak-NG as engine, which is also available for Windows and OSX.
Therefore it's possible to take advantage of this engine by installing it and setting the property "UseESpeak" on the Speaker-class to true.

### Windows
BogaNet.TTS uses the SAPI-voices, visible by running the following command: %windir%\sysWOW64\speech\SpeechUX\SAPI.cpl

## Main classes and example code
* [Speaker](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_t_t_s_1_1_speaker.html): Main component for TTS-operations.
* [Voice](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_t_t_s_1_1_model_1_1_voice.html): Model for a voice.

```csharp
Speaker.Instance.Speak("Hello dear user, how are you?"); //talk with the system default voice

var voice = Speaker.Instance.VoiceForCulture("de"); //talk in German
Speaker.Instance.SpeakAsync("Hallo lieber Benutzer, wie geht es dir?", voice);
```

## Nuget:
[BogaNet.TTS](https://www.nuget.org/packages/BogaNet.TTS/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)