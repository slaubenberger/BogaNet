using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace BogaNet.TrueRandom;

/// <summary>
/// Generates true random integers in configurable intervals.
/// </summary>
public abstract class TRNGInteger
{
   #region Variables

   private static readonly ILogger<TRNGInteger> _logger = GlobalLogging.CreateLogger<TRNGInteger>();

   //[Tooltip("List of the generated integers.")]
   private static System.Collections.Generic.List<int> result = new System.Collections.Generic.List<int>();

   private static bool isRunning;

   #endregion


   #region Static properties

   /// <summary>Returns the list of integers from the last generation.</summary>
   /// <returns>List of integers from the last generation.</returns>
   public static System.Collections.Generic.List<int> Result => new System.Collections.Generic.List<int>(result);

   #endregion


   #region Public methods

   /// <summary>Generates random integers.</summary>
   /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
   /// <param name="silent">Ignore callbacks (default: false, optional)</param>
   /// <param name="id">id to identify the generated result (optional)</param>
   public static async System.Threading.Tasks.Task<System.Collections.Generic.List<int>> Generate(int min, int max, int number = 1, bool prng = false, bool silent = false, string id = "")
   {
      int _min = Math.Min(min, max);
      int _max = Math.Max(min, max);
      int _number;

      if (prng)
      {
         _number = Math.Clamp(number, 1, int.MaxValue);
      }
      else
      {
         if (number > 10000)
            _logger.LogWarning("'number' is larger than 10'000 - returning 10'000 integers.");

         _min = Math.Clamp(_min, -1000000000, 1000000000);
         _max = Math.Clamp(_max, -1000000000, 1000000000);
         _number = Math.Clamp(number, 1, 10000);
      }

      if (_min == _max)
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
               isRunning = true;

               if (true)
               //if (Crosstales.Common.Util.NetworkHelper.isInternetAvailable)
               {
                  _logger.LogDebug("Quota before: " + CheckQuota.Quota);

                  if (CheckQuota.Quota > 0)
                  {
                     string url = $"{TrueRandomNumberGenerator.GENERATOR_URL}integers/?num={_number}&min={_min}&max={_max}&col=1&base=10&format=plain&rnd=new";

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

                     result = GeneratePRNG(_min, _max, _number, TrueRandomNumberGenerator.Seed);
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

   /// <summary>Generates random integers with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="min">Smallest possible number</param>
   /// <param name="max">Biggest possible number</param>
   /// <param name="number">How many numbers you want to generate (default: 1, optional)</param>
   /// <param name="seed">Seed for the PRNG (default: 0 (=standard), optional)</param>
   /// <returns>List with the generated integers.</returns>
   public static System.Collections.Generic.List<int> GeneratePRNG(int min, int max, int number = 1, int seed = 0)
   {
      System.Random rnd = seed == 0 ? new System.Random() : new System.Random(seed);
      int _number = Math.Abs(number);
      int _min = Math.Min(min, max);
      int _max = Math.Max(min, max);

      System.Collections.Generic.List<int> _result = new System.Collections.Generic.List<int>(_number);

      for (int ii = 0; ii < _number; ii++)
      {
         _result.Add(rnd.Next(_min, _max + 1));
      }

      return _result;
   }

   #endregion
}