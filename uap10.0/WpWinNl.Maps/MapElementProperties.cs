
using Windows.UI.Xaml;

namespace WpWinNl.Maps
{
  public static class MapElementProperties
  {
    #region Attached Dependency Property ViewModel
    public static readonly DependencyProperty ViewModelProperty =
         DependencyProperty.RegisterAttached("ViewModel",
         typeof(object),
         typeof(MapElementProperties),
         new PropertyMetadata(default(object), ViewModelChanged));

    // Called when Property is retrieved
    public static object GetViewModel(DependencyObject obj)
    {
      return obj.GetValue(ViewModelProperty) as object;
    }

    // Called when Property is set
    public static void SetViewModel(
       DependencyObject obj,
       object value)
    {
      obj.SetValue(ViewModelProperty, value);
    }

    // Called when property is changed
    private static void ViewModelChanged(
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

    #region Attached Dependency Property LayerName
    public static readonly DependencyProperty LayerNameProperty =
         DependencyProperty.RegisterAttached("LayerName",
         typeof(string),
         typeof(MapElementProperties),
         new PropertyMetadata(default(string), LayerNameChanged));

    // Called when Property is retrieved
    public static string GetLayerName(DependencyObject obj)
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

  }
}
