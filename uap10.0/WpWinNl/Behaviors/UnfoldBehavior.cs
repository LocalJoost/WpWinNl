using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WpWinNl.Utilities;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// Behavior that unfolds an object using an animation
  /// </summary>
  public class UnfoldBehavior : BaseScaleBehavior
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      AssociatedObject.Opacity = GetOpacity();
    }

    protected override Storyboard BuildStoryBoard()
    {
      var transform = BuildTransform();
      if (AssociatedObject.GetOpacityProperty() != GetOpacity() ||
          transform.ScaleX != AssociatedObject.GetScaleXProperty() ||
          transform.ScaleY != AssociatedObject.GetScaleYProperty())
      {
        var storyboard = new Storyboard {FillBehavior = FillBehavior.HoldEnd};
        var duration = new Duration(TimeSpan.FromMilliseconds(Duration));
        storyboard.AddScalingAnimation(
          AssociatedObject,
          AssociatedObject.GetScaleXProperty(), transform.ScaleX,
          AssociatedObject.GetScaleYProperty(), transform.ScaleY,
          duration);
        storyboard.AddOpacityAnimation(AssociatedObject, AssociatedObject.GetOpacityProperty(), GetOpacity(), duration);
        return storyboard;
      }
      return null;
    }

    protected override CompositeTransform BuildTransform()
    {
      return new CompositeTransform
      {
        ScaleX = (Direction == Direction.Horizontal || Direction == Direction.Both) && !Activated ? 0 : 1,
        ScaleY = (Direction == Direction.Vertical || Direction == Direction.Both) && !Activated ? 0 : 1
      };
    }

    private double GetOpacity()
    {
      return Activated ? 1 : 0;
    }
  }
}
