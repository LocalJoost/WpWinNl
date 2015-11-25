using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using GalaSoft.MvvmLight;
using WpWinNl.MapBindingDemo.Models;
using WpWinNl.Utilities;

namespace WpWinNl.MapBindingDemo.ViewModels
{
  public class MapBindingViewModel : ViewModelBase
  {
    public MapBindingViewModel()
    {
      Icons = new ObservableCollection<PointList>();
      Lines = new ObservableCollection<LinearList>();
      Polygons = new ObservableCollection<LinearList>();
      MultiPolygons = new ObservableCollection<MultiPathList>();
    }


    public void ShowArea()
    {
      Area = new GeoboundingBox(
        new BasicGeoposition { Latitude = 52.1848798915744, Longitude = 5.39574773982167 },
        new BasicGeoposition { Latitude = 52.1779848542064, Longitude = 5.40379419922829 });
    }

    public ObservableCollection<PointList> Icons { get; set; }
    public ObservableCollection<LinearList> Lines { get; set; }
    public ObservableCollection<LinearList> Polygons { get; set; }
    public ObservableCollection<MultiPathList> MultiPolygons { get; set; }

    public void DeleteAll()
    {
      Icons.Clear();
      Lines.Clear();
      Polygons.Clear();
      MultiPolygons.Clear();
    }

    private GeoboundingBox _area;

    public GeoboundingBox Area
    {
      get { return _area; }
      set
      {
        if (_area != value)
        {
          _area = value;
          RaisePropertyChanged(() => Area);
        }
      }
    }

    public void LoadIcons()
    {
      Icons.AddRange(PointList.GetRandomPoints(new Geopoint(_area.NorthwestCorner),
        new Geopoint(_area.SoutheastCorner), 50));
    }

    public void LoadLines()
    {
      Lines.Clear();
      Lines.AddRange(LinearList.GetLines());
    }

    public void LoadPolygons()
    {
      Polygons.Clear();
      Polygons.AddRange(LinearList.GetAreas());
    }

    public void LoadMultiPolygons()
    {
      MultiPolygons.Clear();
      MultiPolygons.AddRange(MultiPathList.GetMultiPolygons());
    }
  }
}
