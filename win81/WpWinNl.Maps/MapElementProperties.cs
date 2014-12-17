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
         new PropertyMetadata(default(object)));

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

    #endregion

    #region Attached Dependency Property LayerName
    public static readonly DependencyProperty LayerNameProperty =
         DependencyProperty.RegisterAttached("LayerName",
         typeof(string),
         typeof(MapElementProperties),
         new PropertyMetadata(default(string)));

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

    #endregion

  }
}
