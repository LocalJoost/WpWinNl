using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Windows.Devices.Geolocation;
using Windows.Foundation;
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
      LabeledIcons = new ObservableCollection<PointList>();
      Lines = new ObservableCollection<LinearList>();
      Polygons = new ObservableCollection<LinearList>();
      MultiPolygons = new ObservableCollection<MultiPathList>();
      Flags = new ObservableCollection<FlagList>();
      Center = new Geopoint(new BasicGeoposition { Latitude = 0, Longitude = 0 });
      ZoomLevel = 0;
    }


    public void ShowArea()
    {
      ZoomLevel = 16;
      Center = new Geopoint(new BasicGeoposition
      {
        Latitude = 52.1814323728904,
        Longitude = 5.39977096952498
      });
    }

    public ObservableCollection<PointList> Icons { get; set; }
    public ObservableCollection<PointList> LabeledIcons { get; set; }
    public ObservableCollection<LinearList> Lines { get; set; }
    public ObservableCollection<LinearList> Polygons { get; set; }
    public ObservableCollection<MultiPathList> MultiPolygons { get; set; }

    public ObservableCollection<FlagList> Flags { get; set; }


    public void DeleteAll()
    {
      Icons.Clear();
      LabeledIcons.Clear();
      Flags.Clear();
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

    public void LoadLabeledIcons()
    {
      string[] cheeses = { "Gorgonozla", "Cacio magno", "Taleggio", "Tosèla del Primiero", "Ubriaco", "Toma lucana", "Spalèm", "Salignon", "Morello", "Falagnone" };
      var icons = PointList.GetRandomPoints(new Geopoint(_area.NorthwestCorner),
        new Geopoint(_area.SoutheastCorner), 50).ToList();
      for (var i = 0; i < icons.Count(); i++)
      {
        icons[i].Name = cheeses[i % 10];
      }
      LabeledIcons.AddRange(icons);
    }

    public void LoadFlags()
    {
      Flags.AddRange(FlagList.GetRandomFlags(new Geopoint(_area.NorthwestCorner),
        new Geopoint(_area.SoutheastCorner), 50));
    }

    public void ChangeToPirate()
    {
      foreach (var flag in Flags.Where(p => p.Name == "Arrr!").ToList())
      {
        flag.IsVisible = false;
      }

      var r = new Random(DateTime.Now.Millisecond * 2);
      var flagIdx = (int)Math.Round(r.NextDouble() * 5);
      var flagName = FlagList.Countries[flagIdx];

      foreach (var flag in Flags.Where(p => p.Name == flagName).ToList())
      {
        flag.Name = "Arrr!";
        flag.Icon = new Uri("ms-appx:///Assets/JollyRoger.png");
        flag.AnchorPoint = new Point(0.5, 1);
      }
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
