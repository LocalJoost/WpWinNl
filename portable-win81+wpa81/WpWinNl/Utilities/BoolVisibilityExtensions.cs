#if WINDOWS_PHONE
using System.Windows;
#else
using Windows.UI.Xaml;
#endif

namespace WpWinNl.Utilities
{
  public static class BoolVisibilityExtensions
  {
    public static Visibility ToVisibility(this bool boolValue, bool invert = false)
    {
      if (invert)
      {
        boolValue = !boolValue;
      }
      return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }
  }
}
