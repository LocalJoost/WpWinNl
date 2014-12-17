using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using WpWinNl.BaseModels;

namespace WpWinNl.Controls
{
  /// <summary>
  /// Base class for a view that handles smooth rotation
  /// and tries to send backkeypress events to the viewmodel
  /// bound to it
  /// </summary>
  public class ExtendedPhonePage : PhoneApplicationPage
  {
    public ExtendedPhonePage()
    {
      Loaded += ExtendedPhonePageLoaded;
    }

    void ExtendedPhonePageLoaded(object sender, RoutedEventArgs e)
    {
      _lastOrientation = Orientation;
    }

    PageOrientation _lastOrientation;
    protected override void OnOrientationChanged(OrientationChangedEventArgs e)
    {
      var newOrientation = e.Orientation;
      var transitionElement = new RotateTransition();

      switch (newOrientation)
      {
        case PageOrientation.Landscape:
        case PageOrientation.LandscapeRight:
          // Come here from PortraitUp (i.e. clockwise) or LandscapeLeft?
          if (_lastOrientation == PageOrientation.PortraitUp)
            transitionElement.Mode = RotateTransitionMode.In90Counterclockwise;
          else
            transitionElement.Mode = RotateTransitionMode.In180Clockwise;
          break;
        case PageOrientation.LandscapeLeft:
          // Come here from LandscapeRight or PortraitUp?
          if (_lastOrientation == PageOrientation.LandscapeRight)
            transitionElement.Mode = RotateTransitionMode.In180Counterclockwise;
          else
            transitionElement.Mode = RotateTransitionMode.In90Clockwise;
          break;
        case PageOrientation.Portrait:
        case PageOrientation.PortraitUp:
          // Come here from LandscapeLeft or LandscapeRight?
          if (_lastOrientation == PageOrientation.LandscapeLeft)
            transitionElement.Mode = RotateTransitionMode.In90Counterclockwise;
          else
            transitionElement.Mode = RotateTransitionMode.In90Clockwise;
          break;
        default:
          break;
      }

      // Execute the transition
      if (Application.Current.RootVisual != null)
      {
        var phoneApplicationPage =
          (((PhoneApplicationFrame)Application.Current.RootVisual)).Content
          as PhoneApplicationPage;
        var transition = transitionElement.GetTransition(phoneApplicationPage);

        transition.Completed += (sender, args) => transition.Stop();
        transition.Begin();

        _lastOrientation = newOrientation;
      }
      base.OnOrientationChanged(e);

    }


    private const string FocusedElement = "FOCUSED_ELEMENT";

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
      if (State.ContainsKey(FocusedElement))
      {
        State.Remove(FocusedElement);
      }

      var focusedElement = FocusManager.GetFocusedElement() as Control;
      if (focusedElement == null) return;
      if (!String.IsNullOrEmpty(focusedElement.Name))
      {
        State.Add(FocusedElement, focusedElement.Name);
      }

      BindingExpression be = null;

      //TODO - Developers, add additional controls here like a date picker, combobox, etc.
      if (focusedElement is TextBox)
      {
        be = focusedElement.GetBindingExpression(TextBox.TextProperty);
      }
      if (be != null)
      {
        be.UpdateSource();
      }
      base.OnNavigatingFrom(e);
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      var dc = DataContext as IBackKeyPressHandler;
      if (dc != null) dc.OnBackKeyPress(e);
    }
  }
}
