using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Devices;
using Microsoft.Phone.Controls;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A behavior that shows a camera view on the background of a panel
  /// </summary>
  public class CameraViewBackgroundBehavior : SafeBehavior<Panel>
  {

    private PhotoCamera camera;
    private VideoBrush backgroundBrush;

    public CameraViewBackgroundBehavior()
    {
      ListenToPageBackEvent = true;
    }

    protected override void OnSetup()
    {
      if (camera == null)
      {
        camera = new PhotoCamera();
        ParentPage.OrientationChanged += ParentPageOrientationChanged;
      }

      // Create a video brush with the right parameters
      backgroundBrush = new VideoBrush
                          {
                            Stretch = Stretch.UniformToFill,
                            AlignmentX = AlignmentX.Left,
                            AlignmentY = AlignmentY.Top
                          };

      // Set the video brush to the background of the panel 
      // and and do an initial display
      AssociatedObject.Background = backgroundBrush;
      backgroundBrush.SetSource(camera);
      SetVideoOrientation(ParentPage.Orientation);
    }

    protected override void OnCleanup()
    {
      ParentPage.OrientationChanged -= ParentPageOrientationChanged;
      camera.Dispose();
      camera = null;
    }

    private void ParentPageOrientationChanged(object sender, OrientationChangedEventArgs e)
    {
      SetVideoOrientation(e.Orientation);
    }

    /// <summary>
    /// Fired whe page navigation happens
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void OnParentPageNavigated(object sender, NavigationEventArgs e)
    {
      // Re-setup when this page is navigated BACK to
     if( IsNavigatingBackToBehaviorPage(e))
     {
       if (camera != null)
       {
         OnCleanup();
         OnSetup();
       }
      }
    }

    /// <summary>
    /// Sets background video brush parameters based upon page orientation
    /// </summary>
    /// <param name="orientation"></param>
    private void SetVideoOrientation(PageOrientation orientation)
    {
      System.Diagnostics.Debug.WriteLine("Switching to {0}", orientation);
      switch (orientation)
      {
        case PageOrientation.PortraitUp:
          backgroundBrush.Transform = new CompositeTransform { Rotation = 90, TranslateX = 480 };
          break;
        case PageOrientation.LandscapeLeft:
          backgroundBrush.Transform = null;
          break;
        case PageOrientation.LandscapeRight:
          if (Microsoft.Phone.Shell.SystemTray.IsVisible )
          {
            backgroundBrush.Transform = new CompositeTransform { Rotation = 180, TranslateX = 728, TranslateY = 480 };
          }
          else
          {
            backgroundBrush.Transform = new CompositeTransform { Rotation = 180, TranslateX = 800, TranslateY = 480 };
          }
          break;
      }
    }
  }
}
