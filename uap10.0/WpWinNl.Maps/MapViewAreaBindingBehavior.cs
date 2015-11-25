using System;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Maps
{
  public class MapViewAreaBindingBehavior : Behavior<MapControl>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      AssociatedObject.ZoomLevelChanged += AssociatedObject_ZoomLevelChanged;
      AssociatedObject.CenterChanged += AssociatedObject_CenterChanged;
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      AssociatedObject.ZoomLevelChanged -= AssociatedObject_ZoomLevelChanged;
      AssociatedObject.CenterChanged -= AssociatedObject_CenterChanged;
    }

    private void AssociatedObject_CenterChanged(MapControl sender, object args)
    {
      TryGetNewCenter();
    }

    private void AssociatedObject_ZoomLevelChanged(MapControl sender, object args)
    {
      TryGetNewCenter();
    }

    private void TryGetNewCenter()
    {
      try
      {
        MapViewArea = AssociatedObject.GetViewArea();
      }
      catch (Exception)
      {
      }
    }

    #region Attached Dependency Property MapViewArea
    public static readonly DependencyProperty MapViewAreaProperty =
         DependencyProperty.RegisterAttached("MapViewArea",
         typeof(GeoboundingBox),
         typeof(MapViewAreaBindingBehavior),
         new PropertyMetadata(default(GeoboundingBox)));

    public GeoboundingBox MapViewArea
    {
      get
      {
        return GetMapViewArea(this);
      }
      set
      {
        SetMapViewArea(this, value);
      }
    }

    // Called when Property is retrieved
    public static GeoboundingBox GetMapViewArea(MapViewAreaBindingBehavior obj)
    {
      return obj.GetValue(MapViewAreaProperty) as GeoboundingBox;
    }

    // Called when Property is set
    public static void SetMapViewArea(
       MapViewAreaBindingBehavior obj,
       GeoboundingBox value)
    {
      obj.SetValue(MapViewAreaProperty, value);
    }

    #endregion
  }
}
