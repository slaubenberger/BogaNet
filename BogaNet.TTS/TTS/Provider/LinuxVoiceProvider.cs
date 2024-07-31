using System.Linq;
using BogaNet.TTS.Model;
using System;
using BogaNet.Extension;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.Util;
using BogaNet.TTS.Model.Enum;
using System.Diagnostics;

namespace BogaNet.TTS.Provider;

/// <summary>
/// Linux voice provider.
/// NOTE: needs eSpeak or eSpeak-NG: http://espeak.sourceforge.net/
/// </summary>
public class LinuxVoiceProvider : Singleton<LinuxVoiceProvider>, IVoiceProvider
{
   #region Variables

   private static readonly ILogger<LinuxVoiceProvider> _logger = GlobalLogging.CreateLogger<LinuxVoiceProvider>();

   private const int _defaultRate = 160;
   private const int _defaultVolume = 100;
   private const int _defaultPitch = 50;

   private readonly List<ProcessRunner> _processes = [];

   private List<Voice>? _cachedVoices;
   private readonly List<string> _cachedCultures = [];

   #endregion

   #region Properties

   /// <summary>eSpeak application name/path.</summary>
   public virtual string ESpeakApplication { get; set; } = "espeak-ng";

   /// <summary>eSpeak application data path.</summary>
   public virtual string ESpeakDataPath { get; set; } = string.Empty;

   /// <summary>Active modifier for all eSpeak voices.</summary>
   public virtual ESpeakModifiers ESpeakModifier { get; set; } = ESpeakModifiers.none;

   /// <summary>Female modifier for female eSpeak voices.</summary>
   public virtual ESpeakModifiers ESpeakFemaleModifier { get; set; } = ESpeakModifiers.f3;

   public virtual List<Voice> Voices => _cachedVoices ??= GetVoices();

/*
      public virtual string DefaultVoiceName => "en";
*/
   public virtual int MaxTextLength => 32000;

   public virtual bool IsPlatformSupported => Constants.IsOSX || Constants.IsLinux || Constants.IsWindows;

   public virtual bool IsSSMLSupported => true;

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

   public virtual bool IsReady { get; private set; }
   public virtual bool IsSpeaking { get; private set; }

   #endregion

   #region Events

   public virtual event IVoiceProvider.VoicesLoaded? OnVoicesLoaded;
   public event IVoiceProvider.SpeakStarted? OnSpeakStarted;

   public virtual event IVoiceProvider.SpeakCompleted? OnSpeakCompleted;

   #endregion

   #region Constructor

   private LinuxVoiceProvider()
   {
   }

   #endregion

   #region Implemented methods

   public virtual List<Voice> GetVoices()
   {
      return Task.Run(GetVoicesAsync).GetAwaiter().GetResult();
   }

   public virtual async Task<List<Voice>> GetVoicesAsync()
   {
      List<Voice> res = await getVoices() ?? [];

      IsReady = res.Count > 0;
      OnVoicesLoaded?.Invoke(res);

      return res;
   }

   public virtual void Silence()
   {
      foreach (var process in _processes)
      {
         process.Stop();
      }

      _processes.Clear();
   }

   public virtual bool Speak(string text, Voice? voice = null, float rate = 1, float pitch = 1, float volume = 1, bool forceSSML = true, bool useThreads = false)
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

   public virtual async Task<bool> SpeakAsync(string text, Voice? voice = null, float rate = 1, float pitch = 1, float volume = 1, bool forceSSML = true)
   {
      return await speakAsync(text, voice, rate, pitch, volume, forceSSML);
   }

   #endregion

   #region Private methods

