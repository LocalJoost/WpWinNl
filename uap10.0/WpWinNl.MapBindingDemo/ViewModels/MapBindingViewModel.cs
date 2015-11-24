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
            Lines = new ObservableCollection<LinearList>();
            Polygons = new ObservableCollection<LinearList>();
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

        public void DeleteAll()
        {
            Icons.Clear();
            Lines.Clear();
            Polygons.Clear();
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

        public void LoadIcons()
        {
            Icons.AddRange(PointList.GetRandomPoints(new Geopoint(area.NorthwestCorner),
              new Geopoint(area.SoutheastCorner), 50));
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

    }
}
