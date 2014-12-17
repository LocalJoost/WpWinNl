using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WpWinNl.Utilities;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A behavior to turn a FlipView into a kind of panorama
  /// </summary>
  public class FlipViewPanoramaBehavior : Behavior<FlipView>
  {
    protected override void OnAttached()
    {
      AssociatedObject.Loaded += AssociatedObjectLoaded;
      base.OnAttached();
    }

    protected override void OnDetaching()
    {
      AssociatedObject.Loaded -= AssociatedObjectLoaded;
      AssociatedObject.SelectionChanged -= AssociatedObjectSelectionChanged;
      AssociatedObject.SizeChanged -= AssociatedObjectSizeChanged;
    }

    private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      //Init changed -> AddTranslates moved to SizePosFlipViewItems;
      SizePosFlipViewItems();
      AssociatedObject.SelectionChanged += AssociatedObjectSelectionChanged;
      AssociatedObject.SizeChanged += AssociatedObjectSizeChanged;
    }

    private async void AssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      await Task.Delay(250);
      SizePosFlipViewItems();
    }

    private async void AssociatedObjectSizeChanged(object sender, SizeChangedEventArgs e)
    {
      await Task.Delay(250);
      SizePosFlipViewItems();
    }

    /// <summary>
    /// Does the actual repositioning and sizing of the items displayed in the Flipview
    /// </summary>
    private void SizePosFlipViewItems()
    {
      AddTranslates();
      var size = AssociatedObject.ActualWidth*(NextPanelScreenPercentage/100);
      var shift = size - 15;

      var items = GetFlipViewItems();
      if (items != null && items.Count > 1)
      {

        // Make all items a bit smaller and make sure they are aligned left
        foreach (var item in items)
        {
          item.GetVisualChild(0).HorizontalAlignment = HorizontalAlignment.Left;
          item.GetVisualChild(0).Width = items[0].ActualWidth - size;
        }

        var selectedIndex = AssociatedObject.SelectedIndex;

        if (selectedIndex > 0)
        { 
          StartTranslateStoryBoard(0, 0, items[selectedIndex - 1].GetVisualChild(0), 0);
        }

        StartTranslateStoryBoard(0, 0, items[selectedIndex].GetVisualChild(0), AnimationTime);

        if (selectedIndex + 1 < items.Count)
        {
          StartTranslateStoryBoard(-shift, 0, items[selectedIndex + 1].GetVisualChild(0), AnimationTime);
        }
      }
    }

    /// <summary>
    /// At compositions transforms to every item within every flip view item
    /// </summary>
    private void AddTranslates()
    {
      var items = GetFlipViewItems();
      if (items != null && items.Count > 1)
      {
        foreach (var item in items)
        {
          var firstChild = item.GetVisualChild(0);
          if (!(firstChild.RenderTransform is CompositeTransform))
          {
            firstChild.RenderTransform = new CompositeTransform();
            firstChild.RenderTransformOrigin = new Point(0.5, 0.5);
          }
        }
      }
    }

    private static void StartTranslateStoryBoard(double desiredX, double desiredY, FrameworkElement fe, int time)
    {
      var translatePoint = fe.GetTranslatePoint();
      var destinationPoint = new Point(desiredX, desiredY);
      if (destinationPoint.DistanceFrom(translatePoint) > 1)
      {
        var storyboard = new Storyboard { FillBehavior = FillBehavior.HoldEnd };
        storyboard.AddTranslationAnimation(fe, translatePoint, destinationPoint,
                                           new Duration(TimeSpan.FromMilliseconds(time)),
                                           new CubicEase { EasingMode = EasingMode.EaseOut });
        storyboard.Begin();
      }
    }

    /// <summary>
    /// Find all Flip view items
    /// </summary>
    /// <returns></returns>
    private List<FlipViewItem> GetFlipViewItems()
    {
      var grid = AssociatedObject.GetVisualChildren().FirstOrDefault();
      if (grid != null)
      {
        return grid.GetVisualDescendents().OfType<FlipViewItem>().ToList();
      }
      return null;
    }

    #region AnimationTime

    /// <summary>
    /// AnimationTime Property name
    /// </summary>
    public const string AnimationTimePropertyName = "AnimationTime";

    public int AnimationTime
    {
      get { return (int)GetValue(AnimationTimeProperty); }
      set { SetValue(AnimationTimeProperty, value); }
    }

    /// <summary>
    /// AnimationTime Property definition
    /// </summary>
    public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register(
        AnimationTimePropertyName,
        typeof(int),
        typeof(FlipViewPanoramaBehavior),
        new PropertyMetadata(250));

    #endregion

    #region NextPanelScreenPercentage

    /// <summary>
    /// NextPanelScreenPercentage Property name
    /// </summary>
    public const string NextPanelScreenPercentagePropertyName = "NextPanelScreenPercentage";

    public double NextPanelScreenPercentage
    {
      get { return (double)GetValue(NextPanelScreenPercentageProperty); }
      set { SetValue(NextPanelScreenPercentageProperty, value); }
    }

    /// <summary>
    /// NextPanelScreenPercentage Property definition
    /// </summary>
    public static readonly DependencyProperty NextPanelScreenPercentageProperty = DependencyProperty.Register(
        NextPanelScreenPercentagePropertyName,
        typeof(double),
        typeof(FlipViewPanoramaBehavior),
        new PropertyMetadata(10.0));
    #endregion
  }
}