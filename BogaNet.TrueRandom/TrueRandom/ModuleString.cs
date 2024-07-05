using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using BogaNet.Helper;

namespace BogaNet.TrueRandom;

/// <summary>
/// This module will generate true random strings of various length and character compositions.
/// </summary>
public abstract class ModuleString
{
   #region Variables

   private static readonly ILogger<ModuleString> _logger = GlobalLogging.CreateLogger<ModuleString>();

   private static System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

   private static bool isRunning;

   #endregion

   #region Static properties

   /// <summary>Returns the list of strings from the last generation.</summary>
   /// <returns>List of strings from the last generation.</returns>
   public static System.Collections.Generic.List<string> Result => result.GetRange(0, result.Count);

   #endregion


   #region Public methods

   /// <summary>Generates random strings.</summary>
   /// <param name="length">How long the strings should be (range: 1 - 20)</param>
   /// <param name="number">How many strings you want to generate (range: 1 - 10'000, default: 1, optional)</param>
   /// <param name="digits">Allow digits (0-9) (default: true, optional)</param>
   /// <param name="upper">Allow uppercase letters (default: true, optional)</param>
   /// <param name="lower">Allow lowercase letters (default: true, optional)</param>
   /// <param name="unique">String should be unique (default: false, optional)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (default: false, optional)</param>
   public static async System.Threading.Tasks.Task<System.Collections.Generic.List<string>> Generate(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false, bool prng = false)
   {
      int _length = length;
      int _number;

      if (prng)
      {
         _number = Math.Clamp(number, 1, int.MaxValue);
      }
      else
      {
         _length = Math.Clamp(length, 1, 20);
         _number = calcMaxNumber(number, _length, digits, upper, lower, unique);

         if (_number < number)
            _logger.LogWarning("'number' is to large - returning " + _number + " strings.");
      }

      if (!digits && !upper && !lower)
      {
         _logger.LogError("'digits', 'upper' and 'lower' are 'false' - string generation not possible!");
      }
      else
      {
         if (prng)
         {
            result = GeneratePRNG(_length, _number, digits, upper, lower, unique, TrueRandomNumberGenerator.Seed);
         }
         else
         {
            if (!isRunning)
            {
               isRunning = true;

               //if (NetworkHelper.isInternetAvailable)
               if (true)
               {
                  _logger.LogDebug("Quota before: " + ModuleQuota.Quota);

                  if (ModuleQuota.Quota > 0)
                  {
                     string url = $"{TrueRandomNumberGenerator.GENERATOR_URL}strings/?num={_number}&len={_length}&digits={boolToString(digits)}&upperalpha={boolToString(upper)}&loweralpha={boolToString(lower)}&unique={boolToString(unique)}&format=plain&rnd=new";

                     _logger.LogDebug("URL: " + url);


                     using System.Net.Http.HttpClient client = new();
                     using System.Net.Http.HttpResponseMessage response = client.GetAsync(url).Result;
                     response.EnsureSuccessStatusCode();

                     if (response.IsSuccessStatusCode)
                     {
                        string data = await response.Content.ReadAsStringAsync();

                        result.Clear();
                        string[] _result = System.Text.RegularExpressions.Regex.Split(data, "\r\n?|\n", System.Text.RegularExpressions.RegexOptions.Singleline);

                        foreach (string valueAsString in _result.Where(valueAsString => !string.IsNullOrEmpty(valueAsString)))
                        {
                           result.Add(valueAsString);
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

                     result = GeneratePRNG(_length, _number, digits, upper, lower, unique, TrueRandomNumberGenerator.Seed);
                  }
               }
               else
               {
                  const string msg = "No Internet access available - using standard prng now!";
                  _logger.LogWarning(msg);

                  result = GeneratePRNG(_length, _number, digits, upper, lower, unique, TrueRandomNumberGenerator.Seed);
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

   /// <summary>Generates random strings with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="length">How long the strings should be</param>
   /// <param name="number">How many strings you want to generate (default: 1, optional)</param>
   /// <param name="digits">Allow digits (0-9) (default: true, optional)</param>
   /// <param name="upper">Allow uppercase (A-Z) letters (default: true, optional)</param>
   /// <param name="lower">Allow lowercase (a-z) letters (default: true, optional)</param>
   /// <param name="unique">String should be unique (default: false, optional)</param>
   /// <param name="seed">Seed for the PRNG (default: 0 (=standard), optional)</param>
   /// <returns>List with the generated strings.</returns>
   public static System.Collections.Generic.List<string> GeneratePRNG(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false, int seed = 0)
   {
      Random rnd = seed == 0 ? new Random() : new System.Random(seed);
      int _length = Math.Abs(length);
      int _number = calcMaxNumber(number, _length, digits, upper, lower, unique);
      System.Collections.Generic.List<string> _result = new System.Collections.Generic.List<string>(_number);

      string glyphs = string.Empty;

      if (upper)
         glyphs += Constants.ALPHABET_LATIN_UPPERCASE;

      if (lower)
         glyphs += Constants.ALPHABET_LATIN_LOWERCASE;

      if (digits)
         glyphs += Constants.NUMBERS;

      for (int ii = 0; ii < _number; ii++)
      {
         string s;
         if (unique)
         {
            bool isNotUnique;
            do
            {
               isNotUnique = false;
               s = string.Empty;
               for (int yy = 0; yy < _length; yy++)
               {
                  s += glyphs[rnd.Next(0, glyphs.Length)];
               }

               foreach (string str in result.Where(str => str == s))
               {
                  isNotUnique = true;
               }
            } while (isNotUnique);
         }
         else
         {
            s = string.Empty;
            for (int yy = 0; yy < _length; yy++)
            {
               s += glyphs[rnd.Next(0, glyphs.Length)];
            }
         }

         _result.Add(s);
      }

      return _result;
   }

   #endregion


   #region Private methods

   private static int calcMaxNumber(int number, int length, bool digits, bool upper, bool lower, bool unique)
   {
      int _number = Math.Clamp(number, 1, 10000);

      if (unique && length > 0 && length <= 10)
      {
         double basis = 0d;

         if (digits)
         {
            basis += 10d;
         }

         if (upper)
         {
            basis += 26d;
         }

         if (lower)
         {
            basis += 26d;
         }

         if (basis > 0d)
         {
            long maxNumber = (long)System.Math.Pow(basis, length);

            if (maxNumber < number)
            {
               _logger.LogWarning($"Too many numbers requested with 'unique' on - result reduced to {maxNumber} numbers!");
               _number = (int)maxNumber;
            }
         }
      }

      return _number;
   }

   private static string boolToString(bool value)
   {
      return value ? "on" : "off";
   }

   #endregion
}