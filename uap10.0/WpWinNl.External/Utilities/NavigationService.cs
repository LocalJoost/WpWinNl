using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WpWinNl.Utilities
{
  public class NavigationService : INavigationService
  {
    public NavigationService(Frame mainFrame)
    {
      _mainFrame = mainFrame;
    }

    private Frame _mainFrame;

    public event NavigatingCancelEventHandler Navigating;

    public void Navigate(Type type)
    {
      _mainFrame.Navigate(type);
    }

    public void Navigate(Type type, object parameter)
    {
      _mainFrame.Navigate(type, parameter);
    }

    public void Navigate(string type, object parameter)
    {
      _mainFrame.Navigate(Type.GetType(type), parameter);
    }

    public void Navigate(string type)
    {
      _mainFrame.Navigate(Type.GetType(type));
    }

    public void GoBack()
    {
      if (_mainFrame.CanGoBack)
      {
        _mainFrame.GoBack();
      }
    }
  }
}
