using System;
using System.Collections.Generic;
using System.Globalization;

namespace WpWinNl.Converters
{
  /// <summary>
  /// Converts a single value to a list with that single value
  /// </summary>
  public class ToListConverter : BaseValueConverter
  {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var l = new List<object>();
      if (value != null) l.Add(value);
      return l;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
}
