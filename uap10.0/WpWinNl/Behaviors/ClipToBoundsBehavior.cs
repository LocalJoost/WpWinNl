using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Behaviors
{
    /// <summary>
    /// This behavior clips display of anything that is displayed using a translation
    /// IN the associated object not to be displayed OUTSIDE that object
    /// </summary>
    public class ClipToBoundsBehavior: Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += AssociatedObjectSizeChanged;
            AssociatedObject.Loaded += AssociatedObjectLoaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= AssociatedObjectSizeChanged;
            AssociatedObject.Loaded -= AssociatedObjectLoaded;
            base.OnDetaching();
        }

        void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            SetClip();
        }

        void AssociatedObjectSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetClip();
        }

        private void SetClip()
        {
            AssociatedObject.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, 
                    AssociatedObject.ActualWidth, AssociatedObject.ActualHeight)
            };
        }
    }
}
