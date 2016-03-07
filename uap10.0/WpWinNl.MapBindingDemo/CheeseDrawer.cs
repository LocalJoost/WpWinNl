using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using WpWinNl.MapBindingDemo.Models;
using WpWinNl.Maps;

namespace WpWinNl.MapBindingDemo
{
  public class CheeseDrawer : MapIconDrawer
  {
    public override MapElement CreateShape(object viewModel, BasicGeoposition pos)
    {
      var icon = (MapIcon) base.CreateShape(viewModel, pos);
      icon.Title = ((PointList) viewModel).Name;
      return icon;
    }
  }
}
