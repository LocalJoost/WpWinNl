using System;
using System.IO;
using System.Windows;
using Microsoft.Phone.Shell;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// Some extensions method that allow serializing and deserializing
  /// a model to and from phone state and/or isolated storage
  /// </summary>
  public static class ApplicationExtensions
  {
    private static string GetModelKey(Type t)
    {
      return t.Name;
    }

    public static T RetrieveFromIsolatedStorage<T>(this Application app) where T : class
    {
      return new IsolatedStorageHelper<T>().RetrieveFromStorage();
    }

    public static void SaveToIsolatedStorage(this Application app, object model)
    {
      new IsolatedStorageHelper<string>().SaveToStorage(model);
    }

    public static void SaveToPhoneState(this Application app, object model)
    {
      var modelKey = GetModelKey(model.GetType());
      if (PhoneApplicationService.Current.State.ContainsKey(modelKey))
      {
        PhoneApplicationService.Current.State.Remove(modelKey);
      }

      using (var ms = new MemoryStream())
      {
        SilverlightSerializer.Serialize(model, ms);
        PhoneApplicationService.Current.State.Add(modelKey, ms.GetBuffer());
      }
    }

    public static T RetrieveFromPhoneState<T>(this Application app) where T : class
    {
      var modelKey = GetModelKey(typeof(T));
      if (PhoneApplicationService.Current.State.ContainsKey(modelKey))
      {
        using (var ms = new MemoryStream(PhoneApplicationService.Current.State[modelKey] as byte[]))
        {
          return SilverlightSerializer.Deserialize(ms) as T;
        }
      }

      return null;
    }
  }
}
