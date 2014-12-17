using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WpWinNl.Utilities;

namespace WpWinNl.MapBindingDemo.ViewModels
{
  public class MapBindingViewModel : ViewModelBase
  {
    public MapBindingViewModel()
    {
      Icons = new ObservableCollection<PointList>();
      Lines = new ObservableCollection<PointList>();
      Polygons = new ObservableCollection<PointList>();
    }


    public void ShowArea()
    {
      Area = new GeoboundingBox(
        new BasicGeoposition { Latitude = 52.1848798915744, Longitude = 5.39574773982167 },
        new BasicGeoposition { Latitude = 52.1779848542064, Longitude = 5.40379419922829 });
    }

    public ICommand ShowAreaCommand
    {
      get
      {
        return new RelayCommand(
            ShowArea);
      }
    }

    public ObservableCollection<PointList> Icons { get; set; }
    public ObservableCollection<PointList> Lines { get; set; }
    public ObservableCollection<PointList> Polygons { get; set; }

    public ICommand DeleteAllCommand
    {
      get
      {
        return new RelayCommand(
            () =>
            {
              Icons.Clear();
              Lines.Clear();
              Polygons.Clear();
            });
      }
    }

    private GeoboundingBox area;

    public GeoboundingBox Area
    {
      get { return area; }
      set
      {
        if (area != value)
        {
          area = value;
          RaisePropertyChanged(() => Area);
        }
      }
    }

    public ICommand LoadIconsCommand
    {
      get
      {
        return new RelayCommand(
            () =>
            {
              Icons.Clear();
              Icons.AddRange(PointList.GetRandomPoints(new Geopoint(area.NorthwestCorner),
                new Geopoint(area.SoutheastCorner), 50));
            });
      }
    }

    public ICommand LoadLinesCommand
    {
      get
      {
        return new RelayCommand(
            () =>
            {
              Lines.Clear();
              Lines.AddRange(PointList.GetLines());
            });
      }
    }

    public ICommand LoadPolygonsCommand
    {
      get
      {
        return new RelayCommand(
            () =>
            {
              Polygons.Clear();
              Polygons.AddRange(PointList.GetAreas());
            });
      }
    }

    public ICommand SelectCommand
    {
      get
      {
        return new RelayCommand(
            () =>
            {
              Polygons.Clear();
              Polygons.AddRange(PointList.GetAreas());
            });
      }
    }
  }
}
