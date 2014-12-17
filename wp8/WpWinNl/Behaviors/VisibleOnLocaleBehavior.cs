using System.Globalization;
using System.Windows;
using System.Windows.Interactivity;

namespace WpWinNl.Behaviors
{
    /// <summary>
    ///     Behavior that sets the attached textblock to visibile if it
    ///     matches the sepecified two letter ISO language name
    /// </summary>
    public class VisibleOnLocaleBehavior : SafeBehavior<FrameworkElement>
    {
        public static readonly DependencyProperty LanguageCodeProperty = DependencyProperty.Register(
            "LanguageCode",
            typeof(string),
            typeof(VisibleOnLocaleBehavior),
            new PropertyMetadata(OnLanguageCodeChanged));

        public string LanguageCode
        {
            get { return (string)GetValue(LanguageCodeProperty); }
            set { SetValue(LanguageCodeProperty, value); }
        }

        private static void OnLanguageCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var visibileOnLocaleBehavior = d as VisibleOnLocaleBehavior;
            if (visibileOnLocaleBehavior != null && visibileOnLocaleBehavior.AssociatedObject != null)
            {
                visibileOnLocaleBehavior.SetVisibility();
            }
        }

        private void SetVisibility()
        {
            CultureInfo currentUiCulture = CultureInfo.CurrentUICulture;

            if (LanguageCode.ToLower() == currentUiCulture.TwoLetterISOLanguageName.ToLower())
            {
                base.AssociatedObject.Visibility = Visibility.Visible;
            }
            else
            {
                base.AssociatedObject.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            SetVisibility();
        }
    }
}