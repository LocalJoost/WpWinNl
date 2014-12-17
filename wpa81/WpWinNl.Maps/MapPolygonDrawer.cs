
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;

namespace WpWinNl.Maps
{
  public class MapPolygonDrawer : MapLinearShapeDrawer
  {
    public MapPolygonDrawer()
    {
      Color = Colors.Black;
    }
    public Color StrokeColor { get; set; }

    public override MapElement CreateShape(object viewModel, Geopath path)
    {
      return new MapPolygon { Path = path, FillColor = Color, StrokeDashed = StrokeDashed, StrokeThickness = Width, StrokeColor = StrokeColor, ZIndex = ZIndex};
    }
  }
}
