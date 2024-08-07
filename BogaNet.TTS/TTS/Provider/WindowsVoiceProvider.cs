﻿using System.Linq;
using BogaNet.TTS.Model;
using System;
using BogaNet.Extension;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.Util;
using System.Text;
using System.Diagnostics;
using BogaNet.Encoder;
using BogaNet.Helper;

namespace BogaNet.TTS.Provider;

/// <summary>Windows voice provider.</summary>
public class WindowsVoiceProvider : BaseVoiceProvider
{
   #region Variables

   private static readonly ILogger<WindowsVoiceProvider> _logger = GlobalLogging.CreateLogger<WindowsVoiceProvider>();

   private const string ID_VOICE = "@VOICE:";

   private static readonly char[] splitChar = [':'];

   #endregion

   #region Properties

   //public override string DefaultVoiceName => "Microsoft Zira Desktop";

   public override int MaxTextLength => 32000;

   public override bool IsPlatformSupported => Constants.IsWindows;

   public override bool IsSSMLSupported => true;

   private static string _applicationName => WindowsWrapper.Application;

   #endregion

   #region Events

   public override event IVoiceProvider.VoicesLoaded? OnVoicesLoaded;
   public override event IVoiceProvider.SpeakStarted? OnSpeakStarted;
   public override event IVoiceProvider.SpeakCompleted? OnSpeakCompleted;

   #endregion

   #region Constructor

