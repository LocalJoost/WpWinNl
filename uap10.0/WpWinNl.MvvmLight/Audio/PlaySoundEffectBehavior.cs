using System;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Audio
{
  public class PlaySoundEffectBehavior : Behavior<MediaElement>
  {

    private bool doRepeat;
    public PlaySoundEffectBehavior()
    {
#if WINDOWS_PHONE
      ListenToPageBackEvent = true;
#endif

    }
    protected override void OnAttached()
    {
      Messenger.Default.Register<PlaySoundEffectMessage>(this, DoPlaySoundFile);
      AssociatedObject.IsHitTestVisible = false;
      AssociatedObject.AutoPlay = false;
#if WINDOWS_PHONE
      var soundUri = new Uri(SoundFileLocation, UriKind.Relative);
#else
      var soundUri = new Uri(AssociatedObject.BaseUri, SoundFileLocation);
#endif

      AssociatedObject.Source = soundUri;
      AssociatedObject.Position = TimeSpan.FromSeconds(0);

      AssociatedObject.MediaEnded += AssociatedObjectMediaEnded;

    }

    private void DoPlaySoundFile(PlaySoundEffectMessage message)
    {
      if (SoundName == message.SoundName)
      {
        if (message.Start)
        {
          if (Repeat)
          {
            doRepeat = true;
          }
          AssociatedObject.Position = TimeSpan.FromSeconds(0);
          AssociatedObject.Play();
        }
        else
        {
          doRepeat = false;
        }
      }
    }

    private void AssociatedObjectMediaEnded(object sender, RoutedEventArgs e)
    {
      //AssociatedObject.Stop();
      AssociatedObject.Position = TimeSpan.FromSeconds(0);
      if (doRepeat)
      {
        AssociatedObject.Play();
      }
      else
      {
        //AssociatedObject.Stop();
      }
    }


    protected override void OnDetaching()
    {
      Messenger.Default.Unregister(this);
      AssociatedObject.MediaEnded -= AssociatedObjectMediaEnded;
    }

    #region SoundFileLocation

    public const string SoundFileLocationPropertyName = "SoundFileLocation";

    public string SoundFileLocation
    {
      get { return (string)GetValue(SoundFileLocationProperty); }
      set { SetValue(SoundFileLocationProperty, value); }
    }

    public static readonly DependencyProperty SoundFileLocationProperty = DependencyProperty.Register(
      SoundFileLocationPropertyName,
      typeof(string),
      typeof(PlaySoundEffectBehavior),
      new PropertyMetadata(String.Empty));

    #endregion

    #region SoundName

    /// <summary>
    /// SoundName Property name
    /// </summary>
    public const string SoundNamePropertyName = "SoundName";

    public string SoundName
    {
      get { return (string)GetValue(SoundNameProperty); }
      set { SetValue(SoundNameProperty, value); }
    }

    /// <summary>
    /// SoundName Property definition
    /// </summary>
    public static readonly DependencyProperty SoundNameProperty = DependencyProperty.Register(
        SoundNamePropertyName,
        typeof(string),
        typeof(PlaySoundEffectBehavior),
        new PropertyMetadata(default(string)));

    #endregion

    #region Repeat

    /// <summary>
    /// Repeat Property name
    /// </summary>
    public const string RepeatPropertyName = "Repeat";

    public bool Repeat
    {
      get { return (bool)GetValue(RepeatProperty); }
      set { SetValue(RepeatProperty, value); }
    }

    /// <summary>
    /// Repeat Property definition
    /// </summary>
    public static readonly DependencyProperty RepeatProperty = DependencyProperty.Register(
        RepeatPropertyName,
        typeof(bool),
        typeof(PlaySoundEffectBehavior),
        new PropertyMetadata(default(bool)));


    #endregion
  }
}