using BogaNet.Util;
using System;

namespace BogaNet.TrueRandom;

/// <summary>
/// The TRManager is the manager for all modules.
/// </summary>
public class TrueRandomNumberGenerator : Singleton<TrueRandomNumberGenerator>
{
   #region Variables

   private bool prng;

   private int generateCount;
   private static readonly System.Random rnd = new System.Random();

   public const string GENERATOR_URL = "https://www.random.org/";

   #endregion


   #region Properties

   /// <summary>Enable or disable the C#-standard Pseudo-Random-Number-Generator-mode.</summary>
   public bool PRNG
   {
      get => prng;
      set => prng = value;
   }

   /// <summary>Returns the remaining quota in bits from the last check.</summary>
   /// <returns>Remaining quota in bits from the last check.</returns>
   public int CurrentQuota => CheckQuota.Quota;


   /// <summary>Returns the list of integers from the last generation.</summary>
   /// <returns>List of integers from the last generation.</returns>
   public System.Collections.Generic.List<int> CurrentIntegers => TRNGInteger.Result;

   /// <summary>Returns the list of floats from the last generation.</summary>
   /// <returns>List of floats from the last generation.</returns>
   public System.Collections.Generic.List<float> CurrentFloats => TRNGFloat.Result;

   /// <summary>Returns the sequence from the last generation.</summary>
   /// <returns>Sequence from the last generation.</returns>
   public System.Collections.Generic.List<int> CurrentSequence => TRNGSequence.Result;

   /// <summary>Returns the list of strings from the last generation.</summary>
   /// <returns>List of strings from the last generation.</returns>
   public System.Collections.Generic.List<string> CurrentStrings => TRNGString.Result;

   /// <summary>Checks if True Random is generating numbers on this system.</summary>
   /// <returns>True if True Random is generating numbers on this system.</returns>
   public bool isGenerating => generateCount > 0;

   /// <summary>Returns a seed for the PRNG.</summary>
   /// <returns>Seed for the PRNG.</returns>
   public static int Seed => rnd.Next(int.MinValue, int.MaxValue);

   #endregion

   #region Public methods

   /// <summary>
   /// Calculates needed bits (from the quota) for generating random floats.
   /// </summary>
   /// <param name="number">How many numbers (default: 1, optional)</param>
   /// <returns>Needed bits for generating the floats.</returns>
   public int CalculateFloat(int number = 1)
   {
      int bitsCounter = 32;

      bitsCounter *= Math.Abs(number);

      return bitsCounter;
   }

   /// <summary>
   /// Calculates needed bits (from the quota) for generating random integers.
   /// </summary>
   /// <param name="max">Biggest allowed number</param>
   /// <param name="number">How many numbers (default: 1, optional)</param>
   /// <returns>Needed bits for generating the integers.</returns>
   public int CalculateInteger(int max, int number = 1)
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

      bitsCounter *= Math.Abs(number);

