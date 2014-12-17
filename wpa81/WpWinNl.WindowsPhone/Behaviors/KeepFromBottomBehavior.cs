using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace WpWinNl.Behaviors
{
  public class KeepFromBottomBehavior : SafeBehavior<FrameworkElement>
  {

    private double originalBottomMargin;
    protected override void OnSetup()
    {
      originalBottomMargin = AssociatedObject.Margin.Bottom;
      UpdateBottomMargin();
      ApplicationView.GetForCurrentView().VisibleBoundsChanged += KeepInViewBehaviorVisibleBoundsChanged;

      base.OnSetup();
    }

    void KeepInViewBehaviorVisibleBoundsChanged(ApplicationView sender, object args)
    {
      UpdateBottomMargin();
    }

    private void UpdateBottomMargin()
    {
      if (WindowHeight > 0.01)
      {
        var currentMargins = AssociatedObject.Margin;

        var newMargin = new Thickness(currentMargins.Left, currentMargins.Top, currentMargins.Right,
          originalBottomMargin + (WindowHeight - ApplicationView.GetForCurrentView().VisibleBounds.Bottom));
        AssociatedObject.Margin = newMargin;
      }
    }

    #region WindowHeight

    /// <summary>
    /// WindowHeight Property name
    /// </summary>
    public const string WindowHeightPropertyName = "WindowHeight";

    public double WindowHeight
    {
      get { return (double)GetValue(WindowHeightProperty); }
      set { SetValue(WindowHeightProperty, value); }
    }

    /// <summary>
    /// WindowHeight Property definition
    /// </summary>
    public static readonly DependencyProperty WindowHeightProperty = DependencyProperty.Register(
        WindowHeightPropertyName,
        typeof(double),
        typeof(KeepFromBottomBehavior),
        new PropertyMetadata(default(double), WindowHeightChanged));

    /// <summary>
    /// WindowHeight property changed callback.
    /// </summary>
    /// <param name="d">The depency object (i.e. the behavior).</param>
    /// <param name="e">The property event args <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>  
    public static void WindowHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var thisobj = d as KeepFromBottomBehavior;
      var newValue = (double)e.NewValue;
      if (thisobj != null)
      {
        thisobj.UpdateBottomMargin();
      }
    }

    #endregion


  }
}
