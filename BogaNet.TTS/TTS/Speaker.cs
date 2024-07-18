#if true
using System.Linq;
using BogaNet.TTS.Model.Enum;
using BogaNet.TTS.Provider;
using System.Collections.Generic;
using BogaNet.TTS.Model;
using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using BogaNet.Util;
using System.Threading.Tasks;
using BogaNet.Extension;

namespace BogaNet.TTS;

/// <summary>Main component of RT-Voice.</summary>
public class Speaker : Singleton<Speaker>, IVoiceProvider
{
   #region Variables

   private static readonly ILogger<Speaker> _logger = GlobalLogging.CreateLogger<Speaker>();

   private bool _useUseESpeak;

   private IVoiceProvider? voiceProvider;

   private static readonly char[] splitCharWords = [' '];

   #endregion

   #region Properties

   /// <summary>Enable or disable eSpeak for standalone platforms.</summary>
   public bool UseESpeak
   {
      get => _useUseESpeak;
      set
      {
         if (_useUseESpeak == value) return;

         _useUseESpeak = value;

         initProvider();
      }
   }

   /// <summary>Automatically clear tags from speeches depending on the capabilities of the current TTS-system.</summary>
   public bool AutoClearTags { get; set; }

   /// <summary>Checks if TTS is available on this system.</summary>
   /// <returns>True if TTS is available on this system.</returns>
   public bool IsTTSAvailable => voiceProvider != null && voiceProvider.Voices.Count > 0;

   #region Provider delegates

   //public string DefaultVoiceName => voiceProvider != null ? voiceProvider.DefaultVoiceName : string.Empty;
   public List<Voice> Voices => voiceProvider != null ? voiceProvider.Voices : [];
   public int MaxTextLength => voiceProvider?.MaxTextLength ?? 3999; //minimum (Android)
   public bool IsPlatformSupported => voiceProvider?.IsPlatformSupported == true;
   public bool IsSSMLSupported => voiceProvider != null && voiceProvider.IsSSMLSupported;
   public List<string> Cultures => voiceProvider != null ? voiceProvider.Cultures : new List<string>();

   /// <summary>eSpeak application name/path.</summary>
   public string ESpeakApplication
   {
      get => LinuxVoiceProvider.Instance.ESpeakApplication;
      set => LinuxVoiceProvider.Instance.ESpeakApplication = value;
   }

   /// <summary>eSpeak application data path.</summary>
   public string ESpeakDataPath
   {
      get => LinuxVoiceProvider.Instance.ESpeakDataPath;
      set => LinuxVoiceProvider.Instance.ESpeakDataPath = value;
   }

   /// <summary>Active modifier for all eSpeak voices.</summary>
   public ESpeakModifiers ESpeakModifier
   {
      get => LinuxVoiceProvider.Instance.ESpeakModifier;
      set => LinuxVoiceProvider.Instance.ESpeakModifier = value;
   }

   /// <summary>Female modifier for female eSpeak voices.</summary>
   public ESpeakModifiers ESpeakFemaleModifier
   {
      get => LinuxVoiceProvider.Instance.ESpeakFemaleModifier;
      set => LinuxVoiceProvider.Instance.ESpeakFemaleModifier = value;
   }

   #endregion

   #endregion

   #region Constructor

   private Speaker()
   {
      initProvider();
   }

   #endregion

   #region Public methods

   #region Voices

