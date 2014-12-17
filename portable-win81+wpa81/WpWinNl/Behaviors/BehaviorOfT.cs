using Windows.UI.Xaml;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A base class for behaviors, to appear them to be identical to older frameworks
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class Behavior<T> : Behavior where T: DependencyObject
  {
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public new T AssociatedObject { get; set; }

    public override void Attach(DependencyObject associatedObject)
    {
      base.Attach(associatedObject);
      this.AssociatedObject = (T)associatedObject;
      OnAttached();
    }

    public override  void Detach()
    {
      base.Detach();
      OnDetaching();
    }


    protected virtual void OnAttached()
    {
    }

    protected virtual void OnDetaching()
    {
    }

  }
}
