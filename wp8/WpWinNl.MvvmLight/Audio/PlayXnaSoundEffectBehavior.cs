using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using WpWinNl.Behaviors;

namespace WpWinNl.Audio
{
  public class PlayXnaSoundEffectBehavior : SafeBehavior<FrameworkElement>
  {
    public PlayXnaSoundEffectBehavior()
    {
      ListenToPageBackEvent = true;
    }

    protected override void OnSetup()
    {
      base.OnSetup();
      Messenger.Default.Register<PlaySoundEffectMessage>(this, DoPlaySoundEffect);
    }

    private void DoPlaySoundEffect(PlaySoundEffectMessage message)
    {
      if (SoundName == message.SoundName)
      {
        Dispatcher.BeginInvoke(() =>
                                 {
                                   using (var stream = TitleContainer.OpenStream(SoundFileLocation))
                                   {
                                     if (stream != null)
                                     {
                                       var effect = SoundEffect.FromStream(stream);
                                       FrameworkDispatcher.Update();
                                       effect.Play();
                                     }
                                   }
                                 });
      }
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
      typeof(PlayXnaSoundEffectBehavior),
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
        typeof(PlayXnaSoundEffectBehavior),
        new PropertyMetadata(default(string)));

    #endregion
  }
}
