using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Xaml.Interactivity;


namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A base class for behaviors doing 'something' with animation
  /// </summary>
  public abstract class BaseAnimationBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      SetRenderTransForm();
      AssociatedObject.RenderTransform = BuildTransform();
    }

    protected void Activate()
    {
      if (AssociatedObject != null && AssociatedObject.RenderTransform is CompositeTransform)
      {
        var storyboard = BuildStoryBoard();
        if (storyboard != null)
        {
          storyboard.Begin();
        }
      }
    }

    protected abstract CompositeTransform BuildTransform();

    protected abstract Storyboard BuildStoryBoard();

 
    #region Duration

    /// <summary>
    /// Duration Property name
    /// </summary>
    public const string DurationPropertyName = "Duration";

    public double Duration
    {
      get { return (double)GetValue(DurationProperty); }
      set { SetValue(DurationProperty, value); }
    }

    /// <summary>
    /// Duration Property definition
    /// </summary>
    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
        DurationPropertyName,
        typeof(double),
        typeof(BaseAnimationBehavior),
        new PropertyMetadata(500.0));

    #endregion

    #region Activated

    /// <summary>
    /// Activated Property name
    /// </summary>
    public const string ActivatedPropertyName = "Activated";

    public bool Activated
    {
      get { return (bool)GetValue(ActivatedProperty); }
      set { SetValue(ActivatedProperty, value); }
    }

    /// <summary>
    /// Activated Property definition
    /// </summary>
    public static readonly DependencyProperty ActivatedProperty = DependencyProperty.Register(
        ActivatedPropertyName,
        typeof(bool),
        typeof(BaseAnimationBehavior),
        new PropertyMetadata(default(bool), ActivatedChanged));

    /// <summary>
    /// Activated property changed callback.
    /// </summary>
    /// <param name="d">The depency object (i.e. the behavior).</param>
    /// <param name="e">The property event args <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>  
    public static void ActivatedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var thisobj = d as BaseAnimationBehavior;
      if (thisobj != null && e.OldValue != e.NewValue)
      {
        thisobj.Activate();
      }
    }

    #endregion

    protected void SetRenderTransForm()
    {
      if (AssociatedObject != null)
      {
        AssociatedObject.RenderTransformOrigin = new Point(RenderTransformX, RenderTransformY);
      }
    }

    #region RenderTransformX

    /// <summary>
    /// RenderTransformX Property name
    /// </summary>
    public const string RenderTransformXPropertyName = "RenderTransformX";

    public double RenderTransformX
    {
      get { return (double)GetValue(RenderTransformXProperty); }
      set { SetValue(RenderTransformXProperty, value); }
    }

    /// <summary>
    /// RenderTransformX Property definition
    /// </summary>
    public static readonly DependencyProperty RenderTransformXProperty = DependencyProperty.Register(
        RenderTransformXPropertyName,
        typeof(double),
        typeof(BaseScaleBehavior),
        new PropertyMetadata(0.5, RenderTransformChanged));
    #endregion

    #region RenderTransformY

    /// <summary>
    /// RenderTransformY Property name
    /// </summary>
    public const string RenderTransformYPropertyName = "RenderTransformY";

    public double RenderTransformY
    {
      get { return (double)GetValue(RenderTransformYProperty); }
      set { SetValue(RenderTransformYProperty, value); }
    }

    /// <summary>
    /// RenderTransformY Property definition
    /// </summary>
    public static readonly DependencyProperty RenderTransformYProperty = DependencyProperty.Register(
        RenderTransformYPropertyName,
        typeof(double),
        typeof(BaseScaleBehavior),
        new PropertyMetadata(0.5, RenderTransformChanged));

    #endregion

    /// <summary>
    /// RenderTransform X/Y property changed callback.
    /// </summary>
    /// <param name="d">The depency object (i.e. the behavior).</param>
    /// <param name="e">The property event args <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>  
    public static void RenderTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var thisobj = d as BaseScaleBehavior;
      if (thisobj != null)
      {
        thisobj.SetRenderTransForm();
      }
    }
  }
}
