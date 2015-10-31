
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace WpWinNl.Maps
{
  public static class MapBindingHelpers
  {
    #region Attached Dependency Property ObjectData
    public static readonly DependencyProperty ObjectDataProperty =
         DependencyProperty.RegisterAttached("ObjectData",
         typeof(object),
         typeof(MapBindingHelpers),
         new PropertyMetadata(default(object), null));

    public static object GetObjectData(DependencyObject obj)
    {
      return obj.GetValue(ObjectDataProperty) as object;
    }

    public static void SetObjectData(
       DependencyObject obj,
       object value)
    {
      obj.SetValue(ObjectDataProperty, value);
    }
    #endregion

    public static void AddData(this DependencyObject element, object data)
    {
      SetObjectData(element, data);
    }

    public static T ReadData<T>(this DependencyObject element) where T : class
    {
      return GetObjectData(element) as T;
    }

    public static object ReadData(this DependencyObject element) 
    {
      return GetObjectData(element);
    }

    #region Attached Dependency Property LayerName
    public static readonly DependencyProperty LayerNameProperty =
         DependencyProperty.RegisterAttached("LayerName",
         typeof(string),
         typeof(MapBindingHelpers),
         new PropertyMetadata(default(string), LayerNameChanged));

    // Called when Property is retrieved
    public static string GetLayerName(this DependencyObject obj)
    {
      return obj.GetValue(LayerNameProperty) as string;
    }

    // Called when Property is set
    public static void SetLayerName(
       DependencyObject obj,
       string value)
    {
      obj.SetValue(LayerNameProperty, value);
    }

    // Called when property is changed
    private static void LayerNameChanged(
     object sender,
     DependencyPropertyChangedEventArgs args)
    {
      var attachedObject = sender as FrameworkElement;
      if (attachedObject != null)
      {
        // do whatever is necessary
      }
    }
    #endregion

    public static void SetLayer(this DependencyObject element, string name)
    {
      SetLayerName(element, name);
    }

    public static string GetLayer(this DependencyObject element)
    {
      return GetLayerName(element);
    }

    #region Attached Dependency Property MapViewArea
    public static readonly DependencyProperty MapViewAreaProperty =
         DependencyProperty.RegisterAttached("MapViewArea",
         typeof(GeoboundingBox),
         typeof(MapBindingHelpers),
         new PropertyMetadata(default(GeoboundingBox), MapViewAreaChanged));

    // Called when Property is retrieved
    public static GeoboundingBox GetMapViewArea(DependencyObject obj)
    {
      return obj.GetValue(MapViewAreaProperty) as GeoboundingBox;
    }

    // Called when Property is set
    public static void SetMapViewArea(
       DependencyObject obj,
       GeoboundingBox value)
    {
      obj.SetValue(MapViewAreaProperty, value);
    }

    // Called when property is changed
    private static void MapViewAreaChanged(
     object sender,
     DependencyPropertyChangedEventArgs args)
    {
      var attachedObject = sender as MapControl;
      if (attachedObject != null)
      {
        var box = args.NewValue as GeoboundingBox;
        if (box != null)
        {
          attachedObject.TrySetViewBoundsAsync(box, new Thickness(1.0), MapAnimationKind.Bow);
        }
      }
    }
    #endregion

  }
}
