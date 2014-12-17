using System;
using System.Globalization;
#if !WINDOWS_PHONE
using Windows.UI.Xaml.Data;
#endif

namespace WpWinNl.Converters
{
  /// <summary>
  /// Converts true to false and vice versa
  /// </summary>
  public class BoolInvertConverter : BaseValueConverter
  {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null && value is bool)
      {
        var bValue = (bool)value;
        return !(bValue);
      }

      return null;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Convert(value, targetType, parameter, culture);
    }
  }
}
