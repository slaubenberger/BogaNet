using System.Collections.Generic;
using System.Threading.Tasks;
using BogaNet.TTS.Model;
using System.Linq;
using System;
using BogaNet.Extension;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices.JavaScript;
using BogaNet.TTS.Model.Enum;

namespace BogaNet.TTS.Provider;

/// <summary>
/// Provider for TTS in Avalonia (browser).
/// </summary>
public partial class BrowserVoiceProvider : IVoiceProvider
{
   #region Variables

   private static readonly ILogger<BrowserVoiceProvider> _logger = GlobalLogging.CreateLogger<BrowserVoiceProvider>();

   private static readonly char[] splitChar = [';'];

   private List<Voice>? _cachedVoices;
   private readonly List<string> _cachedCultures = [];

   #endregion

   #region Properties

   public virtual List<Voice> Voices => _cachedVoices ??= GetVoices();
   public virtual int MaxTextLength => 32000;
   public virtual bool IsPlatformSupported => Constants.IsBrowser;
   public virtual bool IsSSMLSupported => true;

   public virtual List<string> Cultures
   {
      get
      {
         if (_cachedCultures.Count != 0) 
            return _cachedCultures;
         
         IEnumerable<Voice> cultures = Voices.GroupBy(cul => cul.Culture)
            .Select(grp => grp.First()).OrderBy(s => s.Culture).ToList();

         foreach (Voice voice in cultures)
         {
            _cachedCultures.Add(voice.Culture);
         }

         return _cachedCultures;
      }
   }

   public virtual bool IsReady { get; private set; }
   public virtual bool IsSpeaking { get; private set; }

   #endregion

   #region Events

   public event IVoiceProvider.VoicesLoaded? OnVoicesLoaded;
   public event IVoiceProvider.SpeakStarted? OnSpeakStarted;
   public event IVoiceProvider.SpeakCompleted? OnSpeakCompleted;

   #endregion

   #region Constructor

   public BrowserVoiceProvider()
   {
      if (!Constants.IsBrowser)
         _logger.LogError("BrowserVoiceProvider works only in a browser!");
   }

   #endregion

   #region Implemented methods

   public virtual List<Voice> GetVoices()
   {
      List<Voice> res = getVoices() ?? [];

      IsReady = res.Count > 0;
      OnVoicesLoaded?.Invoke(res);

      return res;
   }

   public virtual Task<List<Voice>> GetVoicesAsync()
   {
      return Task.FromResult(GetVoices());
   }

   public virtual void Silence()
   {
      JSSilence();
   }

   public virtual bool Speak(string text, Voice? voice = null, float rate = 1, float pitch = 1, float volume = 1, bool forceSSML = true, bool useThreads = false)
   {
      _logger.LogInformation("Speak called.");

      OnSpeakStarted?.Invoke(text);
      IsSpeaking = true;

      JSSpeak(text, voice == null ? string.Empty : voice.Identifier, rate, pitch, volume);

      OnSpeakCompleted?.Invoke(text);
      IsSpeaking = false;

      return true;
   }

   public virtual Task<bool> SpeakAsync(string text, Voice? voice = null, float rate = 1, float pitch = 1, float volume = 1, bool forceSSML = true)
   {
      return Task.FromResult(Speak(text, voice, rate, pitch, volume, forceSSML));
   }

   #endregion

   #region Private methods

   private List<Voice>? getVoices()
   {
      List<Voice> voices = [];
      List<string> jsVoices = JSGetVoices().ToList();

      foreach (var voice in jsVoices)
      {
         //_logger.LogInformation("Voice: " + voice);

         string[] splittedString = voice.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

         if (splittedString.Length == 2)
         {
            voices.Add(new Voice(splittedString[0], "", Gender.UNKNOWN, "unknown", splittedString[1], splittedString[0]));
         }
         else
         {
            _logger.LogWarning($"Voice is invalid: {voice}");
         }
      }

      _cachedVoices = voices.OrderBy(s => s.Name).ToList();

      _logger.LogDebug($"Voices read: {_cachedVoices.BNDump()}");

      return _cachedVoices;
   }

   #endregion

   #region JavaScript methods

   [JSImport("getVoices", "boganet_tts")]
   internal static partial string[] JSGetVoices();

   [JSImport("speak", "boganet_tts")]
   internal static partial void JSSpeak(string text, string voice, float rate, float pitch, float volume);

   [JSImport("silence", "boganet_tts")]
   internal static partial void JSSilence();

   #endregion
}