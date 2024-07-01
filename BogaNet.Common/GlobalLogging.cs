using Microsoft.Extensions.Logging;

namespace BogaNet;

/// <summary>
/// Global logger for dependency injection.
/// </summary>
public abstract class GlobalLogging
{
   private static ILoggerFactory? _Factory = null;

   #region Public methods

   public static ILoggerFactory LoggerFactory
   {
      get => _Factory ??= new LoggerFactory(); //TODO set the desired default log-provider (e.g. NLog)
      set => _Factory = value;
   }

   public static ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();

   public static ILogger CreateLogger(string name) => LoggerFactory.CreateLogger(name);

   #endregion
}