using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if WINDOWS_PHONE
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;
using Microsoft.Phone.Reactive;
#else
using System.Reactive.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
#endif


namespace WpWinNl.Behaviors
{
  public class StartStoryboardBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      AssociatedObject.Loaded += AssociatedObjectLoaded;
    }

    private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      if (StartImmediately)
      {
        StartStoryboard();
      }

      if (!string.IsNullOrWhiteSpace(EventName))
      {
        var evt = AssociatedObject.GetType().GetRuntimeEvent(EventName);
        if (evt != null)
        {
#if WINDOWS_PHONE
         evt.AddEventHandler(AssociatedObject, 
           new EventHandler((p, q) => StartStoryboard()));
#else
          Observable.FromEventPattern<RoutedEventArgs>(AssociatedObject, EventName)
            .Subscribe(se => StartStoryboard());
#endif

        }
      }
    }

    protected override void OnDetaching()
    {
      AssociatedObject.Loaded -= AssociatedObjectLoaded;
      base.OnDetaching();
    }

    private void StartStoryboard()
    {
      if( SearchTopDown)
      {
        StartStoryboardTopDown();
      }
      else
      {
        StartStoryboardBottomUp();
      }
    }

    private void StartStoryboardTopDown()
    {
      var root = AssociatedObject.GetVisualAncestors().Last() ?? AssociatedObject;

      var storyboard = GetStoryBoardInVisualDescendents(root);
      if (storyboard != null)
      {
        storyboard.Begin();
      }
    }

    private void StartStoryboardBottomUp()
    {
      var root = AssociatedObject;
      Storyboard storyboard;
      do
      {
        storyboard = GetStoryBoardInVisualDescendents(root);
        if (storyboard == null)
        {
          root = root.GetVisualParent();
        }
      } while (root != null && storyboard == null);

      if (storyboard != null)
      {
        storyboard.Begin();
      }
    }

    private Storyboard GetStoryBoardInVisualDescendents(FrameworkElement f)
    {
      return f.GetVisualDescendents()
        .Where(p => p.Resources.ContainsKey(Storyboard) && p.Resources[Storyboard] is Storyboard)
        .Select(p => p.Resources[Storyboard] as Storyboard).FirstOrDefault();
    }

    #region EventName

    /// <summary>
    /// EventName Property name
    /// </summary>
    public const string EventNamePropertyName = "EventName";

    public string EventName
    {
      get { return (string)GetValue(EventNameProperty); }
      set { SetValue(EventNameProperty, value); }
    }

    /// <summary>
    /// EventName Property definition
    /// </summary>
    public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(
        EventNamePropertyName,
        typeof(string),
        typeof(StartStoryboardBehavior),
        null);
    #endregion

    #region Storyboard

    /// <summary>
    /// Storyboard Property name
    /// </summary>
    public const string StoryboardPropertyName = "Storyboard";

    public string Storyboard
    {
      get { return (string)GetValue(StoryboardProperty); }
      set { SetValue(StoryboardProperty, value); }
    }

    /// <summary>
    /// Storyboard Property definition
    /// </summary>
    public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register(
        StoryboardPropertyName,
        typeof(string),
        typeof(StartStoryboardBehavior),
        null);

    #endregion

    #region StartImmediately

    /// <summary>
    /// StartImmediately Property name
    /// </summary>
    public const string StartImmediatelyPropertyName = "StartImmediately";

    public bool StartImmediately
    {
      get { return (bool)GetValue(StartImmediatelyProperty); }
      set { SetValue(StartImmediatelyProperty, value); }
    }

    /// <summary>
    /// StartImmediately Property definition
    /// </summary>
    public static readonly DependencyProperty StartImmediatelyProperty = DependencyProperty.Register(
        StartImmediatelyPropertyName,
        typeof(bool),
        typeof(StartStoryboardBehavior),
        null);

    #endregion

    #region SearchTopDown

    /// <summary>
    /// SearchTopDown Property name
    /// </summary>
    public const string SearchTopDownPropertyName = "SearchTopDown";

    public bool SearchTopDown
    {
      get { return (bool)GetValue(SearchTopDownProperty); }
      set { SetValue(SearchTopDownProperty, value); }
    }

    /// <summary>
    /// SearchTopDown Property definition
    /// </summary>
    public static readonly DependencyProperty SearchTopDownProperty = DependencyProperty.Register(
        SearchTopDownPropertyName,
        typeof(bool),
        typeof(StartStoryboardBehavior),
        new PropertyMetadata(true));

    #endregion
  }
}
