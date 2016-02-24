using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WpWinNl.Behaviors
{
  public class KeepFromTopBehavior : KeepFromBottomBehavior
  {
    protected override Thickness GetNewMargin()
    {
      var currentMargin = AssociatedObject.Margin;

      return new Thickness(currentMargin.Left,
                           OriginalMargin + (AppBar.IsOpen ? GetDeltaHeight() : 0) , 
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

