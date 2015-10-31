using System.Windows;
using Microsoft.Xaml.Interactivity;
#if WINDOWS_PHONE
using System.Windows.Interactivity;
#else
using Windows.UI.Xaml;
#endif

namespace WpWinNl.Behaviors
{
  public class SetInitialOpacityBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      AssociatedObject.Loaded += AssociatedObjectLoaded;
      base.OnAttached();
      AssociatedObject.Opacity = 0;
    }

    void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      AssociatedObject.Loaded -= AssociatedObjectLoaded;
      AssociatedObject.Opacity = OpacityAfterLoading;
    }

    #region OpacityAfterLoading
    public const string OpacityAfterLoadingPropertyName = "OpacityAfterLoading";

    public int OpacityAfterLoading
    {
      get { return (int)GetValue(OpacityAfterLoadingProperty); }
      set { SetValue(OpacityAfterLoadingProperty, value); }
    }

    public static readonly DependencyProperty OpacityAfterLoadingProperty = DependencyProperty.Register(
        OpacityAfterLoadingPropertyName,
        typeof(int),
        typeof(SetInitialOpacityBehavior),
        new PropertyMetadata(1));

    #endregion
  }
}
