using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;

namespace WpWinNl.Messages
{
  public class WindowVisibilityMessage : MessageBase
  {
    public WindowVisibilityMessage(bool visible, object sender = null, object target = null) :base(sender, target)
    {
      Visible = visible;
    }
    public bool Visible { get; protected set; }

    public static void Setup()
    {
      Window.Current.VisibilityChanged += (s, f) => Messenger.Default.Send(new WindowVisibilityMessage(f.Visible));
    }
  }
}
