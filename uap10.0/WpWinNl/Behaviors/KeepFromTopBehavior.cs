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
      var baseHeight = 0.0;
      if (ApplicationView.GetForCurrentView().DesiredBoundsMode == ApplicationViewBoundsMode.UseCoreWindow)
      {
        var visibleBounds = ApplicationView.GetForCurrentView().VisibleBounds;
        baseHeight = visibleBounds.Top - CoreApplication.GetCurrentView().CoreWindow.Bounds.Top + AppBar.ActualHeight;
      }
      return new Thickness(currentMargin.Left,
                           OriginalMargin + (AppBar.IsOpen ? GetDeltaHeight() + baseHeight : baseHeight) , 
                           currentMargin.Right, currentMargin.Bottom);
    }

    protected override AppBar SetAppBar(Page page)
    {
      return page.TopAppBar;
    }

    protected override double GetOriginalMargin()
    {
      return AssociatedObject.Margin.Top;
    }
  }
}

