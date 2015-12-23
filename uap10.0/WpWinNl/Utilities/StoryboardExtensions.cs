using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// Extension methods to add animations to a storyboard
  /// </summary>
  public static class StoryboardExtensions
  {
    public static Timeline CreateDoubleAnimation(this Storyboard storyboard,
        Duration duration, double from, double to)
    {
      return storyboard.CreateDoubleAnimation(duration, from, to,
          new SineEase
          {
            EasingMode = EasingMode.EaseInOut
          });
    }

    public static Timeline CreateDoubleAnimation(this Storyboard storyboard,
        Duration duration, double from, double to, EasingFunctionBase easingFunction)
    {
      var animation = new DoubleAnimation
      {
        From = from,
        To = to,
        Duration = duration,
        EasingFunction = easingFunction
      };
      return animation;
    }

    public static void AddAnimation(this Storyboard storyboard,
        DependencyObject item, Timeline t, DependencyProperty p)
    {
      if (p == null) throw new ArgumentNullException("p");
      Storyboard.SetTarget(t, item);
      Storyboard.SetTargetProperty(t, item.GetDependencyPropertyName(p));
      storyboard.Children.Add(t);
    }

    public static void AddAnimation(this Storyboard storyboard,
                                     DependencyObject item, Timeline t, string p)
    {
      if(string.IsNullOrWhiteSpace(p)) throw new ArgumentNullException("p");
      Storyboard.SetTarget(t, item);
      Storyboard.SetTargetProperty(t, p);
      storyboard.Children.Add(t);
    }

    public static void AddTranslationAnimation(this Storyboard storyboard,
        FrameworkElement fe, Point from, Point to, Duration duration)
    {
      storyboard.AddTranslationAnimation(fe, from, to, duration, null);
    }

    public static void AddTranslationAnimation(this Storyboard storyboard,
        FrameworkElement fe, Point from, Point to, Duration duration,
        EasingFunctionBase easingFunction)
    {
      storyboard.AddAnimation(fe.RenderTransform,
                   storyboard.CreateDoubleAnimation(duration, from.X, to.X, easingFunction),
                      CompositeTransform.TranslateXProperty);
      storyboard.AddAnimation(fe.RenderTransform,
                  storyboard.CreateDoubleAnimation(duration, from.Y, to.Y, easingFunction),
                     CompositeTransform.TranslateYProperty);
    }

    public static Timeline CreateKeyFrameAnimation(this Storyboard storyboard, IList<double> values, 
      IList<Duration> times)
    {
      var keyFrameAnimation = new DoubleAnimationUsingKeyFrames();
      var keyTime = TimeSpan.FromMilliseconds(0);
      for (var i = 0; i < values.Count(); i++)
      {
        keyTime += times[i].TimeSpan;
        var frame = new LinearDoubleKeyFrame { Value = values[i], KeyTime = keyTime };
        keyFrameAnimation.KeyFrames.Add(frame);
        keyFrameAnimation.Duration += times[i];
      }
      return keyFrameAnimation;
    }

    public static void AddWayPointAnimation(this Storyboard storyboard, FrameworkElement fe, IList<LineF> legs, double speed)
    {
      var points = legs.Select(p => p.From).ToList();
      points.Add(legs.Select(p => p.To).Last());
      AddWayPointAnimation(storyboard, fe, points, speed);
    }

    public static void AddWayPointAnimation(this Storyboard storyboard, FrameworkElement fe, 
      IList<Point> points, double speed)
    {
      var durations = new List<Duration> { new Duration(TimeSpan.FromSeconds(0)) };
      for (var i = 0; i < points.Count - 1; i++)
      {
        durations.Add(points[i].CalculateDuration(points[i + 1], speed));
      }
      var xValues = points.Select(p => p.X).ToList();
      storyboard.AddAnimation(fe.RenderTransform,
        storyboard.CreateKeyFrameAnimation(xValues, durations), CompositeTransform.TranslateXProperty);
      var yValues = points.Select(p => p.Y).ToList();
      storyboard.AddAnimation(fe.RenderTransform,
        storyboard.CreateKeyFrameAnimation(yValues, durations), CompositeTransform.TranslateYProperty);
    }

    public static void AddScalingAnimation(this Storyboard storyboard,
    FrameworkElement fe, double fromX, double toX, double fromY, double toY, Duration duration)
    {
      storyboard.AddScalingAnimation(fe, fromX, toX, fromY, toY, duration, null);
    }

    public static void AddScalingAnimation(this Storyboard storyboard,
        FrameworkElement fe, double fromX, double toX, double fromY, double toY, Duration duration,
        EasingFunctionBase easingFunction)
    {
      storyboard.AddAnimation(fe.RenderTransform,
                   storyboard.CreateDoubleAnimation(duration, fromX, toX, easingFunction),
                      CompositeTransform.ScaleXProperty);
      storyboard.AddAnimation(fe.RenderTransform,
                  storyboard.CreateDoubleAnimation(duration, fromY, toY, easingFunction),
                     CompositeTransform.ScaleYProperty);
    }

    public static void AddOpacityAnimation(this Storyboard storyboard,
          UIElement fe, double from, double to, Duration duration)
    {
      AddOpacityAnimation(storyboard, fe, from, to, duration, null);
    }

    public static void AddOpacityAnimation(this Storyboard storyboard,
              UIElement fe, double from, double to, Duration duration,
              EasingFunctionBase easingFunction)
    {
      storyboard.AddAnimation(fe,
             storyboard.CreateDoubleAnimation(duration, from, to, easingFunction),
             UIElement.OpacityProperty);
    }
  }
}