   /// <summary>
   /// Approximates the speech length in seconds of a given text and rate.
   /// NOTE: This method does not provide an exact value; +/- 15% is "normal"!
   /// </summary>
   /// <param name="text">Text for the length approximation.</param>
   /// <param name="rate">Speech rate of the speaker in percent for the length approximation (1 = 100%, default: 1, optional).</param>
   /// <param name="wordsPerMinute">Words per minute (default: 175, optional).</param>
   /// <param name="timeFactor">Time factor for the calculated value (default: 0.9, optional).</param>
   /// <returns>Approximated speech length in seconds of the given text and rate.</returns>
   public float ApproximateSpeechLength(string text, float rate = 1f, float wordsPerMinute = 175f, float timeFactor = 0.9f)
   {
      float words = text.Split(splitCharWords, System.StringSplitOptions.RemoveEmptyEntries).Length;
      float characters = text.Length - words + 1;
      float ratio = characters / words;

      if (BogaNet.Constants.IsWindows && !UseESpeak)
      {
         if (Math.Abs(rate - 1f) > BogaNet.Constants.FLOAT_TOLERANCE)
         {
            //relevant?
            if (rate > 1f)
            {
               //larger than 1
               if (rate >= 2.75f)
               {
                  rate = 2.78f;
               }
               else if (rate is >= 2.6f and < 2.75f)
               {
                  rate = 2.6f;
               }
               else if (rate is >= 2.35f and < 2.6f)
               {
                  rate = 2.39f;
               }
               else if (rate is >= 2.2f and < 2.35f)
               {
                  rate = 2.2f;
               }
               else if (rate is >= 2f and < 2.2f)
               {
                  rate = 2f;
               }
               else if (rate is >= 1.8f and < 2f)
               {
                  rate = 1.8f;
               }
               else if (rate is >= 1.6f and < 1.8f)
               {
                  rate = 1.6f;
               }
               else if (rate is >= 1.4f and < 1.6f)
               {
                  rate = 1.45f;
               }
               else if (rate is >= 1.2f and < 1.4f)
               {
                  rate = 1.28f;
               }
               else if (rate is > 1f and < 1.2f)
               {
                  rate = 1.14f;
               }
            }
            else
            {
               //smaller than 1
               if (rate <= 0.3f)
               {
                  rate = 0.33f;
               }
               else if (rate is > 0.3f and <= 0.4f)
               {
                  rate = 0.375f;
               }
               else if (rate is > 0.4f and <= 0.45f)
               {
                  rate = 0.42f;
               }
               else if (rate is > 0.45f and <= 0.5f)
               {
                  rate = 0.47f;
               }
               else if (rate is > 0.5f and <= 0.55f)
               {
                  rate = 0.525f;
               }
               else if (rate is > 0.55f and <= 0.6f)
               {
                  rate = 0.585f;
               }
               else if (rate is > 0.6f and <= 0.7f)
               {
                  rate = 0.655f;
               }
               else if (rate is > 0.7f and <= 0.8f)
               {
                  rate = 0.732f;
               }
               else if (rate is > 0.8f and <= 0.9f)
               {
                  rate = 0.82f;
               }
               else if (rate is > 0.9f and < 1f)
               {
                  rate = 0.92f;
               }
            }
         }
      }

      float speechLength = words / (wordsPerMinute / 60 * rate);

      if (ratio < 2)
      {
         speechLength *= 1f;
      }
      else if (ratio is >= 2f and < 3f)
      {
         speechLength *= 1.05f;
      }
      else if (ratio is >= 3f and < 3.5f)
      {
         speechLength *= 1.15f;
      }
      else if (ratio is >= 3.5f and < 4f)
      {
         speechLength *= 1.2f;
      }
      else if (ratio is >= 4f and < 4.5f)
      {
         speechLength *= 1.25f;
      }
      else if (ratio is >= 4.5f and < 5f)
      {
         speechLength *= 1.3f;
      }
      else if (ratio is >= 5f and < 5.5f)
      {
         speechLength *= 1.4f;
      }
      else if (ratio is >= 5.5f and < 6f)
      {
         speechLength *= 1.45f;
      }
      else if (ratio is >= 6f and < 6.5f)
      {
         speechLength *= 1.5f;
      }
      else if (ratio is >= 6.5f and < 7f)
      {
         speechLength *= 1.6f;
      }
      else if (ratio is >= 7f and < 8f)
      {
         speechLength *= 1.7f;
      }
      else if (ratio is >= 8f and < 9f)
      {
         speechLength *= 1.8f;
      }
      else
      {
         speechLength *= ratio * (ratio / 100f + 0.02f) + 1f;
      }

      if (speechLength < 0.8f)
         speechLength += 0.6f;

      return speechLength * timeFactor;
   }

   /// <summary>Is a voice available for a given gender and optional culture from the current TTS-system?</summary>
   /// <param name="gender">Gender of the voice</param>
   /// <param name="culture">Culture of the voice (e.g. "en", optional)</param>
   /// <returns>True if a voice is available for a given gender and culture.</returns>
   public bool IsVoiceForGenderAvailable(Gender gender, string culture = "")
   {
      return VoicesForGender(gender, culture).Count > 0;
   }

