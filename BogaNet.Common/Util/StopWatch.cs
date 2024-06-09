using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace BogaNet.Util;

/// <summary>
/// Stopwatch to measure performance etc.
/// </summary>
public class StopWatch
{
   #region Variables

   private readonly Stopwatch _watch = Stopwatch.StartNew();

   #endregion

   #region Properties

   /// <summary>
   /// Recorded points.
   /// </summary>
   public List<Tuple<object, long>> Points { get; } = new();

   /// <summary>
   /// Recorded points and time as string list.
   /// </summary>
   public List<string> PointsAndTime
   {
      get
      {
         List<string> result = new();

         foreach (var point in Points)
         {
            result.Add($"{point.Item1}: {point.Item2}");
         }

         return result;
      }
   }

   /// <summary>
   /// Elapsed time in milliseconds.
   /// </summary>
   public long? ElapsedTime => _watch.ElapsedMilliseconds;

   #endregion

   #region Public methods

   /// <summary>
   /// Starts the stopwatch.
   /// </summary>
   public void Start()
   {
      Points.Clear();
      _watch.Restart();
   }

   /// <summary>
   /// Adds point as an object to the current elapsed time.
   /// </summary>
   /// <param name="obj">Object for the point</param>
   /// <returns>Elapsed time in milliseconds</returns>
   public long AddPoint(object obj)
   {
      if (!_watch.IsRunning)
         Start();

      Points.Add(new Tuple<object, long>(obj, _watch.ElapsedMilliseconds));
      return _watch.ElapsedMilliseconds;
   }

   /// <summary>
   /// Stops the stopwatch.
   /// </summary>
   public long Stop()
   {
      if (!_watch.IsRunning)
         return 0;

      _watch.Stop();

      return _watch.ElapsedMilliseconds;
   }

   #endregion
}