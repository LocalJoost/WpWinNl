using Windows.UI.Xaml;


namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A base class for behaviors doing 'something' with scaling
  /// </summary>
  public abstract class BaseScaleBehavior : BaseAnimationBehavior
  {

    #region Direction

    /// <summary>
    /// Direction Property name
    /// </summary>
    public const string DirectionPropertyName = "Direction";

    public Direction Direction
    {
      get { return (Direction)GetValue(DirectionProperty); }
      set { SetValue(DirectionProperty, value); }
    }

    /// <summary>
    /// Direction Property definition
    /// </summary>
    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
        DirectionPropertyName,
        typeof(Direction),
        typeof(BaseScaleBehavior),
        new PropertyMetadata(default(Direction)));

    #endregion
 
  }
}
