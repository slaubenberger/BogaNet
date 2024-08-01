using System.Linq;
using BogaNet.TTS.Model;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.Util;

namespace BogaNet.TTS.Provider;

/// <summary>
/// Base-class for all voice providers.
/// </summary>
public abstract class BaseVoiceProvider : IVoiceProvider
{
   #region Variables

   protected readonly List<ProcessRunner> _processes = [];

   protected List<Voice>? _cachedVoices;
   private readonly List<string> _cachedCultures = [];

   #endregion

   #region Properties

   public virtual List<Voice> Voices => _cachedVoices ??= GetVoices();

   public abstract int MaxTextLength { get; }

   public abstract bool IsPlatformSupported { get; }

   public abstract bool IsSSMLSupported { get; }

   public virtual List<string> Cultures
   {
      get
      {
         if (_cachedCultures.Count == 0)
         {
            IEnumerable<Voice> cultures = Voices.GroupBy(cul => cul.Culture)
               .Select(grp => grp.First()).OrderBy(s => s.Culture).ToList();

            foreach (Voice voice in cultures)
            {
               _cachedCultures.Add(voice.Culture);
            }
         }

         return _cachedCultures;
      }
   }

   public virtual bool IsReady { get; protected set; }
   public virtual bool IsSpeaking => _processes.Count > 0;

   #endregion

   #region Events

   public abstract event IVoiceProvider.VoicesLoaded? OnVoicesLoaded;
   public abstract event IVoiceProvider.SpeakStarted? OnSpeakStarted;
   public abstract event IVoiceProvider.SpeakCompleted? OnSpeakCompleted;

   #endregion

   #region Public methods

   public virtual List<Voice> GetVoices()
   {
      return Task.Run(GetVoicesAsync).GetAwaiter().GetResult();
   }

   public abstract Task<List<Voice>> GetVoicesAsync();

   public virtual void Silence()
   {
      foreach (var process in _processes)
      {
         process.Stop();
      }

      _processes.Clear();
   }

   public virtual bool Speak(string text, Voice? voice = null, float rate = 1f, float pitch = 1f, float volume = 1f, bool forceSSML = true, bool useThreads = false)
   {
      ArgumentNullException.ThrowIfNull(text);

      if (useThreads)
      {
         System.Threading.Thread t = new(() => _ = speakAsync(text, voice, rate, pitch, volume, forceSSML));
         t.Start();

         return true;
      }

      return Task.Run(() => speakAsync(text, voice, rate, pitch, volume, forceSSML)).GetAwaiter().GetResult();
   }

   public virtual async Task<bool> SpeakAsync(string text, Voice? voice = null, float rate = 1f, float pitch = 1f, float volume = 1f, bool forceSSML = true)
   {
      return await speakAsync(text, voice, rate, pitch, volume, forceSSML);
   }

   #endregion

   #region Private methods

   protected abstract Task<bool> speakAsync(string text, Voice? voice = null, float rate = 1f, float pitch = 1f, float volume = 1f, bool forceSSML = true);

   #endregion
}