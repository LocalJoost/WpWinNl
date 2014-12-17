using System.Device.Location;
using System.Windows;
using Microsoft.Phone.Maps.Controls;

namespace WpWinNl.Behaviors
{
  public class ViewportAreaBehavior
  {
    /// <summary>
    /// A behavior to make southwest and northeast bindable
    /// </summary>
    public class ViewportwatcherBehavior : SafeBehavior<Map>
    {
      protected override void OnSetup()
      {
        AssociatedObject.ViewChanged += AssociatedObjectViewChangeEnd;
        CalcRectangle();
      }

      void AssociatedObjectViewChangeEnd(object sender, MapEventArgs e)
      {
        CalcRectangle();
      }

      protected override void OnCleanup()
      {
        AssociatedObject.ViewChanged -= AssociatedObjectViewChangeEnd;
      }

      private void CalcRectangle()
      {
        NorthWest = AssociatedObject.ConvertViewportPointToGeoCoordinate(new Point(0, 0));
        SouthEast = AssociatedObject.ConvertViewportPointToGeoCoordinate(
          new Point(AssociatedObject.ActualWidth, AssociatedObject.ActualHeight));
      }

      #region NorthWest
      public const string NorthWestPropertyName = "NorthWest";

      public GeoCoordinate NorthWest
      {
        get { return (GeoCoordinate)GetValue(NorthWestProperty); }
        set { SetValue(NorthWestProperty, value); }
      }

      public static readonly DependencyProperty NorthWestProperty = DependencyProperty.Register(
          NorthWestPropertyName,
          typeof(GeoCoordinate),
          typeof(ViewportwatcherBehavior),
          new PropertyMetadata(null));

      #endregion

      #region SouthEast
      public const string SouthEastPropertyName = "SouthEast";

      public GeoCoordinate SouthEast
      {
        get { return (GeoCoordinate)GetValue(SouthEastProperty); }
        set { SetValue(SouthEastProperty, value); }
      }

      public static readonly DependencyProperty SouthEastProperty = DependencyProperty.Register(
          SouthEastPropertyName,
          typeof(GeoCoordinate),
          typeof(ViewportwatcherBehavior),
          new PropertyMetadata(null));

      #endregion
    }
  }
}
