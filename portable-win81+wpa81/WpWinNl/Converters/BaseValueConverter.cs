using System;
using System.Globalization;
#if WINDOWS_PHONE
using System.Windows.Data;
#else
using Windows.UI.Xaml.Data;
#endif

namespace WpWinNl.Converters
{
  public abstract class BaseValueConverter : IValueConverter
  {
    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
    public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

#if !WINDOWS_PHONE
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      return Convert(value, targetType, parameter, new CultureInfo(language));
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      return ConvertBack(value, targetType, parameter, new CultureInfo(language));
    }
#endif
  }
}