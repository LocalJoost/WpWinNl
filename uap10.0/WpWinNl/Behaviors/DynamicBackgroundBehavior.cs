using System;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Xaml.Interactivity;

namespace WpWinNl.Behaviors
{
  /// <summary>
  /// A behavior that puts an image on the background of the Attached object
  /// using Bing Image Search
  /// </summary>
  public class DynamicBackgroundBehavior : Behavior<Panel>
  {
    private ImageBrush backgroundBrush;

    public DynamicBackgroundBehavior()
    {
      Opacity = 1.0;
    }

    #region SearchString
    public const string SearchStringPropertyName = "SearchString";

    /// <summary>
    /// The search string to be used on Bing Maps
    /// </summary>
    public string SearchString
    {
      get { return (string)GetValue(SearchStringProperty); }
      set { SetValue(SearchStringProperty, value); }
    }

    public static readonly DependencyProperty SearchStringProperty = DependencyProperty.Register(
        SearchStringPropertyName,
        typeof(string),
        typeof(DynamicBackgroundBehavior),
        new PropertyMetadata(String.Empty, SearchStringChanged));

    public static void SearchStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var behavior = d as DynamicBackgroundBehavior;
      if (behavior != null)
      {
        behavior.StartGetFirstImage((string)e.NewValue);
      }
    }
    #endregion

    /// <summary>
    /// Bing search key
    /// </summary>
    public string BingSearchKey { get; set; }

    /// <summary>
    /// Stretch used for the image
    /// </summary>
    public Stretch Stretch { get; set; }

    /// <summary>
    /// Opacity used for the image
    /// </summary>
    public double Opacity { get; set; }

    public string Size { get; set; }

    /// <summary>
    /// Setup the behavior
    /// </summary>
    protected override void OnAttached()
    {
      backgroundBrush = new ImageBrush
      {
        Stretch = Stretch,
        Opacity = Opacity
      };

      // Set the image brush to the background of the Panel 
      AssociatedObject.Background = backgroundBrush;
    }

    /// <summary>
    /// Start the image request using Bing Serach
    /// </summary>
    /// <param name="searchString"></param>
    protected async void StartGetFirstImage(string searchString)
    {
      if (!string.IsNullOrWhiteSpace(searchString))
      {

#if WINDOWS_PHONE
        var decoded = HttpUtility.HtmlDecode(searchString);
#else
        var decoded = WebUtility.HtmlDecode(searchString);
#endif
        var queryUri =
          string.Format(
            "https://api.datamarket.azure.com/Data.ashx/Bing/Search/v1/Image?Query=%27{0}%27&$top=1&$format=Atom",
             decoded);
        var request = WebRequest.Create(queryUri) as HttpWebRequest;
        request.Headers["Authorization"] = "Basic " + Base64Encode(string.Format("{0}:{0}", BingSearchKey));
#if WINDOWS_PHONE
        var response =
  Observable.FromAsyncPattern<WebResponse>(
    request.BeginGetResponse, request.EndGetResponse)();
        response.Subscribe(WebClientOpenReadCompleted, WebClientOpenReadError);
#else
        var response = await request.GetResponseAsync();
        WebClientOpenReadCompleted(response);
#endif
      }
    }

    /// <summary>
    /// Called when image search returns
    /// </summary>
    /// <param name="result"></param>
    private void WebClientOpenReadCompleted(WebResponse result)
    {
      using (var stream = result.GetResponseStream())
      {
        using (var reader = XmlReader.Create(stream,
           new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore }))
        {
          var doc = XDocument.Load(reader);

          // Get the first image from the result
          XNamespace ns = "http://schemas.microsoft.com/ado/2007/08/dataservices";
          if (doc.Root != null)
          {
            var firstImage = doc.Root.Descendants(ns + "MediaUrl").FirstOrDefault();
            if (firstImage != null)
            {
#if WINDOWS_PHONE
              Deployment.Current.Dispatcher.BeginInvoke(() =>
              {
                CreateImage(firstImage.Value);
              });
#else
              CreateImage(firstImage.Value);
#endif

            }
          }
        }
      }
    }

    private void CreateImage(string imageUrl)
    {

      var bi = new BitmapImage
      {
        UriSource = new Uri(imageUrl),
#if WINDOWS_PHONE
        CreateOptions = BitmapCreateOptions.BackgroundCreation
#else
        CreateOptions = BitmapCreateOptions.None
#endif
      };
      backgroundBrush.ImageSource = bi;
    }

    /// <summary>
    /// Converts string to base64
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private string Base64Encode(string data)
    {
      try
      {
        var encData_byte = new byte[data.Length];
        encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
        return Convert.ToBase64String(encData_byte);
      }
      catch (Exception e)
      {
        throw new Exception("Error in Base64Encode" + e.Message);
      }
    }

#if WINDOWS_PHONE    
    /// <summary>
    /// Called upon a search error (not used)
    /// </summary>
    /// <param name="ex"></param>
    private void WebClientOpenReadError(Exception ex)
    {
    }
#endif
  }
}
