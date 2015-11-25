using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;

namespace WpWinNl.Maps
{
  public class MapMultiPolygonDrawer : MapLinearShapeDrawer
  {
    public MapMultiPolygonDrawer()
    {

      Color = Colors.Black;
    }
    public Color StrokeColor { get; set; }

    public override MapElement CreateShape(object viewModel, IList<Geopath> paths)
    {
      var multiPolygon =  new MapPolygon { FillColor = Color, StrokeDashed = StrokeDashed, StrokeThickness = Width, StrokeColor = StrokeColor, ZIndex = ZIndex };
      foreach (var path in paths)
      {
        multiPolygon.Paths.Add(path);
      }
      return multiPolygon;
    }
  }
}
