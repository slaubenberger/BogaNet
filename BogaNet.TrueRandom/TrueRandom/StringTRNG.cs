using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.Helper;

namespace BogaNet.TrueRandom;

/// <summary>
/// Generates true random strings of various length and character compositions.
/// </summary>
public abstract class StringTRNG : BaseTRNG //NUnit
{
   #region Variables

   private static readonly ILogger<StringTRNG> _logger = GlobalLogging.CreateLogger<StringTRNG>();

   #endregion

   #region Properties

   /// <summary>Returns the list of strings from the last generation.</summary>
   /// <returns>List of strings from the last generation.</returns>
   public static List<string> Result { get; private set; } = [];

   #endregion

   #region Public methods

   /// <summary>
   /// Calculates needed bits (from the quota) for generating random strings.
   /// NOTE: The calculated value is an approximation and will may differ from the real quota deducted from the server.
   /// </summary>
   /// <param name="length">Length of the strings</param>
   /// <param name="number">How many strings (default: 1, optional)</param>
   /// <returns>Needed bits for generating the strings.</returns>
   public static int CalcBits(int length, int number = 1)
   {
      int factor = length > 10 ? 2 : 1;
      return Math.Abs(number) * Math.Abs(length) * 8 * factor;
   }

   /// <summary>Generates random strings.</summary>
   /// <param name="length">How long the strings should be (range: 1 - 20)</param>
   /// <param name="number">How many strings you want to generate (optional, range: 1 - 10'000, default: 1)</param>
   /// <param name="digits">Allow digits (0-9) (optional, default: true)</param>
   /// <param name="upper">Allow uppercase letters (optional, default: true)</param>
   /// <param name="lower">Allow lowercase letters (optional, default: true)</param>
   /// <param name="unique">String should be unique (optional, default: false)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (optional, default: false)</param>
   public static List<string> Generate(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false, bool prng = false)
   {
      return Task.Run(() => GenerateAsync(length, number, digits, upper, lower, unique, prng)).GetAwaiter().GetResult();
   }

   /// <summary>Generates random strings asynchronously.</summary>
   /// <param name="length">How long the strings should be (range: 1 - 20)</param>
   /// <param name="number">How many strings you want to generate (optional, range: 1 - 10'000, default: 1)</param>
   /// <param name="digits">Allow digits (0-9) (optional, default: true)</param>
   /// <param name="upper">Allow uppercase letters (optional, default: true)</param>
   /// <param name="lower">Allow lowercase letters (optional, default: true)</param>
   /// <param name="unique">String should be unique (optional, default: false)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (optional, default: false)</param>
   public static async Task<List<string>> GenerateAsync(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false, bool prng = false)
   {
      int len = Math.Clamp(Math.Abs(length), 1, 20);
      int num = Math.Clamp(Math.Abs(number), 1, 10000);
      num = calcMaxNumber(num, len, digits, upper, lower, unique);

      if (!digits && !upper && !lower)
      {
         _logger.LogWarning("'digits', 'upper' and 'lower' are all 'false' - setting 'lower' to true!");
         lower = true;
      }

      bool hasInternet = await NetworkHelper.CheckInternetAvailabilityAsync();

      if (!hasInternet)
         _logger.LogWarning("No Internet access available - using standard prng!");

      if (prng || !hasInternet)
         return GeneratePRNG(len, num, digits, upper, lower, unique, Seed);

      if (!_isRunning)
      {
         _isRunning = true;

         if (await CheckQuota.GetQuotaAsync() > CalcBits(len, num))
         {
            string url = $"{GENERATOR_URL}strings/?num={num}&len={len}&digits={boolToString(digits)}&upperalpha={boolToString(upper)}&loweralpha={boolToString(lower)}&unique={boolToString(unique)}&format=plain&rnd=new";

            _logger.LogDebug("URL: " + url);

            string[] result = await NetworkHelper.ReadAllLinesAsync(url);

            Result.Clear();

            foreach (string valueAsString in result.Where(valueAsString => !string.IsNullOrEmpty(valueAsString)))
            {
               Result.Add(valueAsString);
            }
         }
         else
         {
            _logger.LogWarning("Quota exceeded - using standard prng!");

            Result = GeneratePRNG(len, num, digits, upper, lower, unique, Seed);
         }

         _isRunning = false;
      }
      else
      {
         _logger.LogWarning("There is already a request running - please try again later!");
      }

      return Result;
   }

   /// <summary>Generates random strings with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="length">How long the strings should be</param>
   /// <param name="number">How many strings you want to generate (optional, default: 1)</param>
   /// <param name="digits">Allow digits (0-9) (optional, default: true)</param>
   /// <param name="upper">Allow uppercase (A-Z) letters (optional, default: true)</param>
   /// <param name="lower">Allow lowercase (a-z) letters (optional, default: true)</param>
   /// <param name="unique">String should be unique (optional, default: false)</param>
   /// <param name="seed">Seed for the PRNG (optional, default: 0 (=standard))</param>
   /// <returns>List with the generated strings.</returns>
   public static List<string> GeneratePRNG(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false, int seed = 0)
   {
      Random rnd = seed == 0 ? new Random() : new Random(seed);
      int len = Math.Abs(length);
      int num = calcMaxNumber(Math.Abs(number), len, digits, upper, lower, unique);
      List<string> result = new(num);

      string glyphs = string.Empty;

      if (upper)
         glyphs += Constants.ALPHABET_LATIN_UPPERCASE;

      if (lower)
         glyphs += Constants.ALPHABET_LATIN_LOWERCASE;

      if (digits)
         glyphs += Constants.NUMBERS;

      for (int ii = 0; ii < num; ii++)
      {
         string s;
         if (unique)
         {
            bool isNotUnique;
            do
            {
               s = string.Empty;
               for (int yy = 0; yy < len; yy++)
               {
                  s += glyphs[rnd.Next(0, glyphs.Length)];
               }

               isNotUnique = Result.Any(str => str == s);
            } while (isNotUnique);
         }
         else
         {
            s = string.Empty;
            for (int yy = 0; yy < len; yy++)
            {
               s += glyphs[rnd.Next(0, glyphs.Length)];
            }
         }

         result.Add(s);
      }

      return result;
   }

   #endregion

   #region Private methods

   private static int calcMaxNumber(int number, int length, bool digits, bool upper, bool lower, bool unique)
   {
      int num = number;

      if (!unique || length is <= 0 or > 10)
         return num;
      
      double basis = 0d;

      if (digits)
         basis += 10d;

      if (upper)
         basis += 26d;

      if (lower)
         basis += 26d;

      if (!(basis > 0d)) 
         return num;
      
      long maxNumber = (long)Math.Pow(basis, length);

      if (maxNumber >= num) 
         return num;

      _logger.LogWarning($"Too many numbers requested with 'unique' on - result reduced to {maxNumber} numbers!");
      num = (int)maxNumber;

      return num;
   }

   private static string boolToString(bool value)
   {
      return value ? "on" : "off";
   }

   #endregion
}