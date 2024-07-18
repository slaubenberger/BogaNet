using System.Linq;
using BogaNet.TTS.Model;
using System;
using BogaNet.Extension;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.Util;
using System.Text;

namespace BogaNet.TTS.Provider
{
   /// <summary>Windows voice provider.</summary>
   public class WindowsVoiceProvider : Singleton<OSXVoiceProvider>, IVoiceProvider
   {
      #region Variables

      private static readonly ILogger<WindowsVoiceProvider> _logger = GlobalLogging.CreateLogger<WindowsVoiceProvider>();

      private const string _applicationName = "say";

      private const string idVoice = "@VOICE:";

      private static readonly char[] splitChar = [':'];

      private ProcessRunner? _process;

      private List<Voice>? _cachedVoices;
      private readonly List<string> _cachedCultures = [];

      #endregion


      #region Properties

      public virtual List<Voice> Voices => _cachedVoices ??= GetVoices();

      //public override string DefaultVoiceName => "Microsoft Zira Desktop";

      public int MaxTextLength => 32000;

      public bool IsPlatformSupported => Constants.IsWindows;

      public bool IsSSMLSupported => true;

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

      private WindowsVoiceProvider()
      {
         if (!Constants.IsOSX)
            _logger.LogError("WindowsVoiceProvider works only under Windows!");
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

      public virtual void Silence()
      {
         if (_process != null)
            _process.Stop();
      }

      public virtual bool Speak(string text, Voice? voice = null, float rate = 1f, float pitch = 1f, float volume = 1f, bool forceSSML = true)
      {
         return Task.Run(() => SpeakAsync(text, voice, rate, pitch, volume, forceSSML)).GetAwaiter().GetResult();
      }

      public virtual async Task<bool> SpeakAsync(string text, Voice? voice = null, float rate = 1f, float pitch = 1f, float volume = 1f, bool forceSSML = true)
      {
         ArgumentNullException.ThrowIfNull(text);

         string voiceName = getVoiceName(voice);
         int calculatedRate = calculateRate(rate);
         int calculatedVolume = calculateVolume(volume);

         StringBuilder sb = new();
         sb.Append("--speak");
         sb.Append($" -text \"{prepareText(text, voice, pitch, forceSSML)}\"");
         sb.Append($" -rate {calculatedRate}");
         sb.Append($" -volume {calculatedVolume}");
         sb.Append(string.IsNullOrEmpty(voiceName) ? string.Empty : $" -voice \"{voiceName.Replace('"', '\'')}\"");

         string args = sb.ToString();

         _process = new();

         var process = await _process.StartAsync(_applicationName, args, true, Encoding.UTF8);

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

