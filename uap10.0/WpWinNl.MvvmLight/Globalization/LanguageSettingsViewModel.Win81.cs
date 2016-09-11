using System.Linq;
using Windows.Globalization;

namespace WpWinNl.Globalization
{

  public partial class LanguageSettingsViewModel
  {
    /// <summary>
    /// Determine current language
    /// </summary>
    /// <returns></returns>
    private Language GetDefaultLanguage()
    {
      var language =
        SupportedLanguages.FirstOrDefault(p => p.Locale == ApplicationLanguages.PrimaryLanguageOverride);
      if (language == null)
      {
        // Try to select from current UI thread on full name
        language =
          ApplicationLanguages.Languages.Select(p => SupportedLanguages.FirstOrDefault(q => q.Locale == p))
            .FirstOrDefault();

        if (language == null)
        {
          // Try to select from current UI thread on 2 letter ISO code
          language =
            ApplicationLanguages.Languages.Select(
              p => SupportedLanguages.FirstOrDefault(q => q.Locale.Substring(0, 2) == p.Substring(0, 2)))
              .FirstOrDefault();
        }
      }
      return language ?? (SupportedLanguages.First(p => p.Locale.StartsWith("en")));
    }

    /// <summary>
    /// Sets the language from current locale.
    /// </summary>
    public void SetLanguageFromCurrentLocale()
    {
      if (CurrentLanguage == null)
      {
        CurrentLanguage = GetDefaultLanguage();
      }
      ApplicationLanguages.PrimaryLanguageOverride = CurrentLanguage.Locale;
    }
  }
}
