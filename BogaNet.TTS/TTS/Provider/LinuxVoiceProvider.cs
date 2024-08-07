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
public class LinuxVoiceProvider : BaseVoiceProvider
{
   #region Variables

   private static readonly ILogger<LinuxVoiceProvider> _logger = GlobalLogging.CreateLogger<LinuxVoiceProvider>();

   private const int DEFAULT_RATE = 160;
   private const int DEFAULT_VOLUME = 100;
   private const int DEFAULT_PITCH = 50;

   #endregion

   #region Properties

   /// <summary>eSpeak application name/path.</summary>
   public static string ESpeakApplication { get; set; } = "espeak-ng";

   /// <summary>eSpeak application data path.</summary>
   public static string ESpeakDataPath { get; set; } = string.Empty;

   /// <summary>Active modifier for all eSpeak voices.</summary>
   public static ESpeakModifiers ESpeakModifier { get; set; } = ESpeakModifiers.none;

   /// <summary>Female modifier for female eSpeak voices.</summary>
   public static ESpeakModifiers ESpeakFemaleModifier { get; set; } = ESpeakModifiers.f3;

   //public virtual string DefaultVoiceName => "en";

   public override int MaxTextLength => 32000;

   public override bool IsPlatformSupported => Constants.IsOSX || Constants.IsLinux || Constants.IsWindows;

   public override bool IsSSMLSupported => true;

   #endregion

   #region Events

   public override event IVoiceProvider.VoicesLoaded? OnVoicesLoaded;
   public override event IVoiceProvider.SpeakStarted? OnSpeakStarted;
   public override event IVoiceProvider.SpeakCompleted? OnSpeakCompleted;

   #endregion

   #region Public methods

   public override async Task<List<Voice>> GetVoicesAsync()
   {
      List<Voice> res = await getVoicesAsync() ?? [];

      IsReady = res.Count > 0;
      OnVoicesLoaded?.Invoke(res);

      return res;
   }

   #endregion

   #region Private methods

   protected override async Task<bool> speakAsync(string text, Voice? voice = null, float rate = 1, float pitch = 1, float volume = 1, bool forceSSML = true)
   {
      OnSpeakStarted?.Invoke(text);

      _logger.LogDebug("Starting TTS2: " + text);

      string voiceName = getVoiceName(voice);
      int calculatedRate = calculateRate(rate);
      int calculatedVolume = calculateVolume(volume);
      int calculatedPitch = calculatePitch(pitch);

      System.Text.StringBuilder sb = new();
      sb.Append(string.IsNullOrEmpty(voiceName) ? string.Empty : "-v \"" + voiceName.Replace('"', '\'') + '"');
      sb.Append(calculatedRate != DEFAULT_RATE ? " -s " + calculatedRate + " " : string.Empty);
      sb.Append(calculatedVolume != DEFAULT_VOLUME ? " -a " + calculatedVolume + " " : string.Empty);
      sb.Append(calculatedPitch != DEFAULT_PITCH ? " -p " + calculatedPitch + " " : string.Empty);
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
      _processes.Remove(pr);

      if (process.ExitCode is 0 or -1 or 137) //0 = normal ended, -1/137 = killed
      {
         _logger.LogDebug($"Text spoken: {text}");
         return true;
      }

      _logger.LogError($"Could not speak the text: {text} - Exit code: {process.ExitCode} - Error: {pr.Error.BNDump()}");

      return false;
   }

   private static string getVoiceName(Voice? voice)
   {
      if (voice == null || string.IsNullOrEmpty(voice.Name))
      {
         _logger.LogWarning("'Voice' or 'Voice.Name' is null! Using the providers 'default' voice.");

         //return DefaultVoiceName;
         return string.Empty;
      }

      if (ESpeakModifier != ESpeakModifiers.none) 
         return voice.Name + "+" + ESpeakModifier;
      
      if (voice.Gender == Gender.FEMALE)
         return voice.Name + $"+{ESpeakFemaleModifier.ToString()}";

      return voice.Name;
   }

   private async Task<List<Voice>?> getVoicesAsync()
   {
      ProcessRunner pr = new();

      string args = "--voices" + (string.IsNullOrEmpty(ESpeakDataPath) ? string.Empty : $" --path=\"{ESpeakDataPath}\"");

      var process = await pr.StartAsync(ESpeakApplication, args, true);

      if (process.ExitCode == 0)
      {
         List<Voice> voices = new(150);

         List<string> lines = pr.Output.ToList();

         foreach (var line in lines.Where(line => !string.IsNullOrEmpty(line)).Where(line => !line.BNStartsWith("Pty")))
         {
            Voice voice;

            if (ESpeakApplication.BNContains("espeak-ng"))
            {
               string endLine = line[30..];
               int index = endLine.BNIndexOf(")");
               string name = index > 0 ? endLine.Substring(0, index + 1).Trim().Replace('_', ' ') : endLine[..19].Trim().Replace('_', ' ');

               string desc = line[50..].Trim();
               Gender gender = Util.Helper.StringToGender(line.Substring(23, 1));
               string culture = line.Substring(4, 15).Trim();

               voice = new Voice(name, desc, gender, "unknown", culture, "", "espeak-ng");
            }
            else
            {
               string name = line.Substring(22, 20).Trim();
               string desc = line[43..].Trim();
               Gender gender = Util.Helper.StringToGender(line.Substring(19, 1));
               string culture = line.Substring(4, 15).Trim();

               voice = new Voice(name, desc, gender, "unknown", culture, "", "espeak");
            }

            voices.Add(voice);
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
         ? (int)(DEFAULT_RATE * rate)
         : DEFAULT_RATE, 1, 3 * DEFAULT_RATE);
   }

   private static int calculateVolume(float volume)
   {
      return Math.Clamp((int)(DEFAULT_VOLUME * volume), 0, 200);
   }

   private static int calculatePitch(float pitch)
   {
      return Math.Clamp((int)(DEFAULT_PITCH * pitch), 0, 99);
   }

   #endregion
}