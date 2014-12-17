namespace WpWinNl.Behaviors
{
  /// <summary>
  /// Simple data class to store data in used by RemindReviewBehavior
  /// </summary>
  public class RemindData
  {
    public int Starts { get; set; }

    public bool Reviewed { get; set; }

    public bool Refused { get; set; }
  }
}
