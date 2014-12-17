using System.ComponentModel;

namespace WpWinNl.BaseModels
{
  /// <summary>
  /// Interface for a viewmodel that is able to handle a backkeypress
  /// </summary>
  public interface IBackKeyPressHandler
  {
    void OnBackKeyPress(CancelEventArgs e);
  }
}
