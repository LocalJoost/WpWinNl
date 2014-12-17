using Bing.Maps;
using Windows.UI;

namespace WpWinNl.Maps
{
  public abstract class MapShapeDrawer
  {
    public abstract MapShape CreateShape(object viewModel, LocationCollection path);

    public Color Color { get; set; }
  }
}
