using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// Behavior that switches to a visual state based upon the size of the associated object
  /// Inspired by a blog post by Iris Classon 
  /// http://irisclasson.com/2013/12/16/visualstates-and-the-missing-snapped-mode-made-easy-in-windows-store-apps/
  /// </summary>
  public class SizeVisualStateChangeBehavior : Behavior<Control>
  {
    protected override void OnAttached()
    {
      AssociatedObject.SizeChanged += AssociatedObjectSizeChanged;
      base.OnAttached();
      UpdateVisualState();

    }

    protected override void OnDetaching()
    {
      AssociatedObject.SizeChanged -= AssociatedObjectSizeChanged;
      base.OnDetaching();
    }

    void AssociatedObjectSizeChanged(object sender, SizeChangedEventArgs e)
    {
      UpdateVisualState();
    }

    private void UpdateVisualState()
    {
      if (SizeMappings != null)
      {
        SizeVisualStateMapping wantedMapping = null;
        var wantedMappings = SizeMappings.Where(p => p.Width >= AssociatedObject.ActualWidth).ToList();
        if (wantedMappings.Any())
        {
          wantedMapping = wantedMappings.OrderBy(p => p.Width).First();
        }
        else
        {
          var orderedMappings = SizeMappings.OrderBy(p => p.Width);
          if (AssociatedObject.ActualWidth < orderedMappings.First().Width)
          {
            wantedMapping = orderedMappings.First();
          }
          else if (AssociatedObject.ActualWidth > orderedMappings.Last().Width)
          {
            wantedMapping = orderedMappings.Last();
          }
        }

        if (wantedMapping != null)
        {
          Debug.WriteLine(AssociatedObject.ActualWidth + " -> " + wantedMapping.VisualState);

          VisualStateManager.GoToState(AssociatedObject, wantedMapping.VisualState, false);
        }
        else
        {
          Debug.WriteLine(AssociatedObject.ActualWidth + " -> " + "NONE!");

        }
      }
    }

    #region SizeMappings

    /// <summary>
    /// SizeMappings Property name
    /// </summary>
    public const string SizeMappingsPropertyName = "SizeMappings";

    public List<SizeVisualStateMapping> SizeMappings
    {
      get { return (List<SizeVisualStateMapping>)GetValue(SizeMappingsProperty); }
      set { SetValue(SizeMappingsProperty, value); }
    }

    /// <summary>
    /// SizeMappings Property definition
    /// </summary>
    public static readonly DependencyProperty SizeMappingsProperty = DependencyProperty.Register(
        SizeMappingsPropertyName,
        typeof(List<SizeVisualStateMapping>),
        typeof(SizeVisualStateChangeBehavior),
        new PropertyMetadata(new List<SizeVisualStateMapping>()));

    #endregion
  }
}
