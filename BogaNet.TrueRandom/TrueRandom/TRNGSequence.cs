using System.Linq;
using System;
using BogaNet.Extension;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.Helper;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace BogaNet.TrueRandom;

/// <summary>
/// Randomizes a given interval of integers, i.e. arrange them in random order.
/// </summary>
public abstract class TRNGSequence : TRNGBase
{
   #region Variables

   private static readonly ILogger<TRNGSequence> _logger = GlobalLogging.CreateLogger<TRNGSequence>();
   private static List<int> _result = [];

   #endregion


   #region Properties

   /// <summary>Returns the sequence from the last generation.</summary>
   /// <returns>Sequence from the last generation.</returns>
   public static List<int> Result => _result;

   #endregion


   #region Public methods

   /// <summary>
   /// Calculates needed bits (from the quota) for generating a random sequence.
   /// NOTE: The calculated value may differ from the real value due the calculation of the server.
   /// </summary>
   /// <param name="min">Start of the interval</param>
   /// <param name="max">End of the interval</param>
   /// <returns>Needed bits for generating the sequence.</returns>
   public static int CalcBits(int min, int max)
   {
      int _min = min;
      int _max = max;

      if (_min > _max)
      {
         _min = max;
         _max = min;
      }

      if (_min == 0 && _max == 0)
         _max = 1;

      int bitsCounter = 0;

      if (_min != _max)
         bitsCounter = (_max - _min) * 36;

      return bitsCounter;
   }

   /// <summary>Generates random sequence asynchronously.</summary>
   /// <param name="min">Start of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">End of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you have in the result (optional, max range: max - min)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (optional, default: false)</param>
   /// <returns>List with the generated sequence.</returns>
   public static async Task<List<int>> GenerateAsync(int min, int max, int number = 0, bool prng = false)
   {
      int minValue = Math.Min(min, max);
      int maxValue = Math.Max(min, max);
      minValue = Math.Clamp(Math.Min(min, max), -1000000000, 1000000000);
      maxValue = Math.Clamp(Math.Max(min, max), -1000000000, 1000000000);

      if (maxValue - minValue >= 10000)
      {
         _logger.LogError($"Sequence range ('max' - 'min') is larger than 10'000 elements: {maxValue - minValue + 1}! Setting max = min + 10000.");
         maxValue = minValue + 10000;
      }

      bool hasInternet = await NetworkHelper.CheckInternetAvailabilityAsync();

      if (!hasInternet)
         _logger.LogWarning("No Internet access available - using standard prng!");

      if (prng || !hasInternet)
         return GeneratePRNG(minValue, maxValue, Seed);

      if (!_isRunning)
      {
         _isRunning = true;

         if (await CheckQuota.GetQuotaAsync() > CalcBits(minValue, maxValue))
         {
            string url = $"{GENERATOR_URL}sequences/?min={minValue}&max={maxValue}&col=1&format=plain&rnd=new";

            _logger.LogDebug("URL: " + url);


            using HttpClient client = new();
            using HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
               string data = await response.Content.ReadAsStringAsync();

               _result.Clear();
               string[] result = Regex.Split(data, "\r\n?|\n", RegexOptions.Singleline);

               int value = 0;
               foreach (string valueAsString in result.Where(valueAsString => int.TryParse(valueAsString, out value)))
               {
                  _result.Add(value);
               }
            }
            else
            {
               _logger.LogError($"Could not download data: {response.StatusCode} - {response.ReasonPhrase}");
            }
         }
         else
         {
            _logger.LogWarning("Quota exceeded - using standard prng!");
            _result = GeneratePRNG(minValue, maxValue, Seed);
         }

         _isRunning = false;
      }
      else
      {
         _logger.LogWarning("There is already a request running - please try again later!");
      }

      if (number > 0 && number < _result.Count)
         _result = _result.GetRange(0, number);

      return _result;
   }

   /// <summary>Generates a random sequence with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="min">Start of the interval</param>
   /// <param name="max">End of the interval</param>
   /// <param name="number">How many numbers you have in the result (optional, max range: max - min)</param>
   /// <param name="seed">Seed for the PRNG (optional, default: 0 (=standard))</param>
   /// <returns>List with the generated sequence.</returns>
   public static List<int> GeneratePRNG(int min, int max, int number = 0, int seed = 0)
   {
      int _min = Math.Min(min, max);
      int _max = Math.Max(min, max);
      List<int> result = new(_max - _min + 1);

      for (int ii = _min; ii <= _max; ii++)
      {
         result.Add(ii);
      }

      result.BNShuffle(seed);

      if (number > 0 && number < result.Count)
         return result.GetRange(0, number);

      return result;
   }

   #endregion
}