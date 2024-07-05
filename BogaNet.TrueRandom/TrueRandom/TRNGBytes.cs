using System;
using Microsoft.Extensions.Logging;

namespace BogaNet.TrueRandom;

/// <summary>
/// Generates true random bytes in configurable intervals.
/// </summary>
public abstract class TRNGBytes
{
   #region Variables

   private static readonly ILogger<TRNGBytes> _logger = GlobalLogging.CreateLogger<TRNGBytes>();
   private static System.Collections.Generic.List<byte[]> result = new System.Collections.Generic.List<byte[]>();

   private static bool isRunning;

   #endregion

   #region Static properties

   /// <summary>Returns the list of byte-arrays from the last generation.</summary>
   /// <returns>List of byte-arrays from the last generation.</returns>
   public static System.Collections.Generic.List<byte[]> Result => result;

   #endregion


   #region Public methods

   /// <summary>Generates random byte-arrays.</summary>
   /// <param name="length">Defines how many bytes the byte-arrays contains (range: 1 - 20)</param>
   /// <param name="number">How many byte-arrays to generate (range: 1 - 10'000, default: 1, optional)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
   public static async System.Threading.Tasks.Task<System.Collections.Generic.List<byte[]>> Generate(int length, int number = 1, bool prng = false)
   {
      //TODO magic!
/*      
      int _number;

      if (prng)
      {
         _number = Math.Clamp(number, 1, int.MaxValue);
      }
      else
      {
         if (number > 10000)
            _logger.LogWarning("'number' is larger than 10'000 - returning 10'000 floats.");

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
*/
      return result;
   }

   /// <summary>Generates random floats with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="min">Smallest possible number</param>
   /// <param name="max">Biggest possible number</param>
   /// <param name="number">How many numbers you want to generate (default: 1, optional)</param>
   /// <param name="seed">Seed for the PRNG (default: 0 (=standard), optional)</param>
   /// <returns>List with the generated floats.</returns>
   public static System.Collections.Generic.List<byte[]> GeneratePRNG(int length, int number = 1, int seed = 0)
   {
      System.Random rnd = seed == 0 ? new System.Random() : new System.Random(seed);
      int _number = Math.Abs(number);

      System.Collections.Generic.List<byte[]> _result = new System.Collections.Generic.List<byte[]>(_number);
//TODO do the magic
      /*
      for (int ii = 0; ii < _number; ii++)
      {
         _result.Add((float)(rnd.NextDouble() * (_max - _min) + _min));
      }
*/
      return _result;
   }

   #endregion
}