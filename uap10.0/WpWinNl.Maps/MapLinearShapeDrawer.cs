using Windows.UI;

namespace WpWinNl.Maps
{
  public abstract class MapLinearShapeDrawer : MapShapeDrawer
  {
    public Color Color { get; set; }

    public bool StrokeDashed { get; set; }

    public double Width { get; set; }
  }
}
