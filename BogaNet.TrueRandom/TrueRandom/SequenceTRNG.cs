﻿using System.Linq;
using System;
using BogaNet.Extension;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.Helper;

namespace BogaNet.TrueRandom;

/// <summary>
/// Randomizes a given interval of integers, i.e. arrange them in random order.
/// </summary>
public abstract class SequenceTRNG : BaseTRNG //NUnit
{
   #region Variables

   private static readonly ILogger<SequenceTRNG> _logger = GlobalLogging.CreateLogger<SequenceTRNG>();

   #endregion

   #region Properties

   /// <summary>Returns the sequence from the last generation.</summary>
   /// <returns>Sequence from the last generation.</returns>
   public static List<int> Result { get; private set; } = [];

   #endregion

   #region Public methods

   /// <summary>
   /// Calculates needed bits (from the quota) for generating a random sequence.
   /// NOTE: The calculated value is an approximation and will may differ from the real quota deducted from the server.
   /// </summary>
   /// <param name="min">Start of the interval</param>
   /// <param name="max">End of the interval</param>
   /// <returns>Needed bits for generating the sequence.</returns>
   public static int CalcBits(int min, int max)
   {
      int minValue = Math.Min(min, max);
      int maxValue = Math.Max(min, max);

      if (minValue == 0 && maxValue == 0)
         maxValue = 1;

      int bitsCounter = 0;

      if (minValue != maxValue)
         bitsCounter = (maxValue - minValue) * 36;

      return bitsCounter;
   }

   /// <summary>Generates random sequence.</summary>
   /// <param name="min">Start of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">End of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you have in the result (optional, max range: max - min)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (optional, default: false)</param>
   /// <returns>List with the generated sequence.</returns>
   public static List<int> Generate(int min, int max, int number = 0, bool prng = false)
   {
      return Task.Run(() => GenerateAsync(min, max, number, prng)).GetAwaiter().GetResult();
   }

   /// <summary>Generates random sequence asynchronously.</summary>
   /// <param name="min">Start of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">End of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you have in the result (optional, max range: max - min)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (optional, default: false)</param>
   /// <returns>List with the generated sequence.</returns>
   public static async Task<List<int>> GenerateAsync(int min, int max, int number = 0, bool prng = false)
   {
      int minValue = Math.Clamp(Math.Min(min, max), -1000000000, 1000000000);
      int maxValue = Math.Clamp(Math.Max(min, max), -1000000000, 1000000000);

      if (maxValue - minValue >= 10000)
      {
         _logger.LogWarning($"Sequence range ('max' - 'min') is larger than 10'000 elements: {maxValue - minValue + 1}! Setting max = min + 10000.");
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

            string[] result = await NetworkHelper.ReadAllLinesAsync(url);

            Result.Clear();

            int value = 0;
            foreach (string unused in result.Where(valueAsString => int.TryParse(valueAsString, out value)))
            {
               Result.Add(value);
            }
         }
         else
         {
            _logger.LogWarning("Quota exceeded - using standard prng!");
            Result = GeneratePRNG(minValue, maxValue, Seed);
         }

         _isRunning = false;
      }
      else
      {
         _logger.LogWarning("There is already a request running - please try again later!");
      }

      if (number > 0 && number < Result.Count)
         Result = Result.GetRange(0, number);

      return Result;
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