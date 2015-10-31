using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Behaviors
{
  public class ScreenshotBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      Messenger.Default.Register<ScreenshotMessage>(this, ProcessMessage );
      base.OnAttached();
    }

    private async void ProcessMessage(ScreenshotMessage m)
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

    private async Task DoRender(ScreenshotCallback callback)
    {
      var renderTargetBitmap = new RenderTargetBitmap();
      await renderTargetBitmap.RenderAsync(AssociatedObject);
      var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

      var storageFile = await KnownFolders.SavedPictures.CreateFileAsync(string.Concat(Prefix, ".png"),
        CreationCollisionOption.GenerateUniqueName);
      using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
      {
        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
        encoder.SetPixelData(
          BitmapPixelFormat.Bgra8,
          BitmapAlphaMode.Ignore,
          (uint) renderTargetBitmap.PixelWidth,
          (uint) renderTargetBitmap.PixelHeight, 96d, 96d,
          pixelBuffer.ToArray());

        await encoder.FlushAsync();
        if (callback != null)
        {
          callback(storageFile);
        }
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

    #region Prefix

    /// <summary>
    /// Prefix Property name
    /// </summary>
    public const string PrefixPropertyName = "Prefix";

    public string Prefix
    {
      get { return (string)GetValue(PrefixProperty); }
      set { SetValue(PrefixProperty, value); }
    }

    /// <summary>
    /// Prefix Property definition
    /// </summary>
    public static readonly DependencyProperty PrefixProperty = DependencyProperty.Register(
        PrefixPropertyName,
        typeof(string),
        typeof(ScreenshotBehavior),
        new PropertyMetadata("Screenshot"));

    #endregion

  }
}
