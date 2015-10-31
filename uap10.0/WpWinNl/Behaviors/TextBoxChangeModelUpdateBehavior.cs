using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A behavior to for text box model update when text changes
  /// </summary>
  public class TextBoxChangeModelUpdateBehavior : Behavior<TextBox>
  {
    protected override void OnAttached()
    {
      AssociatedObject.TextChanged += AssociatedObjectTextChanged;
    }

    protected override void OnDetaching()
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
