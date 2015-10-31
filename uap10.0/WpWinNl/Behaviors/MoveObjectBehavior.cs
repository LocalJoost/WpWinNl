using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.Foundation;
using WpWinNl.Utilities;

namespace WpWinNl.Behaviors
{
  public class MoveObjectBehavior : BaseAnimationBehavior
  {
    protected override void OnAttached()
    {
      originalTranslation = GetInitialTranslation();
      base.OnAttached();
    }

    protected override CompositeTransform BuildTransform()
    {
      return new CompositeTransform
      {
        TranslateX = Activated ? ActivatedXValue : originalTranslation.X,
        TranslateY = Activated ? ActivatedYValue : originalTranslation.Y
      };
    }

    protected override Storyboard BuildStoryBoard()
    {
      var storyboard = new Storyboard { FillBehavior = FillBehavior.HoldEnd };
      var transform = BuildTransform();
      var duration = new Duration(TimeSpan.FromMilliseconds(Duration));
      storyboard.AddTranslationAnimation(
        AssociatedObject,
        AssociatedObject.GetTranslatePoint(), new Point(transform.TranslateX, transform.TranslateY),
        duration);
      return storyboard;
    }

    private Point originalTranslation;

    private Point GetInitialTranslation()
    {
      return AssociatedObject.GetCompositeTransform() != null ? AssociatedObject.GetTranslatePoint() : new Point(0, 0);
    }

    #region ActivatedXValue

    /// <summary>
    /// ActivatedXValue Property name
    /// </summary>
    public const string ActivatedXValuePropertyName = "ActivatedXValue";

    public double ActivatedXValue
    {
      get { return (double)GetValue(ActivatedXValueProperty); }
      set { SetValue(ActivatedXValueProperty, value); }
    }

    /// <summary>
    /// ActivatedXValue Property definition
    /// </summary>
    public static readonly DependencyProperty ActivatedXValueProperty = DependencyProperty.Register(
        ActivatedXValuePropertyName,
        typeof(double),
        typeof(MoveObjectBehavior),
        new PropertyMetadata(default(double), ActivatedValueChanged));

    public static void ActivatedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var behavior = d as MoveObjectBehavior;
      if (behavior != null)
      {
        if (behavior.Activated)
        {
          behavior.Activate();
        }
      }
    }


    #endregion

    #region ActivatedYValue

    /// <summary>
    /// ActivatedYValue Property name
    /// </summary>
    public const string ActivatedYValuePropertyName = "ActivatedYValue";

    public double ActivatedYValue
    {
      get { return (double)GetValue(ActivatedYValueProperty); }
      set { SetValue(ActivatedYValueProperty, value); }
    }

    /// <summary>
    /// ActivatedYValue Property definition
    /// </summary>
    public static readonly DependencyProperty ActivatedYValueProperty = DependencyProperty.Register(
        ActivatedYValuePropertyName,
        typeof(double),
        typeof(MoveObjectBehavior),
        new PropertyMetadata(default(double), ActivatedValueChanged));

    #endregion

  }
}
