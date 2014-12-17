#if WINDOWS_PHONE
using System.Windows.Controls;
#else
using Windows.UI.Xaml.Controls;
#endif

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A behavior to for text box model update when text changes
  /// </summary>
  public class TextBoxChangeModelUpdateBehavior : SafeBehavior<TextBox>
  {
    protected override void OnSetup()
    {
      AssociatedObject.TextChanged += AssociatedObjectTextChanged;
    }

    protected override void OnCleanup()
    {
      AssociatedObject.TextChanged -= AssociatedObjectTextChanged;
    }

    void AssociatedObjectTextChanged(object sender, TextChangedEventArgs e)
    {
      var binding = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
      if (binding != null)
      {
        binding.UpdateSource();
      }
    }
  }
}
