using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A base class for behaviors, can be used in native environments too
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class Behavior : DependencyObject, IBehavior
  {
    public DependencyObject AssociatedObject { get; set; }

    public virtual void Attach(DependencyObject associatedObject)
    {
      AssociatedObject = associatedObject;
    }

    public virtual void Detach()
    {
    }
  }
}
