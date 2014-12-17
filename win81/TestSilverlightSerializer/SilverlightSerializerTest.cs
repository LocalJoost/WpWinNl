using System;
using System.IO;
using Windows.Devices.Geolocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WpWinNl.Utilities;

namespace TestSilverlightSerializer
{
  [TestClass]
  public class SilverlightSerializerTest
  {
    [TestMethod]
    public void TestSimpleSerialize()
    {
      var bla = new ReflectionTestSubject();
      bla.MyList.Add("hallo");
      bla.MyList.Add("hallo2");
      bla.MyList.Add("hallo3");

      bla.MyString = "hoi";

      var m = new MemoryStream();
      SilverlightSerializer.Serialize(bla, m);
      m.Seek(0, SeekOrigin.Begin);

      var bla2 = SilverlightSerializer.Deserialize(m) as ReflectionTestSubject;

      Assert.AreEqual(bla.MyString, bla2.MyString);
      Assert.AreEqual(bla.MyList.Count, bla2.MyList.Count);

    }

    [TestMethod]
    public void TestDateTimeOffset1()
    {
      var testDate = DateTimeOffset.Now;
      TestDateTimeOffsetEqualness(testDate);

    }

    [TestMethod]
    public void TestDateTimeOffset2()
    {
      var testDate = new DateTimeOffset(DateTime.UtcNow);

      TestDateTimeOffsetEqualness(testDate);
    }

    [TestMethod]
    public void TestDateTimeOffset3()
    {
      var testObj = new ComplexTestObject();
      var d1 = new DateTimeOffset(DateTime.UtcNow);
      var d2 = new DateTimeOffset(DateTime.Now);

      testObj.Alist.Add(d1);
      testObj.Alist.Add(d2);
      testObj.DateTime1 = d1;
      testObj.DateTime2 = d2;

      var m = new MemoryStream();
      SilverlightSerializer.Serialize(testObj, m);
      m.Seek(0, SeekOrigin.Begin);

      var testObj2 = (ComplexTestObject)SilverlightSerializer.Deserialize(m);
      Assert.AreEqual(testObj2.Alist[0], d1);
      Assert.AreEqual(testObj2.Alist[1], d2);
      Assert.AreEqual(testObj2.DateTime1, d1);
      Assert.AreEqual(testObj2.DateTime2, d2);
    }

    private void TestDateTimeOffsetEqualness(DateTimeOffset testDate)
    {
      var m = new MemoryStream();
      SilverlightSerializer.Serialize(testDate, m);
      m.Seek(0, SeekOrigin.Begin);

      var testDate2 = (DateTimeOffset)SilverlightSerializer.Deserialize(m);

      Assert.AreEqual(testDate, testDate2);
    }

    [TestMethod]
    public void TestDateTime()
    {
      var testDate = DateTime.Now;

      var m = new MemoryStream();
      SilverlightSerializer.Serialize(testDate, m);
      m.Seek(0, SeekOrigin.Begin);

      var testDate2 = (DateTime)SilverlightSerializer.Deserialize(m);

      Assert.AreEqual(testDate, testDate2);
    }

    [TestMethod]
    public void TestBasicPosition()
    {
      var testobj = new BasicGeoposition { Latitude = 52.155915, Longitude = 5.390376, Altitude = 10.0 };

      var m = new MemoryStream();
      SilverlightSerializer.Serialize(testobj, m);
      m.Seek(0, SeekOrigin.Begin);

      var testObj2 = (BasicGeoposition)SilverlightSerializer.Deserialize(m);

      Assert.AreEqual(testobj, testObj2);
    }

    [TestMethod]
    public void TestObjectWithSimpleProperties()
    {
      var testobj = new SimpleTestObject { Prop1 = 1, Prop2 = "BLABLA", Prop3 = 21.0 };

      var m = new MemoryStream();
      SilverlightSerializer.Serialize(testobj, m);
      m.Seek(0, SeekOrigin.Begin);

      var testObj2 = (SimpleTestObject)SilverlightSerializer.Deserialize(m);

      Assert.AreEqual(testobj.Prop1, testObj2.Prop1);
      Assert.AreEqual(testobj.Prop2, testObj2.Prop2);
      Assert.AreEqual(testobj.Prop3, testObj2.Prop3);
    }
  }
}
