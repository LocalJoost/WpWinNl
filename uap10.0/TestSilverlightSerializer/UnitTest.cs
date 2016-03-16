using System;
using System.Diagnostics;
using System.Reflection;
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


    [TestMethod]
    public void TestNullBoolean1()
    {
      bool? p = null;
      var serialized = SilverlightSerializer.Serialize(p);
      var deserialized = SilverlightSerializer.Deserialize(serialized) as bool?;
      Assert.IsNull(deserialized);
    }

    [TestMethod]
    public void TestNullBoolean2()
    {
      bool? p = false;
      var serialized = SilverlightSerializer.Serialize(p);
      var deserialized = SilverlightSerializer.Deserialize(serialized) as bool?;
      Assert.IsFalse(deserialized.Value);
    }


    [TestMethod]
    public void TestNullBoolean3()
    {
      bool? p = true;
      var serialized = SilverlightSerializer.Serialize(p);
      var deserialized = SilverlightSerializer.Deserialize(serialized) as bool?;
      Assert.IsTrue(deserialized.Value);
    }

    [TestMethod]
    public void TestDoNotSerialize()
    {
      var p  = new TestSerializable {  ToSerialize = "serialize this", ToSkip = "skip this"};
      var serialized = SilverlightSerializer.Serialize(p);
      var deserialized = SilverlightSerializer.Deserialize(serialized) as TestSerializable;
      Assert.AreEqual(p.ToSerialize, deserialized.ToSerialize);
      Assert.IsNull(deserialized.ToSkip);
    }


    [TestMethod]
    public void TestGetAttributeNotToSerialize()
    {
      var p = new TestSerializable { ToSerialize = "serialize this", ToSkip = "skip this" };
      var propInfo = p.GetType().GetRuntimeProperty("ToSkip");
      Assert.IsNotNull(propInfo.GetCustomAttribute<DoNotSerialize>());
    }

    [TestMethod]
    public void TestGetAttributeToSerialize()
    {
      var p = new TestSerializable { ToSerialize = "serialize this", ToSkip = "skip this" };
      var propInfo = p.GetType().GetRuntimeProperty("ToSerialize");
      Assert.IsNull(propInfo.GetCustomAttribute<DoNotSerialize>());
    }


    [TestMethod]
    public void TestComplexSerializable()
    {
      var p = new TestComplexSerializable {String1 = "Hi there", String2 = "bye"};
      var g1 = new GenericThingy<int> { SomethingElse = 21.0, Whatever = 1};
      var g2 = new GenericThingy<int> { SomethingElse = 22.0, Whatever = 2};
      p.GenericPropery.Add(g1);
      p.GenericPropery.Add(g2);

      var serialized = SilverlightSerializer.Serialize(p);
      foreach (var a in SilverlightSerializer.RdXmlEntries)
      {
        Debug.WriteLine(a);
      }
    }
  }

}
