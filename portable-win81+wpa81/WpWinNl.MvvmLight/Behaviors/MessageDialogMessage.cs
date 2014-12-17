using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace WpWinNl.Behaviors
{
  public class MessageDialogMessage : MessageBase
  {
    public MessageDialogMessage(string message, string title,  string okText, string cancelText, MessageDialogCallBack okCallback = null, MessageDialogCallBack cancelCallback = null, object sender = null, object target = null)
      : base(sender, target)
    {
      Message = message;
      Title = title;
      OkText = okText;
      CancelText = cancelText;
      OkCallback = okCallback;
      CancelCallback = cancelCallback;
    }

    public MessageDialogCallBack OkCallback { get; private set; }
    public MessageDialogCallBack CancelCallback { get; private set; }

    public string Title { get; private set; }

    public string Message { get; private set; }

    public string OkText { get; private set; }

    public string CancelText { get; private set; }
  }

  public delegate Task MessageDialogCallBack();
}