   /// <summary>Is a voice available for a given gender and language  from the current TTS-system?</summary>
   /// <param name="gender">Gender of the voice</param>
   /// <param name="culture">Culture of the voice</param>
   /// <returns>True if a voice is available for a given gender and language.</returns>
   public bool IsVoiceForGenderAvailable(Gender gender, CultureInfo culture)
   {
      return IsVoiceForGenderAvailable(gender, culture.TwoLetterISOLanguageName);
   }

   /// <summary>Get all available voices for a given gender and optional culture from the current TTS-system.</summary>
   /// <param name="gender">Gender of the voice</param>
   /// <param name="culture">Culture of the voice (e.g. "en", optional)</param>
   /// <param name="isFuzzy">Always returns voices if there is no match with the gender and/or culture (default: false, optional)</param>
   /// <returns>All available voices (alphabetically ordered by 'Name') for a given gender and culture as a list.</returns>
   public List<Voice> VoicesForGender(Gender gender, string culture = "", bool isFuzzy = false)
   {
      List<Voice> voices = new List<Voice>(Voices.Count);

      if (string.IsNullOrEmpty(culture))
      {
         if (Gender.UNKNOWN == gender)
            return Voices;

         voices.AddRange(Voices.Where(voice => voice.Gender == gender));
      }
      else
      {
         if (Gender.UNKNOWN == gender)
            return VoicesForCulture(culture, isFuzzy);

         voices.AddRange(VoicesForCulture(culture, isFuzzy).Where(voice => voice.Gender == gender));

         if (voices.Count == 0)
            return VoicesForCulture(culture, isFuzzy);
      }

      return voices;
   }

   /// <summary>Get all available voices for a given gender and language from the current TTS-system.</summary>
   /// <param name="gender">Gender of the voice</param>
   /// <param name="culture">Culture of the voice</param>
   /// <param name="isFuzzy">Always returns voices if there is no match with the gender and/or language (default: false, optional)</param>
   /// <returns>All available voices (alphabetically ordered by 'Name') for a given gender and language as a list.</returns>
   public List<Voice> VoicesForGender(Gender gender, CultureInfo culture, bool isFuzzy = false)
   {
      return VoicesForGender(gender, culture.TwoLetterISOLanguageName, isFuzzy);
   }

   /// <summary>Get a voice from for a given gender, optional culture and optional index from the current TTS-system.</summary>
   /// <param name="gender">Gender of the voice</param>
   /// <param name="culture">Culture of the voice (e.g. "en", optional)</param>
   /// <param name="index">Index of the voice (default: 0, optional)</param>
   /// <param name="fallbackCulture">Fallback culture of the voice (default "en", optional)</param>
   /// <param name="isFuzzy">Always returns voices if there is no match with the gender and/or culture (default: false, optional)</param>
   /// <returns>Voice for the given gender, culture and index.</returns>
   public Voice? VoiceForGender(Gender gender, string culture = "", int index = 0, string fallbackCulture = "en", bool isFuzzy = false)
   {
      Voice? result = null;

      List<Voice> voices = VoicesForGender(gender, culture, isFuzzy);

      if (voices.Count > 0)
      {
         if (voices.Count - 1 >= index && index >= 0)
         {
            result = voices[index];
         }
         else
         {
            //use the default voice
            //result = voices[0];
            _logger.LogWarning($"No voice for gender '{gender}' and culture '{culture}' with index {index} found! Speaking with the default voice!");
         }
      }
      else
      {
         voices = VoicesForGender(gender, fallbackCulture, isFuzzy);

         if (voices.Count > 0)
         {
            result = voices[0];
            _logger.LogWarning($"No voice for gender '{gender}' and culture '{culture}' found! Speaking with the fallback culture: '{fallbackCulture}'");
         }
         else
         {
            //use the default voice
            _logger.LogWarning($"No voice for gender '{gender}' and culture '{culture}' found! Speaking with the default voice!");
         }
      }

      return result;
   }

