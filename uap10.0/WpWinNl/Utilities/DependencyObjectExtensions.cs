using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace WpWinNl.Utilities
{
  public static class DependencyObjectExtensions
  {
    public static string GetDependencyPropertyName(this DependencyObject obj, DependencyProperty prop)
    {
      var wantedProperty =
        obj.GetType().GetRuntimeProperties().
          FirstOrDefault(p => p.PropertyType == typeof (DependencyProperty) && p.GetValue(obj) == prop);
      if (wantedProperty != null)
      {
        return wantedProperty.Name.EndsWith("Property") ? wantedProperty.Name.Substring(0, wantedProperty.Name.Length - 8 ) : wantedProperty.Name;
      }

      if (prop == UIElement.OpacityProperty) return "Opacity";
      if (prop == CompositeTransform.TranslateXProperty) return "TranslateX";
      if (prop == CompositeTransform.TranslateYProperty) return "TranslateY";

      throw new ArgumentException("No dependecy property name found");
    }
  }
}
