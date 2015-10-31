using Windows.Storage.Streams;
using GalaSoft.MvvmLight.Messaging;

namespace WpWinNl.Behaviors
{
  public class ScreenshotToStreamMessage: MessageBase
  {
    public ScreenshotToStreamMessage(object sender = null, object target = null, ScreenshotToStreamCallback callback = null)
      : base(sender, target)
    {
      Callback = callback;
    }

    public ScreenshotToStreamCallback Callback { get; set; }
  }

  public delegate void ScreenshotToStreamCallback(IRandomAccessStream stream);
}
