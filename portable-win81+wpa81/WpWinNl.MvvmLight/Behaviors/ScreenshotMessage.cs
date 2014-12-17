using Windows.Storage;
using GalaSoft.MvvmLight.Messaging;

namespace WpWinNl.Behaviors
{
  public class ScreenshotMessage : MessageBase
  {
    public ScreenshotMessage(ScreenshotCallback callback = null, object sender = null, object target = null) : base(sender, target)
    {
      Callback = callback;
    }

    public ScreenshotCallback Callback { get; set; }
  }

  public delegate void ScreenshotCallback(StorageFile file);
}
