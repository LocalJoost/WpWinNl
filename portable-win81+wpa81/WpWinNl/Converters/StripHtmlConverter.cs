using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
#if !WINDOWS_PHONE
using Windows.UI.Xaml;
#endif

namespace WpWinNl.Converters
{
  /// <summary>
  /// Removes all HTML from a string and decodes '&XXXX' to regular chars.
  /// </summary>
  public class StripHtmlValueConverter : BaseValueConverter
  {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return value;

      var regex = new Regex(
          @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);

      string str = value.ToString().Replace("\n", string.Empty);
      str = Regex.Replace(str, "<br>", "\n", RegexOptions.IgnoreCase);
      str = Regex.Replace(str, "<br/>", "\n", RegexOptions.IgnoreCase);
      str = Regex.Replace(str, "<br />", "\n", RegexOptions.IgnoreCase);

      str = regex.Replace(str, string.Empty);
#if WINDOWS_PHONE
      str = HttpUtility.HtmlDecode(str);
#else
      str = WebUtility.HtmlDecode(str);
#endif


      return str;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return DependencyProperty.UnsetValue;
    }
  }
}