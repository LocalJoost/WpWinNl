using System;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;

namespace WpWinNl.Behaviors
{
  public class MessageDialogBehavior : SafeBehavior<FrameworkElement>
  {
    protected override void OnSetup()
    {
      Messenger.Default.Register<MessageDialogMessage>(this, ProcessMessage);
      base.OnSetup();
    }

    private async void ProcessMessage(MessageDialogMessage m)
    {
      bool result = false;
      var dialog = new MessageDialog(m.Message, m.Title);

      if (!string.IsNullOrWhiteSpace(m.OkText))
      {
        dialog.Commands.Add(new UICommand(m.OkText, cmd => result = true));
      }

      if (!string.IsNullOrWhiteSpace(m.CancelText))
      {
        dialog.Commands.Add(new UICommand(m.CancelText, cmd => result = false));
      }

      try
      {
        await dialog.ShowAsync();
        if (result && m.OkCallback != null)
        {
          await m.OkCallback();
        }

        if (!result && m.CancelCallback != null)
        {
          await m.CancelCallback();
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine("double call - ain't going to work");
      }
    }
  }
}
