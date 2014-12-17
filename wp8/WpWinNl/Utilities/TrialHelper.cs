namespace WpWinNl.Utilities
{
  /// <summary>
  /// Helper to see if your app is running in trial mode. 
  /// Add TRIAL as compiler directive to fake it when debugging.
  /// </summary>
  public static class TrialHelper
  {
    /// <summary>
    /// Fakes the trial mode... WARNING!!! Remove this calls to this method when publishing!!
    /// </summary>
    public static void FakeTrial()
    {
      IsTrial = true;
    }

    public static bool IsTrial { get; private set; }

    static TrialHelper()
    {
#if TRIAL
            // return true if debugging with trial enabled (DebugTrial configuration is active)
            IsTrial = true;
#else
      var license = new Microsoft.Phone.Marketplace.LicenseInformation();
      IsTrial = license.IsTrial();
#endif
    }
  }
}
