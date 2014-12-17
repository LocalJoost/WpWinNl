using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using WpWinNl.Utilities;
using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace TestSilverlightSerializer
{
  [TestClass]
  public class TestAnimationDepProperties
  {
    [UITestMethod]
    public void TestDepProps()
    {
      //var b = new Border();
      //b.Opacity = 0.5;
      //var transform = new CompositeTransform();
      //transform.ScaleX = 0.5;
      //b.RenderTransform = transform;

      //var p1 = transform.GetDependencyPropertyName(CompositeTransform.ScaleXProperty);
      //Assert.AreEqual("ScaleX", p1);

      //var p2 = b.GetPropertyName(UIElement.OpacityProperty);
      //Assert.AreEqual("Opacity", p2);
    }
  }
}
