using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Numerics;

namespace BogaNet.Util;

/// <summary>
/// Various helper functions of very general utility.
/// </summary>
public abstract class Helper
{
   #region Variables

   private static readonly ILogger _logger = GlobalLogging.CreateLogger("Helper");

   private static readonly Random _rnd = new();

   #endregion

   #region Properties

   /// <summary>
   /// True if the current platform is Windows.
   /// </summary>
   public static bool isWindows => System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);

   /// <summary>
   /// True if the current platform is Mac.
   /// </summary>
   public static bool isOSX => System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);

   /// <summary>
   /// True if the current platform is Linux.
   /// </summary>
   public static bool isLinux => System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);

   /// <summary>
   /// True if the current platform is FreeBSD.
   /// </summary>
   public static bool isFreeBSD => System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.FreeBSD);

   /// <summary>
   /// True if the current platform is Unix-based.
   /// </summary>
   public static bool isUnix => isLinux || isFreeBSD || isOSX;

   /// <summary>
   /// The current culture of the application.
   /// </summary>
   /// <returns>Culture of the application</returns>
   public static CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

   #endregion

   #region Public methods

   /// <summary>
   /// Format byte-value to Human-Readable-Form.
   /// </summary>
   /// <param name="bytes">Value in bytes</param>
   /// <param name="useSI">Use SI-system (optional, default: false)</param>
   /// <returns>Formatted byte-value in Human-Readable-Form</returns>
   public static string FormatBytesToHRF(long bytes, bool useSI = false)
   {
      const string ci = "kMGTPE";
      int index = 0;

      if (useSI)
      {
         if (bytes is > -1000 and < 1000)
            return bytes + " B";

         while (bytes is <= -999_950 or >= 999_950)
         {
            bytes /= 1000;
            index++;
         }

         return $"{(bytes / 1000f):N2} {ci[index]}B";
      }

      long absB = bytes == long.MinValue ? long.MaxValue : Math.Abs(bytes);
      if (absB < 1024)
         return bytes + " B";

      long value = absB;

      for (int i = 40; i >= 0 && absB > 0xfffccccccccccccL >> i; i -= 10)
      {
         value >>= 10;
         index++;
      }

      value *= Math.Sign(bytes);

      return $"{(value / 1024f):N2} {ci[index]}iB";
   }

   /// <summary>
   /// Format seconds to Human-Readable-Form.
   /// </summary>
   /// <param name="seconds">Value in seconds</param>
   /// <returns>Formatted seconds in Human-Readable-Form</returns>
   public static string FormatSecondsToHRF<T>(T seconds) where T : INumber<T>
   {
      long val = Convert.ToInt64(seconds);

      bool wasMinus = val < 0;

      long totalSeconds = Math.Abs(val);
      long calcSeconds = totalSeconds % 60;

      if (val >= 86400)
      {
         long calcDays = totalSeconds / 86400;
         long calcHours = (totalSeconds -= calcDays * 86400) / 3600;
         long calcMinutes = (totalSeconds - calcHours * 3600) / 60;

         return $"{(wasMinus ? "-" : "")}{calcDays}d {calcHours}:{addLeadingZero(calcMinutes)}:{addLeadingZero(calcSeconds)}";
      }

      if (val >= 3600)
      {
         long calcHours = totalSeconds / 3600;
         long calcMinutes = (totalSeconds - calcHours * 3600) / 60;

         return $"{(wasMinus ? "-" : "")}{calcHours}:{addLeadingZero(calcMinutes)}:{addLeadingZero(calcSeconds)}";
      }
      else
      {
         long calcMinutes = totalSeconds / 60;

         return $"{(wasMinus ? "-" : "")}{calcMinutes}:{addLeadingZero(calcSeconds)}";
      }
   }

   /// <summary>
   /// Generates a "Lorem Ipsum" based on various parameters.
   /// </summary>
   /// <param name="length">Length of the text</param>
   /// <param name="minSentences">Minimum number of sentences for the text (optional, default: 1)</param>
   /// <param name="maxSentences">Maximal number of sentences for the text (optional, default: int.MaxValue)</param>
   /// <param name="minWords">Minimum number of words per sentence (optional, default: 1)</param>
   /// <param name="maxWords">Maximal number of words per sentence (optional, default: 15)</param>
   /// <returns>"Lorem Ipsum" based on the given parameters</returns>
   public static string GenerateLoremIpsum(int length, int minSentences = 1, int maxSentences = int.MaxValue, int minWords = 1, int maxWords = 15)
   {
      string[] words =
      [
         "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", "elit", "sed", "diam",
         "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
      ];

      int numSentences = _rnd.Next(maxSentences - minSentences) + minSentences + 1;

      StringBuilder result = new();

      for (int s = 0; s < numSentences && result.Length <= length; s++)
      {
         int numWords = _rnd.Next(maxWords - minWords) + minWords + 1;
         for (int w = 0; w < numWords && result.Length <= length; w++)
         {
            if (w > 0)
               result.Append(' ');

            result.Append(
               w == 0 ? words[_rnd.Next(words.Length)].BNToTitleCase() : words[_rnd.Next(words.Length)]);
         }

         result.Append(". ");
      }

      string text = result.ToString();

      if (length > 0 && text.Length > length)
         text = text.Substring(0, length - 1) + ".";

      return text;
   }

   /// <summary>
   /// Invokes a method on a full qualified class.
   /// </summary>
   /// <param name="className">Full qualified name of the class</param>
   /// <param name="methodName">Public static method of the class to execute</param>
   /// <param name="flags">Binding flags for the method (optional, default: static/public)</param>
   /// <param name="parameters">Parameters for the method (optional)</param>
   /// <exception cref="Exception"></exception>
   public static object? InvokeMethod(string? className, string? methodName, BindingFlags flags = BindingFlags.Static | BindingFlags.Public, params object[] parameters)
   {
      if (string.IsNullOrEmpty(className))
      {
         _logger.LogWarning("'className' is null or empty; can not execute.");
         return null;
      }

      if (string.IsNullOrEmpty(methodName))
      {
         _logger.LogWarning("'methodName' is null or empty; can not execute.");
         return null;
      }

      foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
      {
         try
         {
            if (type.FullName?.Equals(className) == true)
               if (type.IsClass)
               {
                  MethodInfo? method = type.GetMethod(methodName, flags);

                  if (method != null)
                     return method.Invoke(null, parameters);
               }
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Could not execute method call '{methodName}' for '{className}'");
            throw;
         }
      }

      _logger.LogWarning($"Could not find class ' {className}' or method '{methodName}'");

      return null;
   }

   /// <summary>
   /// Returns a CLI argument for a name from the command line.
   /// </summary>
   /// <param name="name">Name for the argument</param>
   /// <param name="args">Arguments to search for (optional)</param>
   /// <returns>Argument for a name from the command line</returns>
   public static string? GetCLIArgument(string? name, string[]? args = null)
   {
      if (!string.IsNullOrEmpty(name))
      {
         string[] cliArguments = args ?? GetCLIArguments();

         for (int ii = 0; ii < cliArguments.Length; ii++)
         {
            if (name.BNEquals(cliArguments[ii]) && cliArguments.Length > ii + 1)
               return cliArguments[ii + 1];
         }
      }

      return null;
   }

   /// <summary>
   /// Returns all CLI arguments.
   /// </summary>
   /// <returns>Arguments from the command line</returns>
   public static string[] GetCLIArguments()
   {
      return Environment.GetCommandLineArgs();
   }

