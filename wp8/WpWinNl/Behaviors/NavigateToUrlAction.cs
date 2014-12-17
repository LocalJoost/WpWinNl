using System;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Tasks;

namespace WpWinNl.Behaviors
{
    /// <summary>
    /// Trigger action to use to navigate to a URL. Includes network availability detection.
    /// </summary>
    public class NavigateToUrlAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(
            "Url", typeof (string), typeof (NavigateToUrlAction), new PropertyMetadata(null));

        public static readonly DependencyProperty NoNetworkMessageProperty = DependencyProperty.Register(
            "NoNetworkMessage", typeof (string), typeof (NavigateToUrlAction),
            new PropertyMetadata("No network available"));

        public string Url
        {
            get { return (string) GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public string NoNetworkMessage
        {
            get { return (string) GetValue(NoNetworkMessageProperty); }
            set { SetValue(NoNetworkMessageProperty, value); }
        }


        protected override void Invoke(object parameter)
        {
            string uriString = Url;
            if (uriString != null)
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    var wbt = new WebBrowserTask {Uri = new Uri(uriString)};
                    try
                    {
                        wbt.Show();
                    }
                    catch (InvalidOperationException e)
                    {
                    }
                }
                else
                {
                    MessageBox.Show(NoNetworkMessage);
                }
            }
        }
    }
}