   public WindowsVoiceProvider()
   {
      if (!Constants.IsWindows)
         _logger.LogError("WindowsVoiceProvider works only under Windows!");
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
      int calculatedVolume = calculateVolume(volume);

      StringBuilder sb = new();
      sb.Append("--speak");
      sb.Append($" -text \"{prepareText(text, voice, pitch, forceSSML)}\"");
      sb.Append($" -rate {calculatedRate}");
      sb.Append($" -volume {calculatedVolume}");
      sb.Append(string.IsNullOrEmpty(voiceName) ? string.Empty : $" -voice \"{voiceName.Replace('"', '\'')}\"");

      string args = sb.ToString();

      ProcessRunner pr = new();
      _processes.Add(pr);

      Process process = await pr.StartAsync(_applicationName, args, true, Encoding.UTF8);

      OnSpeakCompleted?.Invoke(text);
      _processes.Remove(pr);

      if (process.ExitCode is 0 or -1 or 137) //0 = normal ended, -1/137 = killed
      {
         _logger.LogDebug($"Text spoken: {text}");
         return true;
      }

      _logger.LogError(
         $"Could not speak the text: {text} - Exit code: {process.ExitCode} - Error: {pr.Error.BNDump()}");

      return false;
   }

   private async Task<List<Voice>?> getVoicesAsync()
   {
      ProcessRunner pr = new();

      Process process = await pr.StartAsync(_applicationName, "--voices", true, Encoding.UTF8);

      if (process.ExitCode == 0)
      {
         List<Voice> voices = [];

         List<string> lines = pr.Output.ToList();

         foreach (var line in lines)
         {
            if (string.IsNullOrEmpty(line)) continue;
            if (!line.BNStartsWith(ID_VOICE)) continue;
            
            string[] splittedString = line.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

            if (splittedString.Length == 6)
            {
               voices.Add(new Voice(splittedString[1], splittedString[2],
                  Util.Helper.StringToGender(splittedString[3]), splittedString[4],
                  splittedString[5]));
            }
            else
            {
               _logger.LogWarning($"Voice is invalid: {line}");
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
      if (voice != null && !string.IsNullOrEmpty(voice.Name))
         return voice.Name;
      
      _logger.LogWarning("'Voice' or 'Voice.Name' is null! Using the providers 'default' voice.");

      //return DefaultVoiceName;
      return string.Empty;

   }

   private static string prepareText(string text, Voice? voice, float pitch, bool forceSSML)
   {
      //TEST
      //wrapper.ForceSSML = false;

      if (!forceSSML) 
         return text.Replace('"', '\'');
      
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

      if (!(Math.Abs(rate - 1f) > Constants.FLOAT_TOLERANCE)) 
         return result;
      
      if (rate > 1f)
      {
         //larger than 1
         result = rate switch
         {
            >= 2.75f => 10,
            >= 2.6f and < 2.75f => 9,
            >= 2.35f and < 2.6f => 8,
            >= 2.2f and < 2.35f => 7,
            >= 2f and < 2.2f => 6,
            >= 1.8f and < 2f => 5,
            >= 1.6f and < 1.8f => 4,
            >= 1.4f and < 1.6f => 3,
            >= 1.2f and < 1.4f => 2,
            > 1f and < 1.2f => 1,
            _ => result
         };
      }
      else
      {
         //smaller than 1
         result = rate switch
         {
            <= 0.3f => -10,
            > 0.3f and <= 0.4f => -9,
            > 0.4f and <= 0.45f => -8,
            > 0.45f and <= 0.5f => -7,
            > 0.5f and <= 0.55f => -6,
            > 0.55f and <= 0.6f => -5,
            > 0.6f and <= 0.7f => -4,
            > 0.7f and <= 0.8f => -3,
            > 0.8f and <= 0.9f => -2,
            > 0.9f and < 1f => -1,
            _ => result
         };
      }

      return result;
   }

   #endregion
}

internal static class WindowsWrapper
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger(nameof(WindowsWrapper));

   /// <summary>
   /// BogaNetTTSWrapper.exe
   /// Version: 1.0.0.0
   /// Date: 21.07.2024 13:34
   /// SHA256: zyv1d27bIFRF0QsdDwY8doiSGpJ4+y79hNOqo+QhrVY=
   /// </summary>
   private const string WRAPPER_BINARY =
      "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bGV0ZWQAc3ludGhTcGVha0NvbXBsZXRlZABhZGRfU3BlYWtTdGFydGVkAHN5bnRoU3BlYWtTdGFydGVkAGdldF9TdGFja1RyYWNlAGdldF9Wb2ljZQBJbnN0YWxsZWRWb2ljZQBTZWxlY3RWb2ljZQBzZWxlY3RWb2ljZQBTZXRPdXRwdXRUb0RlZmF1bHRBdWRpb0RldmljZQBfcmV0dXJuQ29kZQBnZXRfQWdlAFZvaWNlQWdlAGdldF9NZXNzYWdlAGFkZF9Wb2ljZUNoYW5nZQBzeW50aFZvaWNlQ2hhbmdlAElEaXNwb3NhYmxlAFNldE91dHB1dFRvV2F2ZUZpbGUAc3BlYWtUb0ZpbGUAQ29uc29sZQBnZXRfTmFtZQB2b2ljZU5hbWUAX25hbWUAc2V0X1ZvbHVtZQBnZXRWb2x1bWUAV3JpdGVMaW5lAGdldF9OZXdMaW5lAGdldF9DdWx0dXJlAERpc3Bvc2UAVHJ5UGFyc2UAc2V0X1JhdGUAZ2V0UmF0ZQBnZXRfU3RhdGUAU3ludGhlc2l6ZXJTdGF0ZQBHdWlkQXR0cmlidXRlAERlYnVnZ2FibGVBdHRyaWJ1dGUAQ29tVmlzaWJsZUF0dHJpYnV0ZQBBc3NlbWJseVRpdGxlQXR0cmlidXRlAEFzc2VtYmx5VHJhZGVtYXJrQXR0cmlidXRlAFRhcmdldEZyYW1ld29ya0F0dHJpYnV0ZQBBc3NlbWJseUZpbGVWZXJzaW9uQXR0cmlidXRlAEFzc2VtYmx5Q29uZmlndXJhdGlvbkF0dHJpYnV0ZQBBc3NlbWJseURlc2NyaXB0aW9uQXR0cmlidXRlAENvbXBpbGF0aW9uUmVsYXhhdGlvbnNBdHRyaWJ1dGUAQXNzZW1ibHlQcm9kdWN0QXR0cmlidXRlAEFzc2VtYmx5Q29weXJpZ2h0QXR0cmlidXRlAEFzc2VtYmx5Q29tcGFueUF0dHJpYnV0ZQBSdW50aW1lQ29tcGF0aWJpbGl0eUF0dHJpYnV0ZQB2YWx1ZQBCb2dhTmV0VFRTV3JhcHBlci5leGUAc2V0X091dHB1dEVuY29kaW5nAFN5c3RlbS5SdW50aW1lLlZlcnNpb25pbmcAU3RyaW5nAFN5c3RlbS5TcGVlY2gAU3BlYWsAc3BlYWsAU3lzdGVtLkNvbGxlY3Rpb25zLk9iamVjdE1vZGVsAFNwZWFrU3NtbAB2b2wAUHJvZ3JhbQBTeXN0ZW0ATWFpbgBtaW4ASm9pbgBfdmVyc2lvbgBTeXN0ZW0uR2xvYmFsaXphdGlvbgBTeXN0ZW0uUmVmbGVjdGlvbgBBcmd1bWVudE51bGxFeGNlcHRpb24AZ2V0X0Rlc2NyaXB0aW9uAFN0cmluZ0NvbXBhcmlzb24AZ2V0X1ZvaWNlSW5mbwBDdWx0dXJlSW5mbwBjbGFtcABnZXRfR2VuZGVyAFZvaWNlR2VuZGVyAHNlbmRlcgBCb2dhTmV0VFRTV3JhcHBlcgBUZXh0V3JpdGVyAFNwZWVjaFN5bnRoZXNpemVyAHNwZWVjaFN5bnRoZXNpemVyAGdldF9FcnJvcgBJRW51bWVyYXRvcgBHZXRFbnVtZXJhdG9yAC5jdG9yAHdyaXRlRXJyAFN5c3RlbS5EaWFnbm9zdGljcwBHZXRJbnN0YWxsZWRWb2ljZXMAdm9pY2VzAFN5c3RlbS5SdW50aW1lLkludGVyb3BTZXJ2aWNlcwBTeXN0ZW0uUnVudGltZS5Db21waWxlclNlcnZpY2VzAERlYnVnZ2luZ01vZGVzAEdldENvbW1hbmRMaW5lQXJncwBTdGF0ZUNoYW5nZWRFdmVudEFyZ3MAU3BlYWtDb21wbGV0ZWRFdmVudEFyZ3MAU3BlYWtTdGFydGVkRXZlbnRBcmdzAFZvaWNlQ2hhbmdlRXZlbnRBcmdzAGFyZ3MAU3lzdGVtLlNwZWVjaC5TeW50aGVzaXMARXF1YWxzAENvbnRhaW5zAFN5c3RlbS5Db2xsZWN0aW9ucwBDb25jYXQARm9ybWF0AE9iamVjdABFbnZpcm9ubWVudABnZXRDTElBcmd1bWVudABnZXRfQ3VycmVudABnZXRfQ291bnQAd3JpdGVPdXQATW92ZU5leHQAU3lzdGVtLlRleHQAc3BlZWNoVGV4dAB0ZXh0AG1heAB3cml0ZUxvZ09ubHkASXNOdWxsT3JFbXB0eQAAAAB3TgBvACAAYQByAGcAdQBtAGUAbgB0AHMAIQAgAFUAcwBlACAAJwAtAC0AaABlAGwAcAAnACAAYQBzACAAYQByAGcAdQBtAGUAbgB0ACAAdABvACAAcwBlAGUAIABtAG8AcgBlACAAZABlAHQAYQBpAGwAcwAuAAEXQQByAGcAdQBtAGUAbgB0AHMAOgAgAAADIAAAES0ALQB2AG8AaQBjAGUAcwABDy0ALQBzAHAAZQBhAGsAARstAC0AcwBwAGUAYQBrAFQAbwBGAGkAbABlAAETLQAtAHYAZQByAHMAaQBvAG4AATNCAG8AZwBhAE4AZQB0AFQAVABTAFcAcgBhAHAAcABlAHIAIAAtACAAMQAuADAALgAwAAENLQAtAGgAZQBsAHAAARVBAHIAZwB1AG0AZQBuAHQAcwA6AAAVLQAtAC0ALQAtAC0ALQAtAC0ALQABbS0ALQB2AG8AaQBjAGUAcwAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgACAAUgBlAHQAdQByAG4AcwAgAGEAbABsACAAYQB2AGEAaQBsAGEAYgBsAGUAIAB2AG8AaQBjAGUAcwAuAAGAny0ALQBzAHAAZQBhAGsAIAAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgACAAUwBwAGUAYQBrAHMAIABhACAAdABlAHgAdAAgAHcAaQB0AGgAIABhAG4AIABvAHAAdABpAG8AbgBhAGwAIAByAGEAdABlACwAIAB2AG8AbAB1AG0AZQAgAGEAbgBkACAAdgBvAGkAYwBlAC4AAU0gACAALQB0AGUAeAB0ACAAPAB0AGUAeAB0AD4AIAAgACAAIAAgACAAIAAgACAAIAAgAFQAZQB4AHQAIAB0AG8AIABzAHAAZQBhAGsAAYCfIAAgAC0AcgBhAHQAZQAgADwAcgBhAHQAZQA+ACAAIAAgACAAIAAgACAAIAAgACAAIABTAHAAZQBlAGQAIAByAGEAdABlACAAYgBlAHQAdwBlAGUAbgAgAC0AMQAwACAALQAgADEAMAAgAG8AZgAgAHQAaABlACAAcwBwAGUAYQBrAGUAcgAgACgAbwBwAHQAaQBvAG4AYQBsACkALgABgJUgACAALQB2AG8AbAB1AG0AZQAgADwAdgBvAGwAdQBtAGUAPgAgACAAIAAgACAAIAAgAFYAbwBsAHUAbQBlACAAYgBlAHQAdwBlAGUAbgAgADAAIAAtACAAMQAwADAAIABvAGYAIAB0AGgAZQAgAHMAcABlAGEAawBlAHIAIAAoAG8AcAB0AGkAbwBuAGEAbAApAC4AAYCLIAAgAC0AdgBvAGkAYwBlACAAPAB2AG8AaQBjAGUATgBhAG0AZQA+ACAAIAAgACAAIABOAGEAbQBlACAAbwBmACAAdABoAGUAIAB2AG8AaQBjAGUAIABmAG8AcgAgAHQAaABlACAAcwBwAGUAZQBjAGgAIAAoAG8AcAB0AGkAbwBuAGEAbAApAC4AAYCzLQAtAHMAcABlAGEAawBUAG8ARgBpAGwAZQAgACAAIAAgACAAIAAgACAAIAAgACAAIABTAHAAZQBhAGsAcwAgAGEAIAB0AGUAeAB0ACAAdABvACAAYQAgAGYAaQBsAGUAIAB3AGkAdABoACAAYQBuACAAbwBwAHQAaQBvAG4AYQBsACAAcgBhAHQAZQAsACAAdgBvAGwAdQBtAGUAIABhAG4AZAAgAHYAbwBpAGMAZQAuAAFbIAAgAC0AZgBpAGwAZQAgADwAZgBpAGwAZQBQAGEAdABoAD4AIAAgACAAIAAgACAAIABOAGEAbQBlACAAbwBmACAAbwB1AHQAcAB1AHQAIABmAGkAbABlAC4AAWstAC0AdgBlAHIAcwBpAG8AbgAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgAFYAZQByAHMAaQBvAG4AIABvAGYAIAB0AGgAaQBzACAAYQBwAHAAbABpAGMAYQB0AGkAbwBuAC4AAVUtAC0AaABlAGwAcAAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgACAAIAAgAFQAaABpAHMAIABpAG4AZgBvAHIAbQBhAHQAaQBvAG4ALgABaVYAaQBzAGkAdAAgACcAaAB0AHQAcABzADoALwAvAHcAdwB3AC4AYwByAG8AcwBzAHQAYQBsAGUAcwAuAGMAbwBtACcAIABmAG8AcgAgAG0AbwByAGUAIABkAGUAdABhAGkAbABzAC4AASNVAG4AawBuAG8AdwBuACAAYwBvAG0AbQBhAG4AZAA6ACAAAA9AAFYATwBJAEMARQBTAAArTgB1AG0AYgBlAHIAIABvAGYAIAB2AG8AaQBjAGUAcwA6ACAAewAwAH0AADVAAFYATwBJAEMARQA6AHsAMAB9ADoAewAxAH0AOgB7ADIAfQA6AHsAMwB9ADoAewA0AH0AAF9XAEEAUgBOAEkATgBHADoAIABWAG8AaQBjAGUAIABpAHMAIABkAGkAcwBhAGIAbABlAGQAOgAgAHsAMAB9ADoAewAxAH0AOgB7ADIAfQA6AHsAMwB9ADoAewA0AH0AAAtAAEQATwBOAEUAAAstAHQAZQB4AHQAAUVBAHIAZwB1AG0AZQBuAHQAIAAnAC0AdABlAHgAdAAnACAAaQBzACAAbgB1AGwAbAAgAG8AcgAgAGUAbQBwAHQAeQAhAAELLQByAGEAdABlAAEPLQB2AG8AbAB1AG0AZQABDS0AdgBvAGkAYwBlAAENQABTAFAARQBBAEsAAAstAGYAaQBsAGUAAUVBAHIAZwB1AG0AZQBuAHQAIAAnAC0AZgBpAGwAZQAnACAAaQBzACAAbgB1AGwAbAAgAG8AcgAgAGUAbQBwAHQAeQAhAAEZQABTAFAARQBBAEsAVABPAEYASQBMAEUAABE8AC8AcwBwAGUAYQBrAD4AAD9WAG8AaQBjAGUAIABoAGEAZAAgAGkAbgB2AGEAbABpAGQAIABzAGUAdAB0AGkAbgBnAHMAOgAgAHsAMAB9AAApQwBvAHUAbABkACAAbgBvAHQAIABzAHAAZQBhAGsAOgAgAHsAMAB9AAAzRQBSAFIATwBSADoAIABWAG8AaQBjAGUAIABuAG8AdAAgAGYAbwB1AG4AZAA6ACAAJwABAycAAVVXAEEAUgBOAEkATgBHADoAIABBAHIAZwB1AG0AZQBuAHQAIAAtAHIAYQB0AGUAIABpAHMAIABuAG8AdAAgAGEAIABuAHUAbQBiAGUAcgA6ACAAJwABXVcAQQBSAE4ASQBOAEcAOgAgAEEAcgBnAHUAbQBlAG4AdAAgACcALQB2AG8AbAB1AG0AZQAnACAAaQBzACAAbgBvAHQAIABhACAAbgB1AG0AYgBlAHIAOgAgACcAARFAAFMAVABBAFIAVABFAEQAABVAAEMATwBNAFAATABFAFQARQBEAABLQwB1AHIAcgBlAG4AdAAgAHMAdABhAHQAZQAgAG8AZgAgAHQAaABlACAAcwB5AG4AdABoAGUAcwBpAHoAZQByADoAIAB7ADAAfQAAL04AYQBtAGUAIABvAGYAIAB0AGgAZQAgAG4AZQB3ACAAdgBvAGkAYwBlADoAIAAAQZWnZyYGik+uSKOLIx2DUgAEIAEBCAMgAAEFIAEBEREEIAEBDgQgAQECCQcEDhJFEkUSRQQAABJtBQABARJtBgACDg4dDgUA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";

   private const string APPLICATION_NAME = "BogaNetTTSWrapper.exe";
   private static readonly string _path = FileHelper.Combine(FileHelper.TempPath, APPLICATION_NAME);

   #endregion

   #region Properties

   public static string Application
   {
      get
      {
         try
         {
            if (!FileHelper.ExistsFile(_path))
               Base64.FileFromBase64(_path, WRAPPER_BINARY);
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, "Could not write the TTS-wrapper to the destination!");
         }

         return _path;
      }
   }

   #endregion

   #region Static block

   static WindowsWrapper()
   {
      try
      {
         if (FileHelper.ExistsFile(_path))
            FileHelper.DeleteFile(_path);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not delete the TTS-wrapper in the destination!");
      }
   }

   #endregion
}