using System.Windows;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// Extension method to make StartStoryBoard behavior compatible with Windows Phone
  /// </summary>
  public static class ResourceDictionaryExtensions
  {
    public static bool ContainsKey(this ResourceDictionary resourceDictionary, string keyToFind)
    {
      foreach (string key in resourceDictionary)
      {
        if (key == keyToFind) return true;
      }
      return false;
    }
  }
}
