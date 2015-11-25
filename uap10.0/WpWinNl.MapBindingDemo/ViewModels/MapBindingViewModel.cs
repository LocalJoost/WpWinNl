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
      Center = new Geopoint(new BasicGeoposition {Latitude = 0, Longitude = 0});
      ZoomLevel = 0;
    }


    public void ShowArea()
    {
      ZoomLevel = 16;
      Center = new Geopoint(new BasicGeoposition
      {
        Latitude = 52.1814323728904, Longitude = 5.39977096952498
      });
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
      set { Set(() => Area, ref _area, value); }
    }

    private double _zoomlevel;
    public double ZoomLevel
    {
      get { return _zoomlevel; }
      set { Set(() => ZoomLevel, ref _zoomlevel, value); }
    }

    private Geopoint _center;
    public Geopoint Center
    {
      get { return _center; }
      set { Set(() => Center, ref _center, value); }
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
