using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace WpWinNl.Geofencing
{
  public static class GeofenceMonitorExtensions
  {
    public static IList<IList<Geopoint>> GetFenceGeometries(this GeofenceMonitor monitor)
    {
      return monitor.Geofences.Select( p=> p.ToCirclePoints()).ToList();
    }
  }
}
