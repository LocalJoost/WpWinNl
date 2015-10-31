using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;

namespace WpWinNl.Maps
{
  public class MapPolylineDrawer : MapLinearShapeDrawer
  {
    public MapPolylineDrawer()
    {
      Color = Colors.Black;
      Width = 5;
    }

    public override MapElement CreateShape(object viewModel, Geopath path)
    {
      return new MapPolyline { Path = path, StrokeThickness = Width, StrokeColor = Color, StrokeDashed = StrokeDashed, ZIndex = ZIndex};
    }
  }
}
