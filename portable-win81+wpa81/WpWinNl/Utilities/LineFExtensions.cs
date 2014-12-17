using System;
using System.Collections.Generic;
#if WINDOWS_PHONE
using System.Windows;
#else
using Windows.Foundation;
#endif

namespace WpWinNl.Utilities
{
  public static class LineFExtensions
  {
    public static List<Point> Intersection(this LineF line, RectangleF rectangle)
    {
      var result = new List<Point>();
      AddIfIntersect(line, rectangle.X, rectangle.Y, rectangle.X2, rectangle.Y, result);
      AddIfIntersect(line, rectangle.X2, rectangle.Y, rectangle.X2, rectangle.Y2, result);
      AddIfIntersect(line, rectangle.X2, rectangle.Y2, rectangle.X, rectangle.Y2, result);
      AddIfIntersect(line, rectangle.X, rectangle.Y2, rectangle.X, rectangle.Y, result);
      return result;
    }

    private static void AddIfIntersect(LineF line, double x1, double y1, double x2, double y2, ICollection<Point> result)
    {
      var l2 = new LineF(x1, y1, x2, y2);
      var intersection = line.Intersection(l2);
      if (intersection != null)
      {
        result.Add(intersection.Value);
      }
    }

    /// <summary>
    /// If dx =1 , dy = ??
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static double GetDy(this LineF line)
    {
       var dx = Math.Abs(line.X1 - line.X2);
      var dy = line.Y1 - line.Y2;
      return (dy / dx);
    }
  }
}
