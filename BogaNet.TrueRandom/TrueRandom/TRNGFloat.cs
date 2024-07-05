using System;
using Microsoft.Extensions.Logging;

namespace BogaNet.TrueRandom;

/// <summary>
/// Generates true random floats in configurable intervals.
/// </summary>
public abstract class TRNGFloat
{
   #region Variables

   private static readonly ILogger<TRNGFloat> _logger = GlobalLogging.CreateLogger<TRNGFloat>();
   private static System.Collections.Generic.List<float> result = new System.Collections.Generic.List<float>();

   private static bool isRunning;

   #endregion

   #region Static properties

   /// <summary>Returns the list of floats from the last generation.</summary>
   /// <returns>List of floats from the last generation.</returns>
   public static System.Collections.Generic.List<float> Result => result;

   #endregion


   #region Public methods

   /// <summary>Generates random floats.</summary>
   /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
   /// <param name="silent">Ignore callbacks (default: false, optional)</param>
   /// <param name="id">id to identify the generated result (optional)</param>
   public static async System.Threading.Tasks.Task<System.Collections.Generic.List<float>> Generate(float min, float max, int number = 1, bool prng = false, bool silent = false, string id = "")
   {
      float _min = Math.Min(min, max);
      float _max = Math.Max(min, max);
      int _number;

      if (prng)
      {
         _number = Math.Clamp(number, 1, int.MaxValue);
      }
      else
      {
         if (number > 10000)
            _logger.LogWarning("'number' is larger than 10'000 - returning 10'000 floats.");

         _min = Math.Clamp(_min, -1000000000f, 1000000000f);
         _max = Math.Clamp(_max, -1000000000f, 1000000000f);
         _number = Math.Clamp(number, 1, 10000);
      }

      if (Math.Abs(_min - _max) < Constants.FLOAT_TOLERANCE)
      {
         result = GeneratePRNG(_min, _max, _number, TrueRandomNumberGenerator.Seed);
      }
      else
      {
         if (prng)
         {
            result = GeneratePRNG(_min, _max, _number, TrueRandomNumberGenerator.Seed);
         }
         else
         {
            if (!isRunning)
            {
               if (true)
               //if (Crosstales.Common.Util.NetworkHelper.isInternetAvailable)
               {
                  isRunning = true;

                  double factorMax = Math.Abs(_max) > Constants.FLOAT_TOLERANCE ? 1000000000f / Math.Abs(_max) : 1f;
                  double factorMin = Math.Abs(_min) > Constants.FLOAT_TOLERANCE ? 1000000000f / Math.Abs(_min) : 1f;

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
                     factor = Math.Abs(_min) > Constants.FLOAT_TOLERANCE ? factorMin : factorMax;
                  }

                  await TRNGInteger.Generate((int)(_min * factor), (int)(_max * factor), _number, false, true);

                  result.Clear();
                  foreach (int value in TRNGInteger.Result)
                  {
                     result.Add(value / (float)factor);
                  }
               }
               else
               {
                  const string msg = "No Internet access available - using standard prng now!";
                  _logger.LogWarning(msg);

                  result = GeneratePRNG(_min, _max, _number, TrueRandomNumberGenerator.Seed);
               }

               isRunning = false;
            }
            else
            {
               _logger.LogWarning("There is already a request running - please try again later!");
            }
         }
      }

      return result;
   }

   /// <summary>Generates random floats with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="min">Smallest possible number</param>
   /// <param name="max">Biggest possible number</param>
   /// <param name="number">How many numbers you want to generate (default: 1, optional)</param>
   /// <param name="seed">Seed for the PRNG (default: 0 (=standard), optional)</param>
   /// <returns>List with the generated floats.</returns>
   public static System.Collections.Generic.List<float> GeneratePRNG(float min, float max, int number = 1, int seed = 0)
   {
      System.Random rnd = seed == 0 ? new System.Random() : new System.Random(seed);
      int _number = Math.Abs(number);
      float _min = Math.Min(min, max);
      float _max = Math.Max(min, max);

      System.Collections.Generic.List<float> _result = new System.Collections.Generic.List<float>(_number);

      for (int ii = 0; ii < _number; ii++)
      {
         _result.Add((float)(rnd.NextDouble() * (_max - _min) + _min));
      }

      return _result;
   }

   #endregion
}