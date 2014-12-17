using System.Collections.Generic;
using System.Xml.Linq;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// A helper class to easily retrieve data from the WMAppManifest.xml
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
        if (null == _properties)
        {
          _properties = new Dictionary<string, string>();
          var appManifestXml = XDocument.Load("WMAppManifest.xml");
          using (var rdr = appManifestXml.CreateReader(ReaderOptions.None))
          {
            rdr.ReadToDescendant("App");
            if (!rdr.IsStartElement())
            {
              throw new System.FormatException("App tag not found in WMAppManifest.xml ");
            }
            rdr.MoveToFirstAttribute();
            while (rdr.MoveToNextAttribute())
            {
              _properties.Add(rdr.Name, rdr.Value);
            }
          }
        }
        return _properties;
      }
    }

    public string Version
    {
      get { return Properties["Version"]; }
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
      get { return Properties["Genre"]; }
    }

    public string Description
    {
      get { return Properties["Description"]; }
    }

    public string Publisher
    {
      get { return Properties["Publisher"]; }
    }
  }
}
