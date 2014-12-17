using System;
#if WINDOWS_PHONE
using System.Windows;
#else
using Windows.Foundation;
using Windows.UI.Xaml;
#endif

namespace WpWinNl.Utilities
{
  public static class PointExtensions
  {
    /// <summary>
    /// Distances from point 1 to point 2
    /// </summary>
    /// <param name="p1">The first point.</param>
    /// <param name="p2">The second point.</param>
    /// <returns></returns>
    public static double DistanceFrom(this Point p1, Point p2)
    {
      var dX = p2.X - p1.X;
      var dY = p2.Y - p1.Y;
      return Math.Sqrt(dX * dX + dY * dY);
    }

    /// <summary>
    /// Calculates the speed in pixels per second
    /// </summary>
    /// <param name="p1">The first point.</param>
    /// <param name="p2">The second point.</param>
    /// <param name="duration">The duration</param>
    /// <returns>Speed in pixels per second</returns>
    public static double CalculateSpeed(this Point p1, Point p2, Duration duration)
    {
      return p1.DistanceFrom(p2) / duration.TimeSpan.TotalSeconds;
    }

    /// <summary>
    /// Calculates the duration given a distance and a speed.
    /// </summary>
    /// <param name="p1">The first point.</param>
    /// <param name="p2">The second point.</param>
    /// <param name="speed">The speed in pixels per second.</param>
    /// <returns>Time it takes to get from p1 to p2</returns>
    public static Duration CalculateDuration(this Point p1, Point p2, double speed)
    {
      return new Duration(TimeSpan.FromSeconds(p1.DistanceFrom(p2) / speed));
    }
  }
}
