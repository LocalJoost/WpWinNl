using System;
using System.Globalization;
using Windows.UI.Xaml;

namespace WpWinNl.Converters
{
  /// <summary>
  /// Converts a string to all lowercase letters
  /// </summary>
  public class ToLowerValueConverter : BaseValueConverter
  {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null) return string.Empty;

      return value.ToString().ToLowerInvariant();
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return DependencyProperty.UnsetValue;
    }
  }
}