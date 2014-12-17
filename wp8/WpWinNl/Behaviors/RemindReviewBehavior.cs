using System;
using System.Windows;
using WpWinNl.Utilities;
#if WINDOWS_PHONE
using System.Windows.Controls;
using Microsoft.Phone.Tasks;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A behavior to remind the user to review the app after a couple
  /// times using it
  /// </summary>
  public class RemindReviewBehavior : SafeBehavior<Page>
  {
    protected override void OnSetup()
    {
      try
      {
        CheckRemind();
      }
      catch (Exception)
      {
      }
    }

    private void CheckRemind()
    {
      var helper = new IsolatedStorageHelper<RemindData>();
      var remindData = helper.ExistsInStorage() ? helper.RetrieveFromStorage() : new RemindData();

      if (!remindData.Reviewed && !remindData.Refused)
      {
        remindData.Starts++;
        if (remindData.Starts % RemindFrequency == 0)
        {
          var result = MessageBox.Show(Message, Caption, MessageBoxButton.OKCancel);
          if (result == MessageBoxResult.OK)
          {
            Review();
            remindData.Reviewed = true;
          }
          else
          {
            if (remindData.Starts >= MaxReminders * RemindFrequency)
            {
              remindData.Refused = true;
            }
          }
        }
        helper.SaveToStorage(remindData);
      }
    }

    private void Review()
    {
      var marketplaceReviewTask = new MarketplaceReviewTask();
      try
      {
        marketplaceReviewTask.Show();
      }
      catch(Exception ex)
      {

      }
    }

    #region RemindFrequency

    /// <summary>
    /// RemindFrequency Property name
    /// </summary>
    public const string RemindFrequencyPropertyName = "RemindFrequency";

    public int RemindFrequency
    {
      get { return (int)GetValue(RemindFrequencyProperty); }
      set { SetValue(RemindFrequencyProperty, value); }
    }

    /// <summary>
    /// RemindFrequency Property definition
    /// </summary>
    public static readonly DependencyProperty RemindFrequencyProperty = DependencyProperty.Register(
        RemindFrequencyPropertyName,
        typeof(int),
        typeof(RemindReviewBehavior),
        new PropertyMetadata(7));

    #endregion

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
        typeof(RemindReviewBehavior),
        new PropertyMetadata("Review app"));

    #endregion

    #region MaxReminders

    /// <summary>
    /// MaxReminders Property name
    /// </summary>
    public const string MaxRemindersPropertyName = "MaxReminders";

    public int MaxReminders
    {
      get { return (int)GetValue(MaxRemindersProperty); }
      set { SetValue(MaxRemindersProperty, value); }
    }

    /// <summary>
    /// MaxReminders Property definition
    /// </summary>
    public static readonly DependencyProperty MaxRemindersProperty = DependencyProperty.Register(
        MaxRemindersPropertyName,
        typeof(int),
        typeof(RemindReviewBehavior),
        new PropertyMetadata(3));

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
        typeof(RemindReviewBehavior),
        new PropertyMetadata("You have used this app a few times now, would you like to review it in the store?"));

    #endregion
  }
}
