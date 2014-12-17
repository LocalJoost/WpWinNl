using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

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

    /// <summary>
    ///Setting this value to true in the constructor makes the behavior
    ///re-init after a page back event.
    /// </summary>
    protected bool ListenToPageBackEvent { get; set; }

    #region Setup

    /// <summary>
    /// The page this behavior is on
    /// </summary>
    protected PhoneApplicationFrame ParentPage;

    /// <summary>
    /// The uri of the page this behavior is on
    /// </summary>
    private Uri pageSource;

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
        AssociatedObject.Loaded += AssociatedObjectLoaded;
        AssociatedObject.Unloaded += AssociatedObjectUnloaded;
      }
    }

    /// <summary>
    /// Does further event wiring and initialization after load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      // Find the page this control is on and listen to its orientation changed events
      if (ParentPage == null && ListenToPageBackEvent)
      {
        ParentPage = Application.Current.RootVisual as PhoneApplicationFrame;
        pageSource = ParentPage.CurrentSource;
        ParentPage.Navigated += ParentPageNavigated;
      }
      OnSetup();
    }

    /// <summary>
    /// Fired whe page navigation happens
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ParentPageNavigated(object sender, NavigationEventArgs e)
    {
      // Re-setup when this page is navigated BACK to
      if (IsNavigatingBackToBehaviorPage(e))
      {
        if (IsCleanedUp)
        {
          InitBehavior();
        }
      }
      OnParentPageNavigated(sender, e);
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

    /// <summary>
    /// Checks if the back navigation navigates back to the page
    /// on which this behavior is on
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected bool IsNavigatingBackToBehaviorPage(NavigationEventArgs e)
    {
      return (e.NavigationMode == NavigationMode.Back && e.Uri.Equals(pageSource));
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
        AssociatedObject.Loaded -= AssociatedObjectLoaded;
        AssociatedObject.Unloaded -= AssociatedObjectUnloaded;
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
