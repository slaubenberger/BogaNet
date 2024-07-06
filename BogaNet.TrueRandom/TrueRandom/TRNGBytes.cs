﻿using System;
using BogaNet.Util;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using BogaNet.Helper;
using System.Security.Cryptography;

namespace BogaNet.TrueRandom;

/// <summary>
/// Generates true random byte-arrays in configurable intervals.
/// </summary>
public abstract class TRNGBytes : TRNGBase
{
   #region Variables

   private static readonly ILogger<TRNGBytes> _logger = GlobalLogging.CreateLogger<TRNGBytes>();
   private static List<byte[]> _result = [];

   #endregion

   #region Static properties

   /// <summary>Returns the list of byte-arrays from the last generation.</summary>
   /// <returns>List of byte-arrays from the last generation.</returns>
   public static List<byte[]> Result => _result;

   #endregion


   #region Public methods

   /// <summary>
   /// Calculates needed bits (from the quota) for generating random byte-arrays.
   /// NOTE: The calculated value may differ from the real value due the calculation of the server.
   /// </summary>
   /// <param name="length">Length of the byte-array</param>
   /// <param name="number">How many byte-arrays (default: 1, optional)</param>
   /// <returns>Needed bits for generating the byte-arrays.</returns>
   public static int CalcBits(int length, int number = 1)
   {
      return TRNGString.CalcBits(length, number);
   }

   /// <summary>Generates random byte-arrays asynchronously.</summary>
   /// <param name="length">Defines how many bytes the byte-arrays contains (range: 1 - 20)</param>
   /// <param name="number">How many byte-arrays to generate (optional, range: 1 - 10'000, default: 1)</param>
   /// <param name="prng">Use Pseudo-Random-Number-Generator (optional, default: false)</param>
   /// <returns>List with the generated byte-arrays.</returns>
   public static async Task<List<byte[]>> GenerateAsync(int length, int number = 1, bool prng = false)
   {
      int len = Math.Clamp(length, 1, 20);
      int num = Math.Clamp(number, 1, 10000);

      if (num < number)
         _logger.LogWarning($"'number' is to large - returning {num} byte-arrays.");

      bool hasInternet = await NetworkHelper.CheckInternetAvailabilityAsync();

      if (!hasInternet)
         _logger.LogWarning("No Internet access available - using standard prng now!");

      if (prng || !hasInternet)
         return GeneratePRNG(len, num, Seed);

      if (!_isRunning) //TODO needed?
      {
         List<string> list = await TRNGString.GenerateAsync(len, num);

         _result.Clear();
         foreach (string str in list)
         {
            byte[] data = Obfuscator.Obfuscate(str, Obfuscator.GenerateIV());
            _logger.LogDebug($"{str.Length} - {data.Length}");
            _result.Add(data);
         }
      }
      else
      {
         _logger.LogWarning("There is already a request running - please try again later!");
      }


      return _result;
   }

   /// <summary>Generates random byte-arrays.</summary>
   /// <param name="length">Defines how many bytes the byte-arrays contains</param>
   /// <param name="number">How many byte-arrays to generate (optional, default: 1)</param>
   /// <param name="seed">Seed for the PRNG (optional, default: 0 (=standard))</param>
   /// <returns>List with the generated byte-arrays.</returns>
   public static List<byte[]> GeneratePRNG(int length, int number = 1, int seed = 0)
   {
      int num = Math.Abs(number);

      List<byte[]> result = new(num);

      for (int ii = 0; ii < num; ii++)
      {
         result.Add(RandomNumberGenerator.GetBytes(length));
      }

      return result;
   }

   #endregion
}