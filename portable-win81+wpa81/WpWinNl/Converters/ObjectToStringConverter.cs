using System;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
#if !WINDOWS_PHONE
using Windows.UI.Xaml;
#endif

namespace WpWinNl.Converters
{
  /// <summary>
  /// Converter to create a nicer representation of an object.
  /// Add properties of the binded object between { }. The entered propertynames are not case sensitive.
  /// Example:
  /// add {firstname}{lastname} to the converter parameter 
  /// for binding to 'person' class Person.FirstName and Person.LastName
  /// </summary>
  public class ObjectToStringValueConverter : BaseValueConverter
  {
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (parameter == null)
      {
        return value.ToString();
      }

      var formatString = parameter.ToString().Replace("\\n", "\n");
      var resultstring = formatString;
      var r = new Regex(@"\{[a-z,A-Z]+(?:\.[a-z,A-Z]*)?\}");
      var q = r.Matches(formatString);

      foreach (Match match in q)
      {
        var extracted = ExtractValue(value, match.Value.Substring(1, match.Value.Length - 2));
        if (!string.IsNullOrWhiteSpace(extracted))
        {
          resultstring = resultstring.Replace(match.Value, extracted);
        }
      }
      return resultstring;
    }

    private static string ExtractValue(object value, string match)
    {
      var type = value.GetType();
      var d = type.GetRuntimeProperties().FirstOrDefault(x => x.Name.ToLower() == match.ToLower());
      if (d != null)
      {
        return d.GetValue(value, null).ToString().Trim();
      }
      else
      {
        return null;
      }
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return DependencyProperty.UnsetValue;
    }
  }
}
