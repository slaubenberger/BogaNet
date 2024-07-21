using System.Linq;
using BogaNet.TTS.Model;
using System;
using BogaNet.Extension;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BogaNet.Util;
using System.Text;
using System.Diagnostics;

namespace BogaNet.TTS.Provider;

/// <summary>
/// MacOS voice provider.
/// </summary>
public partial class OSXVoiceProvider : Singleton<OSXVoiceProvider>, IVoiceProvider
{
   #region Variables

   private static readonly ILogger<OSXVoiceProvider> _logger = GlobalLogging.CreateLogger<OSXVoiceProvider>();

   private const string _applicationName = "say";

   private static readonly Regex _sayRegex = sayRegex();

   private const int _defaultRate = 175;

   private ProcessRunner? _process;

   private List<Voice>? _cachedVoices;
   private readonly List<string> _cachedCultures = [];

   #endregion

   #region Properties

   public virtual List<Voice> Voices => _cachedVoices ??= GetVoices();

   //public virtual string DefaultVoiceName => "Daniel";

   public virtual int MaxTextLength => 256000;

   public virtual bool IsPlatformSupported => Constants.IsOSX;

   public virtual bool IsSSMLSupported => false;

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

   private OSXVoiceProvider()
   {
      if (!Constants.IsOSX)
         _logger.LogError("OSXVoiceProvider works only under OSX!");
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
      _process?.Stop();
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

      StringBuilder sb = new();
      sb.Append(string.IsNullOrEmpty(voiceName) ? string.Empty : " -v \"" + voiceName.Replace('"', '\'') + '"');
      sb.Append(calculatedRate != _defaultRate ? " -r " + calculatedRate : string.Empty);
      sb.Append(" \"");
      sb.Append(text.Replace('"', '\''));
      sb.Append('"');

      string args = sb.ToString();

      _process = new();

      Process process = await _process.StartAsync(_applicationName, args, true, Encoding.UTF8);

      if (process.ExitCode is 0 or -1 or 137) //0 = normal ended, -1/137 = killed
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

      Process process = await _process.StartAsync(_applicationName, "-v ?", true, Encoding.UTF8);

      if (process.ExitCode == 0)
      {
         List<Voice> voices = new(200);

         List<string> lines = _process.Output.ToList();

         foreach (var line in lines)
         {
            if (!string.IsNullOrEmpty(line))
            {
               Match match = _sayRegex.Match(line);

               if (match.Success)
               {
                  string name = match.Groups[1].ToString();
                  voices.Add(new Voice(name, match.Groups[3].ToString(),
                     BogaNet.TTS.Util.Helper.AppleVoiceNameToGender(name), "unknown",
                     match.Groups[2].ToString(), "", "Apple"));
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

   private static string getVoiceName(Voice? voice)
   {
      if (voice == null || string.IsNullOrEmpty(voice.Name))
      {
         _logger.LogWarning("'Voice' or 'Voice.Name' is null! Using the providers 'default' voice.");

         //return DefaultVoiceName;
         return string.Empty;
      }

      return voice.Name;
   }

   private static int calculateRate(float rate)
   {
      return Math.Clamp(Math.Abs(rate - 1f) > Constants.FLOAT_TOLERANCE
         ? (int)(_defaultRate * rate)
         : _defaultRate, 1, 3 * _defaultRate);
   }

   [GeneratedRegex(@"^([^#]+?)\s*([^ ]+)\s*# (.*?)$")]
   private static partial Regex sayRegex();

   #endregion
}