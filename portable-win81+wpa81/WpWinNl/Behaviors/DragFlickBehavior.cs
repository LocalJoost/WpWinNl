using System;

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
#else
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
#endif

using WpWinNl.Utilities;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// This behavior allows objects to be moved and flicked, adding a little
  /// inertia to their movement
  /// </summary>
  public class DragFlickBehavior : Behavior<FrameworkElement>
  {
    private FrameworkElement elementToAnimate;

    protected override void OnAttached()
    {
      base.OnAttached();
      AssociatedObject.Loaded += AssociatedObjectLoaded;
      AssociatedObject.ManipulationDelta += AssociatedObjectManipulationDelta;
      AssociatedObject.ManipulationCompleted += AssociatedObjectManipulationCompleted;
    }

    void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      elementToAnimate = AssociatedObject.GetElementToAnimate();
      if (!(elementToAnimate.RenderTransform is CompositeTransform))
      {
        elementToAnimate.RenderTransform = new CompositeTransform();
        elementToAnimate.RenderTransformOrigin = new Point(0.5, 0.5);
      }
#if !WINDOWS_PHONE
      AssociatedObject.ManipulationMode = ManipulationModes.All;
#endif
    }

#if WINDOWS_PHONE
    void AssociatedObjectManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      var dx = e.DeltaManipulation.Translation.X;
      var dy = e.DeltaManipulation.Translation.Y;
      var currentPosition = elementToAnimate.GetTranslatePoint();
      elementToAnimate.SetTranslatePoint(currentPosition.X + dx, currentPosition.Y + dy);
    }

#else

    void AssociatedObjectManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
      var dx = e.Delta.Translation.X;
      var dy = e.Delta.Translation.Y;
      var currentPosition = elementToAnimate.GetTranslatePoint();
      elementToAnimate.SetTranslatePoint(currentPosition.X + dx, currentPosition.Y + dy);
    }
#endif

#if WINDOWS_PHONE

    private void AssociatedObjectManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      // Create a storyboard that will emulate a 'flick'
      var currentPosition = elementToAnimate.GetTranslatePoint();
      var velocity = e.FinalVelocities.LinearVelocity;
      var storyboard = new Storyboard { FillBehavior = FillBehavior.HoldEnd };

      var to = new Point(currentPosition.X + (velocity.X / BrakeSpeed),
          currentPosition.Y + (velocity.Y / BrakeSpeed));
      storyboard.AddTranslationAnimation(elementToAnimate, currentPosition, to,
          new Duration(TimeSpan.FromMilliseconds(500)),
          new CubicEase { EasingMode = EasingMode.EaseOut });
      storyboard.Begin();
    }
#else
    private void AssociatedObjectManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
    {
      // Create a storyboard that will emulate a 'flick'
      var currentPosition = elementToAnimate.GetTranslatePoint();
      var xVelocity = e.Velocities.Linear.X * 1000;
      var yVelocity = e.Velocities.Linear.Y * 1000;
      var storyboard = new Storyboard { FillBehavior = FillBehavior.HoldEnd };
      var to = new Point(currentPosition.X + (xVelocity / BrakeSpeed / 5),
          currentPosition.Y + (yVelocity / BrakeSpeed / 5));
      storyboard.AddTranslationAnimation(elementToAnimate, currentPosition, to,
          new Duration(TimeSpan.FromMilliseconds(500)),
          new CubicEase { EasingMode = EasingMode.EaseOut });
      storyboard.Begin();
    }
#endif

    protected override void OnDetaching()
    {
      AssociatedObject.Loaded -= AssociatedObjectLoaded;
      AssociatedObject.ManipulationCompleted -= AssociatedObjectManipulationCompleted;
      AssociatedObject.ManipulationDelta -= AssociatedObjectManipulationDelta;

      base.OnDetaching();
    }

    #region BrakeSpeed
    public const string BrakeSpeedPropertyName = "BrakeSpeed";

    /// <summary>
    /// Describes how fast the element should brake, i.e. come to rest,
    /// after a flick. Higher = apply more brake ;-)
    /// </summary>
    public int BrakeSpeed
    {
      get { return (int)GetValue(BrakeSpeedProperty); }
      set { SetValue(BrakeSpeedProperty, value); }
    }

    public static readonly DependencyProperty BrakeSpeedProperty = DependencyProperty.Register(
        BrakeSpeedPropertyName,
        typeof(int),
        typeof(DragFlickBehavior),
        new PropertyMetadata(10));

    #endregion
  }

}
