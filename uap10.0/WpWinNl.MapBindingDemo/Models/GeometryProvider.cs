using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using WpWinNl.Behaviors;
using WpWinNl.Maps;

namespace WpWinNl.MapBindingDemo.Models
{
  public class GeometryProvider : ViewModelBase
  {
    private string _name;
    public string Name
    {
      get { return _name; }
      set { Set(() => Name, ref _name, value); }
    }

    public ICommand SelectCommand => new RelayCommand<MapSelectionParameters>(Select);

    public void Select(MapSelectionParameters parameters)
    {
      DispatcherHelper.CheckBeginInvokeOnUI(
        () => Messenger.Default.Send(new MessageDialogMessage(Name, "Selected object", "Ok", "Cancel")));
    }
  }
}