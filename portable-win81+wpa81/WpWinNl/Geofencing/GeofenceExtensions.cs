using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using WpWinNl.Maps;

namespace WpWinNl.Geofencing
{
  public static class GeofenceExtensions
  {
    /// <summary>
    /// Converts a geofence to a list of points that makes a circle-shaped of nrOfPoints + 1 points
    /// </summary>
    /// <param name="fence"></param>
    /// <returns></returns>
    public static IList<Geopoint> ToCirclePoints(this Geofence fence, int nrOfPoints = 50)
    {
      var geoCircle = fence.Geoshape as Geocircle;

      if (geoCircle != null)
      {
        return geoCircle.Center.GetCirclePoints(geoCircle.Radius, nrOfPoints);
      }
      return null;
    }
  }
}
