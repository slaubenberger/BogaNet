#if true
using System.Linq;
using BogaNet.TTS.Model;
using System;
using BogaNet.Extension;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BogaNet.Util;
using BogaNet.TTS.Model.Enum;

namespace BogaNet.TTS.Provider
{
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

      private ProcessRunner? _process;

      private List<Voice>? _cachedVoices;
      private readonly List<string> _cachedCultures = [];

      private string eSpeakApplication = "espeak-ng";
      //private string eSpeakApplication = "/opt/local/bin/espeak-ng";

      private string eSpeakDataPath = string.Empty;
      //private string eSpeakDataPath = "/opt/local/bin/";

      private ESpeakModifiers eSpeakModifier = ESpeakModifiers.none;
      private ESpeakModifiers eSpeakFemaleModifier = ESpeakModifiers.f3;

      #endregion


      #region Properties

      /// <summary>eSpeak application name/path.</summary>
      public string ESpeakApplication
      {
         get => eSpeakApplication;
         set => eSpeakApplication = value;
      }

      /// <summary>eSpeak application data path.</summary>
      public string ESpeakDataPath
      {
         get => eSpeakDataPath;
         set => eSpeakDataPath = value;
      }

      /// <summary>Active modifier for all eSpeak voices.</summary>
      public ESpeakModifiers ESpeakModifier
      {
         get => eSpeakModifier;
         set => eSpeakModifier = value;
      }

      /// <summary>Female modifier for female eSpeak voices.</summary>
      public ESpeakModifiers ESpeakFemaleModifier
      {
         get => eSpeakFemaleModifier;
         set => eSpeakFemaleModifier = value;
      }

      public virtual List<Voice> Voices => _cachedVoices ??= GetVoices();

      public virtual string DefaultVoiceName => "en";

      public virtual int MaxTextLength => 32000;

      public virtual bool isPlatformSupported => Constants.IsOSX || Constants.IsLinux || Constants.IsWindows;

