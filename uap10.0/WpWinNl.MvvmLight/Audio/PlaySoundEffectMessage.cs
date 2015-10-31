namespace WpWinNl.Audio
{
  public class PlaySoundEffectMessage
  {
    public PlaySoundEffectMessage(string soundName, bool start = true)
    {
      SoundName = soundName;
      Start = start;
    }

    public string SoundName { get; private set; }

    public bool Start { get; private set; }
  }
}
