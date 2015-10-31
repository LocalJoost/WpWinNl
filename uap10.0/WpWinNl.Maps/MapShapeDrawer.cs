using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace WpWinNl.Maps
{
  public abstract class MapShapeDrawer
  {
    public abstract MapElement CreateShape(object viewModel, Geopath path);

    public int ZIndex { get; set; }

  }
}
