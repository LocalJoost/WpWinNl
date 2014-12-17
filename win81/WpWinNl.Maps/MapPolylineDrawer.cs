using Bing.Maps;
using Windows.UI;

namespace WpWinNl.Maps
{
  public class MapPolylineDrawer : MapShapeDrawer
  {
    public MapPolylineDrawer()
    {
      Color = Colors.Black;
      Width = 5;
    }

    public override MapShape CreateShape(object viewModel, LocationCollection path)
    {
      return new MapPolyline { Locations = path, Color = Color, Width = Width };
    }

    public double Width { get; set; }
  }
}
