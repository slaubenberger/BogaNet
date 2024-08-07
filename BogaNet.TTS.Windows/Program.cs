#if true //change this to false if .NET 4.8 is not installed
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;

namespace BogaNet.TTS
{
   internal class Program
   {
      #region Variables

      private const string _name = "BogaNetTTSWrapper";
      private const string _version = "1.0.0";
      private static int _returnCode = 0;

#if DEBUG
      private static readonly string logFile = $"{System.IO.Path.GetTempPath()}{_name}_{DateTime.Now:yyyyMMddHHmmsss}.log";
#endif

      #endregion


      #region Main method

      public static int Main(string[] args)
      {
#if DEBUG
         Console.WriteLine(logFile);
#endif
         Console.OutputEncoding = Encoding.UTF8;

         if (args.Length == 0)
         {
            writeErr("No arguments! Use '--help' as argument to see more details.");
            _returnCode = 1;
         }
         else
         {
            writeOut($"Arguments: {string.Join(" ", args)}", true);

            string command = args[0];

            if (command.Equals("--voices", StringComparison.InvariantCultureIgnoreCase))
            {
               try
               {
                  voices();
               }
               catch (Exception ex)
               {
                  writeErr(ex.Message + Environment.NewLine + ex.StackTrace);
                  _returnCode = 10;
               }
            }
            else if (command.Equals("--speak", StringComparison.InvariantCultureIgnoreCase))
            {
               try
               {
                  speak();
               }
               catch (Exception ex)
               {
                  writeErr(ex.Message + Environment.NewLine + ex.StackTrace);
                  _returnCode = 20;
               }
            }
            else if (command.Equals("--speakToFile", StringComparison.InvariantCultureIgnoreCase))
            {
               try
               {
                  speakToFile();
               }
               catch (Exception ex)
               {
                  writeErr(ex.Message + Environment.NewLine + ex.StackTrace);
                  _returnCode = 30;
               }
            }
            else if (command.Equals("--version", StringComparison.InvariantCultureIgnoreCase))
            {
               Console.WriteLine($"{_name} - {_version}");
            }
            else if (command.Equals("--help", StringComparison.InvariantCultureIgnoreCase))
            {
               Console.WriteLine($"{_name} - {_version}");
               Console.WriteLine(" ");
               Console.WriteLine("Arguments:");
               Console.WriteLine("----------");
               Console.WriteLine("--voices                 Returns all available voices.");
               Console.WriteLine(" ");
               Console.WriteLine("--speak                  Speaks a text with an optional rate, volume and voice.");
               Console.WriteLine("  -text <text>           Text to speak");
               Console.WriteLine("  -rate <rate>           Speed rate between -10 - 10 of the speaker (optional).");
               Console.WriteLine("  -volume <volume>       Volume between 0 - 100 of the speaker (optional).");
               Console.WriteLine("  -voice <voiceName>     Name of the voice for the speech (optional).");
               Console.WriteLine(" ");
               Console.WriteLine("--speakToFile            Speaks a text to a file with an optional rate, volume and voice.");
               Console.WriteLine("  -text <text>           Text to speak");
               Console.WriteLine("  -file <filePath>       Name of output file.");
               Console.WriteLine("  -rate <rate>           Speed rate between -10 - 10 of the speaker (optional).");
               Console.WriteLine("  -volume <volume>       Volume between 0 - 100 of the speaker (optional).");
               Console.WriteLine("  -voice <voiceName>     Name of the voice for the speech (optional).");
               Console.WriteLine(" ");
               Console.WriteLine("--version                Version of this application.");
               Console.WriteLine(" ");
               Console.WriteLine("--help                   This information.");
               Console.WriteLine(" ");
               Console.WriteLine(" ");
               Console.WriteLine("Visit 'https://www.crosstales.com' for more details.");
            }
            else
            {
               writeErr($"Unknown command: {command}");
               _returnCode = 50;
            }
         }

         return _returnCode;
      }

      #endregion


      #region Private methods

      private static string getCLIArgument(string name)
      {
         string[] args = Environment.GetCommandLineArgs();

         for (int ii = 0; ii < args.Length; ii++)
         {
            if (name.Equals(args[ii], StringComparison.OrdinalIgnoreCase) && args.Length > ii + 1)
               return args[ii + 1];
         }

         return null;
      }

      // Get all TTS voices
      private static void voices()
      {
         writeOut("@VOICES");

         using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer())
         {
            ReadOnlyCollection<InstalledVoice> installedVoices = speechSynthesizer.GetInstalledVoices();

            writeOut($"Number of voices: {installedVoices.Count}", true);

            foreach (InstalledVoice installedVoice in installedVoices)
            {
               if (installedVoice.Enabled)
               {
                  string voice = $"@VOICE:{installedVoice.VoiceInfo.Name}:{installedVoice.VoiceInfo.Description}:{installedVoice.VoiceInfo.Gender}:{installedVoice.VoiceInfo.Age}:{installedVoice.VoiceInfo.Culture}";
                  writeOut(voice);
               }
               else
               {
                  string voice = $"WARNING: Voice is disabled: {installedVoice.VoiceInfo.Name}:{installedVoice.VoiceInfo.Description}:{installedVoice.VoiceInfo.Gender}:{installedVoice.VoiceInfo.Age}:{installedVoice.VoiceInfo.Culture}";
                  writeOut(voice);
               }
            }
         }

         writeOut("@DONE");
      }

