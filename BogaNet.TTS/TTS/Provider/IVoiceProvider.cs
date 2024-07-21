using System.Collections.Generic;
using BogaNet.TTS.Model;
using System.Threading.Tasks;

namespace BogaNet.TTS.Provider;

/// <summary>Interface for all voice providers.</summary>
public interface IVoiceProvider
{
   #region Properties

/*
      /// <summary>Returns the default voice name of the current TTS-provider.</summary>
      /// <returns>Default voice name of the current TTS-provider.</returns>
      string DefaultVoiceName { get; }
*/
   /// <summary>Get all available voices from the current TTS-provider and fills it into a given list.</summary>
   /// <returns>All available voices (alphabetically ordered by 'Name') as a list.</returns>
   List<Voice> Voices { get; }

   /// <summary>Maximal length of the speech text (in characters).</summary>
   /// <returns>The maximal length of the speech text.</returns>
   int MaxTextLength { get; }

   /// <summary>Indicates if this provider is supporting the current platform.</summary>
   /// <returns>True if this provider supports current platform.</returns>
   bool IsPlatformSupported { get; }

   /// <summary>Indicates if this provider is supporting SSML.</summary>
   /// <returns>True if this provider supports SSML.</returns>
   bool IsSSMLSupported { get; }

   /// <summary>Get all available cultures from the current provider (ISO 639-1).</summary>
   /// <returns>All available cultures (alphabetically ordered by 'Culture') as a list.</returns>
   List<string> Cultures { get; }

   #endregion

   #region Methods

   /// <summary>Get all available voices from the current TTS-provider as a list.</summary>
   /// <returns>All available voices (alphabetically ordered by 'Name') as a list.</returns>
   List<Voice> GetVoices();

   /// <summary>Get all available voices from the current TTS-provider as a list asynchronously.</summary>
   /// <returns>All available voices (alphabetically ordered by 'Name') as a list.</returns>
   Task<List<Voice>> GetVoicesAsync();

   /// <summary>Silence all active TTS speeches.</summary>
   void Silence();

   /// <summary>The current provider speaks a text with a given voice.</summary>
   /// <param name="text">Text to speak.</param>
   /// <param name="voice">Voice to speak (optional).</param>
   /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, values: 0.01-3, default: 1, optional).</param>
   /// <param name="pitch">Pitch of the speech in percent (1 = 100%, values: 0-2, default: 1, optional).</param>
   /// <param name="volume">Volume of the speaker in percent (1 = 100%, values: 0.01-1, default: 1, optional).</param>
   /// <param name="forceSSML">Force SSML on supported platforms (default: true, optional).</param>
   /// <returns>True if the speech was successful</returns>
   bool Speak(string text, Voice? voice = null, float rate = 1f, float pitch = 1f, float volume = 1f, bool forceSSML = true);

   /// <summary>The current provider speaks a text with a given voice asynchronously.</summary>
   /// <param name="text">Text to speak.</param>
   /// <param name="voice">Voice to speak (optional).</param>
   /// <param name="rate">Speech rate of the speaker in percent (1 = 100%, values: 0.01-3, default: 1, optional).</param>
   /// <param name="pitch">Pitch of the speech in percent (1 = 100%, values: 0-2, default: 1, optional).</param>
   /// <param name="volume">Volume of the speaker in percent (1 = 100%, values: 0.01-1, default: 1, optional).</param>
   /// <param name="forceSSML">Force SSML on supported platforms (default: true, optional).</param>
   /// <returns>True if the speech was successful</returns>
   Task<bool> SpeakAsync(string text, Voice? voice = null, float rate = 1f, float pitch = 1f, float volume = 1f, bool forceSSML = true);

   #endregion
}