      return bitsCounter;
   }

   /// <summary>
   /// Calculates needed bits (from the quota) for generating a random sequence.
   /// </summary>
   /// <param name="min">Start of the interval</param>
   /// <param name="max">End of the interval</param>
   /// <returns>Needed bits for generating the sequence.</returns>
   public int CalculateSequence(int min, int max)
   {
      int _min = min;
      int _max = max;

      if (_min > _max)
      {
         _min = max;
         _max = min;
      }

      if (_min == 0 && _max == 0)
      {
         _max = 1;
      }

      int bitsCounter = 0;

      if (_min != _max)
      {
         bitsCounter = (_max - _min) * 31;
      }

      return bitsCounter;
   }

   /// <summary>
   /// Calculates needed bits (from the quota) for generating random strings.
   /// </summary>
   /// <param name="length">Length of the strings</param>
   /// <param name="number">How many strings (default: 1, optional)</param>
   /// <returns>Needed bits for generating the strings.</returns>
   public int CalculateString(int length, int number = 1)
   {
      int bitsCounter = Math.Abs(number) * Math.Abs(length) * 30;

      return bitsCounter;
   }

   /// <summary>Generates random integers.</summary>
   /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
   /// <param name="id">UID to identify the generated result (optional)</param>
   /// <returns>UID of the generator.</returns>
   public string GenerateInteger(int min, int max, int number = 1, string id = "")
   {
      string _id = string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString() : id;

      TRNGInteger.Generate(min, max, number, PRNG, false, _id);

      return _id;
   }

   /// <summary>Generates random floats.</summary>
   /// <param name="min">Smallest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">Biggest possible number (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you want to generate (range: 1 - 10'000, default: 1, optional)</param>
   /// <param name="id">UID to identify the generated result (optional)</param>
   /// <returns>UID of the generator.</returns>
   public string GenerateFloat(float min, float max, int number = 1, string id = "")
   {
      string _id = string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString() : id;

      TRNGFloat.Generate(min, max, number, PRNG, false, string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString() : id);

      return _id;
   }

   /// <summary>Generates random sequence.</summary>
   /// <param name="min">Start of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="max">End of the interval (range: -1'000'000'000 - 1'000'000'000)</param>
   /// <param name="number">How many numbers you have in the result (max range: max - min, optional)</param>
   /// <param name="id">UID to identify the generated result (optional)</param>
   /// <returns>UID of the generator.</returns>
   public string GenerateSequence(int min, int max, int number = 0, string id = "")
   {
      string _id = string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString() : id;

      TRNGSequence.Generate(min, max, number, PRNG, false, string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString() : id);

      return _id;
   }

   /// <summary>Generates random strings.</summary>
   /// <param name="length">How long the strings should be (range: 1 - 20)</param>
   /// <param name="number">How many strings you want to generate (range: 1 - 10'000, default: 1, optional)</param>
   /// <param name="digits">Allow digits (0-9) (default: true, optional)</param>
   /// <param name="upper">Allow uppercase (A-Z) letters (default: true, optional)</param>
   /// <param name="lower">Allow lowercase (a-z) letters (default: true, optional)</param>
   /// <param name="unique">String should be unique in the result (default: false, optional)</param>
   /// <param name="id">UID to identify the generated result (optional)</param>
   /// <returns>UID of the generator.</returns>
   public string GenerateString(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false, string id = "")
   {
      string _id = string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString() : id;
      TRNGString.Generate(length, number, digits, upper, lower, unique, PRNG);
      return _id;
   }

   /// <summary>Gets the remaining quota in bits from the server.</summary>
   public void GetQuota()
   {
      CheckQuota.GetQuota();
   }

   /// <summary>Generates random integers with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="min">Smallest possible number</param>
   /// <param name="max">Biggest possible number</param>
   /// <param name="number">How many numbers you want to generate (default: 1, optional)</param>
   /// <param name="seed">Seed for the PRNG (default: 0 (=standard), optional)</param>
   /// <returns>List with the generated integers.</returns>
   public System.Collections.Generic.List<int> GenerateIntegerPRNG(int min, int max, int number = 1, int seed = 0)
   {
      return TRNGInteger.GeneratePRNG(min, max, number, seed);
   }

   /// <summary>Generates random floats with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="min">Smallest possible number</param>
   /// <param name="max">Biggest possible number</param>
   /// <param name="number">How many numbers you want to generate (default: 1, optional)</param>
   /// <param name="seed">Seed for the PRNG (default: 0 (=standard), optional)</param>
   /// <returns>List with the generated floats.</returns>
   public System.Collections.Generic.List<float> GenerateFloatPRNG(float min, float max, int number = 1, int seed = 0)
   {
      return TRNGFloat.GeneratePRNG(min, max, number, seed);
   }

   /// <summary>Generates a random sequence with the C#-standard Pseudo-Random-Number-Generator.</summary>
   /// <param name="min">Start of the interval</param>
   /// <param name="max">End of the interval</param>
   /// <param name="number">How many numbers you have in the result (max range: max - min, optional)</param>
   /// <param name="seed">Seed for the PRNG (default: 0 (=standard), optional)</param>
   /// <returns>List with the generated sequence.</returns>
   public System.Collections.Generic.List<int> GenerateSequencePRNG(int min, int max, int number = 0, int seed = 0)
   {
      return TRNGSequence.GeneratePRNG(min, max, number, seed);
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
   public System.Collections.Generic.List<string> GenerateStringPRNG(int length, int number = 1, bool digits = true, bool upper = true, bool lower = true, bool unique = false, int seed = 0)
   {
      return TRNGString.GeneratePRNG(length, number, digits, upper, lower, unique, seed);
   }

   #endregion
}