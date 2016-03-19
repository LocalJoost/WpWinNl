using System;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Behaviors
{
  public class MessageDialogBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      Messenger.Default.Register<MessageDialogMessage>(this, ProcessMessage);
      base.OnAttached();
    }

    protected override void OnDetaching()
    {
      Messenger.Default.Unregister<MessageDialogMessage>(this);
      base.OnAttached();
    }

    private async void ProcessMessage(MessageDialogMessage m)
    {
      bool result = false;
      var dialog = new MessageDialog(m.Title, m.Message);

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
      catch (Exception)
      {
        Debug.WriteLine("double call - ain't going to work");
      }
    }
  }
}
