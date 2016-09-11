using Windows.UI.Xaml.Navigation;

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
