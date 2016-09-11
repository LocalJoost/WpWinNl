using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WpWinNl.Behaviors
{
  public class KeepFromTopBehavior : KeepFromBottomBehavior
  {
    protected override Thickness GetNewMargin()
    {
      var currentMargin = AssociatedObject.Margin;
      var baseMargin = 0.0;
      if (ApplicationView.GetForCurrentView().DesiredBoundsMode == ApplicationViewBoundsMode.UseCoreWindow)
      {
        var visibleBounds = ApplicationView.GetForCurrentView().VisibleBounds;
        baseMargin = visibleBounds.Top - CoreApplication.GetCurrentView().CoreWindow.Bounds.Top + AppBar.ActualHeight;
      }
      return new Thickness(currentMargin.Left,
        OriginalMargin.Top + (AppBar.IsOpen ? GetDeltaMargin() + baseMargin : baseMargin) , 
                           currentMargin.Right, currentMargin.Bottom);
    }

    protected override AppBar GetAppBar(Page page)
    {
      return page.TopAppBar;
    }

  }
}

