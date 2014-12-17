using System.Windows;
using WpWinNl.Utilities;

namespace WpWinNl.Behaviors
{
  /// <summary>
  ///     Behavior that sets the attached FrameworkElement to visibile if the app is in trial mode.
  /// </summary>
  public class VisibilityInTrialBehavior : SafeBehavior<FrameworkElement>
  {

    public Visibility Visibility { get; set; }

    protected override void OnAttached()
    {
      if (TrialHelper.IsTrial)
      {
        this.AssociatedObject.Visibility = this.Visibility;
      }
    }

  }
}