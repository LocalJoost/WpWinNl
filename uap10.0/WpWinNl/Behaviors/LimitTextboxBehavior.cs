using System;
using Microsoft.Xaml.Interactivity;
#if WINDOWS_PHONE
using System.Windows.Controls;
using Microsoft.Devices;
#else
using Windows.UI.Xaml.Controls;
#endif

namespace WpWinNl.Behaviors
{
  /// <summary>
  ///     Limits the number of characters that can be entered into a textbox.
  ///     Optionally vibrates the phone when the limit is reached.
  /// </summary>
  public class LimitTextBoxBehavior : Behavior<TextBox>
  {
    public int MaxChars { get; set; }

    protected override void OnAttached()
    {
      base.AssociatedObject.TextChanged += OnTextChanged;
      base.OnAttached();
    }

    protected override void OnDetaching()
    {
      base.AssociatedObject.TextChanged -= OnTextChanged;
      base.OnDetaching();
    }

    private void OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
    {
      if (AssociatedObject.Text.Length >= MaxChars)
      {
        AssociatedObject.Text = AssociatedObject.Text.Substring(0, MaxChars);
        AssociatedObject.SelectionStart = MaxChars;
#if WINDOWS_PHONE
        if (Vibrate)
        {
          VibrateController.Default.Start(TimeSpan.FromMilliseconds(15));
        }
#endif
      }
    }
  }
}