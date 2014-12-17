using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Windows.UI.Xaml;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;

namespace TestSilverlightSerializer
{
  [TestClass]
  public class PartialCodeTests
  {
    [TestMethod]
    public void TestMethod1()
    {
      var myObject = new ReflectionTestSubject();
      var allproperties = myObject.GetType().GetRuntimeProperties();
      foreach (var info in allproperties)
      {
        if (ImplementsInterface(info, "IEnumerable"))
        {
          Debug.WriteLine("{0} implements IEnumerable", info.Name);
        }
        else
        {
          Debug.WriteLine("{0} does not implement IEnumerable", info.Name);
        }
      }
    }

    private bool ImplementsInterface(PropertyInfo info, string interfaceName)
    {
      var result = info.PropertyType.GetTypeInfo().ImplementedInterfaces.Any(p => p.Name == interfaceName);
      return result;
    }

    [TestMethod]
    public void TestConstructor()
    {
      var itemType = typeof(ReflectionTestSubject);

      var constructorInfo = itemType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(p => !p.GetParameters().Any());

      var result =  constructorInfo != null ? constructorInfo.Invoke(new object[] { }) : null;

      var a = 1;

    }


    [TestMethod]
    public void TestCulture()
    {
      var t = new CultureInfo("");
      Assert.IsNotNull(t);
    }
  }
}
