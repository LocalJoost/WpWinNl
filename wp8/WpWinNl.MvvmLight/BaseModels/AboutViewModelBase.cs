using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WpWinNl.Utilities;
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Tasks;

namespace WpWinNl.BaseModels
{
  /// <summary>
  /// Base class for an about view model
  /// </summary>
  public class AboutViewModelBase : ViewModelBase
  {
    protected ManifestAppInfo _manifestAppInfo;
    public void LoadValuesFromResource<T>()
    {
      var targetType = GetType();
      var sourceType = typeof(T);
      foreach (var targetProperty in targetType.GetRuntimeProperties().Where(p => p.GetMethod != null && p.SetMethod != null &&
                                     !p.SetMethod.IsStatic && !p.GetMethod.IsStatic &&
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
      _manifestAppInfo = new ManifestAppInfo();
      if (IsInDesignMode)
      {
        _about = "SampleAbout";
        _buy = "SampleBuy";
        _buyTheApp = "If you prefer the full version blah awesome blah blah please buy this thing please blah.";
        _companyUrl = "http://freakingawesomesample.co.uk.dot.org";
        _copyright = "(c) 2011 #WpWinNl";
        _review = "SampleReview";
        _reviewTheApp =
            "If you like this app please blah rate blah whatever blah lorum ipsum and whatever text you like here blah highly appreciated.";
        _support = "SampleSupport";
        _supportMessage =
            "For technical support, blah blah blah more sample text, don't hesitate to contact me at: john@doe.com.";
        _supportEmail = "john@doe.com";
        _otherapps = "other apps";
        _shareapp = "share app";
        _shareappmessage = "Great Phone7 app";
      }
    }

    /// <Summary>A string value for the AppTitle</Summary>
    public virtual string AppTitle
    {
      get
      {
        if (!IsInDesignMode)
        {
          return _manifestAppInfo.Title;
        }

        return "Application Title";
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

    private string _about;
    /// <Summary>A string value for the About</Summary>
    public virtual string About
    {
      get { return _about; }
      set
      {
        _about = value;
        RaisePropertyChanged(() => About);
      }
    }

    private string _buy;
    /// <Summary>A string value for the Buy</Summary>
    public virtual string Buy
    {
      get { return _buy; }
      set
      {
        _buy = value;
        RaisePropertyChanged(() => Buy);
      }
    }

    private string _buyTheApp;
    /// <Summary>A string value for the BuyTheApp</Summary>
    public virtual string BuyTheApp
    {
      get { return _buyTheApp; }
      set
      {
        _buyTheApp = value;
        RaisePropertyChanged(() => BuyTheApp);
      }
    }

    private string _companyUrl;
    /// <Summary>A string value for the CompanyUrl</Summary>
    public virtual string CompanyUrl
    {
      get { return _companyUrl; }
      set
      {
        _companyUrl = value;
        RaisePropertyChanged(() => CompanyUrl);
      }
    }

    private string _copyright;
    /// <Summary>A string value for the Copyright</Summary>
    public virtual string Copyright
    {
      get { return _copyright; }
      set
      {
        _copyright = value;
        RaisePropertyChanged(() => Copyright);
      }
    }

    private string _review;
    /// <Summary>A string value for the Review</Summary>
    public virtual string Review
    {
      get { return _review; }
      set
      {
        _review = value;
        RaisePropertyChanged(() => Review);
      }
    }

    private string _reviewTheApp;
    /// <Summary>A string value for the ReviewTheApp</Summary>
    public virtual string ReviewTheApp
    {
      get { return _reviewTheApp; }
      set
      {
        _reviewTheApp = value;
        RaisePropertyChanged(() => ReviewTheApp);
      }
    }

    private string _support;
    /// <Summary>A string value for the Support</Summary>
    public virtual string Support
    {
      get { return _support; }
      set
      {
        _support = value;
        RaisePropertyChanged(() => Support);
      }
    }

    private string _supportMessage;
    /// <Summary>A string value for the SupportMessage</Summary>
    public virtual string SupportMessage
    {
      get { return _supportMessage; }
      set
      {
        _supportMessage = value;
        RaisePropertyChanged(() => SupportMessage);
      }
    }

    private string _supportEmail;
    /// <Summary>A string value for the SupportEmail</Summary>
    public virtual string SupportEmail
    {
      get { return _supportEmail; }
      set
      {
        _supportEmail = value;
        RaisePropertyChanged(() => SupportEmail);
      }
    }

    private string _otherapps;
    /// <Summary>A string value for the OtherApps</Summary>
    public virtual string OtherApps
    {
      get { return _otherapps; }
      set
      {
        _otherapps = value;
        RaisePropertyChanged(() => OtherApps);
      }
    }

    private string _otherappssearch;
    /// <Summary>A string value for the OtherApps</Summary>
    public virtual string OtherAppsSearch
    {
      get { return _otherappssearch; }
      set
      {
        _otherappssearch = value;
        RaisePropertyChanged(() => OtherAppsSearch);
      }
    }

    private string _shareapp;
    /// <Summary>A string value for the ShareApp</Summary>
    public virtual string ShareApp
    {
      get { return _shareapp; }
      set
      {
        _shareapp = value;
        RaisePropertyChanged(() => ShareApp);
      }
    }

    /// <Summary>A string value for the ShareAppUri</Summary>
    public virtual string ShareAppUri
    {
      get
      {
        if (!IsInDesignMode)
        {
          return string.Concat("http://windowsphone.com/s?appid=", _manifestAppInfo.ProductId.TrimStart('{').TrimEnd('}'));
        }

        return "http://dotnetbyexample.blogspot.com";
      }
    }

    private string _shareappmessage;
    /// <Summary>A string value for the ShareAppMessage</Summary>
    public virtual string ShareAppMessage
    {
      get { return _shareappmessage; }
      set
      {
        _shareappmessage = value;
        RaisePropertyChanged(() => ShareAppMessage);
      }
    }


    private bool? _trialMode;
    public virtual bool IsTrialMode
    {
      get
      {
        if (_trialMode == null)
        {
          var s = new LicenseInformation();
#if DEBUG
                    _trialMode = true;
#else
          _trialMode = new bool?(s.IsTrial());
#endif
        }
        return _trialMode.Value;
      }
    }

    public virtual Visibility BuyPanelVisible
    {
      get { return IsTrialMode || IsInDesignMode ? Visibility.Visible : Visibility.Collapsed; }
    }

    public virtual string ApplicationVersion
    {
      get
      {
        if (IsInDesignMode)
          return "version x.x.x";

        var version = _manifestAppInfo.Version;
        return version.Substring(0, version.LastIndexOf("."));
      }
    }


    public ICommand SupportQuestionCommand
    {
      get
      {
        return new RelayCommand(() =>
        {
          var emailComposeTask = new EmailComposeTask
          {
            To = SupportEmail,
            Subject = string.Concat(
                Support, " ", AppTitle, " ",
                ApplicationVersion)
          };
          emailComposeTask.Show();
        });
      }
    }

    public ICommand BuyCommand
    {
      get
      {
        return new RelayCommand(() => new MarketplaceDetailTask().Show());
      }
    }

    public ICommand ReviewCommand
    {
      get
      {
        return new RelayCommand(() => new MarketplaceReviewTask().Show());
      }
    }

    public ICommand ViewWebsiteCommand
    {
      get
      {
        return new RelayCommand(() => new WebBrowserTask { Uri = new Uri(CompanyUrl) }.Show());
      }
    }

    public ICommand ViewOtherAppsCommand
    {
      get { return new RelayCommand(() => new MarketplaceSearchTask() { SearchTerms = OtherAppsSearch }.Show()); }
    }

    public ICommand ShareAppCommand
    {
      get
      {
        return
            new RelayCommand(
                () => new ShareLinkTask() { LinkUri = new Uri(ShareAppUri), Message = ShareAppMessage, Title = AppTitle }.Show());
      }
    }
  }
}