using System.Linq;
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
    protected override void OnAttached()
    {
      if(!Initialize())
      { 
        AssociatedObject.Loaded += AssociatedObjectLoaded;
      }
      base.OnAttached();
    }

    private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      AssociatedObject.Loaded -= AssociatedObjectLoaded;
      Initialize();
    }

    private bool Initialize()
    {
      var page = AssociatedObject.GetVisualAncestors().OfType<Page>().FirstOrDefault();
      if (page != null)
      {
        AppBar = GetAppBar(page);
        if (AppBar != null)
        {
          OriginalMargin = AssociatedObject.Margin;

          AppBar.Opened += AppBarManipulated;
          AppBar.Closed += AppBarManipulated;
          AppBar.SizeChanged += AppBarSizeChanged;
          UpdateMargin();
          ApplicationView.GetForCurrentView().VisibleBoundsChanged += VisibleBoundsChanged;
          return true;
        }
      }
      return false;
    }

    protected override void OnDetaching()
    {
      AppBar.Opened -= AppBarManipulated;
      AppBar.Closed -= AppBarManipulated;
      AppBar.SizeChanged -= AppBarSizeChanged;
      ApplicationView.GetForCurrentView().VisibleBoundsChanged -= VisibleBoundsChanged;
      ResetMargin();
      base.OnDetaching();
    }
    private void AppBarSizeChanged(object sender, SizeChangedEventArgs e)
    {
      UpdateMargin();
    }

    private void VisibleBoundsChanged(ApplicationView sender, object args)
    {
      UpdateMargin();
    }

    private void AppBarManipulated(object sender, object e)
    {
      UpdateMargin();
    }

    private void UpdateMargin()
    {
      AssociatedObject.Margin = GetNewMargin();
    }

    protected double GetDeltaMargin()
    {
      var popup = AppBar.GetVisualDescendents().OfType<Popup>().First();
      return popup.ActualHeight - AppBar.ActualHeight;
    }

    protected virtual Thickness GetNewMargin()
    {
      var currentMargin = AssociatedObject.Margin;
      var baseMargin = 0.0;
      if (ApplicationView.GetForCurrentView().DesiredBoundsMode == ApplicationViewBoundsMode.UseCoreWindow)
      {
        var visibleBounds = ApplicationView.GetForCurrentView().VisibleBounds;
        baseMargin = CoreApplication.GetCurrentView().CoreWindow.Bounds.Height - visibleBounds.Height +
                     AppBar.ActualHeight;

        if(AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
        {
          baseMargin -= visibleBounds.Top;
        }
      }

      return new Thickness(currentMargin.Left, currentMargin.Top, currentMargin.Right,
                           OriginalMargin.Bottom + (AppBar.IsOpen ? GetDeltaMargin() + baseMargin : baseMargin));
    }

    protected AppBar AppBar { get; private set; }

    protected Thickness OriginalMargin { get; private set; }


    private void ResetMargin()
    {
      AssociatedObject.Margin = OriginalMargin;
    }

    protected virtual AppBar GetAppBar(Page page)
    {
      return page.BottomAppBar;
    }
  }
}

