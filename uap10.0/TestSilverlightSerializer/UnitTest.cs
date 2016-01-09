using System;
using Windows.Devices.Geolocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WpWinNl.Utilities;

namespace TestSilverlightSerializer
{
  [TestClass]
  public class TestGeopoint
  {
    [TestMethod]
    public void TestGeopoint1()
    {
      var p = new Geopoint( new BasicGeoposition {Latitude = 5,Longitude = 52} );
      var serialized = SilverlightSerializer.Serialize(p);
      var deserialized = SilverlightSerializer.Deserialize(serialized) as Geopoint;
      Assert.IsTrue(deserialized.Position.Longitude == p.Position.Longitude && 
        deserialized.Position.Latitude == p.Position.Latitude);
    }

  }
}
