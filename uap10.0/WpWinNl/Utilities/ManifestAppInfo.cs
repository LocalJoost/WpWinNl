using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.ApplicationModel;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// A helper class to easily retrieve data from the Package.appxmanifest
  /// App tag
  /// </summary>
  public class ManifestAppInfo
  {
    public ManifestAppInfo()
    {
    }

    static Dictionary<string, string> _properties;

    static Dictionary<string, string> Properties
    {
      get
      {
        if (_properties == null)
        {
          _properties = new Dictionary<string, string>();

          _properties["Title"] = string.Empty;
          _properties["Publisher"] = string.Empty;
          _properties["ProductID"] = string.Empty;

          var appManifestXml = XDocument.Load("AppxManifest.xml");
          var xName = XNamespace.Get("http://schemas.microsoft.com/appx/2010/manifest");
          _properties["Title"] = GetSafeValue(appManifestXml, xName, "DisplayName");
          _properties["Publisher"] = GetSafeValue(appManifestXml, xName, "PublisherDisplayName");
          var mpName = XNamespace.Get("http://schemas.microsoft.com/appx/2014/phone/manifest");
          var node = appManifestXml.Descendants(mpName + "PhoneIdentity").FirstOrDefault();
          if (node != null)
          {
            _properties["ProductID"] = node.AttributeValue("PhoneProductId");
          }
        }
        return _properties;
      }
    }

    private static string GetSafeValue(XContainer d, XNamespace xName, string name)
    {
      var node = d.Descendants(xName + name).FirstOrDefault();
      return node != null ? node.Value : string.Empty;
    }

    public string Version
    {
      get
      {
        var v = Package.Current.Id.Version;
        return string.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build);
      }
    }

    public string ProductId
    {
      get { return Properties["ProductID"]; }
    }

    public string Title
    {
      get { return Properties["Title"]; }
    }

    public string TitleUc
    {
      get { return !string.IsNullOrEmpty(Title) ? Title.ToUpperInvariant() : null; }
    }

    public string Genre
    {
      get { return string.Empty; }
    }

    public string Description
    {
      get { return string.Empty; }
    }

    public string Publisher
    {
      get { return Properties["Publisher"]; }
    }
  }
}
