using System.Linq;
using System;
using BogaNet.Extension;
using Microsoft.Extensions.Logging;

namespace BogaNet.TrueRandom;

/// <summary>
/// This module will randomize a given interval of integers, i.e. arrange them in random order.
/// </summary>
public abstract class ModuleSequence
{
   #region Variables

   private static readonly ILogger<ModuleSequence> _logger = GlobalLogging.CreateLogger<ModuleSequence>();

   private static System.Collections.Generic.List<int> result = new System.Collections.Generic.List<int>();

   private static bool isRunning;

   #endregion


   #region Static properties

   /// <summary>Returns the sequence from the last generation.</summary>
   /// <returns>Sequence from the last generation.</returns>
   public static System.Collections.Generic.List<int> Result => new System.Collections.Generic.List<int>(result);

   #endregion


   #region Public methods

   /// <summary>Generates random sequence.</summary>
   /// <param name="min">Start of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">End of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you have in the result (max range: max - min, optional)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
   /// <param name="silent">Ignore callbacks (default: false, optional)</param>
   /// <param name="id">id to identify the generated result (optional)</param>
   public static async System.Threading.Tasks.Task<System.Collections.Generic.List<int>> Generate(int min, int max, int number = 0, bool prng = false, bool silent = false, string id = "")
   {
      int _min = Math.Min(min, max);
      int _max = Math.Max(min, max);

      if (!prng && _max - _min >= 10000)
      {
         _logger.LogError("Sequence range ('max' - 'min') is larger than 10'000 elements: " + (_max - _min + 1), id);
      }
      else
      {
         if (_min == _max)
         {
            result = GeneratePRNG(_min, _max, TrueRandomNumberGenerator.Seed);
         }
         else
         {
            if (prng)
            {
               result = GeneratePRNG(_min, _max, TrueRandomNumberGenerator.Seed);
            }
            else
            {
               if (!isRunning)
               {
                  _min = Math.Clamp(Math.Min(min, max), -1000000000, 1000000000);
                  _max = Math.Clamp(Math.Max(min, max), -1000000000, 1000000000);

                  isRunning = true;

                  if (true)
                     //if (Crosstales.Common.Util.NetworkHelper.isInternetAvailable)
                  {
                     _logger.LogDebug("Quota before: " + ModuleQuota.Quota);

                     if (ModuleQuota.Quota > 0)
                     {
                        string url = $"{TrueRandomNumberGenerator.GENERATOR_URL}sequences/?min={_min}&max={_max}&col=1&format=plain&rnd=new";

                        _logger.LogDebug("URL: " + url);


                        using System.Net.Http.HttpClient client = new();
                        using System.Net.Http.HttpResponseMessage response = client.GetAsync(url).Result;
                        response.EnsureSuccessStatusCode();

                        if (response.IsSuccessStatusCode)
                        {
                           string data = await response.Content.ReadAsStringAsync();

                           result.Clear();
                           string[] _result = System.Text.RegularExpressions.Regex.Split(data, "\r\n?|\n", System.Text.RegularExpressions.RegexOptions.Singleline);

                           int value = 0;
                           foreach (string valueAsString in _result.Where(valueAsString => int.TryParse(valueAsString, out value)))
                           {
                              result.Add(value);
                           }
                        }
                        else
                        {
                           _logger.LogError($"Could not download data: {response.StatusCode} - {response.ReasonPhrase}");
                        }


                        //if (Config.DEBUG)
                        //   Debug.Log("Quota after: " + ModuleQuota.Quota);
                     }
                     else
                     {
                        const string msg = "Quota exceeded - using standard prng now!";
                        _logger.LogWarning(msg);

                        result = GeneratePRNG(_min, _max, TrueRandomNumberGenerator.Seed);
                     }
                  }
                  else
                  {
                     const string msg = "No Internet access available - using standard prng now!";
                     _logger.LogWarning(msg);

                     result = GeneratePRNG(_min, _max, TrueRandomNumberGenerator.Seed);
                  }

                  isRunning = false;
               }
               else
               {
                  _logger.LogWarning("There is already a request running - please try again later!");
               }
            }
         }

         if (number > 0 && number < result.Count)
            result = result.GetRange(0, number);
      }

      return result;
   }

   /// <summary>Generates a random sequence with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="min">Start of the interval</param>
   /// <param name="max">End of the interval</param>
   /// <param name="number">How many numbers you have in the result (max range: max - min, optional)</param>
   /// <param name="seed">Seed for the PRNG (default: 0 (=standard), optional)</param>
   /// <returns>List with the generated sequence.</returns>
   public static System.Collections.Generic.List<int> GeneratePRNG(int min, int max, int number = 0, int seed = 0)
   {
      int _min = Math.Min(min, max);
      int _max = Math.Max(min, max);
      System.Collections.Generic.List<int> _result = new System.Collections.Generic.List<int>(_max - _min + 1);

      for (int ii = _min; ii <= _max; ii++)
      {
         _result.Add(ii);
      }

      _result.BNShuffle(seed);

      if (number > 0 && number < _result.Count)
         return _result.GetRange(0, number);

      return _result;
   }

   #endregion
}