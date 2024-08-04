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
public partial class OSXVoiceProvider : BaseVoiceProvider
{
   #region Variables

   private static readonly ILogger<OSXVoiceProvider> _logger = GlobalLogging.CreateLogger<OSXVoiceProvider>();

   private const string APPLICATION_NAME = "say";
   private const int DEFAULT_RATE = 175;

   private static readonly Regex _sayRegex = sayRegex();

   #endregion

   #region Properties

   //public virtual string DefaultVoiceName => "Daniel";

   public override int MaxTextLength => 256000;

   public override bool IsPlatformSupported => Constants.IsOSX;

   public override bool IsSSMLSupported => false;

   #endregion

   #region Events

   public override event IVoiceProvider.VoicesLoaded? OnVoicesLoaded;
   public override event IVoiceProvider.SpeakStarted? OnSpeakStarted;
   public override event IVoiceProvider.SpeakCompleted? OnSpeakCompleted;

   #endregion

   #region Constructor

   public OSXVoiceProvider()
   {
      if (!Constants.IsOSX)
         _logger.LogError("OSXVoiceProvider works only under OSX!");
   }

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

   protected override async Task<bool> speakAsync(string text, Voice? voice = null, float rate = 1f, float pitch = 1f, float volume = 1f, bool forceSSML = true)
   {
      OnSpeakStarted?.Invoke(text);

      string voiceName = getVoiceName(voice);
      int calculatedRate = calculateRate(rate);

      StringBuilder sb = new();
      sb.Append(string.IsNullOrEmpty(voiceName) ? string.Empty : " -v \"" + voiceName.Replace('"', '\'') + '"');
      sb.Append(calculatedRate != DEFAULT_RATE ? " -r " + calculatedRate : string.Empty);
      sb.Append(" \"");
      sb.Append(text.Replace('"', '\''));
      sb.Append('"');

      string args = sb.ToString();

      ProcessRunner pr = new();
      _processes.Add(pr);

      Process process = await pr.StartAsync(APPLICATION_NAME, args, true, Encoding.UTF8);

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

   private async Task<List<Voice>?> getVoicesAsync()
   {
      ProcessRunner pr = new();

      Process process = await pr.StartAsync(APPLICATION_NAME, "-v ?", true, Encoding.UTF8);

      if (process.ExitCode == 0)
      {
         List<Voice> voices = new(200);

         List<string> lines = pr.Output.ToList();

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
         _logger.LogError($"Could not get any voices: {process.ExitCode} - Error: {pr.Error.BNDump()}");
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
         ? (int)(DEFAULT_RATE * rate)
         : DEFAULT_RATE, 1, 3 * DEFAULT_RATE);
   }

   [GeneratedRegex(@"^([^#]+?)\s*([^ ]+)\s*# (.*?)$")]
   private static partial Regex sayRegex();

   #endregion
}