   /// <summary>Get a voice from for a given gender, language and index from the current TTS-system.</summary>
   /// <param name="gender">Gender of the voice</param>
   /// <param name="culture">Culture of the voice</param>
   /// <param name="index">Index of the voice (default: 0, optional)</param>
   /// <param name="isFuzzy">Always returns voices if there is no match with the gender and/or language (default: false, optional)</param>
   /// <returns>Voice for the given gender, language and index.</returns>
   public Voice? VoiceForGender(Gender gender, CultureInfo culture, int index = 0, bool isFuzzy = false)
   {
      return VoiceForGender(gender, culture.TwoLetterISOLanguageName, index, "en", isFuzzy);
   }

   /// <summary>Is a voice available for a given culture from the current TTS-system?</summary>
   /// <param name="culture">Culture of the voice (e.g. "en")</param>
   /// <returns>True if a voice is available for a given culture.</returns>
   public bool IsVoiceForCultureAvailable(string culture)
   {
      return VoicesForCulture(culture).Count > 0;
   }

   /// <summary>Is a voice available for a given language from the current TTS-system?</summary>
   /// <param name="culture">Culture of the voice</param>
   /// <returns>True if a voice is available for a given language.</returns>
   public bool IsVoiceForLanguageAvailable(CultureInfo culture)
   {
      return IsVoiceForCultureAvailable(culture.TwoLetterISOLanguageName);
   }

   /// <summary>Get all available voices for a given culture from the current TTS-system.</summary>
   /// <param name="culture">Culture of the voice (e.g. "en")</param>
   /// <param name="isFuzzy">Always returns voices if there is no match with the culture (default: false, optional)</param>
   /// <returns>All available voices (alphabetically ordered by 'Name') for a given culture as a list.</returns>
   public List<Voice> VoicesForCulture(string culture, bool isFuzzy = false)
   {
      if (string.IsNullOrEmpty(culture))
      {
         _logger.LogWarning("The given 'culture' is null or empty! Returning all available voices.");

         return Voices;
      }

      string _culture = culture.Trim().Replace(" ", string.Empty).Replace("_", string.Empty).Replace("-", string.Empty);
#if WSA
//TODO remove?
         List<Voice> voices = Voices.Where(s => s.SimplifiedCulture.StartsWith(_culture, System.StringComparison.OrdinalIgnoreCase)).OrderBy(s => s.Name).ToList();
#else
      List<Voice> voices = Voices.Where(s => s.SimplifiedCulture.StartsWith(_culture, System.StringComparison.InvariantCultureIgnoreCase)).OrderBy(s => s.Name).ToList();
#endif
      if (voices.Count == 0 && isFuzzy)
      {
         return Voices;
      }

      return voices;
   }

   /// <summary>Get all available voices for a given language from the current TTS-system.</summary>
   /// <param name="culture">Culture of the voice</param>
   /// <param name="isFuzzy">Always returns voices if there is no match with the language (default: false, optional)</param>
   /// <returns>All available voices (alphabetically ordered by 'Name') for a given language as a list.</returns>
   public List<Voice> VoicesForLanguage(CultureInfo culture, bool isFuzzy = false)
   {
      return VoicesForCulture(culture.TwoLetterISOLanguageName, isFuzzy);
   }

   /// <summary>Get a voice from for a given culture and optional index from the current TTS-system.</summary>
   /// <param name="culture">Culture of the voice (e.g. "en")</param>
   /// <param name="index">Index of the voice (default: 0, optional)</param>
   /// <param name="fallbackCulture">Fallback culture of the voice (default "en", optional)</param>
   /// <param name="isFuzzy">Always returns voices if there is no match with the culture (default: false, optional)</param>
   /// <returns>Voice for the given culture and index.</returns>
   public Voice? VoiceForCulture(string culture, int index = 0, string fallbackCulture = "en", bool isFuzzy = false)
   {
      Voice? result = null;

      if (!string.IsNullOrEmpty(culture))
      {
         List<Voice> voices = VoicesForCulture(culture, isFuzzy);

         if (voices.Count > 0)
         {
            if (voices.Count - 1 >= index && index >= 0)
            {
               result = voices[index];
            }
            else
            {
               //use the default voice
               //result = voices[0];
               _logger.LogWarning($"No voices for culture '{culture}' with index {index} found! Speaking with the default voice!");
            }
         }
         else
         {
            voices = VoicesForCulture(fallbackCulture, isFuzzy);

            if (voices.Count > 0)
            {
               result = voices[0];
               _logger.LogWarning($"No voices for culture '{culture}' found! Speaking with the fallback culture: '{fallbackCulture}'");
            }
            else
            {
               //use the default voice
               _logger.LogWarning($"No voices for culture '{culture}' found! Speaking with the default voice!");
            }
         }
      }

      return result;
   }