      private async Task<List<Voice>?> getVoices()
      {
         _process = new();

         var process = await _process.StartAsync(_applicationName, "-v ?", true, Encoding.UTF8);

         if (process.ExitCode == 0)
         {
            List<Voice> voices = [];

            var lines = _process.Output.ToList();

            foreach (var line in lines)
            {
               if (!string.IsNullOrEmpty(line))
               {
                  if (line.BNStartsWith(idVoice))
                  {
                     string[] splittedString = line.Split(splitChar,
                        System.StringSplitOptions.RemoveEmptyEntries);

                     if (splittedString.Length == 6)
                     {
                        voices.Add(new Model.Voice(splittedString[1], splittedString[2],
                           BogaNet.TTS.Util.Helper.StringToGender(splittedString[3]), splittedString[4],
                           splittedString[5]));
                     }
                     else
                     {
                        _logger.LogWarning("Voice is invalid: " + line);
                     }
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

      private string getVoiceName(Voice? voice)
      {
         if (voice == null || string.IsNullOrEmpty(voice.Name))
         {
            _logger.LogWarning("'Voice' or 'Voice.Name' is null! Using the providers 'default' voice.");

            //return DefaultVoiceName;
            return string.Empty;
         }

         return voice.Name;
      }

      private static string prepareText(string text, Voice? voice, float pitch, bool forceSSML)
      {
         //TEST
         //wrapper.ForceSSML = false;

         if (forceSSML && !Speaker.Instance.AutoClearTags)
         {
            StringBuilder sbXML = new();

            sbXML.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sbXML.Append("<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"");
            sbXML.Append(voice == null ? "en-US" : voice.Culture);
            sbXML.Append("\">");

            float _pitch = pitch - 1f;

            if (Math.Abs(_pitch) > Constants.FLOAT_TOLERANCE)
            {
               sbXML.Append("<prosody pitch='");

               sbXML.Append(_pitch > 0f
                  ? _pitch.ToString("+#0%", Constants.CurrentCulture)
                  : _pitch.ToString("#0%", Constants.CurrentCulture));

               sbXML.Append("'>");
            }

            sbXML.Append(text);

            if (Math.Abs(_pitch) > Constants.FLOAT_TOLERANCE)
               sbXML.Append("</prosody>");

            sbXML.Append("</speak>");

            return getValidXML(sbXML.ToString().Replace('"', '\''));
         }

         return text.Replace('"', '\'');
      }

      private static string getValidXML(string xml)
      {
         return !string.IsNullOrEmpty(xml)
            ? xml.Replace(" & ", " &amp; ").Replace(" < ", " &lt; ").Replace(" > ", " &gt; ")
            : xml;
      }

      private static int calculateVolume(float volume)
      {
         return Math.Clamp((int)(100 * volume), 0, 100);
      }

      private static int calculateRate(float rate)
      {
         //allowed range: 0 - 3f - all other values were cropped
         int result = 0;

         if (Math.Abs(rate - 1f) > Constants.FLOAT_TOLERANCE)
         {
            //relevant?
            if (rate > 1f)
            {
               //larger than 1
               if (rate >= 2.75f)
               {
                  result = 10; //2.78
               }
               else if (rate is >= 2.6f and < 2.75f)
               {
                  result = 9; //2.6
               }
               else if (rate is >= 2.35f and < 2.6f)
               {
                  result = 8; //2.39
               }
               else if (rate is >= 2.2f and < 2.35f)
               {
                  result = 7; //2.2
               }
               else if (rate is >= 2f and < 2.2f)
               {
                  result = 6; //2
               }
               else if (rate is >= 1.8f and < 2f)
               {
                  result = 5; //1.8
               }
               else if (rate is >= 1.6f and < 1.8f)
               {
                  result = 4; //1.6
               }
               else if (rate is >= 1.4f and < 1.6f)
               {
                  result = 3; //1.45
               }
               else if (rate is >= 1.2f and < 1.4f)
               {
                  result = 2; //1.28
               }
               else if (rate is > 1f and < 1.2f)
               {
                  result = 1; //1.14
               }
            }
            else
            {
               //smaller than 1
               if (rate <= 0.3f)
               {
                  result = -10; //0.33
               }
               else if (rate is > 0.3f and <= 0.4f)
               {
                  result = -9; //0.375
               }
               else if (rate is > 0.4f and <= 0.45f)
               {
                  result = -8; //0.42
               }
               else if (rate is > 0.45f and <= 0.5f)
               {
                  result = -7; //0.47
               }
               else if (rate is > 0.5f and <= 0.55f)
               {
                  result = -6; //0.525
               }
               else if (rate is > 0.55f and <= 0.6f)
               {
                  result = -5; //0.585
               }
               else if (rate is > 0.6f and <= 0.7f)
               {
                  result = -4; //0.655
               }
               else if (rate is > 0.7f and <= 0.8f)
               {
                  result = -3; //0.732
               }
               else if (rate is > 0.8f and <= 0.9f)
               {
                  result = -2; //0.82
               }
               else if (rate is > 0.9f and < 1f)
               {
                  result = -1; //0.92
               }
            }
         }

         return result;
      }

      #endregion
   }
}
