using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace WpWinNl.Utilities
{
  public class StorageHelper<T> where T : class
  {
    public virtual string GetDataFileName(Type t)
    {
      return string.Concat(t.Name, ".dat");
    }

     public async Task<bool> ExistsInStorage()
     {
       var datafile = await GetDataFile();
       return datafile != null;
    }

    private async Task<StorageFile> GetDataFile()
    {
      var result = await ApplicationData.Current.LocalFolder.TryGetItemAsync(GetDataFileName(typeof(T)));
      return result as StorageFile;
    }

    public async Task DeletedFromStorage()
    {
      var datafile = await GetDataFile();
      if (datafile != null)
      {
        await datafile.DeleteAsync();
      }
    }

    public async Task<T> RetrieveFromStorage()
    {
      try
      {
        var datafile = await GetDataFile();
        if (datafile != null)
        {
          using (var fileStream = await datafile.OpenSequentialReadAsync())
          {
            return SilverlightSerializer.Deserialize(fileStream.AsStreamForRead()) as T;
          }
        }
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex);
      }

      return null;
    }

    public async Task SaveToStorage(object model)
    {
      var datafile = await ApplicationData.Current.LocalFolder.CreateFileAsync(GetDataFileName(model.GetType()),
        CreationCollisionOption.ReplaceExisting);

      using (var fileStream = await datafile.OpenStreamForWriteAsync())
      {
        SilverlightSerializer.Serialize(model, fileStream);
      }
    }
  }
}
