using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WpWinNl.Utilities;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// Behaviors that mirrors an object via using an animation
  /// </summary>
  public class MirrorBehavior : BaseScaleBehavior
  {
    protected override CompositeTransform BuildTransform()
    {
      return new CompositeTransform
      {
        ScaleX = (Direction == Direction.Horizontal || Direction == Direction.Both) && Activated ? -1 : 1,
        ScaleY = (Direction == Direction.Vertical || Direction == Direction.Both) && Activated ? -1 : 1
      };
    }

    protected override Storyboard BuildStoryBoard()
    {
      var storyboard = new Storyboard {FillBehavior = FillBehavior.HoldEnd};
      var transform = BuildTransform();
      var duration = new Duration(TimeSpan.FromMilliseconds(Duration));
      storyboard.AddScalingAnimation(
        AssociatedObject,
        AssociatedObject.GetScaleXProperty(), transform.ScaleX,
        AssociatedObject.GetScaleYProperty(), transform.ScaleY,
        duration);
      return storyboard;
    }
  }

}
