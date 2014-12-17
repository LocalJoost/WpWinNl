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
  public abstract class AppViewModelBase : ViewModelBase, IBackKeyPressHandler
  {
    protected AppViewModelBase()
    {
    }

    protected void SendNavigationRequestMessage(Uri uri)
    {
      SimpleIoc.Default.GetInstance<INavigationService>().NavigateTo(uri);
    }

    protected void SendNavigationRequestMessage(string uri)
    {
      SendNavigationRequestMessage(new Uri(uri, UriKind.Relative));
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
