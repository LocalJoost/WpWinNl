using System;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Xaml.Interactivity;


namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A base class implementing the safe event detachment pattern for behaviors.
  /// Optional re-init after page back navigation.
  /// </summary>
  /// <typeparam name="T">The framework element type this behavior attaches to</typeparam>
  public abstract class SafeBehavior<T> : Behavior<T> where T : FrameworkElement
  {
    protected SafeBehavior()
    {
      IsCleanedUp = true;
    }

    #region Setup

    protected override void OnAttached()
    {
      base.OnAttached();
      InitBehavior();
    }

    /// <summary>
    /// Does the initial wiring of events
    /// </summary>
    protected void InitBehavior()
    {
      if (IsCleanedUp)
      {
        IsCleanedUp = false;
        this.AssociatedObject.Loaded += AssociatedObjectLoaded;
        this.AssociatedObject.Unloaded += AssociatedObjectUnloaded;
      }
    }

    /// <summary>
    /// Does further event wiring and initialization after load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      OnSetup();
    }

    protected virtual void OnParentPageNavigated(object sender, NavigationEventArgs e)
    {
      
    }

    /// <summary>
    /// Override this to add your own setup
    /// </summary>
    protected virtual void OnSetup()
    {

    }

    #endregion

    #region Cleanup

    protected bool IsCleanedUp { get; private set; }

  /// <summary>
    /// Executes at OnDetaching or OnUnloaded (usually the last)
    /// </summary>
    private void Cleanup()
    {
      if (!IsCleanedUp)
      {
        this.AssociatedObject.Loaded -= AssociatedObjectLoaded;
        this.AssociatedObject.Unloaded -= AssociatedObjectUnloaded;
        OnCleanup();
        IsCleanedUp = true;
      }
    }

    protected override void OnDetaching()
    {
      Cleanup();
      base.OnDetaching();
    }

    private void AssociatedObjectUnloaded(object sender, RoutedEventArgs e)
    {
      Cleanup();
    }

    /// <summary>
    /// Override this to add your own cleanup
    /// </summary>
    protected virtual void OnCleanup()
    {
    }
    #endregion
  }
}
