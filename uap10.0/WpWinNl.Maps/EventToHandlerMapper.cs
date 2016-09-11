using Windows.UI.Xaml;

namespace WpWinNl.Maps
{
  public class EventToHandlerMapper : FrameworkElement
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
      typeof (EventToHandlerMapper),
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
        typeof(EventToHandlerMapper),
        new PropertyMetadata(default(string)));

    #endregion

    #region MethodName

    /// <summary>
    /// MethodName Property name
    /// </summary>
    public const string MethodNamePropertyName = "MethodName";

    public string MethodName
    {
      get { return (string)GetValue(MethodNameProperty); }
      set { SetValue(MethodNameProperty, value); }
    }

    /// <summary>
    /// MethodName Property definition
    /// </summary>
    public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
        MethodNamePropertyName,
        typeof(string),
        typeof(EventToHandlerMapper),
        new PropertyMetadata(default(string)));
  
    #endregion
  }
}
