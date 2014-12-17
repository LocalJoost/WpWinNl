using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Store;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WpWinNl.Utilities;

namespace WpWinNl.BaseModels
{
  /// <summary>
  /// Base class for an about view model
  /// </summary>
  public class AboutViewModelBase : ViewModelBase
  {

    protected const string StorePrefix = "ms-windows-store:";
    protected ManifestAppInfo ManifestInfo { get; private set; }

    protected string OperatingSystem { get; private set; }

    protected bool IsWp
    {
      get
      {
        return OperatingSystem == "WindowsPhone";
      }
    }

    public void LoadValuesFromResource<T>()
    {

      var targetType = GetType();
      var sourceType = typeof(T);
      foreach (var targetProperty in targetType.GetRuntimeProperties().Where(p => p.GetMethod != null && p.SetMethod != null &&
                                     p.GetMethod.IsPublic && p.SetMethod.IsPublic))
      {
        var sourceProperty = sourceType.GetRuntimeProperty(targetProperty.Name);
        if (sourceProperty != null)
        {
          if (targetProperty.CanWrite)
          {
            targetProperty.SetValue(this, sourceProperty.GetValue(null, null), null);
          }
        }
      }
    }

    public AboutViewModelBase()
    {
      ManifestInfo = new ManifestAppInfo();
      OperatingSystem = new EasClientDeviceInformation().OperatingSystem;

      if (IsInDesignMode)
      {
        about = "about*";
        buy = "buy*";
        buyTheApp = "If you prefer the full version blah awesome blah blah please buy this thing please blah.";
        companyUrl = "http://freakingawesomesample.co.uk.dot.org";
        copyright = "(c) 2014 John Doe";
        review = "review*";
        reviewTheApp =
            "If you like this app please blah rate blah whatever blah lorum ipsum and whatever text you like here blah highly appreciated.*";
        support = "support*";
        supportMessage =
            "For technical support, blah blah blah more sample text, don't hesitate to contact me at: john@doe.com.*";
        supportEmail = "john@doe.com";
        otherapps = "other apps*";
        shareapp = "share app";
        shareappmessage = "Great app";
      }
    }

    /// <Summary>An uppercase string value for the AppTitle</Summary>
    public virtual string AppTitleUpperCase
    {
      get
      {
        return AppTitle != null ? AppTitle.ToUpperInvariant() : null;
      }
    }

    private string about;
    /// <Summary>A string value for the About</Summary>
    public virtual string About
    {
      get { return about; }
      set
      {
        about = value;
        RaisePropertyChanged(() => About);
      }
    }

    private string buy;
    /// <Summary>A string value for the Buy</Summary>
    public virtual string Buy
    {
      get { return buy; }
      set
      {
        buy = value;
        RaisePropertyChanged(() => Buy);
      }
    }

    private string buyTheApp;
    /// <Summary>A string value for the BuyTheApp</Summary>
    public virtual string BuyTheApp
    {
      get { return buyTheApp; }
      set
      {
        buyTheApp = value;
        RaisePropertyChanged(() => BuyTheApp);
      }
    }

    private string companyUrl;
    /// <Summary>A string value for the CompanyUrl</Summary>
    public virtual string CompanyUrl
    {
      get { return companyUrl; }
      set
      {
        companyUrl = value;
        RaisePropertyChanged(() => CompanyUrl);
      }
    }

    private string copyright;
    /// <Summary>A string value for the Copyright</Summary>
    public virtual string Copyright
    {
      get { return copyright; }
      set
      {
        copyright = value;
        RaisePropertyChanged(() => Copyright);
      }
    }

    private string review;
    /// <Summary>A string value for the Review</Summary>
    public virtual string Review
    {
      get { return review; }
      set
      {
        review = value;
        RaisePropertyChanged(() => Review);
      }
    }

    private string reviewTheApp;
    /// <Summary>A string value for the ReviewTheApp</Summary>
    public virtual string ReviewTheApp
    {
      get { return reviewTheApp; }
      set
      {
        reviewTheApp = value;
        RaisePropertyChanged(() => ReviewTheApp);
      }
    }

    private string support;
    /// <Summary>A string value for the Support</Summary>
    public virtual string Support
    {
      get { return support; }
      set
      {
        support = value;
        RaisePropertyChanged(() => Support);
      }
    }

    private string supportMessage;
    /// <Summary>A string value for the SupportMessage</Summary>
    public virtual string SupportMessage
    {
      get { return supportMessage; }
      set
      {
        supportMessage = value;
        RaisePropertyChanged(() => SupportMessage);
      }
    }

    private string supportEmail;
    /// <Summary>A string value for the SupportEmail</Summary>
    public virtual string SupportEmail
    {
      get { return supportEmail; }
      set
      {
        supportEmail = value;
        RaisePropertyChanged(() => SupportEmail);
        RaisePropertyChanged(() => SupportEmailLink);
      }
    }

    // NEW!!!
    /// <Summary>A string value for the SupportEmail</Summary>
    public virtual string SupportEmailLink
    {
      get { return string.Concat("mailto:", SupportEmail); }
    }

    private string otherapps;
    /// <Summary>A string value for the OtherApps</Summary>
    public virtual string OtherApps
    {
      get { return otherapps; }
      set
      {
        otherapps = value;
        RaisePropertyChanged(() => OtherApps);
      }
    }

