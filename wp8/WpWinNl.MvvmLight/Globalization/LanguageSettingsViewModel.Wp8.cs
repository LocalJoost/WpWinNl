using System.Globalization;
using System.Linq;
using System.Threading;

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
      // Try to select from current UI thread on full name
      var language = SupportedLanguages.FirstOrDefault(p => p.Locale == Thread.CurrentThread.CurrentUICulture.Name);
      if (language == null)
      {
        // Try to select from current UI thread on 2 letter ISO code
        language =
          SupportedLanguages.FirstOrDefault(p => p.Locale.StartsWith(
            Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName));
      }
      if (language == null)
      {
        // Still no language: take the first one that starts with English
        language = SupportedLanguages.First(p => p.Locale.StartsWith("en"));
      }

      return language;
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
      Thread.CurrentThread.CurrentUICulture = new CultureInfo(CurrentLanguage.Locale);
      Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
    }
  }
}