   private async Task<bool> speakAsync(string text, Voice? voice = null, float rate = 1, float pitch = 1, float volume = 1, bool forceSSML = true)
   {
      OnSpeakStarted?.Invoke(text);
      IsSpeaking = true;

      _logger.LogDebug("Starting TTS2: " + text);

      string voiceName = getVoiceName(voice);
      int calculatedRate = calculateRate(rate);
      int calculatedVolume = calculateVolume(volume);
      int calculatedPitch = calculatePitch(pitch);

      System.Text.StringBuilder sb = new();
      sb.Append(string.IsNullOrEmpty(voiceName) ? string.Empty : "-v \"" + voiceName.Replace('"', '\'') + '"');
      sb.Append(calculatedRate != _defaultRate ? " -s " + calculatedRate + " " : string.Empty);
      sb.Append(calculatedVolume != _defaultVolume ? " -a " + calculatedVolume + " " : string.Empty);
      sb.Append(calculatedPitch != _defaultPitch ? " -p " + calculatedPitch + " " : string.Empty);
      sb.Append(" -z ");
      sb.Append(" -m \"");
      sb.Append(text.Replace('"', '\''));
      sb.Append('"');
      sb.Append(string.IsNullOrEmpty(ESpeakDataPath) ? string.Empty : " --path=\"" + ESpeakDataPath + '"');

      string args = sb.ToString();

      ProcessRunner pr = new();
      _processes.Add(pr);

      Process process = await pr.StartAsync(ESpeakApplication, args, true);

      OnSpeakCompleted?.Invoke(text);
      IsSpeaking = false;
      _processes.Remove(pr);

      if (process.ExitCode is 0 or -1 or 137) //0 = normal ended, -1/137 = killed
      {
         _logger.LogDebug($"Text spoken: {text}");
         return true;
      }

      _logger.LogError($"Could not speak the text: {text} - Exit code: {process.ExitCode} - Error: {pr.Error.BNDump()}");

      return false;
   }

   private string getVoiceName(Voice? voice)
   {
      if (voice == null || string.IsNullOrEmpty(voice.Name))
      {
         _logger.LogWarning("'Voice' or 'Voice.Name' is null! Using the providers 'default' voice.");

         //return DefaultVoiceName;
         return string.Empty;
      }

      if (ESpeakModifier == ESpeakModifiers.none)
      {
         if (voice.Gender == Gender.FEMALE)
            return voice.Name + $"+{ESpeakFemaleModifier.ToString()}";

         return voice.Name;
      }

      return voice.Name + "+" + ESpeakModifier;
   }

   private async Task<List<Voice>?> getVoices()
   {
      ProcessRunner pr = new();

      string args = "--voices" + (string.IsNullOrEmpty(ESpeakDataPath) ? string.Empty : $" --path=\"{ESpeakDataPath}\"");

      var process = await pr.StartAsync(ESpeakApplication, args, true);

      if (process.ExitCode == 0)
      {
         List<Voice> voices = new(150);

         List<string> lines = pr.Output.ToList();

         foreach (var line in lines)
         {
            if (!string.IsNullOrEmpty(line))
            {
               if (!line.BNStartsWith("Pty")) //ignore header
               {
                  Voice voice;

                  if (ESpeakApplication.BNContains("espeak-ng"))
                  {
                     string endLine = line[30..];
                     int index = endLine.BNIndexOf(")");
                     string name = index > 0 ? endLine.Substring(0, index + 1).Trim().Replace('_', ' ') : endLine[..19].Trim().Replace('_', ' ');

                     string desc = line[50..].Trim();
                     Gender gender = BogaNet.TTS.Util.Helper.StringToGender(line.Substring(23, 1));
                     string culture = line.Substring(4, 15).Trim();

                     voice = new Voice(name, desc, gender, "unknown", culture, "", "espeak-ng");
                  }
                  else
                  {
                     string name = line.Substring(22, 20).Trim();
                     string desc = line[43..].Trim();
                     Gender gender = BogaNet.TTS.Util.Helper.StringToGender(line.Substring(19, 1));
                     string culture = line.Substring(4, 15).Trim();

                     voice = new Voice(name, desc, gender, "unknown", culture, "", "espeak");
                  }

                  voices.Add(voice);
               }
            }
         }

         _cachedVoices = voices.OrderBy(s => s.Name).ToList();

         _logger.LogDebug($"Voices read: {_cachedVoices.BNDump()}");
      }
      else
      {
         _logger.LogError($"Could not get any voices: {process.ExitCode} - Error: {pr.Error.BNDump()}");
      }

      return _cachedVoices;
   }

   private static int calculateRate(float rate)
   {
      return Math.Clamp(Math.Abs(rate - 1f) > Constants.FLOAT_TOLERANCE
         ? (int)(_defaultRate * rate)
         : _defaultRate, 1, 3 * _defaultRate);
   }

   private static int calculateVolume(float volume)
   {
      return Math.Clamp((int)(_defaultVolume * volume), 0, 200);
   }

   private static int calculatePitch(float pitch)
   {
      return Math.Clamp((int)(_defaultPitch * pitch), 0, 99);
   }

   #endregion
}