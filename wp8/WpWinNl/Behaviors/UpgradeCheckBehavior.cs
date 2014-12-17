using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Microsoft.Phone.Tasks;
using WpWinNl.Utilities;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A behavior checkin from in the app if there's an update available. 
  /// </summary>
  public class UpgradeCheckBehavior : SafeBehavior<Page>
  {
    private ManifestAppInfo appInfo;

    protected override void OnSetup()
    {
      appInfo = new ManifestAppInfo();
      CheckUpgrade();
    }

    private void CheckUpgrade()
    {
#if !DEBUG
      GetLatestVersion().ContinueWith(ProcessResult);
#endif
    }

    private void ProcessResult(Task<Version> t)
    {
      if(t.IsCompleted && t.Result != null )
      {
        var currentVersion = new Version(appInfo.Version);
        if (currentVersion < t.Result )
        {
          DoShowUpgrade();
        }
      }
    }

    private void DoShowUpgrade()
    {
      Deployment.Current.Dispatcher.BeginInvoke(() =>
      {
        var result = MessageBox.Show(Message, Caption, MessageBoxButton.OKCancel);
        if (result == MessageBoxResult.OK)
        {
          var marketplaceReviewTask = new MarketplaceDetailTask();
          try
          {
            marketplaceReviewTask.Show();
          }
          catch (InvalidOperationException ex)
          {
          }
        }
      });
    }

    /// <summary>
    /// This method is almost 100% stolen from 
    /// http://www.pedrolamas.com/2013/07/24/checking-for-updates-from-inside-a-windows-phone-app/
    /// </summary>
    private Task<Version> GetLatestVersion()
    {
      var cultureInfoName = CultureInfo.CurrentUICulture.Name;
      var url = string.Format("http://marketplaceedgeservice.windowsphone.com/v8/catalog/apps/{0}?os={1}&cc={2}&oc=&lang={3}​",
          appInfo.ProductId,
          Environment.OSVersion.Version,
          cultureInfoName.Substring(cultureInfoName.Length - 2).ToUpperInvariant(),
          cultureInfoName);

      var request = WebRequest.Create(url);

      return Task.Factory.FromAsync(request.BeginGetResponse, result =>
      {
        try
        {
          var response = (HttpWebResponse)request.EndGetResponse(result);
          if (response.StatusCode != HttpStatusCode.OK)
          {
            throw new WebException("Http Error: " + response.StatusCode);
          }

          using (var outputStream = response.GetResponseStream())
          {
            using (var reader = XmlReader.Create(outputStream))
            {
              reader.MoveToContent();
              var aNamespace = reader.LookupNamespace("a");
              reader.ReadToFollowing("entry", aNamespace);
              reader.ReadToDescendant("version");
              return new Version(reader.ReadElementContentAsString());
            }
          }
        }
        catch (Exception)
        {
          return null;
        }
      },
      null);
    }

    #region Caption

    /// <summary>
    /// Caption Property name
    /// </summary>
    public const string CaptionPropertyName = "Caption";

    public string Caption
    {
      get { return (string)GetValue(CaptionProperty); }
      set { SetValue(CaptionProperty, value); }
    }

    /// <summary>
    /// Caption Property definition
    /// </summary>
    public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(
        CaptionPropertyName,
        typeof(string),
        typeof(UpgradeCheckBehavior),
        new PropertyMetadata("New version!"));

    #endregion
    
    #region Message

    /// <summary>
    /// Message Property name
    /// </summary>
    public const string MessagePropertyName = "Message";

    public string Message
    {
      get { return (string)GetValue(MessageProperty); }
      set { SetValue(MessageProperty, value); }
    }

    /// <summary>
    /// Message Property definition
    /// </summary>
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
        MessagePropertyName,
        typeof(string),
        typeof(UpgradeCheckBehavior),
        new PropertyMetadata("There's a new version of this app available, do you want to update?"));

    #endregion
  }
}
