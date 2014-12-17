using System.Windows;
using System.Windows.Interactivity;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A behavior that shows a message box when the bound property changes value
  /// </summary>
  public class MessageBoxDisplayBehavior : Behavior<FrameworkElement>
  {
    public const string MessagePropertyName = "Message";

    public string Message
    {
      get { return (string)GetValue(MessageProperty); }
      set { SetValue(MessageProperty, value); }
    }

    public static readonly DependencyProperty MessageProperty =
      DependencyProperty.Register(
        MessagePropertyName,
        typeof(string),
        typeof(MessageBoxDisplayBehavior),
        new PropertyMetadata(string.Empty, MessageChanged));

    public static void MessageChanged(DependencyObject d, 
      DependencyPropertyChangedEventArgs e)
    {
      var msg = e.NewValue as string;
      if (!string.IsNullOrEmpty(msg))
      {
        MessageBox.Show(msg);
      }
    }
  }
}