   /// <summary>Get a voice from for a given language and optional index from the current TTS-system.</summary>
   /// <param name="culture">Culture of the voice</param>
   /// <param name="index">Index of the voice (default: 0, optional)</param>
   /// <param name="isFuzzy">Always returns voices if there is no match with the language (default: false, optional)</param>
   /// <returns>Voice for the given language and index.</returns>
   public Voice? VoiceForLanguage(CultureInfo culture, int index = 0, bool isFuzzy = false)
   {
      return VoiceForCulture(culture.TwoLetterISOLanguageName, index, "en", isFuzzy);
   }

   /// <summary>Is a voice available for a given name from the current TTS-system?</summary>
   /// <param name="_name">Name of the voice (e.g. "Alex")</param>
   /// <param name="isExact">Exact match for the voice name (default: false, optional)</param>
   /// <returns>True if a voice is available for a given name.</returns>
   public bool IsVoiceForNameAvailable(string _name, bool isExact = false)
   {
      return VoiceForName(_name, isExact) != null;
   }

   /// <summary>Get a voice for a given name from the current TTS-system.</summary>
   /// <param name="_name">Name of the voice (e.g. "Alex")</param>
   /// <param name="isExact">Exact match for the voice name (default: false, optional)</param>
   /// <returns>Voice for the given name or null if not found.</returns>
   public Voice? VoiceForName(string _name, bool isExact = false)
   {
      Voice? result = null;

      if (string.IsNullOrEmpty(_name))
      {
         _logger.LogWarning("The given 'name' is null or empty! Returning null.");
      }
      else
      {
         result = isExact ? Voices.FirstOrDefault(voice => voice.Name.BNEquals(_name)) : Voices.FirstOrDefault(voice => voice.Name.BNContains(_name));

         if (result == null)
         {
            //use the default voice
            _logger.LogWarning("No voice for name '" + _name + "' found! Speaking with the default voice!");
         }
      }

      return result;
   }

   #endregion

   #region SpeakNative

   public List<Voice> GetVoices()
   {
      return Task.Run(GetVoicesAsync).GetAwaiter().GetResult();
   }

   public async Task<List<Voice>> GetVoicesAsync()
   {
      _logger.LogDebug($"GetVoices called");

      if (voiceProvider != null)
         return await voiceProvider.GetVoicesAsync();

      logPlatformNotSupported();

      return [];
   }

   public void Silence()
   {
      _logger.LogDebug("Silence called");

      if (voiceProvider != null)
      {
         voiceProvider.Silence();
      }
      else
      {
         logPlatformNotSupported();
      }
   }

   public bool Speak(string text, Voice? voice = null, float rate = 1, float pitch = 1, float volume = 1, bool forceSSML = true)
   {
      return Task.Run(() => SpeakAsync(text, voice, rate, pitch, volume, forceSSML)).GetAwaiter().GetResult();
   }

   public async Task<bool> SpeakAsync(string text, Voice? voice = null, float rate = 1, float pitch = 1, float volume = 1, bool forceSSML = true)
   {
      _logger.LogDebug($"Speak called: {text} - Voice: {voice} - Rate: {rate} - Pitch: {pitch} - Volume: {volume} - ForceSSML: {forceSSML}");

      if (voiceProvider != null)
         return await voiceProvider.SpeakAsync(text, voice, rate, pitch, volume, forceSSML);

      logPlatformNotSupported();

      return false;
   }

   #endregion

   #endregion


   #region Private methods

   private void logPlatformNotSupported()
   {
      _logger.LogWarning("The current platform is not supported!");
   }

   private void initProvider()
   {
      if (UseESpeak || Constants.IsLinux)
      {
         voiceProvider = LinuxVoiceProvider.Instance;
      }
      else if (Constants.IsWindows)
      {
         voiceProvider = WindowsVoiceProvider.Instance;
      }
      else if (Constants.IsOSX)
      {
         voiceProvider = OSXVoiceProvider.Instance;
      }
      else
      {
         _logger.LogError("No valid TTS provider found for current platform!");
      }
   }

   #endregion
}
#endif