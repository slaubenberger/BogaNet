# BogaNet.TTS
Use the built-in Text-To-Speech (TTS) engine under Windows, OSX and Linux.
It provides all installed voices and support [SSML](https://www.w3.org/TR/speech-synthesis/).

## Note
The LinuxVoiceProvider uses eSpeak/eSpeak-NG as engine, which is also available for Windows and OSX.
Therefore it's possible to take advantage of this engine by installing it and setting the property "UseESpeak" on the Speaker-class to true.

### Windows
BogaNet.TTS uses the SAPI-voices, visible by running the following command: %windir%\sysWOW64\speech\SpeechUX\SAPI.cpl

## Main classes and example code
* [Speaker](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_t_t_s_1_1_speaker.html): Main component for TTS-operations.
* [Voice](https://www.crosstales.com/media/data/BogaNet/api/class_boga_net_1_1_t_t_s_1_1_model_1_1_voice.html): Model for a voice.

```csharp
Speaker.Instance.Speak("Hello dear user, how are you?"); //Talk with the system default voice

var voice = Speaker.Instance.VoiceForCulture("de"); //talk in German
Speaker.Instance.SpeakAsync("Hallo lieber Benutzer, wie geht es dir?", voice);
```

## Nuget:
[BogaNet.TTS](https://www.nuget.org/packages/BogaNet.TTS/)

## API:
[https://www.crosstales.com/media/data/BogaNet/api/](https://www.crosstales.com/media/data/BogaNet/api/annotated.html)

## GitHub:
[https://github.com/slaubenberger/BogaNet/](https://github.com/slaubenberger/BogaNet/)