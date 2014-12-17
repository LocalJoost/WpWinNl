using System;
using Bing.Maps;
using Windows.UI;

namespace WpWinNl.Maps
{
  public class MapStarDrawer : MapShapeDrawer
  {
    public MapStarDrawer()
    {
      Color = Colors.Black;
      Arms = 8;
      OuterRadius = 50;
      InnerRadius = 25;
      NorthCompressionFactor = 0.60;
    }

    public override MapShape CreateShape(object viewModel, LocationCollection path)
    {
      return new MapPolygon { Locations = CreateStarPoints(path[0]), FillColor = Color };
    }

    /// <summary>
    /// Creates the star points.
    /// </summary>
    /// <param name="center">The center.</param>
    /// <returns>The star points.</returns>
    private LocationCollection CreateStarPoints(Location center )
    {
      var locations = new LocationCollection();
      var ir = InnerRadius / 200000;
      var or = OuterRadius / 200000;
      var angle = Math.PI / Arms;

      for (var i = 0; i <= 2 * Arms; i++)
      {
        var r = (i & 1) == 0 ? or : ir;
        locations.Add(new Location(center.Latitude + ((Math.Cos(i * angle) * r) * NorthCompressionFactor), center.Longitude + (Math.Sin(i * angle) * r)));
      }

      return locations;
    }

    public int Arms { get; set; }

    public double OuterRadius { get; set; }

    public double InnerRadius { get; set; }

    public double NorthCompressionFactor { get; set; }
  }
}
