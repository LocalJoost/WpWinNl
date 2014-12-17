#if WINDOWS_PHONE
using System.Windows.Navigation;
#else
using Windows.UI.Xaml.Navigation;
#endif

namespace WpWinNl.Devices
{
  public class NavigationMessage
  {
    public NavigationEventArgs NavigationEvent { get; set; }

    public bool IsStartedByNfcRequest
    {
      get { return NavigationEvent != null && NavigationEvent.IsStartedByNfcRequest(); }
    }
  }
}
