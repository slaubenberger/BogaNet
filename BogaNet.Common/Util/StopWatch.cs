using System.Diagnostics;

namespace BogaNet.Util;

/// <summary>
/// Stopwatch to measure performance etc.
/// </summary>
public class StopWatch
{
   #region Variables

   private Stopwatch? _watch;

   #endregion

   #region Properties

   /// <summary>
   /// Recorded points.
   /// </summary>
   public List<string> Points { get; } = new();

   /// <summary>
   /// Recorded time in milliseconds.
   /// </summary>
   public List<long> Time { get; } = new();

   /// <summary>
   /// Recorded points and time as string list
   /// </summary>
   public string[] PointsAndTime
   {
      get
      {
         List<string> result = new();

         for (int ii = 0; ii < Points.Count; ii++)
         {
            result.Add($"{Points[ii]}: {Time[ii]}");
         }

         return result.ToArray();
      }
   }

   /// <summary>
   /// Elapsed time in milliseconds.
   /// </summary>
   public long? ElapsedTime => _watch != null ? _watch.ElapsedMilliseconds : 0;

   #endregion

   #region Public methods

   /// <summary>
   /// Starts the stopwatch.
   /// </summary>
   public void Start()
   {
      Points.Clear();
      Time.Clear();
      _watch = Stopwatch.StartNew();
   }

   /// <summary>
   /// Adds a time point.
   /// </summary>
   /// <param name="name">Name of the point</param>
   /// <returns>Elapsed time in milliseconds</returns>
   public long AddPoint(string name)
   {
      Points.Add(name);
      Time.Add((long)_watch?.ElapsedMilliseconds);
      return _watch.ElapsedMilliseconds;
   }

   /// <summary>
   /// Stops the stopwatch.
   /// </summary>
   public long Stop()
   {
      _watch?.Stop();

      return (long)_watch?.ElapsedMilliseconds;
   }

   #endregion
}