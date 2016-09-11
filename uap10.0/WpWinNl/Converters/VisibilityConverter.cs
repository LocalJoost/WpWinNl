using System;
using System.Globalization;
using Windows.UI.Xaml;

namespace WpWinNl.Converters
{
  /// <summary>
  /// Converts true to the value of parameter
  /// </summary>
  public class VisibilityConverter : BaseValueConverter
  {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (parameter == null)
      {
        parameter = Visibility.Visible;
      }

      if (value is bool)
      {
        var bValue = (bool)value;
        var visibility = (Visibility)Enum.Parse(typeof(Visibility), parameter.ToString(), true);
        if (bValue) return visibility;
        return visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
      }

      return parameter;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return DependencyProperty.UnsetValue;
    }
  }
}
