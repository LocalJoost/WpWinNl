using Windows.UI.Xaml;

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
