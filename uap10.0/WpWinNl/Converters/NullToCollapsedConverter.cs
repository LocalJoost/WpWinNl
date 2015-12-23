using System;
using System.Globalization;
using System.Windows;
#if !WINDOWS_PHONE
using Windows.UI.Xaml;
#endif

namespace WpWinNl.Converters
{
  /// <summary>
  /// Returns Visibility.Collapsed for any value that is null, empty, only whitespaces or an empty guid.
  /// </summary>
  public class NullToCollapsedConverter : BaseValueConverter
  {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value == null || string.IsNullOrWhiteSpace(value.ToString()) || value.ToString() == Guid.Empty.ToString() ? Visibility.Collapsed : Visibility.Visible;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return DependencyProperty.UnsetValue;
    }
  }
}
