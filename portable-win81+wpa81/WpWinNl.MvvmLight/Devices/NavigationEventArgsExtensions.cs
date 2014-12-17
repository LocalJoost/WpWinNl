#if WINDOWS_PHONE
using System.Windows.Navigation;
#else
using Windows.UI.Xaml.Navigation;
#endif

namespace WpWinNl.Devices
{
  public static class NavigationEventArgsExtensions
  {
    public static bool IsStartedByNfcRequest(this NavigationEventArgs e)
    {
      var isStartedByNfcRequest = false;
      if (e.Uri != null)
      {
        isStartedByNfcRequest = e.Uri.ToString()
                                 .Contains("ms_nfp_launchargs=Windows.Networking.Proximity.PeerFinder:StreamSocket");
      }
      return isStartedByNfcRequest;
    }
  }
}
