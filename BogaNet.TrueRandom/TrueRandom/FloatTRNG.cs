using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.Helper;

namespace BogaNet.TrueRandom;

/// <summary>
/// Generates true random floats in configurable intervals.
/// </summary>
public abstract class FloatTRNG : BaseTRNG //NUnit
{
   #region Variables

   private static readonly ILogger<FloatTRNG> _logger = GlobalLogging.CreateLogger<FloatTRNG>();
   private static List<float> _result = [];

   #endregion

   #region Properties

   /// <summary>Returns the list of floats from the last generation.</summary>
   /// <returns>List of floats from the last generation.</returns>
   public static List<float> Result => _result;

   #endregion

   #region Public methods

   /// <summary>
   /// Calculates needed bits (from the quota) for generating random floats.
   /// NOTE: The calculated value is an approximation and will may differ from the real quota deducted from the server.
   /// </summary>
   /// <param name="number">How many numbers (optional, default: 1)</param>
   /// <returns>Needed bits for generating the floats.</returns>
   public static int CalcBits(int number = 1)
   {
      int bitsCounter = 32;
      return bitsCounter * Math.Abs(number);
   }

   /// <summary>Generates random floats.</summary>
   /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you want to generate (optional, range: 1 - 10'000, default: 1)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (optional, default: false)</param>
   /// <returns>List with the generated floats.</returns>
   public static List<float> Generate(float min, float max, int number = 1, bool prng = false)
   {
      return Task.Run(() => GenerateAsync(min, max, number, prng)).GetAwaiter().GetResult();
   }

   /// <summary>Generates random floats asynchronously.</summary>
   /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you want to generate (optional, range: 1 - 10'000, default: 1)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (optional, default: false)</param>
   /// <returns>List with the generated floats.</returns>
   public static async Task<List<float>> GenerateAsync(float min, float max, int number = 1, bool prng = false)
   {
      float minValue = Math.Clamp(Math.Min(min, max), -1000000000f, 1000000000f);
      float maxValue = Math.Clamp(Math.Max(min, max), -1000000000f, 1000000000f);
      int num = Math.Clamp(number, 1, 10000);

      bool hasInternet = await NetworkHelper.CheckInternetAvailabilityAsync();

      if (!hasInternet)
         _logger.LogWarning("No Internet access available - using standard prng!");

      if (prng || !hasInternet)
         _result = GeneratePRNG(minValue, maxValue, num, Seed);

      if (!_isRunning)
      {
         double factorMax = Math.Abs(maxValue) > Constants.FLOAT_TOLERANCE ? 1000000000f / Math.Abs(maxValue) : 1f;
         double factorMin = Math.Abs(minValue) > Constants.FLOAT_TOLERANCE ? 1000000000f / Math.Abs(minValue) : 1f;

         double factor;

         if (factorMax > factorMin && Math.Abs(factorMin - 1f) > Constants.FLOAT_TOLERANCE)
         {
            factor = factorMin;
         }
         else if (factorMin > factorMax && Math.Abs(factorMax - 1f) > Constants.FLOAT_TOLERANCE)
         {
            factor = factorMax;
         }
         else
         {
            factor = Math.Abs(minValue) > Constants.FLOAT_TOLERANCE ? factorMin : factorMax;
         }

         List<int> result = await IntegerTRNG.GenerateAsync((int)(minValue * factor), (int)(maxValue * factor), num);

         _result.Clear();
         foreach (int value in result)
         {
            _result.Add(value / (float)factor);
         }
      }
      else
      {
         _logger.LogWarning("There is already a request running - please try again later!");
      }

      return _result;
   }

   /// <summary>Generates random floats with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="min">Smallest possible number</param>
   /// <param name="max">Biggest possible number</param>
   /// <param name="number">How many numbers you want to generate (optional, default: 1)</param>
   /// <param name="seed">Seed for the PRNG (optional, default: 0 (=standard))</param>
   /// <returns>List with the generated floats.</returns>
   public static List<float> GeneratePRNG(float min, float max, int number = 1, int seed = 0)
   {
      Random rnd = seed == 0 ? new Random() : new Random(seed);
      int _number = Math.Abs(number);
      float _min = Math.Min(min, max);
      float _max = Math.Max(min, max);

      List<float> result = new(_number);

      for (int ii = 0; ii < _number; ii++)
      {
         result.Add(rnd.NextSingle() * (_max - _min) + _min);
      }

      return result;
   }

   #endregion
}