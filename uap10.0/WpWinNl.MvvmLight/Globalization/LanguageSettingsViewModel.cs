using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace WpWinNl.Globalization
{
  /// <summary>
  /// A ViewModel class to handle language settings. 
  /// Override this class and add languages in the constructor
  /// </summary>
  public partial class LanguageSettingsViewModel : ViewModelBase
  {
    public LanguageSettingsViewModel()
    {
      AddLanguages(new Language { Description = "English", Locale = "en-US" });
    }

    private readonly ObservableCollection<Language> supportedLanguages =
       new ObservableCollection<Language>();

    /// <summary>
    /// Gets the supported languages.
    /// </summary>
    public ObservableCollection<Language> SupportedLanguages
    {
      get { return supportedLanguages; }
    }

    private Language currentLanguage;
    /// <summary>
    /// Gets or sets the current language.
    /// </summary>
    /// <value>
    /// The current language.
    /// </value>
    public Language CurrentLanguage
    {
      get
      {
        return currentLanguage;
      }
      set
      {
        if (currentLanguage != value && value != null)
        {
          var oldValue = currentLanguage;
          currentLanguage = value;
          RaisePropertyChanged(() => CurrentLanguage, oldValue, currentLanguage, true);
        }
      }
    }

    /// <summary>
    /// Adds the languages.
    /// </summary>
    /// <param name="languages">The languages.</param>
    public void AddLanguages(params Language[] languages)
    {
      if (languages != null && languages.Any())
      {
        foreach (var l in languages)
        {
          if (!supportedLanguages.Contains(l))
          {
            supportedLanguages.Add(l);
          }
        }
      }
    }
  }
}
