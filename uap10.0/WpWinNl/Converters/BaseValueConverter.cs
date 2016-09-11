using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace WpWinNl.Converters
{
  public abstract class BaseValueConverter : IValueConverter
  {
    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
    public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      return Convert(value, targetType, parameter, string.IsNullOrWhiteSpace(language) ? null: new CultureInfo(language));
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      return ConvertBack(value, targetType, parameter, string.IsNullOrWhiteSpace(language) ? null : new CultureInfo(language));
    }
  }
}