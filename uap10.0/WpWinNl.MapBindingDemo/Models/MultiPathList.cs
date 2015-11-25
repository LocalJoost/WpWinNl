using System.Collections.Generic;
using Windows.Devices.Geolocation;

namespace WpWinNl.MapBindingDemo.Models
{
    public class MultiPathList : GeometryProvider
    {
        public MultiPathList()
        {
            Paths = new List<Geopath>();
        }

        public List<Geopath> Paths { get; set; }

        public static List<MultiPathList> GetMultiPolygons()
        {
            var paths = new List<MultiPathList>
          {
            new MultiPathList
            {
                Name = "MultiArea 1",
                Paths = new List<Geopath>
                {
                   new Geopath( new[]
                   {
                     new BasicGeoposition{Latitude = 52.1840454731137, Longitude = 5.40299842134118},
                     new BasicGeoposition{Latitude = 52.182151498273, Longitude = 5.40619041770697},
                     new BasicGeoposition{Latitude = 52.1841113548726, Longitude = 5.40994542650878},
                     new BasicGeoposition{Latitude = 52.1861041523516, Longitude = 5.40627088397741}
                   }),
                   new Geopath( new[]
                   {
                     new BasicGeoposition{Latitude = 52.184210177511, Longitude = 5.40817516855896},
                     new BasicGeoposition{Latitude = 52.185066556558, Longitude = 5.40637808851898},
                     new BasicGeoposition{Latitude = 52.1842925716192, Longitude = 5.4054393991828},
                     new BasicGeoposition{Latitude = 52.1834195964038, Longitude = 5.40739741176367}
                   }),

                   new Geopath( new[]
                   {
                     new BasicGeoposition{Latitude = 52.1840454731137, Longitude = 5.40525156073272},
                     new BasicGeoposition{Latitude = 52.1835678722709, Longitude = 5.40594893507659},
                     new BasicGeoposition{Latitude = 52.183287832886, Longitude = 5.40702181868255},
                     new BasicGeoposition{Latitude = 52.1827772911638, Longitude = 5.40632452815771},
                     new BasicGeoposition{Latitude = 52.1833372861147, Longitude = 5.40476876311004},
                    }),

                   new Geopath( new[]
                   {
                     new BasicGeoposition{Latitude = 52.1822502370924, Longitude = 5.40286439470947},
                     new BasicGeoposition{Latitude = 52.1816079318523, Longitude = 5.404581008479},
                     new BasicGeoposition{Latitude = 52.1804222278297, Longitude = 5.4033740144223},
                     new BasicGeoposition{Latitude = 52.1808504592627, Longitude = 5.40155011229217},
                   }),

                   new Geopath( new[]
                   {
                     new BasicGeoposition{Latitude = 52.1854454185814, Longitude = 5.40332045406103},
                     new BasicGeoposition{Latitude = 52.1859065070748, Longitude = 5.40417867712677},
                     new BasicGeoposition{Latitude = 52.185066556558, Longitude = 5.40525156073272},
                     new BasicGeoposition{Latitude = 52.1845560148358, Longitude = 5.4044737201184},
                     new BasicGeoposition{Latitude = 52.1842431183904, Longitude = 5.40358859114349}
                   }),
                }
              },
              new MultiPathList
              {
                Name = "Smiley (MultiArea 2)",
                Paths = new List<Geopath>
                {
                   new Geopath( new[]
                   {
                     new BasicGeoposition{Latitude = 52.1787753514946, Longitude = 5.40471511892974},
                     new BasicGeoposition{Latitude = 52.1801093313843, Longitude = 5.40570753626525},
                     new BasicGeoposition{Latitude = 52.1801258437335, Longitude = 5.40860432200134},
                     new BasicGeoposition{Latitude = 52.1789400558919, Longitude = 5.4108305554837},
                     new BasicGeoposition{Latitude = 52.1772930957377, Longitude = 5.40975767187774},
                     new BasicGeoposition{Latitude = 52.1764037758112, Longitude = 5.40750461630523},
                     new BasicGeoposition{Latitude = 52.1769636869431, Longitude = 5.40490287356079},
                   }),
                   new Geopath( new[]
                   {
                     new BasicGeoposition{Latitude = 52.1794340852648, Longitude = 5.40608304552734},
                     new BasicGeoposition{Latitude = 52.1796646714211, Longitude = 5.40664630942047},
                     new BasicGeoposition{Latitude = 52.1794670261443, Longitude = 5.40712910704315},
                     new BasicGeoposition{Latitude = 52.1791540458798, Longitude = 5.40696817450225},
                     new BasicGeoposition{Latitude = 52.1791705582291, Longitude = 5.40624397806823},
                   }),
                   new Geopath( new[]
                   {
                     new BasicGeoposition{Latitude = 52.1787423267961, Longitude = 5.40938216261566},
                     new BasicGeoposition{Latitude = 52.1788741741329, Longitude = 5.40879207663238},
                     new BasicGeoposition{Latitude = 52.1785611938685, Longitude = 5.40838974528015},
                     new BasicGeoposition{Latitude = 52.1781330462545, Longitude = 5.40873843245208},
                     new BasicGeoposition{Latitude = 52.1782977506518, Longitude = 5.40948953479528},
                   }),
                   new Geopath( new[]
                   {
                     new BasicGeoposition{Latitude = 52.1782483812422, Longitude = 5.4054393991828},
                     new BasicGeoposition{Latitude = 52.1770131401718, Longitude = 5.40769237093627},
                     new BasicGeoposition{Latitude = 52.1769967116416, Longitude = 5.40645855478942},
                     new BasicGeoposition{Latitude = 52.1772765833884, Longitude = 5.40551978163421},
                     new BasicGeoposition{Latitude = 52.1778365783393, Longitude = 5.40530528873205},
                   }),
                 }
              }

            };

            return paths;
        }
    }
}
