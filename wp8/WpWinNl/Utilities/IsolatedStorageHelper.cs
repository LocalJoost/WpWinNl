using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// Helper class to write stuff to Isolates Storage using SilverlightSerializer 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class IsolatedStorageHelper<T> where T : class
  {
    public virtual string GetDataFileName(Type t)
    {
      return string.Concat(t.Name, ".dat");
    }

    public bool ExistsInStorage()
    {
      using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
      {
        var dataFileName = GetDataFileName(typeof(T));
        return appStorage.FileExists(dataFileName);
      }
    }

    public void DeletedFromStorage()
    {
      var dataFileName = GetDataFileName(typeof(T));
      using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
      {
        if (appStorage.FileExists(dataFileName))
        {
          appStorage.DeleteFile(dataFileName);
        }
      }
    }

    public T RetrieveFromStorage()
    {
      using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
      {
        var dataFileName = GetDataFileName(typeof(T));
        if (appStorage.FileExists(dataFileName))
        {
          using (var iss = appStorage.OpenFile(dataFileName, FileMode.Open))
          {
            try
            {
              return SilverlightSerializer.Deserialize(iss) as T;
            }
            catch (Exception e)
            {
              System.Diagnostics.Debug.WriteLine(e);
            }
          }
        }
      }

      return null;
    }

    public void SaveToStorage(object model)
    {
      var dataFileName = GetDataFileName(model.GetType());
      using (var appStorage = IsolatedStorageFile.GetUserStoreForApplication())
      {
        if (appStorage.FileExists(dataFileName))
        {
          appStorage.DeleteFile(dataFileName);
        }

        using (var iss = appStorage.CreateFile(dataFileName))
        {
          SilverlightSerializer.Serialize(model, iss);
        }
      }
    }
  }
}
