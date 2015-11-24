using Windows.Devices.Geolocation;
using Windows.Foundation;
using System;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;

namespace WpWinNl.Maps
{
  public class MapIconDrawer : MapShapeDrawer
  {
    public MapIconDrawer()
    {
      AnchorX = 0.5;
      AnchorY = 0.5;
    }

    public string Title { get; set; }

    public double AnchorX { get; set; }
    public double AnchorY { get; set; }

    public string ImageUri { get; set; }

    public MapElementCollisionBehavior CollisionBehaviorDesired { get; set; }

    public override MapElement CreateShape(object viewModel, BasicGeoposition pos)
    {
      var icon = new MapIcon { Location = new Geopoint(pos), 
        NormalizedAnchorPoint = new Point(AnchorX,AnchorY),  ZIndex = ZIndex, CollisionBehaviorDesired = CollisionBehaviorDesired};
      if (!string.IsNullOrWhiteSpace(Title))
      {
        icon.Title = Title;
      }

      if (!string.IsNullOrWhiteSpace(ImageUri))
      {

        icon.Image = RandomAccessStreamReference.CreateFromUri(new Uri(ImageUri));
      }

      return icon;
    }
  }
}
