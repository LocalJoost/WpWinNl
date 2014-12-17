using Windows.UI.Xaml;

namespace WpWinNl.Maps
{
  public class EventToCommandMapper : FrameworkElement
  {
    #region EventName

    /// <summary>
    /// EventName Property name
    /// </summary>
    public const string EventNamePropertyName = "EventName";

    public string EventName
    {
      get { return (string) GetValue(EventNameProperty); }
      set { SetValue(EventNameProperty, value); }
    }

    /// <summary>
    /// EventName Property definition
    /// </summary>
    public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(
      EventNamePropertyName,
      typeof (string),
      typeof (EventToCommandMapper),
      new PropertyMetadata(default(string)));

    #endregion

    #region CommandName

    /// <summary>
    /// CommandName Property name
    /// </summary>
    public const string CommandNamePropertyName = "CommandName";

    public string CommandName
    {
      get { return (string)GetValue(CommandNameProperty); }
      set { SetValue(CommandNameProperty, value); }
    }

    /// <summary>
    /// CommandName Property definition
    /// </summary>
    public static readonly DependencyProperty CommandNameProperty = DependencyProperty.Register(
        CommandNamePropertyName,
        typeof(string),
        typeof(EventToCommandMapper),
        new PropertyMetadata(default(string)));

    #endregion
  }
}