      // Speak a text with a given TTS voice to the default audio device
      private static void speak()
      {
         string text = getCLIArgument("-text");

         if (string.IsNullOrEmpty(text))
         {
            writeErr("Argument '-text' is null or empty!");
            _returnCode = 21;
            return;
         }

         string rate = getCLIArgument("-rate");
         string volume = getCLIArgument("-volume");
         string voice = getCLIArgument("-voice");

         writeOut("@SPEAK");
         using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer())
         {
            speechSynthesizer.SpeakStarted += synthSpeakStarted;
            speechSynthesizer.SpeakCompleted += synthSpeakCompleted;
            speechSynthesizer.StateChanged += synthStateChanged;
            speechSynthesizer.VoiceChange += synthVoiceChange;

            selectVoice(string.IsNullOrEmpty(voice) ? string.Empty : voice, speechSynthesizer);

            // Configure the audio output.
            speechSynthesizer.SetOutputToDefaultAudioDevice();

            speechSynthesizer.Rate = getRate(rate);
            speechSynthesizer.Volume = getVolume(volume);

            speak(text, speechSynthesizer);
         }

         writeOut("@DONE");
      }

      // Speak a text with a given TTS voice to a WAV file
      private static void speakToFile()
      {
         string text = getCLIArgument("-text");

         if (string.IsNullOrEmpty(text))
         {
            writeErr("Argument '-text' is null or empty!");
            _returnCode = 31;
            return;
         }

         string file = getCLIArgument("-file");

         if (string.IsNullOrEmpty(file))
         {
            writeErr("Argument '-file' is null or empty!");
            _returnCode = 32;
            return;
         }

         string rate = getCLIArgument("-rate");
         string volume = getCLIArgument("-volume");
         string voice = getCLIArgument("-voice");

         writeOut("@SPEAKTOFILE");

         using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer())
         {
            speechSynthesizer.SpeakStarted += synthSpeakStarted;
            speechSynthesizer.SpeakCompleted += synthSpeakCompleted;
            speechSynthesizer.StateChanged += synthStateChanged;
            speechSynthesizer.VoiceChange += synthVoiceChange;

            selectVoice(string.IsNullOrEmpty(voice) ? string.Empty : voice, speechSynthesizer);

            // Configure the audio output to WAV.
            speechSynthesizer.SetOutputToWaveFile(file);

            speechSynthesizer.Rate = getRate(rate);
            speechSynthesizer.Volume = getVolume(volume);

            speak(text, speechSynthesizer);
         }

         writeOut("@DONE");
         writeOut(file);
      }

      private static void speak(string speechText, SpeechSynthesizer speechSynthesizer)
      {
         try
         {
            if (speechText.Contains("</speak>"))
            {
               //writeOut("Speech SSML text: '" + speechText + "'");

               speechSynthesizer.SpeakSsml(speechText);
            }
            else
            {
               //writeOut("Speech text: '" + speechText + "'");

               speechSynthesizer.Speak(speechText);
            }
         }
         catch (ArgumentNullException ex) //typically happening with eSpeak voices
         {
            writeErr($"Voice had invalid settings: {ex}");
         }
         catch (Exception ex)
         {
            writeErr($"Could not speak: {ex}");
            _returnCode = 40;
         }
      }

      private static void selectVoice(string voiceName, SpeechSynthesizer speechSynthesizer)
      {
         bool found = false;

         if (!string.IsNullOrEmpty(voiceName))
         {
            if (speechSynthesizer.GetInstalledVoices().Any(installedVoice => installedVoice.VoiceInfo.Name.Equals(voiceName)))
            {
               speechSynthesizer.SelectVoice(voiceName);
               found = true;
            }
         }

         if (!found)
            writeOut($"ERROR: Voice not found: '{voiceName}'");
      }

      private static int clamp(int value, int min, int max)
      {
         return value < min ? min : value > max ? max : value;
      }

      private static int getRate(string ra)
      {
         int rate = 1;
         
         if (string.IsNullOrEmpty(ra)) 
            return rate;
         
         if (int.TryParse(ra, out rate))
         {
            rate = clamp(rate, -10, 10);
         }
         else
         {
            writeOut($"WARNING: Argument -rate is not a number: '{ra}'");
            rate = 1;
         }

         return rate;
      }

      private static int getVolume(string vol)
      {
         int volume = 100;
         
         if (string.IsNullOrEmpty(vol)) 
            return volume;
         
         if (int.TryParse(vol, out volume))
         {
            volume = clamp(volume, 0, 100);
         }
         else
         {
            writeOut($"WARNING: Argument '-volume' is not a number: '{vol}'");
            volume = 100;
         }

         return volume;
      }

      private static void writeOut(string text, bool writeLogOnly = false)
      {
#if DEBUG
         try
         {
            System.IO.File.AppendAllText(logFile, text + Environment.NewLine);
         }
         catch (Exception ex)
         {
            writeErr($"Could not write log file: {ex}");
         }
#endif

         if (!writeLogOnly)
            Console.WriteLine(text);
      }

      private static void writeErr(string text, bool writeLogOnly = false)
      {
#if DEBUG
         try
         {
            System.IO.File.AppendAllText(logFile, text + Environment.NewLine);
         }
         catch (Exception ex)
         {
            Console.Error.WriteLine($"Could not write log file: {ex}");
         }

#endif
         if (!writeLogOnly)
            Console.Error.WriteLine(text);
      }

      #endregion


      #region Events

      private static void synthSpeakStarted(object sender, SpeakStartedEventArgs e)
      {
         writeOut("@STARTED");
      }

      private static void synthSpeakCompleted(object sender, SpeakCompletedEventArgs e)
      {
         writeOut("@COMPLETED");
      }

      private static void synthStateChanged(object sender, StateChangedEventArgs e)
      {
         writeOut($"Current state of the synthesizer: {e.State}", true);
      }

      private static void synthVoiceChange(object sender, VoiceChangeEventArgs e)
      {
         writeOut($"Name of the new voice: {e.Voice.Name}", true);
      }

      #endregion
   }
}
#endif