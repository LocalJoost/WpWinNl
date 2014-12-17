using System;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Tasks;

namespace WpWinNl.Behaviors
{
    /// <summary>
    /// Action that executes the ShareLinkTask. 
    /// The "LinkUri", "Message" and "Title" properties can be bound to.
    /// </summary>
    public class ShareLinkAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty LinkUriProperty =
            DependencyProperty.Register("LinkUri", typeof (string), typeof (ShareLinkAction),
                                        new PropertyMetadata(string.Empty));


        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof (string), typeof (ShareLinkAction),
                                        new PropertyMetadata(string.Empty));


        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof (string), typeof (ShareLinkAction),
                                        new PropertyMetadata(string.Empty));

        public string LinkUri
        {
            get { return (string) GetValue(LinkUriProperty); }
            set { SetValue(LinkUriProperty, value); }
        }

        public string Message
        {
            get { return (string) GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        protected override void Invoke(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(LinkUri))
            {
                var task = new ShareLinkTask {LinkUri = new Uri(LinkUri), Message = Message, Title = Title};
                try
                {
                    task.Show();
                }
                catch (InvalidOperationException e)
                {
                }
            }
        }
    }
}