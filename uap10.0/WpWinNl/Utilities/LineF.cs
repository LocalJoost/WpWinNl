using System;
using Windows.Foundation;

namespace WpWinNl.Utilities
{
  public class LineF
  {
    public double X1 { get; set; }
    public double Y1 { get; set; }
    public double X2 { get; set; }
    public double Y2 { get; set; }

    public LineF()
    {
    }

    public LineF(double x1, double y1, double x2, double y2)
    {
      X1 = x1;
      Y1 = y1;
      X2 = x2;
      Y2 = y2;
    }

    public LineF(Point from, Point to)
      : this(from.X, from.Y, to.X, to.Y)
    {
    }

    /// <summary>
    /// Calculates intersection - if any - of two lines
    /// </summary>
    /// <param name="otherLine"></param>
    /// <returns>Intersection or null</returns>
    /// <remarks>Take from http://tog.acm.org/resources/GraphicsGems/gemsii/xlines.c </remarks>
    public Point? Intersection(LineF otherLine)
    {
      var a1 = Y2 - Y1;
      var b1 = X1 - X2;
      var c1 = X2 * Y1 - X1 * Y2;

      /* Compute r3 and r4.
       */

      var r3 = a1 * otherLine.X1 + b1 * otherLine.Y1 + c1;
      var r4 = a1 * otherLine.X2 + b1 * otherLine.Y2 + c1;

      /* Check signs of r3 and r4.  If both point 3 and point 4 lie on
       * same side of line 1, the line segments do not intersect.
       */

      if (r3 != 0 && r4 != 0 && Math.Sign(r3) == Math.Sign(r4))
      {
        return null; // DONT_INTERSECT
      }

      /* Compute a2, b2, c2 */

      var a2 = otherLine.Y2 - otherLine.Y1;
      var b2 = otherLine.X1 - otherLine.X2;
      var c2 = otherLine.X2 * otherLine.Y1 - otherLine.X1 * otherLine.Y2;

      /* Compute r1 and r2 */

      var r1 = a2 * X1 + b2 * Y1 + c2;
      var r2 = a2 * X2 + b2 * Y2 + c2;

      /* Check signs of r1 and r2.  If both point 1 and point 2 lie
       * on same side of second line segment, the line segments do
       * not intersect.
       */
      if (r1 != 0 && r2 != 0 && Math.Sign(r1) == Math.Sign(r2))
      {
        return (null); // DONT_INTERSECT
      }

      /* Line segments intersect: compute intersection point. 
       */

      var denom = a1 * b2 - a2 * b1;
      if (denom == 0)
      {
        return null; //( COLLINEAR );
      }
      var offset = denom < 0 ? -denom / 2 : denom / 2;

      /* The denom/2 is to get rounding instead of truncating.  It
       * is added or subtracted to the numerator, depending upon the
       * sign of the numerator.
       */

      var num = b1 * c2 - b2 * c1;
      var x = (num < 0 ? num - offset : num + offset) / denom;

      num = a2 * c1 - a1 * c2;
      var y = (num < 0 ? num - offset : num + offset) / denom;
      return new Point(x, y);
    }

    public Point From
    {
      get { return new Point(X1, Y1); }
    }

    public Point To
    {
      get { return new Point(X2, Y2); }
    }
  }
}
