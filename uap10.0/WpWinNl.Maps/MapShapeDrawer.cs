using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace WpWinNl.Maps
{
  public abstract class MapShapeDrawer
  {
    public virtual MapElement CreateShape(object viewModel, BasicGeoposition postion)
    {
      return null;
    }

    public virtual MapElement CreateShape(object viewModel, Geopath path)
    {
      return null;
    }

    public virtual MapElement CreateShape(object viewModel, IList<Geopath> paths)
    {
      return null;
    }

    public int ZIndex { get; set; }
  }
}
