using System;
using System.Globalization;

namespace WpWinNl.Converters
{
  public class NegativeValueConverter : BaseValueConverter
  {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var val = System.Convert.ToDecimal(value);
      return -val;
    }
     
    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var val = System.Convert.ToDecimal(value);
      return -val;
    }
  }
}
