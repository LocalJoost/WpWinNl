using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using WpWinNl.Behaviors;
using WpWinNl.Maps;

namespace WpWinNl.MapBindingDemo.ViewModels
{
  public class GeometryProvider : ViewModelBase
  {
    public string Name { get; set; }

    public ICommand SelectCommand
    {
      get
      {
        return new RelayCommand<MapSelectionParameters>(
          (p) => DispatcherHelper.CheckBeginInvokeOnUI(() =>Messenger.Default.Send(new MessageDialogMessage(Name, "Selected object", "Ok", "Cancel"))));
      }
    }
  }
}