using System.Windows;
using Microsoft.Xaml.Interactivity;
#if NETFX_CORE || PORTABLE
using Windows.UI.Xaml;
#endif
namespace WpWinNl.Behaviors
{

  /// <summary>
  /// This behavior listens to a size change of the attached object
  /// and puts the resulting size into two bindable dependecy properties
  /// </summary>
  public class SizeListenerBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      AssociatedObject.SizeChanged += AssociatedObjectSizeChanged;
      base.OnAttached();
      PropagateSizes();
    }

    protected override void OnDetaching()
    {
      AssociatedObject.SizeChanged -= AssociatedObjectSizeChanged;
      base.OnDetaching();
    }

    void AssociatedObjectSizeChanged(object sender, SizeChangedEventArgs e)
    {
      PropagateSizes();
    }

    private void PropagateSizes()
    {
      WatchedObjectHeight = AssociatedObject.ActualHeight;
      WatchedObjectWidth = AssociatedObject.ActualWidth;
    }

    #region WatchedObjectWidth

    /// <summary>
    /// WatchedObjectWidth Property name
    /// </summary>
    public const string WatchedObjectWidthPropertyName = "WatchedObjectWidth";

    public double WatchedObjectWidth
    {
      get { return (double)GetValue(WatchedObjectWidthProperty); }
      set { SetValue(WatchedObjectWidthProperty, value); }
    }

    /// <summary>
    /// WatchedObjectWidth Property definition
    /// </summary>
    public static readonly DependencyProperty WatchedObjectWidthProperty = DependencyProperty.Register(
        WatchedObjectWidthPropertyName,
        typeof(double),
        typeof(SizeListenerBehavior),
        new PropertyMetadata(default(double), null));
    #endregion

    #region WatchedObjectHeight

    /// <summary>
    /// WatchedObjectHeight Property name
    /// </summary>
    public const string WatchedObjectHeightPropertyName = "WatchedObjectHeight";

    public double WatchedObjectHeight
    {
      get { return (double)GetValue(WatchedObjectHeightProperty); }
      set { SetValue(WatchedObjectHeightProperty, value); }
    }

    /// <summary>
    /// WatchedObjectHeight Property definition
    /// </summary>
    public static readonly DependencyProperty WatchedObjectHeightProperty = DependencyProperty.Register(
        WatchedObjectHeightPropertyName,
        typeof(double),
        typeof(SizeListenerBehavior),
        new PropertyMetadata(default(double), null));

    #endregion


  }
}
