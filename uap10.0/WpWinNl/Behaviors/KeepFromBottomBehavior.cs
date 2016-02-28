using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Behaviors
{
  public class KeepFromBottomBehavior : Behavior<FrameworkElement>
  {
    protected AppBar AppBar { get; private set; }

    protected double OriginalMargin { get; private set; }

    protected override void OnAttached()
    {
      AssociatedObject.Loaded += AssociatedObjectLoaded;
      base.OnAttached();
    }

    private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      OriginalMargin = GetOriginalMargin();
      AssociatedObject.Loaded -= AssociatedObjectLoaded;
      AppBar = SetAppBar(AssociatedObject.GetVisualAncestors().OfType<Page>().First());
      if (AppBar != null)
      {
        AppBar.Opened += AppBarManipulated;
        AppBar.Closed += AppBarManipulated;
        UpdateMargin();
        ApplicationView.GetForCurrentView().VisibleBoundsChanged += VisibleBoundsChanged;
      }
    }

    private void VisibleBoundsChanged(ApplicationView sender, object args)
    {
      UpdateMargin();
    }

    void AppBarManipulated(object sender, object e)
    {
      UpdateMargin();
    }

    protected override void OnDetaching()
    {
      AppBar.Opened -= AppBarManipulated;
      AppBar.Closed -= AppBarManipulated;
      ApplicationView.GetForCurrentView().VisibleBoundsChanged -= VisibleBoundsChanged;
      base.OnDetaching();
    }

    private void UpdateMargin()
    {
      AssociatedObject.Margin = GetNewMargin();
    }

    protected double GetDeltaHeight()
    {
      var popup = AppBar.GetVisualDescendents().OfType<Popup>().First();
      return popup.ActualHeight - AppBar.ActualHeight;
    }

    protected virtual Thickness GetNewMargin()
    {

      var currentMargin = AssociatedObject.Margin;
      var baseHeight = 0.0;
      if (ApplicationView.GetForCurrentView().DesiredBoundsMode == ApplicationViewBoundsMode.UseCoreWindow)
      {
        var visibleBounds = ApplicationView.GetForCurrentView().VisibleBounds;
        baseHeight = CoreApplication.GetCurrentView().CoreWindow.Bounds.Height - visibleBounds.Height +
                     AppBar.ActualHeight;

        if(AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
        {
          baseHeight -= visibleBounds.Top;
        }
      }

      return new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right,
                           OriginalMargin + (AppBar.IsOpen ? GetDeltaHeight() + baseHeight : baseHeight));
    }

    protected virtual AppBar SetAppBar(Page page)
    {
      return page.BottomAppBar;
    }

    protected virtual double GetOriginalMargin()
    {
      return AssociatedObject.Margin.Bottom;
    }
  }
}

