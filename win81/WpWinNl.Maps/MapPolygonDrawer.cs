using Bing.Maps;
using Windows.UI;

namespace WpWinNl.Maps
{
  public class MapPolygonDrawer : MapShapeDrawer
  {
    public MapPolygonDrawer()
    {
      Color = Colors.Black;
    }

    public override MapShape CreateShape(object viewModel, LocationCollection path)
    {
      return new MapPolygon { Locations = path, FillColor = Color };
    }
  }
}
