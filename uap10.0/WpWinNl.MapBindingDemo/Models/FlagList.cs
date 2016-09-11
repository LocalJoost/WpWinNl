using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace WpWinNl.MapBindingDemo.Models
{
  public class FlagList : GeometryProvider
  {
    public static readonly string[] Countries = { "Belgium", "Germany", "Italy", "Netherlands", "Sweden", "UK" };


    private Uri _iconUri;
    public Uri Icon
    {
      get { return _iconUri; }
      set { Set(() => Icon, ref _iconUri, value); }
    }

    private BasicGeoposition _point;
    public BasicGeoposition Point
    {
      get { return _point; }
      set { Set(() => Point, ref _point, value); }
    }

    private Point _anchorPoint = new Point(0.5, 0.5);
    public Point AnchorPoint
    {
      get { return _anchorPoint; }
      set { Set(() => AnchorPoint, ref _anchorPoint, value); }
    }

    private bool _isVisible = true;
    public bool IsVisible
    {
      get { return _isVisible; }
      set { Set(() => IsVisible, ref _isVisible, value); }
    }

    public static IEnumerable<FlagList> GetRandomFlags(Geopoint point1, Geopoint point2, int nrOfPoints)
    {
      var flags = new List<FlagList>();
      var points = PointList.GetRandomPoints(point1, point2, nrOfPoints);
      var r = new Random(DateTime.Now.Millisecond * 2);
      foreach (var point in points)
      {
        var flagIdx = (int)Math.Round(r.NextDouble() * 5);
        flags.Add(new FlagList
        {
          Name = Countries[flagIdx],
          Icon = new Uri($"ms-appx:///Assets/{Countries[flagIdx]}.png"),
          Point = point.Point
        });
      }
      return flags;
    }
  }
}
