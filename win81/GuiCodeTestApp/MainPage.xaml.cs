using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using WpWinNl.Utilities;

namespace GuiCodeTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
          button.RenderTransform = new CompositeTransform();
          var storyboard = new Storyboard();
          var list = new List<Point>();
          list.Add(new Point(0, 0));
          list.Add(new Point(1000, 1000));
          list.Add(new Point(1000, 500));

          storyboard.AddWayPointAnimation(button, list, 500);

          storyboard.Begin();
        }
    }
}
