using System;

namespace BogaNet.TrueRandom;

/// <summary>
/// Base-class for all TRNG modules.
/// </summary>
public abstract class BaseTRNG
{
   /// <summary>
   /// URL of the random number generator.
   /// </summary>
   public const string GENERATOR_URL = "https://www.random.org/";

   /// <summary>Returns a seed for the PRNG.</summary>
   /// <returns>Seed for the PRNG.</returns>
   public static int Seed => _rnd.Next(int.MinValue, int.MaxValue);

   protected static readonly Random _rnd = new();
   protected static bool _isRunning;
}