/*
         /// <summary>Generates a string of all latin latin characters (ABC...xyz).</summary>
         /// <returns>"String of all latin latin characters</returns>
         public static string GenerateLatinABC()
         {
            return GenerateLatinUppercaseABC() + GenerateLatinLowercaseABC();
         }

         /// <summary>Generates a string of all latin latin characters in uppercase (ABC...XYZ).</summary>
         /// <returns>"String of all latin latin characters in uppercase</returns>
         public static string GenerateLatinUppercaseABC()
         {
            Text.StringBuilder result = new Text.StringBuilder();

            for (int ii = 65; ii <= 90; ii++)
            {
               result.Append((char)ii);
            }

            return result.ToString();
         }

         /// <summary>Generates a string of all latin latin characters in lowercase (abc...xyz).</summary>
         /// <returns>"String of all latin latin characters in lowercase</returns>
         public static string GenerateLatinLowercaseABC()
         {
            Text.StringBuilder result = new Text.StringBuilder();

            for (int ii = 97; ii <= 122; ii++)
            {
               result.Append((char)ii);
            }

            return result.ToString();
         }
   */

   #endregion

   #region Private methods

   private static string addLeadingZero(long value)
   {
      return value < 10 ? "0" + value : value.ToString();
   }

   #endregion
}