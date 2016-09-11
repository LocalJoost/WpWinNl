using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WpWinNl.Utilities
{
  public static class ApplicationExtensions
  {
    public static async Task<T> RetrieveFromStorage<T>(this Application app) where T : class
    {
      return await new StorageHelper<T>().RetrieveFromStorage();
    }

    public static async Task SaveToStorage(this Application app, object model)
    {
      await new StorageHelper<string>().SaveToStorage(model);
    }
  }
}
