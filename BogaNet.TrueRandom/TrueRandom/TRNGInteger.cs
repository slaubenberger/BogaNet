using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BogaNet.Helper;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace BogaNet.TrueRandom;

/// <summary>
/// Generates true random integers in configurable intervals.
/// </summary>
public abstract class TRNGInteger : TRNGBase
{
   #region Variables

   private static readonly ILogger<TRNGInteger> _logger = GlobalLogging.CreateLogger<TRNGInteger>();
   private static List<int> _result = [];

   #endregion


   #region Properties

   /// <summary>Returns the list of integers from the last generation.</summary>
   /// <returns>List of integers from the last generation.</returns>
   public static List<int> Result => _result;

   #endregion


   #region Public methods

   /// <summary>
   /// Calculates needed bits (from the quota) for generating random integers.
   /// NOTE: The calculated value may differ from the real value due the calculation of the server.
   /// </summary>
   /// <param name="max">Biggest allowed number</param>
   /// <param name="number">How many numbers (default: 1, optional)</param>
   /// <returns>Needed bits for generating the integers.</returns>
   public static int CalcBits(int max, int number = 1)
   {
      int bitsCounter = 0;
      float tmp = Math.Abs(max);

      while (tmp >= 1)
      {
         if (Math.Abs(tmp % 2) < Constants.FLOAT_TOLERANCE)
         {
            tmp /= 2;
         }
         else
         {
            tmp /= 2;
            tmp -= 0.5f;
         }

         bitsCounter++;
      }

      return bitsCounter * Math.Abs(number);
   }

   /// <summary>Generates random integers asynchronously.</summary>
   /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you want to generate (optional, range: 1 - 10'000, default: 1)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (optional, default: false)</param>
   /// <returns>List with the generated integers.</returns>
   public static async Task<List<int>> GenerateAsync(int min, int max, int number = 1, bool prng = false)
   {
      int minValue = Math.Min(min, max);
      int maxValue = Math.Max(min, max);
      minValue = Math.Clamp(minValue, -1000000000, 1000000000);
      maxValue = Math.Clamp(maxValue, -1000000000, 1000000000);
      int num = Math.Clamp(number, 1, 10000);

      if (num < number)
         _logger.LogWarning($"'number' is to large - returning {num} integers.");

      bool hasInternet = await NetworkHelper.CheckInternetAvailabilityAsync();

      if (!hasInternet)
         _logger.LogWarning("No Internet access available - using standard prng!");

      if (prng || !hasInternet)
         GeneratePRNG(minValue, maxValue, num, Seed);

      if (!_isRunning)
      {
         _isRunning = true;

         if (await CheckQuota.GetQuotaAsync() > CalcBits(maxValue, number))
         {
            string url = $"{GENERATOR_URL}integers/?num={num}&min={minValue}&max={maxValue}&col=1&base=10&format=plain&rnd=new";

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
            const string msg = "Quota exceeded - using standard prng!";
            _logger.LogWarning(msg);

            _result = GeneratePRNG(minValue, maxValue, num, Seed);
         }

         _isRunning = false;
      }

      else
      {
         _logger.LogWarning("There is already a request running - please try again later!");
      }


      return _result;
   }

   /// <summary>Generates random integers with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="min">Smallest possible number</param>
   /// <param name="max">Biggest possible number</param>
   /// <param name="number">How many numbers you want to generate (optional, default: 1)</param>
   /// <param name="seed">Seed for the PRNG (optional, default: 0 (=standard))</param>
   /// <returns>List with the generated integers.</returns>
   public static System.Collections.Generic.List<int> GeneratePRNG(int min, int max, int number = 1, int seed = 0)
   {
      Random rnd = seed == 0 ? new Random() : new Random(seed);
      int _number = Math.Abs(number);
      int _min = Math.Min(min, max);
      int _max = Math.Max(min, max);

      List<int> result = new(_number);

      for (int ii = 0; ii < _number; ii++)
      {
         result.Add(rnd.Next(_min, _max + 1));
      }

      return result;
   }

   #endregion
}