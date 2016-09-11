using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using WpWinNl.Utilities;

namespace WpWinNl.BaseModels
{
  /// <summary>
  /// Extended base class for a viewmodel.
  /// Requires navigationservice to be set up
  /// </summary>
  public abstract class AppViewModelBase : ViewModelBase
  {
    protected AppViewModelBase()
    {
    }

    protected void SendNavigationRequestMessage(Type type)
    {
      SimpleIoc.Default.GetInstance<INavigationService>().Navigate(type);
    }

    protected void SendNavigationRequestMessage(Type type, object parameter)
    {
      SimpleIoc.Default.GetInstance<INavigationService>().Navigate(type, parameter);
    }

    protected void SendNavigationRequestMessage(string type)
    {
        SimpleIoc.Default.GetInstance<INavigationService>().Navigate(type);
    }

    protected void SendNavigationRequestMessage(string type, object parameter)
    {
        SimpleIoc.Default.GetInstance<INavigationService>().Navigate(type, parameter);
    }

    protected void GoBack()
    {
      SimpleIoc.Default.GetInstance<INavigationService>().GoBack();
    }

    public virtual void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
    {
    }
  }
}
