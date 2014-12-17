using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using WpWinNl.Converters;

namespace WpWinNl.Maps
{
  public class BasicGeopointConverter : BaseValueConverter
  {
    public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value is BasicGeoposition)
      {
        var p = (BasicGeoposition) value;
        return new Geopoint(p);
      }
      return null;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value is Geopoint)
      {
        var p = (Geopoint) value;
        return p.Position;
      }

      return null;
    }
  }
}
