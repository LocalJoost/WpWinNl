using System;
using System.Linq;
#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
#endif


namespace WpWinNl.Utilities
{
  /// <summary>
  /// Class to help animations from code using the CompositeTransform
  /// </summary>
  public static class FrameworkElementExtensions
  {
    /// <summary>
    /// Finds the composite transform either direct
    /// or as part of a TransformGroup
    /// </summary>
    /// <param name="fe">FrameworkElement</param>
    /// <returns></returns>
    public static CompositeTransform GetCompositeTransform(this FrameworkElement fe)
    {
      if (fe.RenderTransform != null)
      {
        var tt = fe.RenderTransform as CompositeTransform;
        if (tt != null) return tt;

        var tg = fe.RenderTransform as TransformGroup;
        if (tg != null)
        {
          return tg.Children.OfType<CompositeTransform>().FirstOrDefault();
        }
      }
      return null;
    }

    /// <summary>
    /// Gets the point to where FrameworkElement is translated
    /// </summary>
    /// <param name="fe">FrameworkElement</param>
    /// <returns>Translation point</returns>
    public static Point GetTranslatePoint(this FrameworkElement fe)
    {
      var translate = fe.GetCompositeTransform();

      if (translate == null) throw new ArgumentNullException("CompositeTransform");

      return new Point(
          (double)translate.GetValue(CompositeTransform.TranslateXProperty),
          (double)translate.GetValue(CompositeTransform.TranslateYProperty));

    }

    /// <summary>
    /// Translates a FrameworkElement to a new location
    /// </summary>
    /// <param name="fe">FrameworkElement</param>
    /// <param name="p">the new location</param>
    public static void SetTranslatePoint(this FrameworkElement fe, Point p)
    {
      var translate = fe.GetCompositeTransform();
      if (translate == null) throw new ArgumentNullException("CompositeTransform");

      translate.SetValue(CompositeTransform.TranslateXProperty, p.X);
      translate.SetValue(CompositeTransform.TranslateYProperty, p.Y);
    }

    /// <summary>
    /// Translates a FrameworkElement to a new location
    /// </summary>
    /// <param name="fe">FrameworkElement</param>
    /// <param name="x">X coordinate of the new location</param>
    /// <param name="y">Ycoordinate of the new location</param>
    public static void SetTranslatePoint(this FrameworkElement fe, double x, double y)
    {
      fe.SetTranslatePoint(new Point(x, y));
    }

    public static FrameworkElement GetElementToAnimate(this FrameworkElement fe)
    {
      var animated = fe.GetVisualAncestors().FirstOrDefault(p => p is ContentPresenter);
      return animated ?? fe;
    }

    public static bool IsPortrait(this FrameworkElement elem)
    {
      return elem.ActualHeight > elem.ActualWidth;
    }

    /// <summary>
    /// Gets a double property from a framework element
    /// </summary>
    /// <param name="fe">FrameworkElement</param>
    /// <param name="property">DependencyProperty</param>
    /// <returns>Translation point</returns>
    public static double GetDoubleTransformProperty(this FrameworkElement fe, DependencyProperty property)
    {
      var translate = fe.GetCompositeTransform();
      if (translate == null) throw new ArgumentNullException("CompositeTransform");
      return (double)translate.GetValue(property);
    }

    /// <summary>
    /// Gets the ScaleX property from a framework element
    /// </summary>
    /// <param name="fe">FrameworkElement</param>
    /// <returns>Translation X</returns>
    public static double GetScaleXProperty(this FrameworkElement fe)
    {
      return fe.GetDoubleTransformProperty(CompositeTransform.ScaleXProperty);
    }

    /// <summary>
    /// Gets the ScaleY property from a framework element
    /// </summary>
    /// <param name="fe">FrameworkElement</param>
    /// <returns>Translation Y</returns>
    public static double GetScaleYProperty(this FrameworkElement fe)
    {
      return fe.GetDoubleTransformProperty(CompositeTransform.ScaleYProperty);
    }

    /// <summary>
    /// Gets the Opacity property from a framework element
    /// </summary>
    /// <param name="fe">FrameworkElement</param>
    /// <returns>Opacity</returns>
    public static double GetOpacityProperty(this FrameworkElement fe)
    {
      return (double)fe.GetValue(UIElement.OpacityProperty);
    }
  }
}
