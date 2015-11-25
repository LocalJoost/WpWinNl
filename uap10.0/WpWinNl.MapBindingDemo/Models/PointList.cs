using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;

namespace WpWinNl.MapBindingDemo.Models
{
  public class PointList : GeometryProvider
  {
    public BasicGeoposition Point { get; set; }

    /// <summary>
    /// Get 50 random point in the view
    /// </summary>
    public static IEnumerable<PointList> GetRandomPoints(Geopoint point1, Geopoint point2, int nrOfPoints)
    {
      var result = new List<PointList>();
      var p1 = new BasicGeoposition
      {
        Latitude = Math.Min(point1.Position.Latitude, point2.Position.Latitude),
        Longitude = Math.Min(point1.Position.Longitude, point2.Position.Longitude)
      };
      var p2 = new BasicGeoposition
      {
        Latitude = Math.Max(point1.Position.Latitude, point2.Position.Latitude),
        Longitude = Math.Max(point1.Position.Longitude, point2.Position.Longitude)
      };

      var dLat = p2.Latitude - p1.Latitude;
      var dLon = p2.Longitude - p1.Longitude;

      var r = new Random(DateTime.Now.Millisecond);
      for (var i = 0; i < nrOfPoints; i++)
      {
        var item = new PointList
        {
          Name = "Point " + i,
          Point = new BasicGeoposition
          {
            Latitude = p1.Latitude + (r.NextDouble() * dLat),
            Longitude = p1.Longitude + (r.NextDouble() * dLon)
          }
        };
        result.Add(item);
      }
      return result;
    }
  }
}
