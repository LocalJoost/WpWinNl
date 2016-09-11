using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Behaviors
{
  public class ScreenshotToStreamBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      Messenger.Default.Register<ScreenshotToStreamMessage>(this, ProcessMessage);
      base.OnAttached();
    }

    private async void ProcessMessage(ScreenshotToStreamMessage m)
    {
      if (m.Target != null && Target != null)
      {
        if (m.Target.Equals(Target))
        {
          await DoRender(m.Callback);
        }
      }
      else
      {
        await DoRender(m.Callback);
      }
    }

    private async Task DoRender(ScreenshotToStreamCallback callback)
    {
      var renderTargetBitmap = new RenderTargetBitmap();
      await renderTargetBitmap.RenderAsync(AssociatedObject);
      var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

      var stream = new InMemoryRandomAccessStream();

      var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
      encoder.SetPixelData(
        BitmapPixelFormat.Bgra8,
        BitmapAlphaMode.Ignore,
        (uint)renderTargetBitmap.PixelWidth,
        (uint)renderTargetBitmap.PixelHeight, 96d, 96d,
        pixelBuffer.ToArray());

      await encoder.FlushAsync();
      stream.Seek(0);

      if (callback != null)
      {
        callback(stream);
      }
    }

    #region Target

    /// <summary>
    /// Target Property name
    /// </summary>
    public const string TargetPropertyName = "Target";

    public object Target
    {
      get { return GetValue(TargetProperty); }
      set { SetValue(TargetProperty, value); }
    }

    /// <summary>
    /// Target Property definition
    /// </summary>
    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
        TargetPropertyName,
        typeof(object),
        typeof(ScreenshotBehavior),
        new PropertyMetadata(default(object)));

    #endregion
  }
}