    private string otherappssearch;
    /// <Summary>A string value for the OtherApps</Summary>
    public virtual string OtherAppsSearch
    {
      get { return otherappssearch; }
      set
      {
        otherappssearch = value;
        RaisePropertyChanged(() => OtherAppsSearch);
      }
    }

    private string shareapp;
    /// <Summary>A string value for the ShareApp</Summary>
    public virtual string ShareApp
    {
      get { return shareapp; }
      set
      {
        shareapp = value;
        RaisePropertyChanged(() => ShareApp);
      }
    }

    /// <Summary>A string value for the ShareAppUri</Summary>
    public virtual string ShareAppUri
    {
      get
      {
        try
        {
          if (!IsInDesignMode)
          {
            return IsWp
              ? string.Concat("http://windowsphone.com/s?appid=", AppId)
              : string.Concat("http://windows.microsoft.com/en-us/windows/search#", AppTitle.Replace(" ", "%20"));
          }
        }

        catch(Exception)
        {
        }

        return "http://dotnetbyexample.blogspot.com";

      }
    }

    private string shareappmessage;
    /// <Summary>A string value for the ShareAppMessage</Summary>
    public virtual string ShareAppMessage
    {
      get { return shareappmessage; }
      set
      {
        shareappmessage = value;
        RaisePropertyChanged(() => ShareAppMessage);
      }
    }


    private string appTitle;
    public string AppTitle
    {
      get
      {
        if (!IsInDesignMode)
        {
          return string.IsNullOrEmpty(appTitle) ? ManifestInfo.Title : appTitle;
        }
        return "Application Title";
      }
      set
      {
        appTitle = value;
        RaisePropertyChanged(() => AppTitle);
      }
    }



    private LicenseInformation licenseInformation;
    public virtual bool IsTrialMode
    {
      get
      {
        if (licenseInformation == null)
        {
          licenseInformation = CurrentApp.LicenseInformation;
        }

        return licenseInformation.IsTrial;
      }
    }

    public virtual Visibility BuyPanelVisible
    {
      get
      {
        var result = IsTrialMode || IsInDesignMode ? Visibility.Visible : Visibility.Collapsed;
        return result;
      }
    }

    public virtual string ApplicationVersion
    {
      get
      {
        if (IsInDesignMode)
          return "version x.x.x";
        var v = Package.Current.Id.Version;
        return string.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build);
      }
    }


    public ICommand SupportQuestionCommand
    {
      get
      {
        return new RelayCommand(async () =>
            {

              var mailto = new Uri(string.Format("mailto:?to={0}&subject={1}", SupportEmail, string.Concat(
                    Support, " ", AppTitle, " ",
                    ApplicationVersion)));
              await Launcher.LaunchUriAsync(mailto);
            });
      }
    }

    public ICommand BuyCommand
    {
      get
      {
        {
          return new RelayCommand(async () =>
            await Launcher.LaunchUriAsync(new Uri(string.Concat(StorePrefix, IsWp ? "navigate?appid=" : "PDP?PFN=", AppId)))
            );
        }
      }
    }

    public ICommand ReviewCommand
    {
      get
      {

        {
          return new RelayCommand(async () =>
            await Launcher.LaunchUriAsync(new Uri(string.Concat(StorePrefix, IsWp ? "reviewapp?appid=" : "REVIEW?PFN=", AppId)))
            );
        }
      }
    }

    public ICommand ViewWebsiteCommand
    {
      get
      {
        return new RelayCommand(async () => await Launcher.LaunchUriAsync(new Uri(CompanyUrl)));
      }
    }

    public ICommand ViewOtherAppsCommand
    {
      get
      {
        return new RelayCommand(
          async () =>
                  await
                    Launcher.LaunchUriAsync(
                      new Uri(string.Concat(StorePrefix, IsWp ? "search?publisher=" : "PUBLISHER?name=", ManifestInfo.Publisher)))
                );
      }
    }

    public ICommand ShareAppCommand
    {
      get { return new RelayCommand(ShowShareMessage); }
    }

    private string AppId
    {
      get
      {
        if (IsWp)
        {
#if DEBUG
          return "48fd8097-f07e-e011-986b-78e7d1fa76f8";
#else
          return CurrentApp.AppId.ToString();
#endif
        }
        else
        {
#if DEBUG
          return "17852LocalJoost.CatchemBirds_rt0w0x8frck66";

#else
          return Package.Current.Id.FamilyName;
#endif
        }
      }
    }


    public string ShareMessage { get; set; }
    public string ShareTitle { get; set; }

    private void DisableShareMessage()
    {
      DataTransferManager.GetForCurrentView().DataRequested -= ShareTextHandler;
    }

    private void ShowShareMessage()
    {
      DataTransferManager.GetForCurrentView().DataRequested += ShareTextHandler;
      DataTransferManager.ShowShareUI();
    }

    private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
    {
      DataRequest request = e.Request;
      request.Data.Properties.Title = ShareAppMessage;
      request.Data.Properties.Description = ShareApp;
      request.Data.SetText(string.Concat(ShareAppMessage, " ", ShareAppUri));

      DisableShareMessage();
    }
  }
}