      public virtual bool isSSMLSupported => true;

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
         return await getVoices() ?? [];
      }

      public void Silence()
      {
         if (_process != null)
            _process.Stop();
      }

      public bool Speak(string text, Voice? voice = null, float rate = 1, float pitch = 1, float volume = 1, bool forceSSML = true)
      {
         return Task.Run(() => SpeakAsync(text, voice, rate, pitch, volume, forceSSML)).GetAwaiter().GetResult();
      }

      public async Task<bool> SpeakAsync(string text, Voice? voice = null, float rate = 1, float pitch = 1, float volume = 1, bool forceSSML = true)
      {
         ArgumentNullException.ThrowIfNull(text);

         _logger.LogDebug("Starting TTS2: " + text);

         string voiceName = getVoiceName(voice);
         int calculatedRate = calculateRate(rate);
         int calculatedVolume = calculateVolume(volume);
         int calculatedPitch = calculatePitch(pitch);

         System.Text.StringBuilder sb = new();
         sb.Append((string.IsNullOrEmpty(voiceName) ? string.Empty : "-v \"" + voiceName.Replace('"', '\'') + '"'));
         sb.Append((calculatedRate != _defaultRate ? " -s " + calculatedRate + " " : string.Empty));
         sb.Append((calculatedVolume != _defaultVolume ? " -a " + calculatedVolume + " " : string.Empty));
         sb.Append((calculatedPitch != _defaultPitch ? " -p " + calculatedPitch + " " : string.Empty));
         sb.Append(" -z ");
         sb.Append(" -m \"");
         sb.Append(text.Replace('"', '\''));
         sb.Append('"');
         sb.Append((string.IsNullOrEmpty(ESpeakDataPath) ? string.Empty : " --path=\"" + ESpeakDataPath + '"'));

         string args = sb.ToString();

         _process = new();

         var process = await _process.StartAsync(ESpeakApplication, args, true);

         if (process.ExitCode == 0 || process.ExitCode == -1 || process.ExitCode == 137) //0 = normal ended, -1/137 = killed
         {
            _logger.LogDebug($"Text spoken: {text}");
            return true;
         }

         _logger.LogError($"Could not speak the text: {text} - Exit code: {process.ExitCode} - Error: {_process.Error.BNDump()}");

         return false;
      }

      #endregion


      #region Private methods

      private string getVoiceName(Voice? voice)
      {
         if (voice == null || string.IsNullOrEmpty(voice.Name))
         {
            _logger.LogWarning("'Voice' or 'Voice.Name' is null! Using the providers 'default' voice.");

            return DefaultVoiceName;
         }

         if (ESpeakModifier == ESpeakModifiers.none)
         {
            if (voice.Gender == Gender.FEMALE)
               return voice.Name + $"+{ESpeakFemaleModifier.ToString()}";

            return voice?.Name;
         }

         return voice?.Name + "+" + ESpeakModifier;
      }

      private async Task<List<Voice>?> getVoices()
      {
         _process = new();

         string args = "--voices" + (string.IsNullOrEmpty(ESpeakDataPath) ? string.Empty : $" --path=\"{ESpeakDataPath}\"");

         var process = await _process.StartAsync(ESpeakApplication, args, true);

         if (process.ExitCode == 0)
         {
            List<Voice> voices = new(150);

            var lines = _process.Output.ToList();

            foreach (var line in lines)
            {
               if (!string.IsNullOrEmpty(line))
               {
                  if (!line.BNStartsWith("Pty")) //ignore header
                  {
                     _logger.LogDebug(line);

                     var split = line.Split("\t");

                     Voice voice;

                     if (ESpeakApplication.BNContains("espeak-ng"))
                     {
                        string endLine = line.Substring(30);
                        int index = endLine.BNIndexOf(")");
                        string name = index > 0 ? endLine.Substring(0, index + 1).Trim().Replace('_', ' ') : endLine.Substring(0, 19).Trim().Replace('_', ' ');

                        string desc = line.Substring(50).Trim();
                        Gender gender = BogaNet.TTS.Util.Helper.StringToGender(line.Substring(23, 1));
                        string culture = line.Substring(4, 15).Trim();

                        voice = new Voice(name, desc, gender, "unknown", culture, "", "espeak-ng");
                     }
                     else
                     {
                        string name = line.Substring(22, 20).Trim();
                        string desc = line.Substring(43).Trim();
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
            _logger.LogError($"Could not get any voices: {process.ExitCode} - Error: {_process.Error.BNDump()}");
         }

         return _cachedVoices;
      }

/*
         private IEnumerator getVoices()
         {
            _cachedVoices.Clear();

            string args = "--voices" + (string.IsNullOrEmpty(Speaker.Instance.ESpeakDataPath) ? string.Empty : " --path=\"" + Speaker.Instance.ESpeakDataPath + '"');

            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
               process.StartInfo.FileName = Speaker.Instance.ESpeakApplication;
               process.StartInfo.Arguments = args;
               process.OutputDataReceived += process_OutputDataReceived;

               System.Threading.Thread worker = new System.Threading.Thread(() => startProcess(process, Crosstales.RTVoice.Util.Constants.DEFAULT_TTS_KILL_TIME, true));
               worker.Start();

               do
               {
                  yield return null;
               } while (worker.IsAlive || !process.HasExited);

               if (process.ExitCode == 0)
               {
                  //do nothing
               }
               else
               {
                  using (System.IO.StreamReader sr = process.StandardError)
                  {
                     string errorMessage = "Could not get any voices: " + process.ExitCode + System.Environment.NewLine +
                                           sr.ReadToEnd();
                     _logger.LogError(errorMessage);
                  }
               }
            }

            cachedVoices = _cachedVoices.OrderBy(s => s.Name).ToList();

            if (Crosstales.RTVoice.Util.Constants.DEV_DEBUG)
               _logger.Log("Voices read: " + cachedVoices.CTDump());
         }

      private void process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
      {
         //Debug.Log(e.Data);

         string reply = e.Data;

         if (!string.IsNullOrEmpty(reply))
         {
            if (!reply.CTStartsWith("Pty")) //ignore header
            {
               _cachedVoices.Add(Speaker.Instance.ESpeakApplication.CTContains("espeak-ng")
                  ? new Crosstales.RTVoice.Model.Voice(reply.Substring(30, 19).Trim().Replace("_", " "), reply.Substring(50).Trim(),
                     Crosstales.RTVoice.Util.Helper.StringToGender(reply.Substring(23, 1)), Crosstales.RTVoice.Util.Constants.VOICE_AGE_UNKNOWN,
                     reply.Substring(4, 15).Trim(), "", "espeak-ng")
                  : new Crosstales.RTVoice.Model.Voice(reply.Substring(22, 20).Trim(), reply.Substring(43).Trim(),
                     Crosstales.RTVoice.Util.Helper.StringToGender(reply.Substring(19, 1)), Crosstales.RTVoice.Util.Constants.VOICE_AGE_UNKNOWN,
                     reply.Substring(4, 15).Trim(), "", "espeak"));
            }
         }
      }
*/
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
}
#endif