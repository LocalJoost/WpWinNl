using System;
using GalaSoft.MvvmLight;

namespace WpWinNl.Globalization
{
  /// <summary>
  /// Supported languages
  /// </summary>
  ///
  public class Language : ViewModelBase, IEquatable<Language>
  {
    private string locale;
    /// <summary>
    /// Gets or sets the locale.
    /// </summary>
    /// <value>The locale.</value>
    public string Locale
    {
      get { return locale; }
      set
      {
        if (locale != value)
        {
          locale = value;
          RaisePropertyChanged(() => Locale);
        }
      }
    }

    private string description;
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description
    {
      get { return description; }
      set
      {
        if (description != value)
        {
          description = value;
          RaisePropertyChanged(() => Description);
        }
      }
    }

    public override string ToString()
    {
      return Description;
    }

    public bool Equals(Language other)
    {
      return other != null && other.Locale.Equals(Locale);
    }

    public override bool Equals(object obj)
    {
      return Equals(obj as Language);
    }

    public override int GetHashCode()
    {
      return Locale.GetHashCode();
    }
  }
}
